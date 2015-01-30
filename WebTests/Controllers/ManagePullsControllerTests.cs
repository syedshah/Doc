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

  using AbstractConfigurationManager;

  using DocProcessingWorkflow.Constants;
  using DocProcessingWorkflow.Controllers;
  using DocProcessingWorkflow.Models.ManagePulls;
  using Entities;
  using Entities.ADF;
  using FluentAssertions;
  using Logging;
  using Microsoft.AspNet.Identity.EntityFramework;
  using Moq;
  using NUnit.Framework;
  using ServiceInterfaces;

  [Category("Unit")]
  [TestFixture]
  public class ManagePullsControllerTests : ControllerTestsBase
  {
    private Mock<ILogger> logger;
    private Mock<IManCoService> manCoService;
    private Mock<IUserService> userService;
    private Mock<IJobService> jobService;
    private Mock<IPackStoreService> packStoreService;
    private Mock<IEmailService> emailService;
    private Mock<IConfigurationManager> configurationManager;
    private Mock<IAppEnvironmentService> appEnvironmentService;
    private ManagePullsController controller;
    private ApplicationUser currentUser;

    private IdentityRole role;

    [SetUp]
    public void Setup()
    {
      this.logger = new Mock<ILogger>();
      this.manCoService = new Mock<IManCoService>();
      this.userService = new Mock<IUserService>();
      this.jobService = new Mock<IJobService>();
      this.packStoreService = new Mock<IPackStoreService>();
      this.emailService = new Mock<IEmailService>();
      this.configurationManager = new Mock<IConfigurationManager>();
      this.appEnvironmentService = new Mock<IAppEnvironmentService>();
      this.controller = new ManagePullsController(
        this.manCoService.Object,
        this.userService.Object,
        this.jobService.Object,
        this.packStoreService.Object,
        this.emailService.Object,
        this.configurationManager.Object,
        this.appEnvironmentService.Object,
        this.logger.Object);

      this.userService.Setup(x => x.GetApplicationUser()).Returns(new ApplicationUser());

      this.manCoService.Setup(x => x.GetManCos(It.IsAny<String>())).Returns(new List<ManagementCompany>() { new ManagementCompany(), new ManagementCompany() });
      this.jobService.Setup(x => x.GetCompletedJobs("1", It.IsAny<String>())).Returns(new List<JobEntity>
                                                                    {
                                                                      new JobEntity { },
                                                                      new JobEntity { },
                                                                      new JobEntity { },
                                                                    });
      this.currentUser = new ApplicationUser("Username");
      this.role = new IdentityRole(Role.PullAuthorisation);
    }

    [Test]
    public void GivenAManagePullsController_WhenIRequestTheIndexAction_ThenIGetTheCorrectView()
    {
      var result = this.controller.Index() as ViewResult;
      var model = (ManagePullsViewModel)result.Model;

      model.ManCos.Count.Should().Be(2);
    }

    [Test]
    public void GivenAManagePullsController_WhenIRequestTheGetCompletedJobsAction_ThenIGetAJsonResult()
    {
      var result = this.controller.GetCompletedJobs("1") as JsonResult;

      result.Should().BeOfType<JsonResult>();
    }

    [Test]
    public void GivenAPullsController_WhenIRequestTheGetPullsAction_ThenIGetAJsonResult()
    {
      this.userService.Setup(x => x.GetApplicationUser()).Returns(this.currentUser);
      this.userService.Setup(x => x.GetApplicationUser(It.IsAny<String>())).Returns(this.currentUser);
      this.packStoreService.Setup(s => s.GetPulledPacks(1))
          .Returns(new List<PackStore>() { new PackStore() { PackID = Guid.NewGuid(), RecepientRef = 1 }, new PackStore() { PackID = Guid.NewGuid(), RecepientRef = 2 } });

      var result = this.controller.GetPulls(1) as PartialViewResult;
      var model = (PulledDocumentsViewModel)result.Model;
      model.CanAuthorisePullList.Should().Be(false);

      result.ViewName.Should().Be("_PulledDocuments");
    }

    [Test]
    public void GivenAPullsController_WhenIRequestTheGetPullsAction_AndTheUserIsInTheAuthorisePullRole_ThenIGetAJsonResult()
    {
      this.userService.Setup(x => x.GetApplicationUser()).Returns(this.currentUser);
      this.currentUser.Roles.Add(new IdentityUserRole() { UserId = "1", Role = new IdentityRole(Role.PullAuthorisation) });
      this.packStoreService.Setup(s => s.GetPulledPacks(1))
          .Returns(new List<PackStore>() { new PackStore() { PackID = Guid.NewGuid(), RecepientRef = 1 }, new PackStore() { PackID = Guid.NewGuid(), RecepientRef = 2 } });

      var result = this.controller.GetPulls(1) as PartialViewResult;
      var model = (PulledDocumentsViewModel)result.Model;
      model.CanAuthorisePullList.Should().Be(true);

      result.ViewName.Should().Be("_PulledDocuments");
    }

    [Test]
    public void GivenAPullsController_WhenISearchForPacks_AndTheModelIsNotValid_ThenAJsonResultIsReturnedWithTheErrorMessage()
    {
      this.controller.ModelState.AddModelError("JobId", "Job id is required");

      var result = this.controller.Search(new SearchPacksViewModel());
      var jsonResult = result as JsonResult;
      jsonResult.Data.ToString().Should().Be("{ error = True, Error = System.Collections.Generic.List`1[System.Web.Mvc.ModelError] }");
    }

    [Test]
    public void GivenAPullsController_WhenISearchForPacks_AndNoPacksAreFound_ThenAJsonResultIsReturnedWithTheErrorMessage()
    {
      this.packStoreService.Setup(s => s.GetNonPulledPacks("1", 1)).Returns(new List<PackStore>());

      var result = this.controller.Search(new SearchPacksViewModel() { SearchCriteria = "1", JobId = 1 });
      var jsonResult = result as JsonResult;
      jsonResult.Data.ToString().Should().Be("{ error = True, Error = Could not find any packs for the serach criteria }");
    }

    [Test]
    public void GivenManagePullsController_WhenIWantToRemovePull_ThenThePullIsRemovedAndTheRightPartialViewIsReturned()
    {
      this.currentUser.Roles.Add(new IdentityUserRole() { UserId = this.currentUser.Id, RoleId = this.role.Id, Role = this.role, User = this.currentUser });
      this.userService.Setup(x => x.GetApplicationUser()).Returns(this.currentUser);
      this.packStoreService.Setup(x => x.UpdatePullStatus(It.IsAny<String>(), It.IsAny<Boolean>()));

      this.packStoreService.Setup(s => s.GetPulledPacks(It.IsAny<Int32>()))
         .Returns(new List<PackStore>() { new PackStore() { PackID = Guid.NewGuid(), RecepientRef = 1 }, new PackStore() { PackID = Guid.NewGuid(), RecepientRef = 2 } });

      var result = this.controller.RemovePull(It.IsAny<String>(), It.IsAny<Int32>()) as PartialViewResult;

      result.Model.Should().BeOfType<PulledDocumentsViewModel>();

      result.ViewName.Should().Be("_PulledDocuments");
      var model = (PulledDocumentsViewModel)result.Model;
      model.CanAuthorisePullList.ShouldBeEquivalentTo(true);
      
      this.packStoreService.Verify(x => x.UpdatePullStatus(It.IsAny<String>(), It.IsAny<Boolean>()), Times.AtLeastOnce);
      this.userService.Verify(x => x.GetApplicationUser(), Times.AtLeastOnce);
      this.packStoreService.Verify(s => s.GetPulledPacks(It.IsAny<Int32>()),Times.AtLeastOnce);
    }

    [Test]
    public void GivenAPullsController_WhenIUpdateThePackToPulled_IGetASuccessJosonResult()
    {
      this.packStoreService.Setup(s => s.UpdatePullStatus("1", true));

      var result = this.controller.Pull(new PullPacksViewModel()
                                          {
                                            JobId = 1, 
                                            Packs = new List<PullPackViewModel> { new PullPackViewModel() { ClientReference = "1", Selected = true } }
                                          });
      var jsonResult = result as JsonResult;
      jsonResult.Data.ToString().Should().Be("{ error = False, Error =  }");
    }

    //PAUL THIS TEST WONT WORK UNTIL WE FIGURE OUT HOW TO TEST URL.ACTION
    //[Test]
    //public void GivenAPullsController_WhenISendTheAuthoriseEmail_IGetASuccessJosonResult()
    //{
    //  this.configurationManager.Setup(c => c.AppSetting("from")).Returns("from");
    //  this.configurationManager.Setup(c => c.AppSetting("to")).Returns("to");
    //  this.configurationManager.Setup(c => c.AppSetting("subject")).Returns("subject");
    //  this.configurationManager.Setup(c => c.AppSetting("mailServer")).Returns("mailServer");

    //  this.emailService.Setup(s => s.SendEmail("from", "to", "subkect", "body", "mailServer"));

    //  var result = this.controller.SendEmail(new PullPacksViewModel
    //                                      {
    //                                        JobId = 1,
    //                                        Packs = new List<PullPackViewModel>
    //                                                  {
    //                                                    new PullPackViewModel()
    //                                                      {
    //                                                        ClientReference = "ref",
    //                                                        Name = "name",
    //                                                        Selected = true
    //                                                      }
    //                                                  }
    //                                      });

    //  var jsonResult = result as JsonResult;
    //  jsonResult.Data.ToString().Should().Be("{ error = False, Error =  }");
    //}

    //PAUL FIX THIS TEST LATER
    //[Test]
    //public void GivenAPullsController_WhenISearchForPacks_ThenAJsonResultContainsPacks()
    //{
    //  this.SetControllerContext(this.controller);

    //  this.packStoreService.Setup(s => s.GetNonPulledPacks("1", 1)).Returns(new List<PackStore>() { new PackStore(), new PackStore() });

    //  var result = this.controller.Search(new SearchPacksViewModel() { ClientReference = "1", JobId = 1 });
    //  var jsonResult = result as JsonResult;
    //  jsonResult.Data.ToString().Should().Be("{ error = True, Error = Could not find any packs for the client reference supplied }");
    //}
  }
}
