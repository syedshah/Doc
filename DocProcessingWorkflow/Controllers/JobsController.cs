// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JobsController.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   Defines the JobsController type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DocProcessingWorkflow.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;

    using BusinessEngineInterfaces;

    using DocProcessingWorkflow.Constants;
    using DocProcessingWorkflow.Filters;
    using DocProcessingWorkflow.Models.Jobs;
    using FileSystemInterfaces;

    using Entities;

    using Exceptions;

    using Logging;

    using ServiceInterfaces;

    [AuthorizeLoggedInUser]
    [RoutePrefix("Jobs")]
    [Route("{action=index}")]
    public class JobsController : BaseController
    {
        private const Int32 PageSize = 10;

        private readonly IJobService jobService;

        private readonly IAuthoriseJobEngine authoriseJobEngine;

        private readonly IUserService userService;

        private readonly IOneStepFileService oneStepFileService;

        private readonly IReportFileService reportFileService;

        private readonly IInputFileService inputFileService;

        private readonly IPdfFileEngine pdfFileEngine;

        private readonly IEnclosingJobService enclosingJobService;

        private readonly IJobStatusTypeService jobStatusTypeService;

        private readonly IFileInfoFactory _fileInfoFactory;

        public JobsController(
          IJobService jobService,
          IAuthoriseJobEngine authoriseJobEngine,
          IUserService userService,
          IOneStepFileService oneStepFileService,
          IReportFileService reportFileService,
          IInputFileService inputFileService,
          IAppEnvironmentService appEnvironmentService,
          IPdfFileEngine pdfFileEngine,
          ILogger logger,
          IEnclosingJobService enclosingJobService,
          IJobStatusTypeService jobStatusTypeService,
          IFileInfoFactory fileInfoFactory)
            : base(logger, appEnvironmentService, userService)
        {
            this.jobService = jobService;
            this.authoriseJobEngine = authoriseJobEngine;
            this.userService = userService;
            this.oneStepFileService = oneStepFileService;
            this.reportFileService = reportFileService;
            this.inputFileService = inputFileService;
            this.pdfFileEngine = pdfFileEngine;
            this.enclosingJobService = enclosingJobService;
            this.jobStatusTypeService = jobStatusTypeService;
            _fileInfoFactory = fileInfoFactory;
        }

        public ActionResult Index()
        {
            return this.RedirectToAction("View", "Jobs");
        }

        [Route("View")]
        public ActionResult ViewJobs(Int32 page = 1, Boolean isAjaxCall = false)
        {
            this.SetEnvironmentSession();
            var currentuser = this.userService.GetApplicationUser(HttpContext.User.Identity.Name);

            var model = new ViewJobsViewModel();

            this.SetRole(currentuser, model);

            PagedResult<JobEntity> jobs;

            var environment = (String)this.HttpContext.Session[SessionObject.Environment];

            jobs = this.jobService.GetJobs(page, PageSize, currentuser.Id, environment);

            model.AddJobs(jobs);

            //Adds enclosing jobs per each job
            AddEnclosingJobs(model);

            return this.View(model);
        }

        [HttpPost]
        [Route("ViewJobsAjax")]
        public ActionResult ViewJobsAjax(Int32 page = 1)
        {
            this.SetEnvironmentSession();

            var currentuser = this.userService.GetApplicationUser(HttpContext.User.Identity.Name);

            var searchCriteria = String.Empty;

            if (TempData.ContainsKey("SearchField"))
            {
                searchCriteria = this.TempData["SearchField"].ToString();
            }

            var model = new ViewJobsViewModel();
            model.SearchValue = searchCriteria;

            this.SetRole(currentuser, model);

            PagedResult<JobEntity> jobs;

            var environment = (String)this.HttpContext.Session[SessionObject.Environment];

            if (searchCriteria.Length < 1)
            {
                jobs = this.jobService.GetJobs(page, PageSize, currentuser.Id, environment);
            }
            else
            {
                jobs = this.jobService.GetJobs(page, PageSize, searchCriteria, currentuser.Id, environment);
            }

            model.AddJobs(jobs);

            //Adds enclosing jobs per each job
            AddEnclosingJobs(model);

            return this.PartialView("_JobsList", model);
        }

        [Route("AssignSearchValue")]
        public ActionResult AssignSearchValue(String searchfield)
        {
            this.TempData["SearchField"] = searchfield;
            return this.Json(new { Success = true });
        }

        [Route("Search")]
        public ActionResult SearchJobs(String searchCriteria)
        {
            this.TempData["SearchField"] = searchCriteria;
            this.SetEnvironmentSession();
            var currentuser = this.userService.GetApplicationUser(HttpContext.User.Identity.Name);
            Int32 page = 1;

            var model = new ViewJobsViewModel();

            model.SearchValue = searchCriteria;

            this.SetRole(currentuser, model);

            PagedResult<JobEntity> jobs;

            var environment = (String)this.HttpContext.Session[SessionObject.Environment];
            if (searchCriteria.Length < 1)
            {
                jobs = this.jobService.GetJobs(page, PageSize, currentuser.Id, environment);
            }
            else
            {
                jobs = this.jobService.GetJobs(page, PageSize, searchCriteria, currentuser.Id, environment);
            }

            model.AddJobs(jobs);

            //Adds enclosing jobs per each job
            AddEnclosingJobs(model);

            return this.PartialView("_JobsList", model);
        }

        [Route("GetExcelReport")]
        public ActionResult GetExcelReport(String searchCriteria)
        {
            var currentuser = this.userService.GetApplicationUser(HttpContext.User.Identity.Name);

            var environment = (String)this.HttpContext.Session[SessionObject.Environment];

            StringBuilder builder = this.jobService.GetJobsReport(searchCriteria, currentuser.Id, environment);

            return this.File(new UTF8Encoding().GetBytes(builder.ToString()), "text/csv", "Reports123.csv");
        }

        public ActionResult ExportJobToExcel(int jobId)
        {
            var currentuser = this.userService.GetApplicationUser(HttpContext.User.Identity.Name);

            var builder = this.jobService.GetJobReportByJobId(jobId, currentuser.Id);

            return this.File(new UTF8Encoding().GetBytes(builder.ToString()), "text/csv", "JobReport_" + jobId + ".csv");
        }


        public ActionResult GetOneStepLog(String jobId, String grid)
        {
            var model = new OneStepLogViewModel();
            var environment = (String)this.HttpContext.Session[SessionObject.Environment];

            this.ValidateString(environment, "Environment session is null or empty");

            var oneStepLog = this.oneStepFileService.GetOneStepLog(grid, environment);
            model.AddOneStepLog(oneStepLog);
            return this.PartialView("_OneStepLog", model);
        }

        [Route("GetJobReportList")]
        public ActionResult GetJobReportList(String jobId, String grid)
        {
            var model = new JobReportsViewModel();
            var environment = (String)this.HttpContext.Session[SessionObject.Environment];

            this.ValidateString(environment, "Environment session is null or empty");

            var reportFiles = this.reportFileService.GetReportFiles(grid, environment);
            model.AddJobReports(reportFiles);

            return this.PartialView("_JobsReportList", model);
        }

        [Route("GetOneStepExcelReport")]
        public FilePathResult GetOneStepExcelReport(String fileName, String filePath)
        {
            IFileInfo fileInfo = _fileInfoFactory.CreateFileInfo(@filePath);

            var fileNameArray = fileName.Split(Convert.ToChar("["));
            String fullFileName = String.Format("{0}{1}", fileNameArray[0].TrimEnd(), fileInfo.Extension);

            if (fileInfo.Exists && fileInfo.Extension == ".xlsx")
                return File(filePath, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fullFileName);
            else
                return File(@filePath, "application/vnd.ms-excel", fullFileName);
        }

        [Route("GetOneStepReport")]
        public ActionResult GetOneStepReport(String filePath)
        {
            var reportFile = this.reportFileService.GetReportFile(filePath);
            var model = new JobReportViewModel();
            model.AddReportFile(reportFile);
            return this.PartialView("_JobReport", model);
        }

        [Route("GetJobFiles")]
        public ActionResult GetJobFiles(String jobId)
        {
            var model = new JobFilesViewModel();
            var inputFiles = this.inputFileService.GetInputFilesByJobId(Convert.ToInt32(jobId));
            model.AddInputFiles(inputFiles);
            return this.PartialView("_JobFiles", model);
        }

        [Route("Authorise")]
        public ActionResult Authorise(Int32[] jobIds)
        {
            var jobIdList = jobIds.ToList();
            var currentuser = this.userService.GetApplicationUser(HttpContext.User.Identity.Name);
            this.authoriseJobEngine.AuthoriseJob(jobIdList, JobStatusTypes.Complete, currentuser.Id, String.Empty);


            var environment = (String)this.HttpContext.Session[SessionObject.Environment];

            var jobs = this.jobService.GetJobs(1, PageSize, currentuser.Id, environment);
            var model = new ViewJobsViewModel();

            this.SetRole(currentuser, model);

            model.AddJobs(jobs);

            //Adds enclosing jobs per each job
            AddEnclosingJobs(model);

            return this.PartialView("_JobsList", model);
        }

        [Route("GetIfJobHasPdf")]
        public ActionResult GetIfJobHasPdf(String jobId, String grid)
        {
            var environment = (String)this.HttpContext.Session[SessionObject.Environment];

            this.ValidateString(environment, "Environment session is null or empty");

            var pdfFiles = this.pdfFileEngine.GetPdfFiles(grid, environment);

            var model = new JobPdfsViewModel();

            model.AddPdfs(pdfFiles);

            return this.PartialView("_JobPdfList", model);
        }

        [Route("GetPdf")]
        public ActionResult GetPdf(String filePath, String fileName)
        {
            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            var fileStreamResult = new FileStreamResult(fileStream, "Application/pdf");
            return fileStreamResult;
        }

        [Route("SaveDocketNumber")]
        public ActionResult UpdateDocketNumber(int enclosingJobId, int jobId, string postalDocketNumber = null)
        {
            if (!string.IsNullOrEmpty(postalDocketNumber) && (postalDocketNumber.Length < 6 || postalDocketNumber.Length > 25))
            {
                const string errorMessage = "Must be of 6-25 characters and should be aplhanumeric.";
                return this.Json(new { Success = false, Error = errorMessage }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                this.enclosingJobService.UpdateEnclosingJobDocketNumber(enclosingJobId, postalDocketNumber);
                String jobStatus = null;
                var currentuser = this.userService.GetApplicationUser(HttpContext.User.Identity.Name);
                var job = this.jobService.GetJobById(jobId, currentuser.Id);
                var enclosingJobs = this.enclosingJobService.GetEnclosingJobsByJobId(jobId);
                if (job.Status != JobStatusTypes.Cancelled && enclosingJobs.Any() && !enclosingJobs.Any(x => String.IsNullOrWhiteSpace(x.PostalDocketNumber)))
                {
                    jobStatus = JobStatusTypes.Dispatched;
                    var jobStatusTypeId = this.jobStatusTypeService.GetJobStatusType(jobStatus).JobStatusTypeID;
                    this.jobService.UpdateJobStatus(jobId, jobStatusTypeId);
                }
                return this.Json(new { Success = true, Status = jobStatus }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult CancelJob(Int32 jobId)
        {
            IList<EnclosingJob> enclosedJobs = this.enclosingJobService.GetEnclosingJobsByJobId(jobId);
            Boolean docketNumberMissing = enclosedJobs.Any(enclosingJob => String.IsNullOrWhiteSpace(enclosingJob.PostalDocketNumber));
            if (!docketNumberMissing)
            {
                throw new InvalidOperationException("Cannot cancel job as all docket numbers are present");
            }

            JobStatusTypeEntity jobStatusType = this.jobStatusTypeService.GetJobStatusType(JobStatusTypes.Cancelled);
            this.jobService.UpdateJobStatus(jobId, jobStatusType.JobStatusTypeID);
            return this.Json(new { Status = JobStatusTypes.Cancelled });
        }

        private void ValidateString(String stringToValidate, String exceptionMessage)
        {
            if (String.IsNullOrEmpty(stringToValidate))
            {
                throw new DocProcessingException(exceptionMessage);
            }
        }

        private void SetRole(ApplicationUser currentuser, ViewJobsViewModel model)
        {
            if (!currentuser.Roles.Any())
            {
                return;
            }

            var currentUserRoleNames =
                currentuser.Roles
                    .Select(identityUserRole => identityUserRole.Role.Name)
                    .ToList();

            model.CanShowActionMenu = true;

            if (currentUserRoleNames.Contains(Role.ExportSearchResults))
            {
                model.CanExportResults = true;
            }

            if (currentUserRoleNames.Contains(Role.AuthoriseJobs))
            {
                model.CanAuthoriseJobs = true;
            }

            if (currentUserRoleNames.Contains(Role.StatusChange))
            {
                model.CanChangeStatus = true;
            }
        }

        private void AddEnclosingJobs(ViewJobsViewModel model)
        {
            foreach (JobViewModel job in model.Jobs)
            {
                job.AddEnclosingJobs(this.enclosingJobService.GetEnclosingJobsByJobId(Int32.Parse(job.Id)));
            }
        }
    }
}