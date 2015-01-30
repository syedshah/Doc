// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseControllerTests.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   Defines the BaseControllerTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace WebTests.Controllers
{
  using System;
  using System.Collections.Generic;
  using System.Web.Mvc;

  using DocProcessingWorkflow.Models;

  using Entities;

  using FluentAssertions;

  using Logging;

  using Moq;

  using NUnit.Framework;

  using ServiceInterfaces;

  [Category("Unit")]
  [TestFixture]
  internal class BaseControllerTests : ControllerTestsBase
  {
    private Mock<ILogger> logger;
    private Mock<IAppEnvironmentService> appEnvironmentService;

    private Mock<IUserService> userService;

    [SetUp]
    public void Setup()
    {
      this.logger = new Mock<ILogger>();
      this.appEnvironmentService = new Mock<IAppEnvironmentService>();
      this.userService = new Mock<IUserService>();
    }

    [Test]
    public void GivenANullLogger_WhenCreatingTheController_ThenAnExceptionIsThrown()
    {
      Action act = () => new TestableBaseController(null, null, null);
      act.ShouldThrow<ArgumentNullException>();
    }

    [Test]
    public void GivenThereIsNoFilterContext_ThenTheError_IsNotHandled()
    {
      var controller = new TestableBaseController(this.logger.Object, this.userService.Object, this.appEnvironmentService.Object);
      controller.OnException(null);
    }

    [Test]
    public void GivenThereIsNoCustomError_ThenTheError_IsHandled()
    {
      var controller = new TestableBaseController(this.logger.Object, this.userService.Object, this.appEnvironmentService.Object);
      var filterContext = new ExceptionContext();
      MockHttpContext.Setup(h => h.IsCustomErrorEnabled).Returns(false);
      filterContext.HttpContext = MockHttpContext.Object;
      controller.OnException(filterContext);
      filterContext.ExceptionHandled.Should().BeFalse();
    }

    [Test]
    public void GivenThereIsACustomError_AndItIsNotAnHttpRequestValidationException_ThenTheViewIsSetToTheErrorView()
    {
      var controller = new TestableBaseController(this.logger.Object, this.userService.Object, this.appEnvironmentService.Object);
      var filterContext = new ExceptionContext();
      MockHttpContext.Setup(h => h.IsCustomErrorEnabled).Returns(true);
      filterContext.HttpContext = MockHttpContext.Object;
      filterContext.Exception = new Exception();
      controller.OnException(filterContext);
      filterContext.Result.Should().BeOfType<ViewResult>();
    }

    [Test]
    public void GivenThereIsACustomError_AndItIsNotAnHttpRequestValidationException_ThenTheViewIsSetToError()
    {
      var controller = new TestableBaseController(this.logger.Object, this.userService.Object, this.appEnvironmentService.Object);
      var filterContext = new ExceptionContext();
      MockHttpContext.Setup(h => h.IsCustomErrorEnabled).Returns(true);
      filterContext.HttpContext = MockHttpContext.Object;
      filterContext.Exception = new Exception();
      controller.OnException(filterContext);
      filterContext.Result.As<ViewResult>().ViewName.Should().Be("Error");
    }

    [Test]
    public void GivenAnError_ThenAJSONViewResult_Isreturned()
    {
      var controller = new TestableBaseController(this.logger.Object, this.userService.Object, this.appEnvironmentService.Object);
      this.SetControllerContext(controller);
      var error = controller.JsonError(new Exception(), new ErrorCode(), String.Empty);
      error.Should().BeOfType<JsonResult>();
    }

    [Test]
    public void GivenAnError_ThenTheJSONViewResultContains_TheCorrectData()
    {
      var controller = new TestableBaseController(this.logger.Object, this.userService.Object, this.appEnvironmentService.Object);
      this.SetControllerContext(controller);
      const ErrorCode Code = new ErrorCode();
      var ex = new Exception("error message");
      var error = controller.JsonError(ex, Code, "message");
      error.As<JsonResult>().Data.As<ErrorViewModel>().DisplayMessage.Should().Be("message");
      error.As<JsonResult>().Data.As<ErrorViewModel>().ErrorCode.Should().Be(Code);
      error.As<JsonResult>().Data.As<ErrorViewModel>().ErrorMessage.Should().Be("error message");
    }

    [Test]
    public void GivenThatEnvironmentSessionIsNotNull_WhenIWantToSetEnvironmentSession_TheEnvironmentModelIsSelected()
    {
      var controller = new TestableBaseController(this.logger.Object, this.userService.Object, this.appEnvironmentService.Object);
      this.SetControllerContext(controller);
      this.MockHttpContext.Setup(m => m.Session["Environment"]).Returns("Development");

      var appEnvironments = new List<AppEnvironment>();
      appEnvironments.Add(new AppEnvironment() { Name = "environment1" });
      appEnvironments.Add(new AppEnvironment() { Name = "environment2" });

      this.userService.Setup(x => x.GetApplicationUser()).Returns(new ApplicationUser("user1"));
      this.appEnvironmentService.Setup(x => x.GetAppEnvironments(It.IsAny<String>())).Returns(appEnvironments);

      var environmentModel = controller.SetEnvironmentSession();

      environmentModel.Should().NotBeNull();
      environmentModel.SelectedEnvironment.ShouldBeEquivalentTo("Development");
      this.userService.Verify(x => x.GetApplicationUser(), Times.AtLeastOnce);
      this.appEnvironmentService.Verify(x => x.GetAppEnvironments(It.IsAny<String>()), Times.AtLeastOnce);
    }

    [Test]
    public void GivenThatEnvironmentSessionIsNull_WhenIWantToSetEnvironmentSession_TheEnvironmentSessionObjectIsSet()
    {
      var controller = new TestableBaseController(this.logger.Object, this.userService.Object, this.appEnvironmentService.Object);
      this.SetControllerContext(controller);
      this.MockHttpContext.Setup(m => m.Session["Environment"]).Returns(null);

      var appEnvironments = new List<AppEnvironment>();
      appEnvironments.Add(new AppEnvironment() { Name = "environment1" });
      appEnvironments.Add(new AppEnvironment() { Name = "environment2" });

      this.userService.Setup(x => x.GetApplicationUser()).Returns(new ApplicationUser("user1"));
      this.appEnvironmentService.Setup(x => x.GetAppEnvironments(It.IsAny<String>())).Returns(appEnvironments);

      var environmentModel = controller.SetEnvironmentSession();

      environmentModel.Should().NotBeNull();
      environmentModel.SelectedEnvironment.ShouldBeEquivalentTo("environment1");
      this.userService.Verify(x => x.GetApplicationUser(), Times.AtLeastOnce);
      this.appEnvironmentService.Verify(x => x.GetAppEnvironments(It.IsAny<String>()), Times.AtLeastOnce);
    }

    [Test]
    public void GivenThatEnvironmentSessionIsNull_WhenIWantToSetEnvironmentSession_TheEnvironmentSessionObjectIsSetToTheUserPreferredEnvironment()
    {
      var controller = new TestableBaseController(this.logger.Object, this.userService.Object, this.appEnvironmentService.Object);
      this.SetControllerContext(controller);
      this.MockHttpContext.Setup(m => m.Session["Environment"]).Returns(null);

      var appEnvironments = new List<AppEnvironment>();
      appEnvironments.Add(new AppEnvironment() { Name = "environment1" });
      appEnvironments.Add(new AppEnvironment() { Name = "environment2" });

      this.userService.Setup(x => x.GetApplicationUser()).Returns(new ApplicationUser("user1") { PreferredEnvironment = "environment2" });
      this.appEnvironmentService.Setup(x => x.GetAppEnvironments(It.IsAny<String>())).Returns(appEnvironments);

      var environmentModel = controller.SetEnvironmentSession();

      environmentModel.Should().NotBeNull();
      environmentModel.SelectedEnvironment.ShouldBeEquivalentTo("environment2");
      this.userService.Verify(x => x.GetApplicationUser(), Times.AtLeastOnce);
      this.appEnvironmentService.Verify(x => x.GetAppEnvironments(It.IsAny<String>()), Times.AtLeastOnce);
    }
  }
}
