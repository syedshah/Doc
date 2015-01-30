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
  using DocProcessingWorkflow.Controllers;
  using Entities;
  using FluentAssertions;
  using Logging;
  using Moq;
  using NUnit.Framework;
  using ServiceInterfaces;

  [Category("Unit")]
  [TestFixture]
  public class DocTypesControllerTests : ControllerTestsBase
  {
    private Mock<ILogger> logger;
    private Mock<IDocTypeService> docTypeService;
    private Mock<IUserService> userService;
    private Mock<IAppEnvironmentService> appEnvironmentService;
    private DocTypesController controller;

    [SetUp]
    public void Setup()
    {
      this.logger = new Mock<ILogger>();
      this.docTypeService = new Mock<IDocTypeService>();
      this.userService = new Mock<IUserService>();
      this.appEnvironmentService = new Mock<IAppEnvironmentService>();
      this.controller = new DocTypesController(this.docTypeService.Object, this.userService.Object, this.appEnvironmentService.Object, this.logger.Object);

      this.userService.Setup(x => x.GetApplicationUser()).Returns(new ApplicationUser());

      this.docTypeService.Setup(x => x.GetDocTypes(It.IsAny<String>(), "1")).Returns(new List<DocumentType>
                                                                    {
                                                                      new DocumentType { },
                                                                      new DocumentType { },
                                                                      new DocumentType { },
                                                                    });
    }

    [Test]
    public void GivenADocTypesController_WhenIRequestTheSubDocTypeAction_ThenIGetAJsonResult()
    {
      var result = this.controller.Search("1") as JsonResult;

      result.Should().BeOfType<JsonResult>();
    }
  }
}
