// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnvironmentControllerTests.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   Tests for environment controller
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace WebTests.Controllers
{
  using System;
  using System.Collections.Generic;
  using System.Web.Mvc;
  using System.Web.Routing;
  using DocProcessingWorkflow.Controllers;
  using DocProcessingWorkflow.Models.SubmitJobs;
  using Entities;
  using Entities.File;
  using Exceptions;
  using FluentAssertions;
  using Logging;
  using Moq;
  using NUnit.Framework;
  using ServiceInterfaces;

  [Category("Unit")]
  [TestFixture]
  public class SubmitJobsControllerTests : ControllerTestsBase
  {
    private Mock<ILogger> logger;
    private Mock<IManCoService> manCoService;
    private Mock<IUserService> userService;
    private Mock<ISubmitJobsService> submitJobsService;
    private Mock<IDocTypeService> docTypeService;
    private Mock<IJobService> jobService;
    private Mock<IAppEnvironmentService> appEnvironmentService;
    private SubmitJobsController controller;

    [SetUp]
    public void Setup()
    {
      this.logger = new Mock<ILogger>();
      this.manCoService = new Mock<IManCoService>();
      this.userService = new Mock<IUserService>();
      this.submitJobsService = new Mock<ISubmitJobsService>();
      this.docTypeService = new Mock<IDocTypeService>();
      this.jobService = new Mock<IJobService>();
      this.appEnvironmentService = new Mock<IAppEnvironmentService>();
      this.controller = new SubmitJobsController(this.manCoService.Object, this.userService.Object, this.submitJobsService.Object, this.docTypeService.Object, this.jobService.Object, this.logger.Object, this.appEnvironmentService.Object);

      this.userService.Setup(x => x.GetApplicationUser()).Returns(new ApplicationUser());

      this.manCoService.Setup(x => x.GetManCos(It.IsAny<String>())).Returns(new List<ManagementCompany>() { new ManagementCompany(), new ManagementCompany() });
    }

    [Test]
    public void GivenASubmitJobsController_WhenIRequestTheIndexAction_ThenIGetTheCorrectView()
    {
      var result = this.controller.Index() as ViewResult;
      var model = (SubmitJobsViewModel)result.Model;

      model.ManCos.Count.Should().Be(2);
    }

    [Test]
    public void GivenASubmitJobsController_WhenIRequestTheGetFilesAction_ThenIGetAJsonResult()
    {
      this.SetControllerContext(this.controller);
      MockHttpContext.SetupGet(x => x.Session["Environment"]).Returns("environment");

      this.submitJobsService.Setup(s => s.GetInputFiles("environment", "manco", "docType")).Returns(new InputFileInfo()
                                                                                                          {
                                                                                                            Folder = "folder",
                                                                                                            Files = new List<String>(),
                                                                                                            Folders = new List<String>()
                                                                                                          });
      this.docTypeService.Setup(s => s.GetDocType("docType", "name")).Returns(new DocumentType());

      var result = this.controller.GetFiles("manco", "docType", "name") as PartialViewResult;
      var model = (InputFileViewModel)result.Model;

      result.ViewName.Should().Be("_InputFileResults");
    }

    [Test]
    public void GivenASubmitJobsController_WhenIRequestTheGetFilesAction_AndPathIsAlreadyKnown_ThenIGetAJsonResult()
    {
      this.SetControllerContext(this.controller);
      MockHttpContext.SetupGet(x => x.Session["Environment"]).Returns("environment");

      this.submitJobsService.Setup(s => s.GetInputFiles("path", "environment", "manco", "docType")).Returns(new InputFileInfo()
                                                                                                              {
                                                                                                                Folder = "folder",
                                                                                                                Files = new List<String>(),
                                                                                                                Folders = new List<String>(),
                                                                                                              });

      var result = this.controller.GetFolderFiles("path", "manco", "docType") as PartialViewResult;
      var model = (InputFileViewModel)result.Model;

      result.ViewName.Should().Be("_InputFileResults");
    }

    [Test]
    public void GivenASubmitController_WhenCreatingAJob_AndTheModelIsNotValid_ThenAJsonResultIsReturnedWithTheErrorMessage()
    {
      this.controller.ModelState.AddModelError("SortCode", "Sort code is invalid");

      var result = this.controller.Create(new CreateJobViewModel());
      var jsonResult = result as JsonResult;
      jsonResult.Data.ToString().Should().Be("{ Url = , Error = System.Collections.Generic.List`1[System.Web.Mvc.ModelError] }");
    }

    [Test]
    public void GivenASubmitController_WhenCreatingAJob_AndThererAreNoModelError_ThenAJsonResultIsReturnedWithTheRedirectUrl()
    {
      this.controller.Url = new UrlHelper(new RequestContext(MockHttpContext.Object, new RouteData()), Routes);

      this.SetControllerContext(this.controller);
      MockHttpContext.SetupGet(x => x.Session["Environment"]).Returns("environment");

      this.jobService.Setup(
        j =>
        j.CreateJob(
          It.IsAny<String>(),
          It.IsAny<String>(),
          It.IsAny<String>(),
          It.IsAny<String>(),
          It.IsAny<List<String>>(),
          It.IsAny<String>(),
          It.IsAny<Boolean>(),
          It.IsAny<Boolean>(),
          It.IsAny<String>(),
          It.IsAny<String>(),
          It.IsAny<String>(),
          It.IsAny<String>(),
          It.IsAny<String>()));

      var result = this.controller.Create(new CreateJobViewModel());
      var jsonResult = result as JsonResult;
      jsonResult.Data.ToString().Should().Be("{ Url = , Error =  }");
    }

    [Test]
    public void GivenASubmitController_WhenCreatingAJob_AndTheFileHasAlreadyBeenProcessed_ThenAJsonResultIsReturnedWithTheRedirectUrl()
    {
      this.controller.Url = new UrlHelper(new RequestContext(MockHttpContext.Object, new RouteData()), Routes);

      this.SetControllerContext(this.controller);
      MockHttpContext.SetupGet(x => x.Session["Environment"]).Returns("environment");

      this.jobService.Setup(
        j =>
        j.CreateJob(
          It.IsAny<String>(),
          It.IsAny<String>(),
          It.IsAny<String>(),
          It.IsAny<String>(),
          It.IsAny<List<String>>(),
          It.IsAny<String>(),
          It.IsAny<Boolean>(),
          It.IsAny<Boolean>(),
          It.IsAny<String>(),
          It.IsAny<String>(),
          It.IsAny<String>(),
          It.IsAny<String>(),
          It.IsAny<String>())).Throws(new DocProcessingFileAlreadyProcessedException());

      var result = this.controller.Create(new CreateJobViewModel());
      var jsonResult = result as JsonResult;
      jsonResult.Data.ToString().Should().Be("{ Url = , Error = File has already been processed }");
    }

    [Test]
    public void GivenASubmitJobsController_WhenIRequestTheSavedAction_ThenIGetTheCorrectView()
    {
      var result = this.controller.Saved() as ViewResult;
      result.Should().NotBeNull();
    }

    //[Test]
    //public void GivenASubmitJobsController_WhenISearchForPacks_AndTheModelIsNotValid_ThenAJsonResultWithAnEmptyViewIsReturned()
    //{
    //  var result = this.controller.GetAdditionalInfo(null, String.Empty);
    //  var jsonResult = result as JsonResult;
    //  jsonResult.Data.ToString().Should().Be("{ error = True, Error = System.Collections.Generic.List`1[System.Web.Mvc.ModelError] }");
    //}

    //[Test]
    //public void GivenASubmitJobsController_WhenISearchForPacks_ThenAJsonResultWithAdditionalInfo()
    //{
    //  this.submitJobsService.Setup(s => s.GetAdditionalInfo("path", new List<string>() { "file1", "file2" })).Returns(new AdditionalInfoFile());

    //  var result = this.controller.GetAdditionalInfo(new List<string>() { "one", "two" }, "path");
    //  var jsonResult = result as JsonResult;
    //  jsonResult.Data.ToString().Should().Be("{ error = True, Error = Could not find any packs for the serach criteria }");
    //}
  }
}
