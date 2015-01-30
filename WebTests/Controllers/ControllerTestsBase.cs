// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ControllerTestsBase.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   Defines the ControllerTestsBase type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace WebTests.Controllers
{
  using System.Web;
  using System.Web.Mvc;
  using System.Web.Routing;

  using DocProcessingWorkflow;
  using DocProcessingWorkflow.Controllers;

  using Moq;

  using NUnit.Framework;

  using WebTests.Extensions;

  using WebTests.Helpers;

  public class ControllerTestsBase
  {
    protected ControllerContext ControllerContext { get; set; }

    protected FakeResponse FakeResponse { get; set; }

    protected Mock<HttpContextBase> MockHttpContext { get; set; }

    protected Mock<HttpRequestBase> MockRequest { get; set; }

    protected RouteCollection Routes { get; set; }

    protected TempDataDictionary TempData { get; set; }

    [SetUp]
    public void BaseSetup()
    {
      this.MockHttpContext = new Mock<HttpContextBase>();
      this.MockRequest = new Mock<HttpRequestBase>();
      this.FakeResponse = new FakeResponse();
      this.Routes = new RouteCollection();
      //this.Routes.RegisterRoutes();
    
      this.MockRequest.Setup(x => x.AppRelativeCurrentExecutionFilePath).Returns("/");
      this.MockRequest.Setup(r => r.ApplicationPath).Returns("/");

      this.MockHttpContext.Setup(m => m.Request).Returns(this.MockRequest.Object);
      this.MockHttpContext.Setup(m => m.Response).Returns(this.FakeResponse);
      this.MockHttpContext.Setup(m => m.Request.PhysicalApplicationPath).Returns("PhysicalAppPath");
      this.MockHttpContext.Setup(m => m.User.Identity.Name).Returns("Username");
      this.MockHttpContext.Setup(m => m.Session["Environment"]).Returns("Development");
    
      this.TempData = new TempDataDictionary();
    }

    protected void SetControllerContext(BaseController controller)
    {
      this.ControllerContext = new ControllerContext(this.MockHttpContext.Object, new RouteData(), controller);
      controller.ControllerContext = this.ControllerContext;
      controller.TempData = this.TempData;
    }
  }
}
