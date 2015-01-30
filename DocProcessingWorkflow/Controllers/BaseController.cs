// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseController.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   Defines the BaseController type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DocProcessingWorkflow.Controllers
{
    using System;
    using System.Net;
    using System.Web;
    using System.Web.Mvc;
    using Constants;
    using Models;
    using Models.Environment;
    using Entities;
    using Logging;
    using ServiceInterfaces;

    public class BaseController : Controller
    {
        protected readonly ILogger Logger;
        protected readonly IAppEnvironmentService AppEnvironmentService;
        protected readonly IUserService UserService;

        public BaseController(
          ILogger logger,
          IAppEnvironmentService appEnvironmentService,
          IUserService userService)
        {
            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }

            this.Logger = logger;
            this.AppEnvironmentService = appEnvironmentService;
            this.UserService = userService;
        }

        public EnvironmentViewModel SetEnvironmentSession()
        {
            var user = this.UserService.GetApplicationUser();
            var environmentViewModel = this.GetEnvironmentModel(user);

            if (this.HttpContext.Session[SessionObject.Environment] == null)
            {
                this.SetEnvironmentModel(environmentViewModel, user);
            }
            else
            {
                environmentViewModel.SelectedEnvironment = (String)this.HttpContext.Session[SessionObject.Environment];
            }

            return environmentViewModel;
        }

        protected void AddEnvironmentToSession(String environment)
        {
            if (this.HttpContext.Session != null)
            {
                this.HttpContext.Session[SessionObject.Environment] = environment;
            }
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            // Bail if we can't do anything; app will crash.
            if (filterContext == null)
            {
                return;
            }

            if (!filterContext.HttpContext.IsCustomErrorEnabled)
            {
                return;
            }

            var ex = filterContext.Exception ?? new Exception("No further information exists.");
            filterContext.ExceptionHandled = true;
            if (ex.GetType() != typeof(HttpRequestValidationException))
            {
                this.Logger.Error(ex, filterContext.HttpContext.Request.Path, filterContext.HttpContext.Request.RawUrl);
                var data = new ErrorViewModel
                             {
                                 ErrorMessage = HttpUtility.HtmlEncode(ex.Message),
                                 DisplayMessage = "Unhandled exception",
                                 ErrorCode = ErrorCode.Unknown,
                                 Severity = ErrorSeverity.Error
                             };
                filterContext.Result = this.View("Error", data);
            }
        }

        protected JsonResult JsonError(Exception e, ErrorCode errorCode, String displayMessage)
        {
            Response.StatusCode = (System.Int16)HttpStatusCode.InternalServerError;
            return
              this.Json(
                new ErrorViewModel
                  {
                      DisplayMessage = displayMessage,
                      ErrorCode = errorCode,
                      ErrorMessage = e.Message,
                      Severity = ErrorSeverity.Error
                  });
        }

        private EnvironmentViewModel GetEnvironmentModel(ApplicationUser user)
        {
            var environments = this.AppEnvironmentService.GetAppEnvironments(user.Id);

            var environmentViewModel = new EnvironmentViewModel(user);
            environmentViewModel.AddAllowedEnvironments(environments);

            return environmentViewModel;
        }

        private void SetEnvironmentModel(EnvironmentViewModel environmentViewModel, ApplicationUser user)
        {
            if (user.PreferredEnvironment == null)
            {
                this.AddEnvironmentToSession(environmentViewModel.SelectedEnvironment);
            }
            else
            {
                this.AddEnvironmentToSession(user.PreferredEnvironment);
            }
        }
    }
}