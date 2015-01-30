// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SubmitJobsController.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   Controller for submitt
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using DocProcessingWorkflow.Filters;

namespace DocProcessingWorkflow.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using DocProcessingWorkflow.Constants;
    using DocProcessingWorkflow.HtmlHelpers;
    using DocProcessingWorkflow.Models.SubmitJobs;
    using Exceptions;
    using Logging;
    using ServiceInterfaces;

    [AuthorizeLoggedInUser]
    public class SubmitJobsController : BaseController
    {
        private readonly IManCoService manCoService;
        private readonly IUserService userService;
        private readonly ISubmitJobsService submitJobsService;
        private readonly IDocTypeService docTypeService;
        private readonly IJobService jobService;

        public SubmitJobsController(IManCoService manCoService, IUserService userService, ISubmitJobsService submitJobsService, IDocTypeService docTypeService, IJobService jobService, ILogger logger, IAppEnvironmentService appEnvironmentService)
            : base(logger, appEnvironmentService, userService)
        {
            this.manCoService = manCoService;
            this.userService = userService;
            this.submitJobsService = submitJobsService;
            this.docTypeService = docTypeService;
            this.jobService = jobService;
        }

        [HttpGet]
        [Route("SubmitJobs/Index")]
        public ActionResult Index()
        {
            var model = new SubmitJobsViewModel();

            var currentUser = this.userService.GetApplicationUser();
            var manCos = this.manCoService.GetManCos(currentUser.Id);

            model.AddManCos(manCos);

            return this.View(model);
        }

        [Route("SubmitJobs/GetFiles")]
        public ActionResult GetFiles(String manCo, String docTypeCode, String docTypeName)
        {
            if (String.IsNullOrEmpty(manCo) || String.IsNullOrEmpty(docTypeCode))
            {
                return this.PartialView("_InputFileResults", new InputFileViewModel());
            }

            var fileInfo = this.submitJobsService.GetInputFiles((String)this.HttpContext.Session[SessionObject.Environment], manCo, docTypeCode);
            var documentType = this.docTypeService.GetDocType(docTypeCode, docTypeName);

            //var model = new InputFileViewModel(fileInfo.Folder, fileInfo.Folder, fileInfo.AdditionalFileContent, documentType);
            var model = new InputFileViewModel(fileInfo.Folder, fileInfo.Folder, documentType);
            model.AddFiles(fileInfo.Files);
            model.AddSubFolders(fileInfo.Folders);

            return this.PartialView("_InputFileResults", model);
        }

        [Route("SubmitJobs/GetFolderFiles")]
        public ActionResult GetFolderFiles(String path, String manCo, String docType)
        {
            if (String.IsNullOrEmpty(manCo) || String.IsNullOrEmpty(docType) || String.IsNullOrEmpty(path))
            {
                return this.PartialView("_InputFileResults", new InputFileViewModel());
            }

            var fileInfo = this.submitJobsService.GetInputFiles(path, (String)this.HttpContext.Session[SessionObject.Environment], manCo, docType);

            //var model = new InputFileViewModel(fileInfo.Folder, path, fileInfo.AdditionalFileContent);
            var model = new InputFileViewModel(fileInfo.Folder, path);
            model.AddFiles(fileInfo.Files);
            model.AddSubFolders(fileInfo.Folders);

            return this.PartialView("_InputFileResults", model);
        }

        [HttpPost]
        [Route("SubmitJobs/Create")]
        public ActionResult Create(CreateJobViewModel createJobViewModel)
        {
            if (!this.ModelState.IsValid)
            {
                var errorList = this.ModelState.Values.SelectMany(x => x.Errors).ToList();
                return this.Json(new { Url = String.Empty, Error = errorList });
            }

            try
            {
                String selectedFolder = this.GetSelectedFolder(createJobViewModel.SelectedFolder, createJobViewModel.DocTypeCode);

                var currentUser = this.userService.GetApplicationUser();

                this.jobService.CreateJob(
                  (String)this.HttpContext.Session[SessionObject.Environment],
                  createJobViewModel.ManCo,
                  createJobViewModel.DocTypeCode,
                  createJobViewModel.DocTypeName,
                  createJobViewModel.ChosenFiles,
                  currentUser.Id,
                  createJobViewModel.AllowReprocessing,
                  createJobViewModel.AdditionalInfoRequired,
                  createJobViewModel.AdditionalSetupViewModel.AdditionalFileContent,
                  String.Format("{0}{1}{2}", createJobViewModel.AdditionalSetupViewModel.SortCodeOne, createJobViewModel.AdditionalSetupViewModel.SortCodeTwo, createJobViewModel.AdditionalSetupViewModel.SortCodeThree),
                  createJobViewModel.AdditionalSetupViewModel.AccountNumber,
                  createJobViewModel.AdditionalSetupViewModel.ChequeNumber,
                  selectedFolder);

                var redirectUrl = Url.Action("Index", "Jobs");
                return this.Json(new { Url = redirectUrl, Error = string.Empty });
            }
            catch (DocProcessingFileAlreadyProcessedException)
            {
                return this.Json(new { Url = string.Empty, Error = "File has already been processed" });
            }
        }

        [Route("SubmitJobs/Saved")]
        public ActionResult Saved()
        {
            return this.View();
        }

        [Route("SubmitJobs/GetAdditionalInfo")]
        public ActionResult GetAdditionalInfo(List<String> files, String path)
        {
            if (String.IsNullOrEmpty(path) || files == null || files.Count == 0)
            {
                return this.Json
                 (new
                 {
                     error = false,
                     message = RenderRazorView.RenderRazorViewToString("_AdditionalInfo", new AdditionalSetupViewModel(), ControllerContext, ViewData, TempData)
                 }, JsonRequestBehavior.AllowGet);
            }

            var additionalFileInfo = this.submitJobsService.GetAdditionalInfo(path, files);
            var additionalSetupViewModel = new AdditionalSetupViewModel(additionalFileInfo);

            return this.Json(new
            {
                error = false,
                message = RenderRazorView.RenderRazorViewToString("_AdditionalInfo", additionalSetupViewModel, ControllerContext, ViewData, TempData)
            }, JsonRequestBehavior.AllowGet);
        }

        private String GetSelectedFolder(String selectedFolder, String docTypeCode)
        {
            String result = string.Empty;
            if (!String.IsNullOrEmpty(selectedFolder))
            {
                var folderIndex = selectedFolder.IndexOf(docTypeCode, 0, StringComparison.OrdinalIgnoreCase);
                result = selectedFolder.Substring(folderIndex);
            }

            return result;
        }
    }
}