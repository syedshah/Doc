// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnvironmentController.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//  Controller for managing the users environment 
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using DocProcessingWorkflow.Filters;

namespace DocProcessingWorkflow.Controllers
{
    using System;
    using System.Web.Mvc;

    using DocProcessingWorkflow.Constants;
    using DocProcessingWorkflow.Models.Environment;
    using Logging;
    using ServiceInterfaces;

      [AuthorizeLoggedInUser]
    public class EnvironmentController : BaseController
    {
        private readonly IAppEnvironmentService appEnvironmentService;
        private readonly IUserService userService;

        public EnvironmentController(IAppEnvironmentService appEnvironmentService, IUserService userService, ILogger logger)
            : base(logger, appEnvironmentService, userService)
        {
            this.appEnvironmentService = appEnvironmentService;
            this.userService = userService;
        }

        [ChildActionOnly]
        [Route("Environment/Index")]
        public ActionResult Index()
        {
            var environmentViewModel = this.SetEnvironmentSession();

            return this.PartialView("_environment", environmentViewModel);
        }

        [Route("Environment/Change")]
        public ActionResult Change(String environment, String returnUrl)
        {
            this.AddEnvironmentToSession(environment);
            String redirectUrl = String.IsNullOrEmpty(returnUrl) ? Url.Action("Index", "Home") : returnUrl;
            return this.Json(new { Url = redirectUrl, Error = string.Empty });
        }
        
    }
}