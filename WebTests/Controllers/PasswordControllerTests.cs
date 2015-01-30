// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PasswordControllerTests.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace WebTests.Controllers
{
  using System;
  using System.Web.Mvc;
  using DocProcessingWorkflow.Controllers;
  using DocProcessingWorkflow.Models.Password;
  using Entities;
  using FluentAssertions;
  using Logging;
  using Moq;
  using NUnit.Framework;
  using ServiceInterfaces;

  [Category("Unit")]
  [TestFixture]
  public class PasswordControllerTests : ControllerTestsBase
  {
    private Mock<IUserService> userService;
    private Mock<IAppEnvironmentService> appEnvironmentService;
    private Mock<ILogger> logger;
    private PasswordController controller;

    [SetUp]
    public void SetUp()
    {
      this.userService = new Mock<IUserService>();
      this.appEnvironmentService = new Mock<IAppEnvironmentService>();
      this.logger = new Mock<ILogger>();

      this.controller = new PasswordController(this.userService.Object, this.logger.Object, this.appEnvironmentService.Object);
      SetControllerContext(this.controller);
    }

    [Test]
    public void WhenISubmitTheChangeCurrentPasswordPage_AndTheModelIsValid_ItShouldReturnTheRightViewAndContainTheModel()
    {
      MockHttpContext.SetupGet(x => x.User.Identity.Name).Returns("testUser");
      var model = new ChangeCurrentPasswordModel();

      var user = new ApplicationUser();

      this.userService.Setup(x => x.ChangePassword(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>()));
      this.userService.Setup(x => x.GetApplicationUserById(It.IsAny<String>())).Returns(user);
      this.userService.Setup(x => x.SignIn(user, false));
      this.userService.Setup(x => x.UpdateUserLastLogindate(It.IsAny<String>()));

      var result = this.controller.ChangeCurrent(model) as RedirectToRouteResult;

      result.Should().NotBeNull();

      result.Should().BeOfType<RedirectToRouteResult>();
      this.userService.Verify(x => x.ChangePassword(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>()), Times.AtLeastOnce);
      this.userService.Verify(x => x.GetApplicationUserById(It.IsAny<String>()), Times.AtLeastOnce);
      this.userService.Verify(x => x.SignIn(user, false), Times.AtLeastOnce);
      this.userService.Verify(x => x.UpdateUserLastLogindate(It.IsAny<String>()), Times.AtLeastOnce);
      result.RouteValues["controller"].Should().Be("Home");
      result.RouteValues["action"].Should().Be("Index");
    }

    [Test]
    public void WhenISubmitTheChangeCurrentPasswordPage_AndTheModelIsInValid_ItShouldReturnTheRightViewAndContainTheModel()
    {
      this.controller.ModelState.AddModelError("Password", "Password has already been used recently");

      var model = new ChangeCurrentPasswordModel();

      var result = (ViewResult)this.controller.ChangeCurrent(model);

      result.Should().NotBeNull();
      result.Model.Should().BeOfType<ChangeCurrentPasswordModel>();
      result.TempData["message"].ToString().ShouldBeEquivalentTo("Please correct the errors and try again.");
    }

    [Test]
    public void WhenIAskForTheChangeCurrentPassowrdPage_TheViewIsReturnedAndContainsTheRightModel()
    {
      this.controller.TempData["username"] = "Greg";

      var user = new ApplicationUser("Greg");

      this.userService.Setup(x => x.GetApplicationUser(It.IsAny<String>())).Returns(user);

      var result = (ViewResult)this.controller.ChangeCurrent();

      result.Model.Should().BeOfType<ChangeCurrentPasswordModel>();

      this.userService.Verify(x => x.GetApplicationUser(It.IsAny<String>()), Times.AtLeastOnce);
    }
  }
}
