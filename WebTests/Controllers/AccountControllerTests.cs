// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AccountControllerTests.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   Account controller tests
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace WebTests.Controllers
{
  using System;
  using System.Web.Mvc;
  using DocProcessingWorkflow.Controllers;

  using Entities;

  using FluentAssertions;
  using Logging;
  using Moq;
  using NUnit.Framework;

  using ServiceInterfaces;

  [Category("Unit")]
  [TestFixture]
  public class AccountControllerTests : ControllerTestsBase
  {
    private Mock<ILogger> logger;
    private Mock<IAppEnvironmentService> appEnvironmentService;

    private Mock<IUserService> userService;
    private AccountController accountController;

    [SetUp]
    public void Setup()
    {
      this.logger = new Mock<ILogger>();
      this.appEnvironmentService = new Mock<IAppEnvironmentService>();
      this.userService = new Mock<IUserService>();
      this.accountController = new AccountController(this.logger.Object, this.appEnvironmentService.Object, this.userService.Object);
      SetControllerContext(this.accountController);
    }

    [Test]
    public void GivenInvalidDate_WhenIGoToTheChangePasswrodAction_IAmRedirectedToTheLogOnPage()
    {
      var result = this.accountController.ChangePassword(string.Empty) as RedirectToRouteResult;

      result.Should().NotBeNull();
      result.RouteValues["action"].Should().Be("New");
      result.RouteValues["controller"].Should().Be("Session");
    }

    [Test]
    public void GivenValidDate_WhenIGoToTheChangePasswrodAction_IAmRedirectedToChangePasswordPage()
    {
      var result = this.accountController.ChangePassword("username");

      result.Should().NotBeNull();
      result.Should().BeOfType<RedirectToRouteResult>();
      result.As<RedirectToRouteResult>().RouteValues["controller"].Should().Be("Password");
      result.As<RedirectToRouteResult>().RouteValues["action"].Should().Be("ChangeCurrent");
    }

    [Test]
    public void GivenALoggedInUser_WhenIAskForTheUserSummary_ThenIGetTheCorrectView()
    {
      var userMock = new Mock<ApplicationUser>();
      userMock.SetupGet(u => u.UserName).Returns("test");
      userMock.SetupGet(u => u.LastLoginDate).Returns(DateTime.Now);
      this.userService.Setup(m => m.GetApplicationUser()).Returns(userMock.Object);

      var result = (PartialViewResult)this.accountController.Summary();
      result.ViewName.Should().Be("_Summary");
    }
  }
}
