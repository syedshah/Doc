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
  using System.Collections.Generic;
  using System.Web.Mvc;
  using System.Web.Routing;

  using DocProcessingWorkflow.Controllers;
  using DocProcessingWorkflow.Models.Environment;

  using Entities;

  using FluentAssertions;

  using Logging;
  using Moq;
  using NUnit.Framework;
  using ServiceInterfaces;

  [Category("Unit")]
  [TestFixture]
  public class EnvironmentControllerTests : ControllerTestsBase
  {
    private Mock<ILogger> logger;
    private Mock<IAppEnvironmentService> appEnvironmentService;
    private Mock<IUserService> userService;
    private EnvironmentController controller;
    private const string ExpectedRefererUrl = "/Jobs/View";

    [SetUp]
    public void Setup()
    {
      this.logger = new Mock<ILogger>();
      this.appEnvironmentService = new Mock<IAppEnvironmentService>();
      this.userService = new Mock<IUserService>();
      this.controller = new EnvironmentController(this.appEnvironmentService.Object, this.userService.Object, this.logger.Object);
      this.controller.Url = new UrlHelper(new RequestContext(MockHttpContext.Object, new RouteData()), RouteTable.Routes);
    }

    [Test]
    public void GivenAnEnvironmentControllerForAUserWhoHasNotChangedTheEnvironment_WhenILogIn_IGetMyPreferedLandingEnvironment()
    {
      this.SetControllerContext(this.controller);
      MockHttpContext.SetupGet(x => x.Session["Environment"]).Returns(null);

      this.userService.Setup(x => x.GetApplicationUser()).Returns(new ApplicationUser() { PreferredEnvironment = "UAT", Id = "guid" } );
      this.appEnvironmentService.Setup(d => d.GetAppEnvironments("guid")).Returns(new List<AppEnvironment>());

      var result = (PartialViewResult)this.controller.Index();
      var model = (EnvironmentViewModel)result.Model;

      model.SelectedEnvironment.Should().Be("UAT");
      result.ViewName.Should().Be("_environment");
    }

    [Test]
    public void GivenAnEnvironmentControllerForAUserWhoHasChangedTheEnvironment_WhenILogIn_IGetMyPreferedLandingEnvironment()
    {
      this.SetControllerContext(this.controller);
      MockHttpContext.SetupGet(x => x.Session["Environment"]).Returns("Production");

      this.userService.Setup(x => x.GetApplicationUser()).Returns(new ApplicationUser() { PreferredEnvironment = "UAT", Id = "guid" });
      this.appEnvironmentService.Setup(d => d.GetAppEnvironments("guid")).Returns(new List<AppEnvironment>());

      var result = (PartialViewResult)this.controller.Index();
      var model = (EnvironmentViewModel)result.Model;

      model.SelectedEnvironment.Should().Be("Production");
      result.ViewName.Should().Be("_environment");
    }

    [Test]
    public void GivenAnEnvironmentControllerForAUserWhoHasNotChangedTheEnvironment_AndNotSpecifiedAPreferedLandingPAge_WhenILogIn_IGetMyTheLowestEnviromentIHaveAccessTo()
    {
      this.SetControllerContext(this.controller);
      MockHttpContext.SetupGet(x => x.Session["Environment"]).Returns(null);

      this.userService.Setup(x => x.GetApplicationUser()).Returns(new ApplicationUser() { Id = "guid" });
      this.appEnvironmentService.Setup(d => d.GetAppEnvironments("guid"))
          .Returns(
            new List<AppEnvironment>()
              {
                new AppEnvironment() { Name = "UAT", AppEnvironmentID = 1 },
                new AppEnvironment() { Name = "Production", AppEnvironmentID = 2}
              });

      var result = (PartialViewResult)this.controller.Index();
      var model = (EnvironmentViewModel)result.Model;

      model.SelectedEnvironment.Should().Be("UAT");
      result.ViewName.Should().Be("_environment");
    }

    [Test]
    public void GivenAnEnvironmentController_WhenIChangeMyEnvironment_MyEnvironmentIsChanged()
    {
      this.SetControllerContext(this.controller);
      MockHttpContext.SetupGet(x => x.Session["Environment"]).Returns(null);
      
      RouteTable.Routes.Add("TestRoutes", new Route("{controller}/{action}", new MvcRouteHandler()));
      var result = (JsonResult)this.controller.Change("UAT", "/Jobs/View");
      
      result.Data.ToString().Should().Be(@"{ Url = /Jobs/View, Error =  }");
      result.Should().NotBeNull();
      result.Should().BeOfType<JsonResult>();

    }
  }
}
