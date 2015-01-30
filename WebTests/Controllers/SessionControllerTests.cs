// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SessionControllerTests.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//  Tests for session controller 
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace WebTests.Controllers
{
  using System;
  using System.Web.Mvc;

  using DocProcessingWorkflow.Constants;
  using DocProcessingWorkflow.Controllers;
  using DocProcessingWorkflow.Models.User;
  using Entities;
  using FluentAssertions;
  using Logging;
  using Moq;
  using NUnit.Framework;
  using ServiceInterfaces;

  [Category("Unit")]
  [TestFixture]
  public class SessionControllerTests : ControllerTestsBase
  {
    private Mock<ILogger> logger;
    private Mock<IUserService> userService;

    private Mock<IAppEnvironmentService> appEnvironmentService;
    private SessionController sessionController;
    private readonly String _sessionValue = DateTime.Now.AddDays(-7).ToString();

    [SetUp]
    public void Setup()
    {
      this.logger = new Mock<ILogger>();
      this.userService = new Mock<IUserService>();
      this.appEnvironmentService = new Mock<IAppEnvironmentService>();
      this.sessionController = new SessionController(this.userService.Object, this.logger.Object, this.appEnvironmentService.Object);
      SetControllerContext(this.sessionController);
    }

    [Test]
    public void GivenAnAuthenticatedUser_WhenILogin_ThenIGetTheRedirectView()
    {
      MockHttpContext.Setup(h => h.User).Returns(new UserViewModel() { IsLoggedIn = true });

      var result = this.sessionController.New() as RedirectToRouteResult;

      result.Should().NotBeNull();
      result.RouteValues["action"].Should().Be("Index");
      result.RouteValues["controller"].Should().Be("Home");
    }

    [Test]
    public void GivenNoAuthenticatedUser_WhenILogin_ThenIGetTheLoginView()
    {
      MockHttpContext.Setup(h => h.User).Returns(new UserViewModel());
      ActionResult result = sessionController.New();
      result.Should().BeOfType<ViewResult>();
    }

    [Test]
    public void GivenAnInvalidEMail_WhenILogin_ThenIGetRedirectedToTheRegistrationPage()
    {
      this.sessionController.ModelState.AddModelError("error", "message");

      var result = this.sessionController.Create(new LoginUserViewModel(), string.Empty) as ViewResult;

      result.Should().NotBeNull();
      result.ViewName.Should().Be("New");
    }

    [Test]
    public void GivenAValidUserAndPassword_WhenILogin_ThenIGetRedirectedToTheDashboardPage()
    {
      var userMock = new Mock<ApplicationUser>();
      this.userService.Setup(m => m.GetApplicationUser("username", "password")).Returns(userMock.Object);
      userMock.SetupGet(u => u.IsApproved).Returns(true);
      userMock.SetupGet(u => u.IsLockedOut).Returns(false);
      MockHttpContext.SetupGet(x => x.User.Identity.Name).Returns("testUser");

      this.userService.Setup(x => x.CheckForPassRenewal(It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(false);
      this.userService.Setup(x => x.SignIn(userMock.Object, false));
      this.userService.Setup(x => x.UpdateUserLastLogindate(It.IsAny<String>()));

      SetControllerContext(this.sessionController);
      MockHttpContext.SetupGet(x => x.Session["LastLoggedInDate"]).Returns(_sessionValue);

      var result =
          this.sessionController.Create(new LoginUserViewModel { Username = "username", Password = "password" }, string.Empty) as
          RedirectToRouteResult;

      this.userService.Verify(m => m.GetApplicationUser(It.IsAny<String>(), It.IsAny<String>()), Times.AtLeastOnce);
      this.userService.Verify(x => x.CheckForPassRenewal(It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.AtLeastOnce);
      this.userService.Verify(x => x.SignIn(userMock.Object, false), Times.AtLeastOnce);
      this.userService.Verify(x => x.UpdateUserLastLogindate(It.IsAny<String>()), Times.AtLeastOnce);

      result.Should().NotBeNull();
      result.RouteValues["controller"].Should().Be("Home");
      result.RouteValues["action"].Should().Be("Index");
    }

    [Test]
    public void GivenAValidUserAndPassword_WhenILoginAsALockedOutUser_ThenIGetRedirectedToTheLockedOutPage()
    {
      var userMock = new Mock<ApplicationUser>();
      this.userService.Setup(m => m.GetApplicationUser("username", "password")).Returns(userMock.Object);
      userMock.SetupGet(u => u.IsLockedOut).Returns(true);

      var result =
          this.sessionController.Create(new LoginUserViewModel { Username = "username", Password = "password" }, string.Empty) as
          RedirectToRouteResult;

      result.RouteValues["controller"].Should().Be("Session");
      result.RouteValues["action"].Should().Be("LockedOut");
    }

    [Test]
    public void GivenAValidUserAndPasswordAndPasswordHasExpired_WhenILogin_ThenIGetRedirectedToTheChangePasswordPage()
    {
      var userMock = new Mock<ApplicationUser>();
      this.userService.Setup(m => m.GetApplicationUser("username", "password")).Returns(userMock.Object);
      userMock.SetupGet(u => u.IsApproved).Returns(true);
      userMock.SetupGet(u => u.IsLockedOut).Returns(false);

      this.userService.Setup(x => x.CheckForPassRenewal(It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(true);
      MockHttpContext.SetupGet(x => x.Session["LastLoggedInDate"]).Returns(_sessionValue);

      var result =
          this.sessionController.Create(new LoginUserViewModel { Username = "username", Password = "password" }, string.Empty) as
          RedirectToRouteResult;

      this.userService.Verify(m => m.GetApplicationUser(It.IsAny<String>(), It.IsAny<String>()), Times.AtLeastOnce);
      this.userService.Verify(x => x.CheckForPassRenewal(It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.AtLeastOnce);

      result.Should().NotBeNull();
      result.RouteValues["controller"].Should().Be("Account");
      result.RouteValues["action"].Should().Be("ChangePassword");
    }

    [Test]
    public void GivenAnInValidUserAndPassword_WhenILogin_TheTempDataContainsTheCorrectMessagee()
    {
      this.userService.Setup(m => m.GetApplicationUser(It.IsAny<String>(), It.IsAny<String>())).Returns(It.IsAny<ApplicationUser>());
      this.userService.Setup(m => m.UpdateUserFailedLogin(It.IsAny<String>()));

      MockHttpContext.SetupGet(x => x.User.Identity.Name).Returns("testUser");
      MockHttpContext.SetupGet(x => x.User.Identity.Name).Returns("testUser");
      MockHttpContext.SetupGet(x => x.Session["LastLoggedInDate"]).Returns(_sessionValue);

      var result =
          this.sessionController.Create(new LoginUserViewModel { Username = "username", Password = "password" }, string.Empty) as
          ViewResult;

      this.userService.Verify(m => m.GetApplicationUser(It.IsAny<String>(), It.IsAny<String>()), Times.AtLeastOnce);
      TempData["message"].Should().Be("Login was unsuccessful. Please correct the errors and try again.");
    }

    [Test]
    public void GivenALockedOutUser_WhenILogin_IGetRedirectedToTheLockedOutView()
    {
      MockHttpContext.SetupGet(x => x.User.Identity.Name).Returns("testUser");
      this.userService.Setup(m => m.GetApplicationUser("username")).Returns(new ApplicationUser { IsLockedOut = true });
      this.userService.Setup(m => m.UpdateUserFailedLogin(It.IsAny<String>()));
      this.userService.Setup(m => m.IsLockedOut(It.IsAny<String>())).Returns(true);
      MockHttpContext.SetupGet(x => x.Session["LastLoggedInDate"]).Returns(_sessionValue);

      var result = this.sessionController.Create(new LoginUserViewModel { Username = "username", Password = "password" }, string.Empty) as RedirectToRouteResult;

      result.RouteValues["action"].Should().Be("LockedOut");
      result.RouteValues["controller"].Should().Be("Session");
    }

    [Test]
    public void GivenAValidUserIdAndPassword_WhenILoginAndHaveAPreferredLandingPage_IGetRedirectedToMyDefaultLandingPage()
    {
      var userMock = new Mock<ApplicationUser>();
      this.userService.Setup(m => m.GetApplicationUser("username", "password")).Returns(userMock.Object);
      userMock.SetupGet(u => u.IsApproved).Returns(true);
      userMock.SetupGet(u => u.IsLockedOut).Returns(false);
      userMock.SetupGet(u => u.PreferredLandingPage).Returns(LandingPage.JobsView);
      MockHttpContext.SetupGet(x => x.User.Identity.Name).Returns("testUser");

      this.userService.Setup(x => x.CheckForPassRenewal(It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(false);
      this.userService.Setup(x => x.SignIn(userMock.Object, false));
      this.userService.Setup(x => x.UpdateUserLastLogindate(It.IsAny<String>()));

      SetControllerContext(this.sessionController);
      MockHttpContext.SetupGet(x => x.Session["LastLoggedInDate"]).Returns(_sessionValue);

      var result =
          this.sessionController.Create(new LoginUserViewModel { Username = "username", Password = "password" }, string.Empty) as
          RedirectToRouteResult;

      this.userService.Verify(m => m.GetApplicationUser(It.IsAny<String>(), It.IsAny<String>()), Times.AtLeastOnce);
      this.userService.Verify(x => x.CheckForPassRenewal(It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.AtLeastOnce);
      this.userService.Verify(x => x.SignIn(userMock.Object, false), Times.AtLeastOnce);
      this.userService.Verify(x => x.UpdateUserLastLogindate(It.IsAny<String>()), Times.AtLeastOnce);

      result.Should().NotBeNull();
      result.RouteValues["controller"].Should().Be("Jobs");
      result.RouteValues["action"].Should().Be("Index");
    }

    [Test]
    public void GivenAnInValidPassword_WhenILogin_TheFailedLogInCountIsIncremented()
    {
      ApplicationUser applicationUser = null;

      MockHttpContext.SetupGet(x => x.User.Identity.Name).Returns("testUser");
      this.userService.Setup(m => m.GetApplicationUser("username", "InValidPassword")).Returns(applicationUser);
      this.userService.Setup(m => m.GetApplicationUser("username")).Returns(new ApplicationUser { Id = "1" });

      var result =
          this.sessionController.Create(new LoginUserViewModel { Username = "username", Password = "InValidPassword" }, string.Empty) as
          ViewResult;

      this.userService.Verify(m => m.UpdateUserFailedLogin(It.IsAny<String>()), Times.Once);
    }

    [Test]
    public void GivenALockedOutUser_WhenITryToLogIn_IGetTheLockedOutView()
    {
      MockHttpContext.SetupGet(x => x.User.Identity.Name).Returns("testUser");
      var result = this.sessionController.LockedOut() as ActionResult;
      result.Should().BeOfType<ViewResult>();
    }

    [Test]
    public void GivenALoggedInUser_WhenILoginOut_ThenILoginView()
    {
      MockHttpContext.SetupGet(x => x.User.Identity.Name).Returns("testUser");
      MockHttpContext.Setup(x => x.Session.Clear());

      var result = this.sessionController.Remove() as RedirectToRouteResult;
      result.RouteValues["action"].Should().Be("New");
    }

    [Test]
    public void GivenASessionController_WhenIWantToGetExpiredPage_TheRightViewIsReturned()
    {
      this.userService.Setup(x => x.SignOut());

      var result = this.sessionController.Expired();
      result.Should().BeOfType<ViewResult>();

      this.userService.Verify(x => x.SignOut());
    }

      [Test]
      public void GivenASessionController_WhenIhavePreferredLandingPage_IGetRedirectedToMyPreferredLandingPage()
      {
          var result = this.sessionController.RedirectToLandingPage("Home") as RedirectToRouteResult;
          result.Should().NotBeNull();
          result.Should().BeOfType<RedirectToRouteResult>();
          result.As<RedirectToRouteResult>().RouteValues["action"].Should().Be("Index");
          result.As<RedirectToRouteResult>().RouteValues["controller"].Should().Be("Home");
      }

      [Test]
      public void GivenASesssionController_WhenIHaveDefaultLandingPage_IGetRedirectedToMyDefaultPage()
      {
           var userMock = new Mock<ApplicationUser>();
          userMock.SetupGet(u => u.PreferredLandingPage).Returns(LandingPage.JobsView).ToString();
          var result =
              this.sessionController.RedirectToLandingPage(
                  userMock.SetupGet(u => u.PreferredLandingPage).Returns(LandingPage.JobsView).ToString()) as
                  RedirectToRouteResult;

          result.Should().NotBeNull();
          result.Should().BeOfType<RedirectToRouteResult>();
          result.As<RedirectToRouteResult>().RouteValues["action"].Should().Be("Index");
          result.As<RedirectToRouteResult>().RouteValues["controller"].Should().Be("Home");
      }

     [Test]
    public void GivenAsessionController_WhenISessionReset_TheSessionIsReset()
    {
       this.sessionController.SessionReset();
    }
  }
}
