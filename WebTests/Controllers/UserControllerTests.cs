// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserControllerTests.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//  Tests for user controller 
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Microsoft.AspNet.Identity.EntityFramework;

namespace WebTests.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
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
    public class UserControllerTests : ControllerTestsBase
    {
        private Mock<IUserService> userService;
        private Mock<IAppEnvironmentService> appEnvironmentService;
        private Mock<ILogger> logger;
        private Mock<IIdentityRoleService> identityRoleService;
        private Mock<IGroupService> groupService;
        private Mock<IUserGroupsService> userGroupService;
        private UserController userController;
        private int pageNumber;
        private int numberOfItems;
        private ApplicationUser currentUser;
        private IList<AppEnvironment> appEnvironments;
        private PagedResult<ApplicationUser> pagedUsers;

        [SetUp]
        public void Setup()
        {
            this.logger = new Mock<ILogger>();
            this.userService = new Mock<IUserService>();
            this.appEnvironmentService = new Mock<IAppEnvironmentService>();
            this.identityRoleService = new Mock<IIdentityRoleService>();
            this.groupService = new Mock<IGroupService>();
            this.userGroupService = new Mock<IUserGroupsService>();

            this.userController = new UserController(this.userService.Object, this.appEnvironmentService.Object,
                this.logger.Object, identityRoleService.Object, groupService.Object, userGroupService.Object);
            SetControllerContext(this.userController);
            this.currentUser = new ApplicationUser("Username");
            this.pageNumber = 1;
            this.numberOfItems = 2;

            var listUsers = new List<ApplicationUser>
                        {
                          new ApplicationUser(),
                          new ApplicationUser(),
                          new ApplicationUser(),
                          new ApplicationUser()
                        };

            this.pagedUsers = new PagedResult<ApplicationUser>
            {
                CurrentPage = pageNumber,
                ItemsPerPage = numberOfItems,
                TotalItems = listUsers.Count,
                Results = listUsers,
                StartRow = ((pageNumber - 1) * numberOfItems) + 1,
                EndRow = (((pageNumber - 1) * numberOfItems) + 1) + (numberOfItems - 1)
            };

            this.userService.Setup(x => x.GetApplicationUser(It.IsAny<String>())).Returns(this.currentUser);
            this.userService.Setup(x => x.GetUsers(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<String>())).Returns(pagedUsers);
            this.groupService.Setup(x => x.GetGroups()).Returns(new List<Group>());
            this.identityRoleService.Setup(x => x.GetRoles()).Returns(new List<String>());
            this.identityRoleService.Setup(x => x.GetRoles(It.IsAny<String>())).Returns(new List<String>());
            this.userGroupService.Setup(x => x.GetUserGroups(It.IsAny<String>())).Returns(new List<UserGroup>());


            this.appEnvironments = new List<AppEnvironment>();
            this.appEnvironments.Add(new AppEnvironment() { Name = "Dev" });
            this.appEnvironments.Add(new AppEnvironment() { Name = "UAT" });

            this.appEnvironmentService.Setup(d => d.GetAppEnvironments(It.IsAny<String>())).Returns(this.appEnvironments);
        }

        [Test]
        public void GivenAUserController_WhenTheIndexPageIsAccessed_ThenTheIndexVieWModelIsReturned()
        {
            this.userService.Setup(x => x.GetApplicationUser(It.IsAny<String>())).Returns(this.currentUser);
            this.userService.Setup(x => x.GetApplicationUser()).Returns(new ApplicationUser("testUser") { PreferredEnvironment = "Test" });

            var result = this.userController.Index(It.IsAny<Int32>(), false);
            result.Should().BeOfType<ViewResult>();
        }

        [Test]
        public void GivenAUserController_WhenTheIndexPageIsAccessed_AndAjaxCallIsTrue_ThenTheIndexVieWModelIsReturned()
        {
            this.userService.Setup(x => x.GetApplicationUser(It.IsAny<String>())).Returns(this.currentUser);
            this.userService.Setup(x => x.GetApplicationUser()).Returns(new ApplicationUser("testUser") { PreferredEnvironment = "Test" });

            var result = this.userController.Index(It.IsAny<int>(), true);
            result.Should().BeOfType<PartialViewResult>();

            var partialViewResult = result as PartialViewResult;

            partialViewResult.Model.Should().BeOfType<UsersViewModel>();
            partialViewResult.ViewName.Should().BeEquivalentTo("_PagedUserResults");
        }

        [Test]
        public void GivenAUserController_WhenTheIndexPageIsAccessed_ThenTheIndexViewShouldContainTheModel()
        {
            this.userService.Setup(x => x.GetApplicationUser(It.IsAny<String>())).Returns(this.currentUser);
            this.userService.Setup(x => x.GetApplicationUser()).Returns(new ApplicationUser("testUser") { PreferredEnvironment = "Test" });

            var result = (ViewResult)this.userController.Index(It.IsAny<int>(), It.IsAny<bool>());
            result.Model.Should().BeOfType<UsersViewModel>();
        }

        [Test]
        public void GivenAUserController_WhenIViewTheIndexPage_ThenTheViewModelContainsTheCorrectNumberOfUsers()
        {
            this.userService.Setup(x => x.GetApplicationUser(It.IsAny<String>())).Returns(this.currentUser);
            this.userService.Setup(x => x.GetApplicationUser()).Returns(new ApplicationUser("testUser") { PreferredEnvironment = "Test" });

            var result = this.userController.Index(It.IsAny<int>(), It.IsAny<bool>()) as ViewResult;

            var model = result.Model as UsersViewModel;

            model.Users.Should().HaveCount(4);
        }

        [Test]
        public void GivenAUserController_WhenCreatePageIsAccessed_ThenTheCreateViewModelIsAccessed()
        {
            var result = this.userController.Create();
            result.Should().BeOfType<ViewResult>();
        }

        [Test]
        public void GivenAUserController_WhenCreatePageIsAccessed_ThenTheCreateViewShouldContainTheModel()
        {
            var result = (ViewResult)this.userController.Create();
            result.Model.Should().BeOfType<AddUserViewModel>();
        }

        [Test]
        public void GivenAValidAddUserViewModel_WhenITryToPerformACreateUser_ThenItCreatesAndReturnsTheIndexView()
        {
            MockHttpContext.SetupGet(x => x.User.Identity.Name).Returns("testUser");
            var addModel = new AddUserViewModel { };

            this.userService.Setup(x => x.CreateUser(
                    It.IsAny<String>(),
                    It.IsAny<String>(),
                    It.IsAny<String>(),
                    It.IsAny<String>(),
                    It.IsAny<String>()));

            ActionResult result = this.userController.Create(addModel);
            result.Should().BeOfType<RedirectToRouteResult>();

            this.userService.Verify(x => x.CreateUser(
                    It.IsAny<String>(),
                    It.IsAny<String>(),
                    It.IsAny<String>(),
                    It.IsAny<String>(),
                    It.IsAny<String>()),
                    Times.AtLeastOnce);
        }

        [Test]
        public void GivenAnInValidAddUserViewModel_WhenITryToPerformACreateUser_ThenItCreatesAndReturnsTheIndexView()
        {
            var addModel = new AddUserViewModel { };
            this.userController.ModelState.AddModelError("FirstName", "Firtsname is required");
            ActionResult result = this.userController.Create(addModel);
            result.Should().BeOfType<ViewResult>();
        }

        [Test]
        public void GivenAValidUser_WhenITryAndEditAUser_ThenIGetTheCorrectView()
        {
            this.userService.Setup(x => x.GetApplicationUser(It.IsAny<String>())).Returns(new ApplicationUser
            {
                UserName = "roth",
                FirstName = "ryan",
                LastName = "Otherman",
                Email = "roth@google.com",
                Id = "guid"
            });

            this.appEnvironmentService.Setup(d => d.GetAppEnvironments("guid")).Returns(new List<AppEnvironment>());

            ActionResult result = this.userController.Edit("roth");
            result.Should().BeOfType<ViewResult>();
        }

        [Test]
        public void GivenAValidEditUserViewModel_WhenITryToPerformAnEdit_ThenItEditsAndReturnsTheHomeController()
        {
            MockHttpContext.SetupGet(x => x.User.Identity.Name).Returns("testUser");

            var editModel = new EditUserViewModel(new ApplicationUser
            {
                Email = "roth@google.com",
                FirstName = "ryan",
                LastName = "Otherman",
                UserName = "roth"
            });
            editModel.SelectedGroups = new List<Int32>();
            editModel.SelectedRoles = new List<String>();


            this.userService.Setup(
                x =>
                x.Updateuser(
                    It.IsAny<String>(),
                    It.IsAny<String>(),
                    It.IsAny<String>(),
                    It.IsAny<String>(),
                    It.IsAny<String>(),
                    It.IsAny<String>(),
                    It.IsAny<String>(),
                    It.IsAny<String>(),
                    It.IsAny<Boolean>(),
                    It.IsAny<Int32>(), It.IsAny<Boolean>()));

            this.identityRoleService.Setup(x => x.AddRolesToUser(It.IsAny<String>(), new List<string>()));
            this.userGroupService.Setup(x => x.AddGroupsToUser(It.IsAny<String>(), new List<int>()));

            ActionResult result = this.userController.Edit("Save", editModel);
            result.Should().BeOfType<RedirectToRouteResult>();

            this.userService.Verify(
               x =>
               x.Updateuser(
                   It.IsAny<String>(),
                   It.IsAny<String>(),
                   It.IsAny<String>(),
                   It.IsAny<String>(),
                   It.IsAny<String>(),
                   It.IsAny<String>(),
                   It.IsAny<String>(),
                   It.IsAny<String>(),
                   It.IsAny<Boolean>(),
                   It.IsAny<Int32>(), It.IsAny<Boolean>()),
                   Times.Once);
        }

        [Test]
        public void GivenAnInvalidEditUserViewModel_WhenITryToPerformAnEdit_AnEditIsNotDonAndTheCurrentPageIsReturned()
        {
            MockHttpContext.SetupGet(x => x.User.Identity.Name).Returns("testUser");
            var editModel = new EditUserViewModel(new ApplicationUser
            {
                FirstName = "ryan",
                LastName = "Otherman",
                UserName = "roth",
                Id = "ksdhsdids"
            });

            const string errorMess = "Email field must not be empty";

            this.userController.ModelState.AddModelError("Email", errorMess);

            ActionResult result = this.userController.Edit("Save", editModel);
            var viewResult = (ViewResult)result;

            result.Should().BeOfType<ViewResult>();
            viewResult.TempData["comment"].ToString().ShouldBeEquivalentTo(errorMess);

            this.appEnvironmentService.Verify(d => d.GetAppEnvironments(It.IsAny<String>()), Times.AtLeastOnce);
            this.userService.Verify(x => x.GetApplicationUser(It.IsAny<String>()), Times.AtLeastOnce);
            this.groupService.Verify(x => x.GetGroups(), Times.AtLeastOnce);
            this.identityRoleService.Verify(x => x.GetRoles(), Times.AtLeastOnce);
            this.identityRoleService.Verify(x => x.GetRoles(It.IsAny<String>()), Times.AtLeastOnce);
            this.userGroupService.Verify(x => x.GetUserGroups(It.IsAny<String>()), Times.AtLeastOnce);
        }

        [Test]
        public void GivenAUserController_WhenTheSearchUserActionIsCalledWithNoSearchCriteria_ThenThePagedUserResultsPartialViewIsReturned()
        {
            this.userService.Setup(x => x.GetApplicationUser(It.IsAny<String>())).Returns(this.currentUser);
            this.userService.Setup(x => x.GetApplicationUser()).Returns(new ApplicationUser("testUser") { PreferredEnvironment = "Test" });

            var result = (PartialViewResult)this.userController.SearchUsers(String.Empty);

            result.Should().NotBeNull();
            result.Should().BeOfType<PartialViewResult>();
            result.Model.Should().BeOfType<UsersViewModel>();
            result.ViewName.Should().BeEquivalentTo("_PagedUserResults");

            this.userService.Verify(x => x.GetApplicationUser(It.IsAny<String>()), Times.AtLeastOnce);
            this.userService.Verify(x => x.GetApplicationUser(), Times.AtLeastOnce);
            this.appEnvironmentService.Verify(x => x.GetAppEnvironments(It.IsAny<String>()), Times.AtLeastOnce);
        }

        [Test]
        public void GivenAUserController_WhenTheSearchUserActionIsCalledWithSearchCriteria_ThenThePagedUserResultsPartialViewIsReturned()
        {
            this.userService.Setup(x => x.GetApplicationUser(It.IsAny<String>())).Returns(this.currentUser);
            this.userService.Setup(x => x.GetUsers(It.IsAny<Int32>(), It.IsAny<Int32>(), It.IsAny<String>(), It.IsAny<String>())).Returns(this.pagedUsers);
            this.userService.Setup(x => x.GetApplicationUser()).Returns(new ApplicationUser("testUser") { PreferredEnvironment = "Test" });
            this.appEnvironmentService.Setup(x => x.GetAppEnvironments()).Returns(this.appEnvironments);

            var result = (PartialViewResult)this.userController.SearchUsers("Search Criteria");

            result.Should().NotBeNull();
            result.Should().BeOfType<PartialViewResult>();
            result.Model.Should().BeOfType<UsersViewModel>();
            result.ViewName.Should().BeEquivalentTo("_PagedUserResults");

            this.userService.Verify(x => x.GetApplicationUser(It.IsAny<String>()), Times.AtLeastOnce);
            this.userService.Verify(x => x.GetApplicationUser(), Times.AtLeastOnce);
            this.appEnvironmentService.Verify(x => x.GetAppEnvironments(It.IsAny<String>()), Times.AtLeastOnce);
        }

        [Test]
        public void GivenAUserController_WhenIWantToAssignSearchValueToTempData_TempDataContainsValue()
        {
            var result = this.userController.AssignSearchValue(It.IsAny<String>());

            result.Should().NotBeNull();
            result.Should().BeOfType<JsonResult>();

            this.userController.TempData.ContainsKey("SearchUser").ShouldBeEquivalentTo(true);
        }

        [Test]
        public void GivenAUserController_WhenIWantToDeleteUser_IShouldGetRedirectToRouteResult()
        {
            this.userService.Setup(x => x.GetApplicationUser(It.IsAny<String>())).Returns(this.currentUser);

            this.userService.Setup(
               x =>
               x.Updateuser(
                   It.IsAny<String>(),
                   It.IsAny<String>(),
                   It.IsAny<String>(),
                   It.IsAny<String>(),
                   It.IsAny<String>(),
                   It.IsAny<String>(),
                   It.IsAny<String>(),
                   It.IsAny<String>(),
                   It.IsAny<Boolean>(),
                   It.IsAny<Int32>(), It.IsAny<Boolean>()));

            var result = this.userController.Delete(It.IsAny<String>());
            result.Should().NotBeNull();
            result.Should().BeOfType<RedirectToRouteResult>();

            this.userService.Verify(x => x.GetApplicationUser(It.IsAny<String>()), Times.AtLeastOnce);
            this.userService.Verify(
          x =>
          x.Updateuser(
              It.IsAny<String>(),
              It.IsAny<String>(),
              It.IsAny<String>(),
              It.IsAny<String>(),
              It.IsAny<String>(),
              It.IsAny<String>(),
              It.IsAny<String>(),
              It.IsAny<String>(),
              It.IsAny<Boolean>(),
              It.IsAny<Int32>(), It.IsAny<Boolean>()),
              Times.Once);
        }

        [Test]
        public void GivenAUserController_WhenIWantToEnableUser_IShouldGetRedirectToRouteResult()
        {
            this.userService.Setup(x => x.GetApplicationUser(It.IsAny<String>())).Returns(this.currentUser);

            this.userService.Setup(
               x =>
               x.Updateuser(
                   It.IsAny<String>(),
                   It.IsAny<String>(),
                   It.IsAny<String>(),
                   It.IsAny<String>(),
                   It.IsAny<String>(),
                   It.IsAny<String>(),
                   It.IsAny<String>(),
                   It.IsAny<String>(),
                   It.IsAny<Boolean>(),
                   It.IsAny<Int32>(), It.IsAny<Boolean>()));

            var result = this.userController.EnableUser(It.IsAny<String>());
            result.Should().NotBeNull();
            result.Should().BeOfType<RedirectToRouteResult>();

            this.userService.Verify(x => x.GetApplicationUser(It.IsAny<String>()), Times.AtLeastOnce);
            this.userService.Verify(
          x =>
          x.Updateuser(
              It.IsAny<String>(),
              It.IsAny<String>(),
              It.IsAny<String>(),
              It.IsAny<String>(),
              It.IsAny<String>(),
              It.IsAny<String>(),
              It.IsAny<String>(),
              It.IsAny<String>(),
              It.IsAny<Boolean>(),
              It.IsAny<Int32>(), It.IsAny<Boolean>()),
              Times.Once);
        }
    }
}
