
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PromoteManCoDocsController.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   Defines the PromoteManCoDocsController type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using DocProcessingWorkflow.Filters;

namespace DocProcessingWorkflow.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using DocProcessingWorkflow.Constants;
    using DocProcessingWorkflow.Models.PromoteManCoDocs;
    using Logging;
    using ServiceInterfaces;

    [AuthorizeLoggedInUser]
    [Authorize(Roles = Role.PromoteManCoDocs)]
    public class PromoteManCoDocsController : BaseController
    {
        private readonly IManCoService manCoService;
        private readonly IUserService userService;
        private readonly IManCoDocService manCoDocService;


        public PromoteManCoDocsController(IManCoService manCoService, IUserService userService, IManCoDocService manCoDocService, ILogger logger, IAppEnvironmentService appEnvironmentService)
            : base(logger, appEnvironmentService, userService)
        {
            this.manCoService = manCoService;
            this.userService = userService;
            this.manCoDocService = manCoDocService;
        }

        [HttpGet]
        [Route("PromoteManCoDocs/Index")]
        public ActionResult Index()
        {
            this.SetEnvironmentSession();
            var currentuser = this.userService.GetApplicationUser(HttpContext.User.Identity.Name);

            var model = this.GetPromoteManCoDocViewModel(currentuser.Id);

            return this.View(model);
        }

        [Route("PromoteManCoDocs/GetManCoDocs")]
        public ActionResult GetManCoDocs(String manCoCode, String selectedSourceEnvironment)
        {
            if (String.IsNullOrEmpty(manCoCode) || String.IsNullOrEmpty(selectedSourceEnvironment))
            {
                return this.PartialView("_MancoDocResults", new PromoteManCoDocViewModel());
            }
            var currentUser = this.userService.GetApplicationUser();
            var model = new PromoteManCoDocViewModel();

            var manCoDocs = this.manCoDocService.GetManCoDocsByManCoCodeEnvironment(manCoCode, selectedSourceEnvironment, currentUser.Id);
            model.AddManCoDocs(manCoDocs);

            return this.PartialView("_MancoDocResults", model);
        }

        [HttpPost]
        [Route("PromoteManCoDocs/SubmitManCoDocs")]
        public ActionResult SubmitManCoDocs(PromoteManCoDocViewModel promoteManCoDocViewModel)
        {
            if (!ModelState.IsValid)
            {
                var error = this.ModelState.Values.SelectMany(x => x.Errors).ToList().Select(x => x.ErrorMessage);
                var errormessage = String.Join(",", error.ToArray());

                TempData["message"] = errormessage;
                return RedirectToAction("Index", "PromoteManCoDocs");
            }

            if (String.IsNullOrEmpty(promoteManCoDocViewModel.SelectedSourceEnvironment) || String.IsNullOrEmpty(promoteManCoDocViewModel.SelectedTargetEnvironment)
                || promoteManCoDocViewModel.SelectedManCoDocs == null || promoteManCoDocViewModel.SelectedManCoDocs.Count == 0)
            {
                TempData["message"] = "Server Error";
                return RedirectToAction("Index", "PromoteManCoDocs");
            }
            var manCoDocIds = string.Join(",", promoteManCoDocViewModel.SelectedManCoDocs.ToArray());
            var sourceAppEnvironment = promoteManCoDocViewModel.SelectedSourceEnvironment;
            var targetAppEnvironment = promoteManCoDocViewModel.SelectedTargetEnvironment;
            var currentUser = this.userService.GetApplicationUser();
            var comment = promoteManCoDocViewModel.Comment ?? "";
            this.manCoDocService.PromoteMancoDocs(manCoDocIds, sourceAppEnvironment, targetAppEnvironment, currentUser.Id, comment);

            return RedirectToAction("Index", "PromoteManCoDocs");
        }

        private PromoteManCoDocViewModel GetPromoteManCoDocViewModel(String userId)
        {
            var model = new PromoteManCoDocViewModel();

            var manCos = this.manCoService.GetManCos(userId);
            model.SourceEnvironments = this.SetEnvironmentSession().Environments;
            model.TargetEnvironments = this.AppEnvironmentService.GetAppEnvironments().Select(e => e.Name).ToList();

            model.AddManCos(manCos);

            return model;
        }
    }
}