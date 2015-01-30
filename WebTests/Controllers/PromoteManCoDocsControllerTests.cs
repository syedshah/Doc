// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PromoteManCoDocsControllerTests.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   Defines the PromoteManCoDocsControllerTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using DocProcessingWorkflow.Controllers;
using DocProcessingWorkflow.Models.PromoteManCoDocs;
using Entities;
using FluentAssertions;
using FluentAssertions.Equivalency;
using Logging;
using Moq;
using NUnit.Framework;
using ServiceInterfaces;

namespace WebTests.Controllers
{
    [Category("Unit")]
    [TestFixture]
    public class PromoteManCoDocsControllerTests : ControllerTestsBase
    {
        private Mock<IManCoService> manCoService;
        private Mock<IUserService> userService;
        private Mock<IManCoDocService> manCoDocService;
        private Mock<ILogger> logger;
        private Mock<IAppEnvironmentService> appEnvironmentService;
        private PromoteManCoDocsController promoteManCoDocsController;
        private ApplicationUser currentUser;
        private IList<AppEnvironment> appEnvironments;
        private IList<ManagementCompany> manCos;
        private IList<ManCoDoc> manCoDocs;
        private TempDataDictionary tempDataMock;
        private PromoteManCoDocViewModel promoteManCoDocViewModel;

        private string manCoCode;
        private string selectedSourceEnvironment;
        private string selectedTargetEnvironment;

        [SetUp]
        public void Setup()
        {
            manCoService = new Mock<IManCoService>();
            userService = new Mock<IUserService>();
            manCoDocService = new Mock<IManCoDocService>();
            logger = new Mock<ILogger>();
            appEnvironmentService = new Mock<IAppEnvironmentService>();

            promoteManCoDocsController = new PromoteManCoDocsController(manCoService.Object, userService.Object, manCoDocService.Object, logger.Object, appEnvironmentService.Object);

            this.currentUser = new ApplicationUser("Username");
            this.appEnvironments = new List<AppEnvironment>();
            this.appEnvironments.Add(new AppEnvironment() { Name = "Dev" });
            this.appEnvironments.Add(new AppEnvironment() { Name = "UAT" });

            this.manCos = new List<ManagementCompany>();
            this.manCos.Add(new ManagementCompany() { ManCoID = 1, ManCoCode = "manco1", ManCoName = "Description1" });
            this.manCos.Add(new ManagementCompany() { ManCoID = 2, ManCoCode = "manco2", ManCoName = "Description2" });

            this.manCoDocs = new List<ManCoDoc>();
            this.manCoDocs.Add(new ManCoDoc() { ManCoDocID = 1, ManCoID = 1, DocumentTypeID = 1, PubFileName = "Doc1" });
            this.manCoDocs.Add(new ManCoDoc() { ManCoDocID = 2, ManCoID = 1, DocumentTypeID = 2, PubFileName = "Doc2" });

            this.tempDataMock = new TempDataDictionary();
            manCoCode = "TestManCoCode";
            selectedSourceEnvironment = "TestEnvironment";
            selectedTargetEnvironment = "TargetEnvironment";

            promoteManCoDocViewModel = new PromoteManCoDocViewModel();
            promoteManCoDocViewModel.SelectedSourceEnvironment = this.selectedSourceEnvironment;
            promoteManCoDocViewModel.SelectedTargetEnvironment = this.selectedTargetEnvironment;
            promoteManCoDocViewModel.SelectedManCoDocs = this.manCoDocs.ToList().Select(x => x.ManCoDocID).ToList();

        }

        [Test]
        public void GivenAPromoteManCoDocsController_WhenICallIndexAction_ThenItShouldReturnViewResult()
        {
            this.SetControllerContext(this.promoteManCoDocsController);

            this.userService.Setup(x => x.GetApplicationUser()).Returns(new ApplicationUser("testUser") { PreferredEnvironment = "Dev" });
            this.userService.Setup(x => x.GetApplicationUser(It.IsAny<String>())).Returns(this.currentUser);
            this.appEnvironmentService.Setup(x => x.GetAppEnvironments(It.IsAny<String>())).Returns(this.appEnvironments);
            this.appEnvironmentService.Setup(x => x.GetAppEnvironments()).Returns(this.appEnvironments);
            this.manCoService.Setup(x => x.GetManCos(It.IsAny<String>())).Returns(this.manCos);

            var result = this.promoteManCoDocsController.Index();

            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();

            this.userService.Verify(x => x.GetApplicationUser(), Times.AtLeastOnce);
            this.userService.Verify(x => x.GetApplicationUser(It.IsAny<String>()), Times.AtLeastOnce);
            this.appEnvironmentService.Verify(x => x.GetAppEnvironments(It.IsAny<String>()), Times.AtLeastOnce);
            this.appEnvironmentService.Verify(x => x.GetAppEnvironments(), Times.AtLeastOnce);
            this.manCoService.Verify(x => x.GetManCos(It.IsAny<String>()), Times.AtLeastOnce());

        }

