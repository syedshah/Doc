// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ManagePullsController.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   Controller for managing pulls
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using DocProcessingWorkflow.Filters;

namespace DocProcessingWorkflow.Controllers
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;
  using System.Web.Mvc;

  using AbstractConfigurationManager;

  using DocProcessingWorkflow.Constants;
  using DocProcessingWorkflow.HtmlHelpers;
  using DocProcessingWorkflow.Models.ManagePulls;

  using Entities;
  using Entities.ADF;

  using Logging;

  using ServiceInterfaces;

      [AuthorizeLoggedInUser]
  public class ManagePullsController : BaseController
  {
    private readonly IManCoService manCoService;

    private readonly IUserService userService;

    private readonly IJobService jobService;

    private readonly IPackStoreService packStoreService;

    private readonly IEmailService emailService;

    private readonly IConfigurationManager configurationManager;

    public ManagePullsController(
      IManCoService manCoService,
      IUserService userService,
      IJobService jobService,
      IPackStoreService packStoreService,
      IEmailService emailService,
      IConfigurationManager configurationManager,
      IAppEnvironmentService appEnvironmentService,
      ILogger logger)
      : base(logger, appEnvironmentService, userService)
    {
      this.manCoService = manCoService;
      this.userService = userService;
      this.jobService = jobService;
      this.packStoreService = packStoreService;
      this.emailService = emailService;
      this.configurationManager = configurationManager;
    }

    [HttpGet]
    [Route("ManagePulls/Index")]
    public ActionResult Index()
    {
      var model = new ManagePullsViewModel();

      var currentUser = this.userService.GetApplicationUser();
      var manCos = this.manCoService.GetManCos(currentUser.Id);

      model.AddManCos(manCos);

      return this.View(model);
    }

    [Route("ManagePulls/GetCompletedJobs")]
    public ActionResult GetCompletedJobs(String manCo)
    {
      var currentUser = this.userService.GetApplicationUser();
      var completedJobs = this.jobService.GetCompletedJobs(manCo, currentUser.Id);

      var model = new CompletedJobsJsonResponse();
      model.AddJobs(completedJobs);

      return this.Json(model.Jobs, JsonRequestBehavior.AllowGet);
    }

    [Route("ManagePulls/GetPulls")]
    public ActionResult GetPulls(Int32 jobId)
    {
      var currentUser = this.userService.GetApplicationUser();
      var pulls = this.packStoreService.GetPulledPacks(jobId);

      var model = new PulledDocumentsViewModel();
      model.AddPulls(pulls);

      this.SetRole(currentUser, model);

      return this.PartialView("_PulledDocuments", model);
    }

    [HttpPost]
    [Route("ManagePulls/Search")]
    public ActionResult Search(SearchPacksViewModel searchPacksViewModel)
    {
      if (!this.ModelState.IsValid)
      {
        var errorList = this.ModelState.Values.SelectMany(x => x.Errors).ToList();
        return this.Json(new { error = true, Error = errorList });
      }

      var model = new SearchPacksResultsViewModel();

      IList<PackStore> packs = this.packStoreService.GetNonPulledPacks(
        searchPacksViewModel.SearchCriteria, searchPacksViewModel.JobId);

      if (packs.Count == 0)
      {
        return this.Json(new { error = true, Error = "Could not find any packs for the serach criteria" });
      }

      model.AddPacks(packs);

      return this.Json(new
            {
              error = false,
              message = RenderRazorView.RenderRazorViewToString("_PullList", model, ControllerContext, ViewData, TempData)
            }, JsonRequestBehavior.AllowGet);
    }

    [HttpPost]
    [Route("ManagePulls/Pull")]
    public ActionResult Pull(PullPacksViewModel pullPacksViewModel)
    {
      foreach (var pull in pullPacksViewModel.Packs.Where(d => d.Selected))
      {
        this.packStoreService.UpdatePullStatus(pull.ClientReference, true);
      }

      return this.Json(new { error = false, Error = String.Empty });
    }

    [Route("ManagePulls/RemovePull")]
    public ActionResult RemovePull(String clientRef, Int32 jobId)
    {
      var currentUser = this.userService.GetApplicationUser();
      this.packStoreService.UpdatePullStatus(clientRef, false);

      var pulls = this.packStoreService.GetPulledPacks(jobId);

      var model = new PulledDocumentsViewModel();
      model.AddPulls(pulls);

      this.SetRole(currentUser, model);

      return this.PartialView("_PulledDocuments", model);
    }

    [Route("ManagePulls/SendEmail")]
    public ActionResult SendEmail(PullPacksViewModel pullPacksViewModel)
    {
      var packs = new StringBuilder();

      foreach (var pack in pullPacksViewModel.Packs)
      {
        packs.Append(String.Format("<li>{0} {1}</li>", pack.ClientReference, pack.Name));
      }

      String packInfo = String.Format("<ul>{0}</ul>", packs);
      String emailBody = String.Format(
        "<p>The following packs have been pulled for job {0}: </p> <p>{1}</p>", pullPacksViewModel.JobId, packInfo);
      String from = this.configurationManager.AppSetting("from");
      String to = this.configurationManager.AppSetting("to");
      String subject = this.configurationManager.AppSetting("subject");
      String mailServer = this.configurationManager.AppSetting("mailServer");

      this.emailService.SendEmail(from, to, subject, emailBody, mailServer);

      var redirectUrl = Url.Action("Authorised", "ManagePulls");
      return this.Json(new { Url = redirectUrl, Error = string.Empty });
    }

    [Route("ManagePulls/Authorised")]
    public ActionResult Authorised()
    {
      return this.View();
    }

    private void SetRole(ApplicationUser currentuser, PulledDocumentsViewModel model)
    {
      if (currentuser.Roles.Select(x => x.Role.Name).ToList().Contains(Role.PullAuthorisation))
      {
        model.CanAuthorisePullList = true;
      }
    }
  }
}