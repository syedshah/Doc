// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JobsControllerTests.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   Defines the JobsControllerTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using FileSystemInterfaces;
using Moq.Language.Flow;

namespace WebTests.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;

    using BusinessEngineInterfaces;

    using DocProcessingWorkflow.Controllers;
    using DocProcessingWorkflow.Models.Jobs;
    using Entities;
    using Entities.File;

    using Exceptions;

    using FluentAssertions;
    using Logging;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Moq;
    using NUnit.Framework;

    using ServiceInterfaces;
    using DocProcessingWorkflow.Constants;

    [Category("Unit")]
    [TestFixture]
    public class JobsControllerTests : ControllerTestsBase
    {
        private Mock<ILogger> logger;

        private Mock<IJobService> jobService;
        private Mock<IAuthoriseJobEngine> authoriseJobEngine;

        private Mock<IUserService> userService;

        private Mock<IOneStepFileService> oneStepFileService;

        private Mock<IReportFileService> reportFileService;

        private Mock<IInputFileService> inputFileService;

        private Mock<IAppEnvironmentService> appEnvironmentService;

        private Mock<IPdfFileEngine> pdfFileEngine;

        private Mock<IEnclosingJobService> enclosingJobService;

        private Mock<IJobStatusTypeService> jobStatusTypeService;

        private Mock<IFileInfoFactory> fileInfoFactory;

        private TempDataDictionary tempDataMock;

        private JobsController jobsController;

        private IList<JobEntity> jobs;

        private IList<InputFileEntity> inputFiles;

        private IList<EnvironmentServerEntity> servers;

        private IList<ReportFile> reportFiles;

        private IList<PdfFile> pdfFiles;

        private IList<AppEnvironment> appEnvironments;

        private IList<EnclosingJob> enclosingJobs;

        private ReportFile reportFile;

        private PagedResult<JobEntity> pagedJobs;

        private ApplicationUser currentUser;

        private Int32 pageNumber;

        private Int32 numberOfItems;

        private StringBuilder csvBuilder;

        private Int32[] jobIdArray;

        private String filePath;

        private String fileName;

        [SetUp]
        public void Setup()
        {
            this.logger = new Mock<ILogger>();
            this.jobService = new Mock<IJobService>();
            this.authoriseJobEngine = new Mock<IAuthoriseJobEngine>();
            this.userService = new Mock<IUserService>();
            this.oneStepFileService = new Mock<IOneStepFileService>();
            this.reportFileService = new Mock<IReportFileService>();
            this.inputFileService = new Mock<IInputFileService>();
            this.appEnvironmentService = new Mock<IAppEnvironmentService>();
            this.pdfFileEngine = new Mock<IPdfFileEngine>();
            this.enclosingJobService = new Mock<IEnclosingJobService>();
            this.jobStatusTypeService = new Mock<IJobStatusTypeService>();
            this.fileInfoFactory = new Mock<IFileInfoFactory>();
            this.jobsController = new JobsController(
            this.jobService.Object,
            this.authoriseJobEngine.Object,
            this.userService.Object,
            this.oneStepFileService.Object,
            this.reportFileService.Object,
            this.inputFileService.Object,
            this.appEnvironmentService.Object,
            this.pdfFileEngine.Object,
            this.logger.Object,
            this.enclosingJobService.Object,
            this.jobStatusTypeService.Object,
            this.fileInfoFactory.Object
            );

            this.numberOfItems = 1;
            this.pageNumber = 1;

            this.jobs = new List<JobEntity>();
            this.jobs.Add(new JobEntity()
                            {
                                Company = "Company",
                                Document = "dshsik",
                                DocTypeCode = "sks",
                                EnvType = "jsds",
                                EnvironmentId = 1,
                                DocumentTypeID = 3,
                                HoldAuthorisation = true,
                                JobId = 2,
                                ManCoCode = "dfhdf",
                                ManCoDocID = 2,
                                SubmitDateTime = DateTime.Now.ToString(),
                                Owner = "owner",
                                Status = "Completed",
                                Version = "2.0"
                            });

            this.pagedJobs = new PagedResult<JobEntity>
            {
                CurrentPage = this.pageNumber,
                ItemsPerPage = this.numberOfItems,
                TotalItems = this.jobs.Count(),
                Results = this.jobs.OrderBy(c => c.JobId)
                .Skip((this.pageNumber - 1) * this.numberOfItems)
                .Take(this.numberOfItems)
                .ToList(),
                StartRow = ((this.pageNumber - 1) * this.numberOfItems) + 1,
                EndRow = (((this.pageNumber - 1) * this.numberOfItems) + 1) + (this.numberOfItems - 1)
            };

            this.currentUser = new ApplicationUser("Username");
            this.csvBuilder = new StringBuilder();

            this.csvBuilder.Append("JOB REFERENCE");
            this.csvBuilder.Append(",");

            this.csvBuilder.Append("COMPANY");
            this.csvBuilder.Append(",");

            this.csvBuilder.Append("DOCUMENT");
            this.csvBuilder.Append(",");

            this.csvBuilder.Append("VERSION");
            this.csvBuilder.Append(",");

            this.csvBuilder.Append("OWNER");
            this.csvBuilder.Append(",");

            this.csvBuilder.Append("SUBMIT DATE/TIME");
            this.csvBuilder.Append(",");

            this.csvBuilder.Append("STATUS");
            this.csvBuilder.Append(",");

            this.csvBuilder.Append("\n");

            this.servers = new List<EnvironmentServerEntity>();
            this.servers.Add(new EnvironmentServerEntity() { ServerName = "Bravura Server" });
            this.SetControllerContext(this.jobsController);
            this.reportFiles = new List<ReportFile>();
            this.reportFiles.Add(new ReportFile("filename1", "filepath1"));
            this.reportFiles.Add(new ReportFile("filename2", "filepath2"));
            String[] fileContent = new String[1] { "filecontent" };
            this.reportFile = new ReportFile("filename", "filepath", fileContent);

            this.inputFiles = new List<InputFileEntity>();
            this.inputFiles.Add(new InputFileEntity() { FilePath = "file path 1" });
            this.inputFiles.Add(new InputFileEntity() { FilePath = "file path 2" });
            this.jobIdArray = new Int32[] { 2, 5, 8, 9 };

            this.appEnvironments = new List<AppEnvironment>();
            this.appEnvironments.Add(new AppEnvironment() { Name = "environment1" });
            this.appEnvironments.Add(new AppEnvironment() { Name = "environment2" });
            this.tempDataMock = new TempDataDictionary();

            this.filePath = "file path";

            this.fileName = "file name";

            this.pdfFiles = new List<PdfFile>();
            this.pdfFiles.Add(new PdfFile("fileName", "filePath"));

            this.enclosingJobs = new List<EnclosingJob>();
            this.enclosingJobs.Add(new EnclosingJob()
            {
                EnclosingJobID = 1,
                FBE_ID = 123,
                JobID = 2,
                MediaDefID = 1,
                Packs = 10,
                Pages = 12,
                Sheets = 13,
                Filename = "Test.PS",
                DigiQGRID = "Test GRID",
                PostalDocketNumber = "Test Docket Number"
            });
        }

        [Test]
        public void GivenAJobsController_WhenTheIndexPageIsAccessed_ThenARedirectToRouteResultIsReturned()
        {
            var result = this.jobsController.Index();
            result.Should().NotBeNull();
            result.Should().BeOfType<RedirectToRouteResult>();
        }

        [Test]
        public void GivenAJobsController_WhenTheIndexPageIsAccessed_ThenTheViewJobsShouldBeAccessed()
        {
            var result = (RedirectToRouteResult)this.jobsController.Index();
            result.Should().NotBeNull();
            result.RouteValues["Action"].ToString().ShouldBeEquivalentTo("View");
            result.RouteValues["Controller"].ToString().ShouldBeEquivalentTo("Jobs");
        }

        [Test]
        public void GivenAJobsController_WhenTheViewJobsPageIsAccessed_ThenTheViewJobsViewIsAccessed()
        {
            this.userService.Setup(x => x.GetApplicationUser(It.IsAny<String>())).Returns(this.currentUser);
            this.jobService.Setup(x => x.GetJobs(It.IsAny<Int32>(), It.IsAny<Int32>(), It.IsAny<String>(), It.IsAny<String>())).Returns(this.pagedJobs);
            this.userService.Setup(x => x.GetApplicationUser()).Returns(new ApplicationUser("user1") { PreferredEnvironment = "environment2" });
            this.appEnvironmentService.Setup(x => x.GetAppEnvironments(It.IsAny<String>())).Returns(this.appEnvironments);
            this.enclosingJobService.Setup(x => x.GetEnclosingJobsByJobId(It.IsAny<Int32>())).Returns(this.enclosingJobs);

            var result = this.jobsController.ViewJobs(It.IsAny<Int32>(), It.IsAny<Boolean>());

            result.Should().BeOfType<ViewResult>();
            this.userService.Verify(x => x.GetApplicationUser(It.IsAny<String>()), Times.AtLeastOnce);
            this.jobService.Verify(x => x.GetJobs(It.IsAny<Int32>(), It.IsAny<Int32>(), It.IsAny<String>(), It.IsAny<String>()), Times.AtLeastOnce);
            this.userService.Verify(x => x.GetApplicationUser(), Times.AtLeastOnce);
            this.appEnvironmentService.Verify(x => x.GetAppEnvironments(It.IsAny<String>()), Times.AtLeastOnce);
            this.enclosingJobService.Verify(x => x.GetEnclosingJobsByJobId(It.IsAny<Int32>()), Times.AtLeastOnce);
        }

        [Test]
        public void GivenAJobsController_WhenTheViewJobsPageIsAccessed_ThenTheViewJobsViewShouldContainTheViewJobsModel()
        {
            this.userService.Setup(x => x.GetApplicationUser(It.IsAny<String>())).Returns(this.currentUser);
            this.jobService.Setup(x => x.GetJobs(It.IsAny<Int32>(), It.IsAny<Int32>(), It.IsAny<String>(), It.IsAny<String>())).Returns(this.pagedJobs);
            this.userService.Setup(x => x.GetApplicationUser()).Returns(new ApplicationUser("user1") { PreferredEnvironment = "environment2" });
            this.appEnvironmentService.Setup(x => x.GetAppEnvironments(It.IsAny<String>())).Returns(this.appEnvironments);
            this.enclosingJobService.Setup(x => x.GetEnclosingJobsByJobId(It.IsAny<Int32>())).Returns(this.enclosingJobs);

            var result = (ViewResult)this.jobsController.ViewJobs(It.IsAny<Int32>(), It.IsAny<Boolean>());
            result.Model.Should().BeOfType<ViewJobsViewModel>();

            result.Model.As<ViewJobsViewModel>().CanExportResults.Should().BeFalse();
            this.userService.Verify(x => x.GetApplicationUser(It.IsAny<String>()), Times.AtLeastOnce);
            this.jobService.Verify(x => x.GetJobs(It.IsAny<Int32>(), It.IsAny<Int32>(), It.IsAny<String>(), It.IsAny<String>()), Times.AtLeastOnce);
            this.userService.Verify(x => x.GetApplicationUser(), Times.AtLeastOnce);
            this.appEnvironmentService.Verify(x => x.GetAppEnvironments(It.IsAny<String>()), Times.AtLeastOnce);
            this.enclosingJobService.Verify(x => x.GetEnclosingJobsByJobId(It.IsAny<Int32>()), Times.AtLeastOnce);
        }


        [Test]
        public void GivenAJobsController_WhenTheViewJobsPageIsAccessed_ThenTheViewJobsViewShouldHaveTheExportFileLinkEnabled()
        {
            this.currentUser.Roles.Add(new IdentityUserRole() { UserId = this.currentUser.Id, Role = new IdentityRole(Role.ExportSearchResults) });
            this.userService.Setup(x => x.GetApplicationUser(It.IsAny<String>())).Returns(this.currentUser);
            this.jobService.Setup(x => x.GetJobs(It.IsAny<Int32>(), It.IsAny<Int32>(), It.IsAny<String>(), It.IsAny<String>())).Returns(this.pagedJobs);
            this.userService.Setup(x => x.GetApplicationUser()).Returns(new ApplicationUser("user1") { PreferredEnvironment = "environment2" });
            this.appEnvironmentService.Setup(x => x.GetAppEnvironments(It.IsAny<String>())).Returns(this.appEnvironments);
            this.enclosingJobService.Setup(x => x.GetEnclosingJobsByJobId(It.IsAny<Int32>())).Returns(this.enclosingJobs);

            var result = (ViewResult)this.jobsController.ViewJobs(It.IsAny<Int32>(), It.IsAny<Boolean>());
            result.Model.Should().BeOfType<ViewJobsViewModel>();

            result.Model.As<ViewJobsViewModel>().CanExportResults.Should().BeTrue();
            this.userService.Verify(x => x.GetApplicationUser(It.IsAny<String>()), Times.AtLeastOnce);
            this.jobService.Verify(x => x.GetJobs(It.IsAny<Int32>(), It.IsAny<Int32>(), It.IsAny<String>(), It.IsAny<String>()), Times.AtLeastOnce);
            this.userService.Verify(x => x.GetApplicationUser(), Times.AtLeastOnce);
            this.appEnvironmentService.Verify(x => x.GetAppEnvironments(It.IsAny<String>()), Times.AtLeastOnce);
            this.enclosingJobService.Verify(x => x.GetEnclosingJobsByJobId(It.IsAny<Int32>()), Times.AtLeastOnce);
        }

        [Test]
        public void GivenAJobsController_WhenTheSearchJobsActionIsCalledWithNoSearchCriteria_ThenTheJobListPartialViewShouldContainViewJobsViewModel()
        {
            this.userService.Setup(x => x.GetApplicationUser(It.IsAny<String>())).Returns(this.currentUser);
            this.jobService.Setup(x => x.GetJobs(It.IsAny<Int32>(), It.IsAny<Int32>(), It.IsAny<String>(), It.IsAny<String>())).Returns(this.pagedJobs);
            this.userService.Setup(x => x.GetApplicationUser()).Returns(new ApplicationUser("user1") { PreferredEnvironment = "environment2" });
            this.appEnvironmentService.Setup(x => x.GetAppEnvironments(It.IsAny<String>())).Returns(this.appEnvironments);
            this.enclosingJobService.Setup(x => x.GetEnclosingJobsByJobId(It.IsAny<Int32>())).Returns(this.enclosingJobs);

            var result = (PartialViewResult)this.jobsController.SearchJobs(String.Empty);

            result.Model.Should().BeOfType<ViewJobsViewModel>();

            this.userService.Verify(x => x.GetApplicationUser(It.IsAny<String>()), Times.AtLeastOnce);
            this.jobService.Verify(x => x.GetJobs(It.IsAny<Int32>(), It.IsAny<Int32>(), It.IsAny<String>(), It.IsAny<String>()), Times.AtLeastOnce);
            this.userService.Verify(x => x.GetApplicationUser(), Times.AtLeastOnce);
            this.appEnvironmentService.Verify(x => x.GetAppEnvironments(It.IsAny<String>()), Times.AtLeastOnce);
            this.enclosingJobService.Verify(x => x.GetEnclosingJobsByJobId(It.IsAny<Int32>()), Times.AtLeastOnce);
        }

        [Test]
        public void GivenAJobsController_WhenTheSearchJobsActionIsCalledWithSearchCriteria_ThenTheJobListPartialViewShouldContainViewJobsViewModel()
        {
            this.userService.Setup(x => x.GetApplicationUser(It.IsAny<String>())).Returns(this.currentUser);
            this.jobService.Setup(x => x.GetJobs(It.IsAny<Int32>(), It.IsAny<Int32>(), It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>())).Returns(this.pagedJobs);
            this.userService.Setup(x => x.GetApplicationUser()).Returns(new ApplicationUser("user1") { PreferredEnvironment = "environment2" });
            this.appEnvironmentService.Setup(x => x.GetAppEnvironments(It.IsAny<String>())).Returns(this.appEnvironments);
            this.enclosingJobService.Setup(x => x.GetEnclosingJobsByJobId(It.IsAny<Int32>())).Returns(this.enclosingJobs);

            var result = (PartialViewResult)this.jobsController.SearchJobs("Search Criteria");

            result.Model.Should().BeOfType<ViewJobsViewModel>();

            this.userService.Verify(x => x.GetApplicationUser(It.IsAny<String>()), Times.AtLeastOnce);
            this.jobService.Verify(x => x.GetJobs(It.IsAny<Int32>(), It.IsAny<Int32>(), It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>()), Times.AtLeastOnce);
            this.userService.Verify(x => x.GetApplicationUser(), Times.AtLeastOnce);
            this.appEnvironmentService.Verify(x => x.GetAppEnvironments(It.IsAny<String>()), Times.AtLeastOnce);
            this.enclosingJobService.Verify(x => x.GetEnclosingJobsByJobId(It.IsAny<Int32>()), Times.AtLeastOnce);
        }

        [Test]
        public void GivenAJobsController_WhenTheSearchJobsActionIsCalledWithNoSearchCriteria_ThenTheJobListPartialViewIsReturned()
        {
            this.userService.Setup(x => x.GetApplicationUser(It.IsAny<String>())).Returns(this.currentUser);
            this.jobService.Setup(x => x.GetJobs(It.IsAny<Int32>(), It.IsAny<Int32>(), It.IsAny<String>(), It.IsAny<String>())).Returns(this.pagedJobs);

            this.userService.Setup(x => x.GetApplicationUser()).Returns(new ApplicationUser("user1") { PreferredEnvironment = "environment2" });
            this.appEnvironmentService.Setup(x => x.GetAppEnvironments(It.IsAny<String>())).Returns(this.appEnvironments);
            this.enclosingJobService.Setup(x => x.GetEnclosingJobsByJobId(It.IsAny<Int32>())).Returns(this.enclosingJobs);

            var result = this.jobsController.SearchJobs(String.Empty);

            result.Should().BeOfType<PartialViewResult>();

            this.userService.Verify(x => x.GetApplicationUser(It.IsAny<String>()), Times.AtLeastOnce);
            this.jobService.Verify(x => x.GetJobs(It.IsAny<Int32>(), It.IsAny<Int32>(), It.IsAny<String>(), It.IsAny<String>()), Times.AtLeastOnce);
            this.userService.Verify(x => x.GetApplicationUser(), Times.AtLeastOnce);
            this.appEnvironmentService.Verify(x => x.GetAppEnvironments(It.IsAny<String>()), Times.AtLeastOnce);
            this.enclosingJobService.Verify(x => x.GetEnclosingJobsByJobId(It.IsAny<Int32>()), Times.AtLeastOnce);
        }

        [Test]
        public void GivenAJobsController_WhenTheSearchJobsActionIsCalledWithSearchCriteria_ThenTheJobListPartialViewIsReturned()
        {
            this.userService.Setup(x => x.GetApplicationUser(It.IsAny<String>())).Returns(this.currentUser);
            this.jobService.Setup(x => x.GetJobs(It.IsAny<Int32>(), It.IsAny<Int32>(), It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>())).Returns(this.pagedJobs);
            this.userService.Setup(x => x.GetApplicationUser()).Returns(new ApplicationUser("user1") { PreferredEnvironment = "environment2" });
            this.appEnvironmentService.Setup(x => x.GetAppEnvironments(It.IsAny<String>())).Returns(this.appEnvironments);
            this.enclosingJobService.Setup(x => x.GetEnclosingJobsByJobId(It.IsAny<Int32>())).Returns(this.enclosingJobs);

            var result = this.jobsController.SearchJobs("Search Criteria");

            result.Should().BeOfType<PartialViewResult>();

            this.userService.Verify(x => x.GetApplicationUser(It.IsAny<String>()), Times.AtLeastOnce);
            this.jobService.Verify(x => x.GetJobs(It.IsAny<Int32>(), It.IsAny<Int32>(), It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>()), Times.AtLeastOnce);
            this.enclosingJobService.Verify(x => x.GetEnclosingJobsByJobId(It.IsAny<Int32>()), Times.AtLeastOnce);
        }

        [Test]
        public void GivenAjobController_WhenIWantToGetExcelReport_ThenAFileResultIsReturned()
        {
            this.userService.Setup(x => x.GetApplicationUser(It.IsAny<String>())).Returns(this.currentUser);
            this.jobService.Setup(x => x.GetJobsReport("Search Criteria", It.IsAny<String>(), It.IsAny<String>())).Returns(this.csvBuilder);

            var result = this.jobsController.GetExcelReport("Search Criteria");

            result.Should().NotBeNull();

            this.userService.Verify(x => x.GetApplicationUser(It.IsAny<String>()), Times.AtLeastOnce);
            this.jobService.Verify(x => x.GetJobsReport("Search Criteria", It.IsAny<String>(), It.IsAny<String>()), Times.AtLeastOnce);
        }

        [Test]
        public void GivenAJobController_WhenIWanToLoadTheOneStepLog_APartialViewIsReturned()
        {
            String[] content = new String[2] { "first line", "second line" };
            this.oneStepFileService.Setup(x => x.GetOneStepLog(It.IsAny<String>(), It.IsAny<String>()))
                .Returns(new OneStepLog() { LogContent = content, LogFileName = "filename" });

            var result = this.jobsController.GetOneStepLog(It.IsAny<String>(), It.IsAny<String>());

            result.Should().NotBeNull();
            result.Should().BeOfType<PartialViewResult>();

            this.oneStepFileService.Verify(x => x.GetOneStepLog(It.IsAny<String>(), It.IsAny<String>()), Times.AtLeastOnce);
        }

        [Test]
        public void GivenAJobController_WhenIWantToGetOneStepExcelReport_ThenAFileFileResultIsReturned()
        {
            foreach (String extension in new[] { String.Empty, ".xlsx" })
            {
                String closureExtension = extension;

                this.fileInfoFactory
                    .Setup(x => x.CreateFileInfo(It.IsAny<String>()))
                    .Returns(Mock.Of<IFileInfo>(info => info.Exists && info.Extension == closureExtension));

                var result = this.jobsController.GetOneStepExcelReport(this.fileName, this.filePath);

                result.Should().NotBeNull();
            }
        }

        [Test]
        public void GivenAJobController_WhenIWanToLoadTheOneStepLog_ItShouldContainOneStepLogViewModel()
        {
            String[] content = new String[2] { "first line", "second line" };
            this.oneStepFileService.Setup(x => x.GetOneStepLog(It.IsAny<String>(), It.IsAny<String>()))
                .Returns(new OneStepLog() { LogContent = content, LogFileName = "filename" });

            var result = (PartialViewResult)this.jobsController.GetOneStepLog(It.IsAny<String>(), It.IsAny<String>());

            result.Should().NotBeNull();
            result.Model.Should().BeOfType<OneStepLogViewModel>();
            result.ViewName.ShouldBeEquivalentTo("_OneStepLog");
            this.oneStepFileService.Verify(x => x.GetOneStepLog(It.IsAny<String>(), It.IsAny<String>()), Times.AtLeastOnce);
        }

        [Test]
        [ExpectedException(typeof(DocProcessingException), ExpectedMessage = "Environment session is null or empty")]
        public void GiveAJobController_WhenIWantTheOneStepLogAndEnvironmentValueIsEmpty_ThenAnExceptionIsThrown()
        {
            MockHttpContext.Setup(m => m.Session["Environment"]).Returns(String.Empty);

            this.jobsController.GetOneStepLog(It.IsAny<String>(), It.IsAny<String>());
        }

        [Test]
        public void GivenAJobController_WhenIWantTheJobReportList_APartialViewIsReturned()
        {
            this.reportFileService.Setup(x => x.GetReportFiles(It.IsAny<String>(), It.IsAny<String>())).Returns(this.reportFiles);

            var result = this.jobsController.GetJobReportList(It.IsAny<String>(), It.IsAny<String>());

            result.Should().NotBeNull();
            result.Should().BeOfType<PartialViewResult>();
            this.reportFileService.Verify(x => x.GetReportFiles(It.IsAny<String>(), It.IsAny<String>()), Times.AtLeastOnce);
        }

        [Test]
        public void GivenAJobController_WhenIWantTheJobReportList_ItShouldContainTheJobReportsViewModel()
        {
            this.reportFileService.Setup(x => x.GetReportFiles(It.IsAny<String>(), It.IsAny<String>())).Returns(this.reportFiles);

            var result = (PartialViewResult)this.jobsController.GetJobReportList(It.IsAny<String>(), It.IsAny<String>());

            result.Should().NotBeNull();
            result.Model.Should().BeOfType<JobReportsViewModel>();
            result.ViewName.ShouldBeEquivalentTo("_JobsReportList");
            this.reportFileService.Verify(x => x.GetReportFiles(It.IsAny<String>(), It.IsAny<String>()), Times.AtLeastOnce);
        }

        [Test]
        [ExpectedException(typeof(DocProcessingException), ExpectedMessage = "Environment session is null or empty")]
        public void GiveAJobController_WhenIWantTheJobReportListAndEnvironmentValueIsEmpty_ThenAnExceptionIsThrown()
        {
            MockHttpContext.Setup(m => m.Session["Environment"]).Returns(String.Empty);

            this.jobsController.GetJobReportList(It.IsAny<String>(), It.IsAny<String>());
        }

        [Test]
        public void GivenAJobController_WhenIWantAOneStepReport_APartialViewIsReturned()
        {
            this.reportFileService.Setup(x => x.GetReportFile(It.IsAny<String>())).Returns(this.reportFile);

            var result = this.jobsController.GetOneStepReport(It.IsAny<String>());

            result.Should().NotBeNull();
            result.Should().BeOfType<PartialViewResult>();
            this.reportFileService.Verify(x => x.GetReportFile(It.IsAny<String>()), Times.AtLeastOnce);
        }

        [Test]
        public void GivenAJobController_WhenIWantAOneStepReport_ItShouldContainTheJobReportViewModel()
        {
            this.reportFileService.Setup(x => x.GetReportFile(It.IsAny<String>())).Returns(this.reportFile);

            var result = (PartialViewResult)this.jobsController.GetOneStepReport(It.IsAny<String>());

            result.Should().NotBeNull();
            result.Model.Should().BeOfType<JobReportViewModel>();
            result.ViewName.ShouldBeEquivalentTo("_JobReport");
            this.reportFileService.Verify(x => x.GetReportFile(It.IsAny<String>()), Times.AtLeastOnce);
        }

        [Test]
        public void GivenAJobController_WhenIWantJobInputFiles_APartialViewIsReturned()
        {
            this.inputFileService.Setup(x => x.GetInputFilesByJobId(It.IsAny<Int32>())).Returns(this.inputFiles);

            var result = this.jobsController.GetJobFiles(It.IsAny<String>());

            result.Should().NotBeNull();
            result.Should().BeOfType<PartialViewResult>();
            this.inputFileService.Verify(x => x.GetInputFilesByJobId(It.IsAny<Int32>()), Times.AtLeastOnce);
        }

        [Test]
        public void GivenAJobController_WhenIWantJobInputFiles_ItShouldContainTheJobReportViewModel()
        {
            this.inputFileService.Setup(x => x.GetInputFilesByJobId(It.IsAny<Int32>())).Returns(this.inputFiles);

            var result = (PartialViewResult)this.jobsController.GetJobFiles(It.IsAny<String>());

            result.Should().NotBeNull();
            result.Model.Should().BeOfType<JobFilesViewModel>();
            result.ViewName.ShouldBeEquivalentTo("_JobFiles");
            this.inputFileService.Verify(x => x.GetInputFilesByJobId(It.IsAny<Int32>()), Times.AtLeastOnce);
        }

        [Test]
        public void GivenAJobController_WhenIWantToAuthoriseJobs_APartialViewIsReturned()
        {
            this.authoriseJobEngine.Setup(
            x => x.AuthoriseJob(It.IsAny<List<Int32>>(), It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>()));
            this.jobService.Setup(x => x.GetJobs(It.IsAny<Int32>(), It.IsAny<Int32>(), It.IsAny<String>(), It.IsAny<String>())).Returns(this.pagedJobs);
            this.userService.Setup(c => c.GetApplicationUser(It.IsAny<String>())).Returns(new ApplicationUser("username"));
            this.enclosingJobService.Setup(x => x.GetEnclosingJobsByJobId(It.IsAny<Int32>())).Returns(this.enclosingJobs);

            var result = this.jobsController.Authorise(this.jobIdArray);

            result.Should().NotBeNull();
            result.Should().BeOfType<PartialViewResult>();
            this.authoriseJobEngine.Verify(
                     x => x.AuthoriseJob(It.IsAny<List<Int32>>(), It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>()), Times.AtLeastOnce);
            this.jobService.Verify(x => x.GetJobs(It.IsAny<Int32>(), It.IsAny<Int32>(), It.IsAny<String>(), It.IsAny<String>()), Times.AtLeastOnce);
            this.userService.Verify(c => c.GetApplicationUser(It.IsAny<String>()), Times.AtLeastOnce);
            this.enclosingJobService.Verify(x => x.GetEnclosingJobsByJobId(It.IsAny<Int32>()), Times.AtLeastOnce);
        }

        [Test]
        public void GivenAJobController_WhenIWantToAuthoriseJobs_ItShouldContainTheViewJobsViewModel()
        {
            this.authoriseJobEngine.Setup(
              x => x.AuthoriseJob(It.IsAny<List<Int32>>(), It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>()));
            this.jobService.Setup(x => x.GetJobs(It.IsAny<Int32>(), It.IsAny<Int32>(), It.IsAny<String>(), It.IsAny<String>())).Returns(this.pagedJobs);
            this.userService.Setup(c => c.GetApplicationUser(It.IsAny<String>())).Returns(new ApplicationUser("username"));
            this.enclosingJobService.Setup(x => x.GetEnclosingJobsByJobId(It.IsAny<Int32>())).Returns(this.enclosingJobs);

            var result = (PartialViewResult)this.jobsController.Authorise(this.jobIdArray);

            result.Should().NotBeNull();
            result.Model.Should().BeOfType<ViewJobsViewModel>();
            result.ViewName.ShouldBeEquivalentTo("_JobsList");
            this.authoriseJobEngine.Verify(
                    x => x.AuthoriseJob(It.IsAny<List<Int32>>(), It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>()), Times.AtLeastOnce);
            this.jobService.Verify(x => x.GetJobs(It.IsAny<Int32>(), It.IsAny<Int32>(), It.IsAny<String>(), It.IsAny<String>()), Times.AtLeastOnce);
            this.userService.Verify(c => c.GetApplicationUser(It.IsAny<String>()), Times.AtLeastOnce);
            this.enclosingJobService.Verify(x => x.GetEnclosingJobsByJobId(It.IsAny<Int32>()), Times.AtLeastOnce);
        }

        [Test]
        public void GivenAJobsController_WhenIWantToReloadTheJobsListThorughViewJobsAjaxWithASearchCriteriaValue_ThePartialViewResultIsReturned()
        {
            this.userService.Setup(x => x.GetApplicationUser(It.IsAny<String>())).Returns(new ApplicationUser("username"));
            this.jobService.Setup(x => x.GetJobs(It.IsAny<Int32>(), It.IsAny<Int32>(), It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>())).Returns(this.pagedJobs);
            this.userService.Setup(x => x.GetApplicationUser()).Returns(new ApplicationUser("user1") { PreferredEnvironment = "environment2" });
            this.enclosingJobService.Setup(x => x.GetEnclosingJobsByJobId(It.IsAny<Int32>())).Returns(this.enclosingJobs);
            this.appEnvironmentService.Setup(x => x.GetAppEnvironments(It.IsAny<String>())).Returns(this.appEnvironments);
            this.tempDataMock.Add("SearchField", "searchstring");
            this.jobsController.TempData = this.tempDataMock;

            var result = (PartialViewResult)this.jobsController.ViewJobsAjax();

            result.Should().BeOfType<PartialViewResult>();
            this.userService.Verify(x => x.GetApplicationUser(It.IsAny<String>()), Times.AtLeastOnce);
            this.jobService.Verify(x => x.GetJobs(It.IsAny<Int32>(), It.IsAny<Int32>(), It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>()), Times.AtLeastOnce);
            this.userService.Verify(x => x.GetApplicationUser(), Times.AtLeastOnce);
            this.appEnvironmentService.Verify(x => x.GetAppEnvironments(It.IsAny<String>()), Times.AtLeastOnce);
            this.enclosingJobService.Verify(x => x.GetEnclosingJobsByJobId(It.IsAny<Int32>()), Times.AtLeastOnce);
        }

        [Test]
        public void GivenAJobsController_WhenIWantToReloadTheJobsListThorughViewJobsAjaxWithASearchCriteriaValueAndSessionEnvironmentNull_ThePartialViewResultIsReturned()
        {
            this.userService.Setup(x => x.GetApplicationUser(It.IsAny<String>())).Returns(new ApplicationUser("username"));
            this.jobService.Setup(x => x.GetJobs(It.IsAny<Int32>(), It.IsAny<Int32>(), It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>())).Returns(this.pagedJobs);
            this.userService.Setup(x => x.GetApplicationUser()).Returns(new ApplicationUser("user1") { PreferredEnvironment = "environment2" });
            this.enclosingJobService.Setup(x => x.GetEnclosingJobsByJobId(It.IsAny<Int32>())).Returns(this.enclosingJobs);
            this.appEnvironmentService.Setup(x => x.GetAppEnvironments(It.IsAny<String>())).Returns(this.appEnvironments);
            this.tempDataMock.Add("SearchField", "searchstring");
            this.jobsController.TempData = this.tempDataMock;
            //this.MockHttpContext.Setup(m => m.Session["Environment"]).Returns(null);

            var result = (PartialViewResult)this.jobsController.ViewJobsAjax();

            result.Should().BeOfType<PartialViewResult>();
            this.userService.Verify(x => x.GetApplicationUser(It.IsAny<String>()), Times.AtLeastOnce);
            this.jobService.Verify(x => x.GetJobs(It.IsAny<Int32>(), It.IsAny<Int32>(), It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>()), Times.AtLeastOnce);
            this.userService.Verify(x => x.GetApplicationUser(), Times.AtLeastOnce);
            this.appEnvironmentService.Verify(x => x.GetAppEnvironments(It.IsAny<String>()), Times.AtLeastOnce);
            this.enclosingJobService.Verify(x => x.GetEnclosingJobsByJobId(It.IsAny<Int32>()), Times.AtLeastOnce);
        }

        [Test]
        public void GivenAJobsController_WhenIWantToReloadTheJobsListThorughViewJobsAjaxWithoutASearchCriteriaValue_ThePartialViewResultIsReturned()
        {
            this.userService.Setup(x => x.GetApplicationUser(It.IsAny<String>())).Returns(new ApplicationUser("username"));
            this.jobService.Setup(x => x.GetJobs(It.IsAny<Int32>(), It.IsAny<Int32>(), It.IsAny<String>(), It.IsAny<String>())).Returns(this.pagedJobs);
            this.userService.Setup(x => x.GetApplicationUser()).Returns(new ApplicationUser("user1") { PreferredEnvironment = "environment2" });
            this.enclosingJobService.Setup(x => x.GetEnclosingJobsByJobId(It.IsAny<Int32>())).Returns(this.enclosingJobs);
            this.appEnvironmentService.Setup(x => x.GetAppEnvironments(It.IsAny<String>())).Returns(this.appEnvironments);
            this.tempDataMock.Add("SearchField", String.Empty);
            this.jobsController.TempData = this.tempDataMock;

            var result = (PartialViewResult)this.jobsController.ViewJobsAjax();

            result.Should().BeOfType<PartialViewResult>();
            this.userService.Verify(x => x.GetApplicationUser(It.IsAny<String>()), Times.AtLeastOnce);
            this.jobService.Verify(x => x.GetJobs(It.IsAny<Int32>(), It.IsAny<Int32>(), It.IsAny<String>(), It.IsAny<String>()), Times.AtLeastOnce);
            this.userService.Verify(x => x.GetApplicationUser(), Times.AtLeastOnce);
            this.appEnvironmentService.Verify(x => x.GetAppEnvironments(It.IsAny<String>()), Times.AtLeastOnce);
            this.enclosingJobService.Verify(x => x.GetEnclosingJobsByJobId(It.IsAny<Int32>()), Times.AtLeastOnce);
        }

        [Test]
        public void GivenAJobsController_WhenIWantToReloadTheJobsListThorughViewJobsAjaxWithoutASearchCriteriaValueAndSessionEnvironmentNull_ThePartialViewResultIsReturned()
        {
            this.userService.Setup(x => x.GetApplicationUser(It.IsAny<String>())).Returns(new ApplicationUser("username"));
            this.jobService.Setup(x => x.GetJobs(It.IsAny<Int32>(), It.IsAny<Int32>(), It.IsAny<String>(), It.IsAny<String>())).Returns(this.pagedJobs);
            this.userService.Setup(x => x.GetApplicationUser()).Returns(new ApplicationUser("user1") { PreferredEnvironment = "environment2" });
            this.enclosingJobService.Setup(x => x.GetEnclosingJobsByJobId(It.IsAny<Int32>())).Returns(this.enclosingJobs);
            this.appEnvironmentService.Setup(x => x.GetAppEnvironments(It.IsAny<String>())).Returns(this.appEnvironments);
            this.tempDataMock.Add("SearchField", String.Empty);
            this.jobsController.TempData = this.tempDataMock;
            this.MockHttpContext.Setup(m => m.Session["Environment"]).Returns(null);

            var result = (PartialViewResult)this.jobsController.ViewJobsAjax();

            result.Should().BeOfType<PartialViewResult>();
            this.userService.Verify(x => x.GetApplicationUser(It.IsAny<String>()), Times.AtLeastOnce);
            this.jobService.Verify(x => x.GetJobs(It.IsAny<Int32>(), It.IsAny<Int32>(), It.IsAny<String>(), It.IsAny<String>()), Times.AtLeastOnce);
            this.userService.Verify(x => x.GetApplicationUser(), Times.AtLeastOnce);
            this.appEnvironmentService.Verify(x => x.GetAppEnvironments(It.IsAny<String>()), Times.AtLeastOnce);
            this.enclosingJobService.Verify(x => x.GetEnclosingJobsByJobId(It.IsAny<Int32>()), Times.AtLeastOnce);
        }

        [Test]
        public void GivenAJobController_WhenIWantToAssignASearchValueToTempData_TempDataContainsTheValue()
        {
            var result = this.jobsController.AssignSearchValue(It.IsAny<String>());

            result.Should().NotBeNull();
            result.Should().BeOfType<JsonResult>();

            this.jobsController.TempData.ContainsKey("SearchField").ShouldBeEquivalentTo(true);
        }

        [Test]
        public void GivenAJobController_WhenIwantToGetPdfIconIfJobHasPdf_ThenAPartialViewResultIsReturned()
        {
            this.pdfFileEngine.Setup(x => x.GetPdfFiles(It.IsAny<String>(), It.IsAny<String>())).Returns(this.pdfFiles);

            var result = this.jobsController.GetIfJobHasPdf(It.IsAny<String>(), It.IsAny<String>());

            result.Should().NotBeNull();
            result.Should().BeOfType<PartialViewResult>();

            var partialViewResult = result as PartialViewResult;

            partialViewResult.Model.Should().BeOfType<JobPdfsViewModel>();
            partialViewResult.ViewName.Should().Be("_JobPdfList");

            this.pdfFileEngine.Verify(x => x.GetPdfFiles(It.IsAny<String>(), It.IsAny<String>()), Times.AtLeastOnce);
        }

        [Test]
        public void GivenAJobsController_WhenIWantToGetAPdf_AFileStreamResultIsReturned()
        {
            string ResourcePath = Path.GetDirectoryName(Path.GetDirectoryName(Directory.GetCurrentDirectory()));

            var result = this.jobsController.GetPdf(ResourcePath + @"/Resources/Dist.pdf", "fileName");

            result.Should().NotBeNull();

            result.Should().BeOfType<FileStreamResult>();
        }

        [Test]
        public void GivenAJObController_WhenIWantToUpdateDocketNumber_ThenIShouldGetJsonResult()
        {
            this.userService.Setup(x => x.GetApplicationUser(It.IsAny<String>())).Returns(this.currentUser);

            this.jobService.Setup(x => x.GetJobById(It.IsAny<Int32>(), It.IsAny<String>()))
                .Returns(new JobEntity() { Status = JobStatusTypes.Processing });

            this.enclosingJobService.Setup(x => x.UpdateEnclosingJobDocketNumber(It.IsAny<Int32>(), It.IsAny<String>()));
            this.enclosingJobService.Setup(x => x.GetEnclosingJobsByJobId(It.IsAny<Int32>())).Returns(new List<EnclosingJob>());

            var result = this.jobsController.UpdateDocketNumber(It.IsAny<Int32>(), It.IsAny<Int32>(), It.IsAny<String>());

            result.Should().NotBeNull();
            result.Should().BeOfType<JsonResult>();

            this.enclosingJobService.Verify(x => x.UpdateEnclosingJobDocketNumber(It.IsAny<Int32>(), It.IsAny<String>()), Times.AtLeastOnce);
            this.enclosingJobService.Verify(x => x.GetEnclosingJobsByJobId(It.IsAny<Int32>()), Times.AtLeastOnce);
        }

        [Test]
        public void GivenAjObController_WhenIGiveInValidDocketNumber_IShouldGetErrorMessage()
        {
            var result = this.jobsController.UpdateDocketNumber(It.IsAny<Int32>(), It.IsAny<Int32>(), "1234");
            result.Should().NotBeNull();
            result.Should().BeOfType<JsonResult>();
            Assert.AreEqual(@"{ Success = False, Error = Must be of 6-25 characters and should be aplhanumeric. }", ((JsonResult)result).Data.ToString());
        }

        [Test]
        public void GivenAJobController_WhenIUpdateDocketNumber_AndAllTheDocketNumbersAreUpdated_IShouldGetJobStatusDispatched()
        {
            this.userService.Setup(x => x.GetApplicationUser(It.IsAny<String>())).Returns(this.currentUser);

            this.jobService.Setup(x => x.GetJobById(It.IsAny<Int32>(), It.IsAny<String>()))
                .Returns(new JobEntity() { Status = JobStatusTypes.Processing });

            this.enclosingJobService.Setup(x => x.UpdateEnclosingJobDocketNumber(It.IsAny<Int32>(), It.IsAny<String>()));
            this.enclosingJobService.Setup(x => x.GetEnclosingJobsByJobId(It.IsAny<Int32>())).Returns(this.enclosingJobs);
            this.jobStatusTypeService.Setup(x => x.GetJobStatusType(It.IsAny<String>())).Returns(new JobStatusTypeEntity());
            this.jobService.Setup(x => x.UpdateJobStatus(It.IsAny<Int32>(), It.IsAny<Int32>()));

            var result = this.jobsController.UpdateDocketNumber(It.IsAny<Int32>(), It.IsAny<Int32>(), It.IsAny<String>());

            result.Should().NotBeNull();
            result.Should().BeOfType<JsonResult>();
            Assert.AreEqual(@"{ Success = True, Status = Dispatched }", ((JsonResult)result).Data.ToString());

            this.enclosingJobService.Verify(x => x.UpdateEnclosingJobDocketNumber(It.IsAny<Int32>(), It.IsAny<String>()), Times.AtLeastOnce);
            this.enclosingJobService.Verify(x => x.GetEnclosingJobsByJobId(It.IsAny<Int32>()), Times.AtLeastOnce);
            this.jobStatusTypeService.Verify(x => x.GetJobStatusType(It.IsAny<String>()), Times.AtLeastOnce);
            this.jobService.Verify(x => x.UpdateJobStatus(It.IsAny<Int32>(), It.IsAny<Int32>()), Times.AtLeastOnce);
        }

        [Test]
        public void GivenAJobControllerAndValidData_WhenICallExportJobToExcel_IShouldGetACsvFileResult()
        {
            this.userService.Setup(x => x.GetApplicationUser(It.IsAny<String>())).Returns(new ApplicationUser("testUser"));
            this.jobService.Setup(x => x.GetJobReportByJobId(It.IsAny<Int32>(), It.IsAny<String>()))
                .Returns(new StringBuilder());

            var result = this.jobsController.ExportJobToExcel(It.IsAny<Int32>());

            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(FileContentResult));

            this.userService.Verify(x => x.GetApplicationUser(It.IsAny<String>()), Times.AtLeastOnce);
            this.jobService.Verify(x => x.GetJobReportByJobId(It.IsAny<Int32>(), It.IsAny<String>()), Times.AtLeastOnce);
        }

        [Test]
        public void GivenAJobController_WhenICancelAJobMissingDocketNumbers_IShouldGetACancelledJsonResult()
        {
            foreach (String missingDocketNumber in new[] { String.Empty, null, "  " })
            {
                this.enclosingJobService
                    .Setup(service => service.GetEnclosingJobsByJobId(It.IsAny<Int32>()))
                    .Returns(new List<EnclosingJob>
                             {
                                 new EnclosingJob() { EnclosingJobID = 10, PostalDocketNumber = missingDocketNumber },
                                 new EnclosingJob() { EnclosingJobID = 20, PostalDocketNumber = "test" }
                             });

                this.jobStatusTypeService.Setup(service => service.GetJobStatusType(It.IsAny<String>()))
                    .Returns(
                        new JobStatusTypeEntity()
                            {
                                JobStatusTypeID = 1,
                                JobStatusDescription = JobStatusTypes.Cancelled
                            });

                this.jobService.Setup(service => service.UpdateJobStatus(It.IsAny<Int32>(), It.IsAny<Int32>()));

                JsonResult result = this.jobsController.CancelJob(1);

                result.Should().NotBeNull();

                result.Data.Should().NotBeNull();

                result.Data.ToString().Should().Be("{ Status = Cancelled }");
            }
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException), ExpectedMessage = "Cannot cancel job as all docket numbers are present")]
        public void GivenAJobController_WhenICancelAJobWithoutMissingDocketNumbers_ThenAnExceptionIsThrown()
        {
            this.enclosingJobService
                .Setup(service => service.GetEnclosingJobsByJobId(It.IsAny<Int32>()))
                .Returns(new List<EnclosingJob>
                             {
                                 new EnclosingJob() { EnclosingJobID = 10, PostalDocketNumber = "test 1" },
                                 new EnclosingJob() { EnclosingJobID = 20, PostalDocketNumber = "test 2" }
                             });

            this.jobsController.CancelJob(1);
        }
    }
}