        [Test]
        public void GivenAPromoteManCoDocsController_WhenICallIndexAction_ThenItShouldReturnPromoteManCoDocViewModel()
        {
            this.SetControllerContext(this.promoteManCoDocsController);

            this.userService.Setup(x => x.GetApplicationUser()).Returns(new ApplicationUser("testUser") { PreferredEnvironment = "Dev" });
            this.userService.Setup(x => x.GetApplicationUser(It.IsAny<String>())).Returns(this.currentUser);
            this.appEnvironmentService.Setup(x => x.GetAppEnvironments(It.IsAny<String>())).Returns(this.appEnvironments);
            this.appEnvironmentService.Setup(x => x.GetAppEnvironments()).Returns(this.appEnvironments);
            this.manCoService.Setup(x => x.GetManCos(It.IsAny<String>())).Returns(this.manCos);

            var result = (ViewResult)this.promoteManCoDocsController.Index();

            result.Should().NotBeNull();
            result.Model.Should().BeOfType<PromoteManCoDocViewModel>();

            this.userService.Verify(x => x.GetApplicationUser(), Times.AtLeastOnce);
            this.userService.Verify(x => x.GetApplicationUser(It.IsAny<String>()), Times.AtLeastOnce);
            this.appEnvironmentService.Verify(x => x.GetAppEnvironments(It.IsAny<String>()), Times.AtLeastOnce);
            this.appEnvironmentService.Verify(x => x.GetAppEnvironments(), Times.AtLeastOnce);
            this.manCoService.Verify(x => x.GetManCos(It.IsAny<String>()), Times.AtLeastOnce());
        }

        [Test]
        public void GivenAPromoteManCoDocsController_WhenICallGetManCoDocsAction_ThenItShouldReturnAPartialView()
        {
            this.userService.Setup(x => x.GetApplicationUser()).Returns(this.currentUser);
            this.manCoDocService.Setup(
                x => x.GetManCoDocsByManCoCodeEnvironment(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>()))
                .Returns(this.manCoDocs);

            var result = (PartialViewResult)this.promoteManCoDocsController.GetManCoDocs(It.IsAny<String>(), It.IsAny<String>());

            result.Should().NotBeNull();
            result.Should().BeOfType<PartialViewResult>();
            result.Model.Should().BeOfType<PromoteManCoDocViewModel>();
        }

        [Test]
        public void GivenAPromoteManCoDocsController_GivenValidManCoIdEnvironment_WhenICallGetManCoDocsAction_ThenItShouldReturnPromoteManCoDocViewModel()
        {
            this.userService.Setup(x => x.GetApplicationUser()).Returns(this.currentUser);
            this.manCoDocService.Setup(
                x => x.GetManCoDocsByManCoCodeEnvironment(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>()))
                .Returns(this.manCoDocs);

            var result = (PartialViewResult)this.promoteManCoDocsController.GetManCoDocs(this.manCoCode, this.selectedSourceEnvironment);

            result.Should().NotBeNull();
            result.Model.Should().BeOfType<PromoteManCoDocViewModel>();

            this.userService.Verify(x => x.GetApplicationUser(), Times.AtLeastOnce);
            this.manCoDocService.Verify(x => x.GetManCoDocsByManCoCodeEnvironment(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>()), Times.AtLeastOnce);

        }

        [Test]
        public void GivenAPromoteManCoDocController_WhenICallSubmitManCoDocs_IShouldGetRedirectToRouteResult()
        {
           
            this.manCoDocService.Setup(x => x.PromoteMancoDocs(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>()));

            var result = (RedirectToRouteResult)this.promoteManCoDocsController.SubmitManCoDocs(new PromoteManCoDocViewModel());

            result.Should().NotBeNull();
            result.Should().BeOfType<RedirectToRouteResult>();
            result.RouteValues["action"].ShouldBeEquivalentTo("Index");
            result.RouteValues["controller"].ShouldBeEquivalentTo("PromoteManCoDocs");
        }

        [Test]
        public void GivenAValidData_AndPromoteManCoDocController_WhenICallSubmitManCoDocs_IShouldGetRedirectToRouteResult()
        {
            this.userService.Setup(x => x.GetApplicationUser()).Returns(this.currentUser);
            this.manCoDocService.Setup(x => x.PromoteMancoDocs(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>()));

            var result = (RedirectToRouteResult)this.promoteManCoDocsController.SubmitManCoDocs(this.promoteManCoDocViewModel);

            result.Should().NotBeNull();
            result.Should().BeOfType<RedirectToRouteResult>();
            result.RouteValues["action"].ShouldBeEquivalentTo("Index");
            result.RouteValues["controller"].ShouldBeEquivalentTo("PromoteManCoDocs");

            this.manCoDocService.Verify(x => x.PromoteMancoDocs(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>()), Times.AtLeastOnce());
        }
    }
}
