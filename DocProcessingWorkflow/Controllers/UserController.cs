// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserController.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using Microsoft.AspNet.Identity.EntityFramework;
using WebGrease.Css.Extensions;

namespace DocProcessingWorkflow.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using Models.User;
    using Logging;
    using ServiceInterfaces;
    using Entities;
    using Constants;
    using Filters;

    [AuthorizeLoggedInUser]
    public class UserController : BaseController
    {
        private const Int32 PageSize = 10;
        private readonly IUserService userService;
        private readonly IAppEnvironmentService appEnvironmentService;
        private readonly IIdentityRoleService _identityRoleService;
        private readonly IGroupService _groupService;
        private readonly IUserGroupsService _userGroupsService;

        public UserController(IUserService userService, IAppEnvironmentService appEnvironmentService, ILogger logger,
            IIdentityRoleService identityRoleService, IGroupService groupService, IUserGroupsService userGroupsService)
            : base(logger, appEnvironmentService, userService)
        {
            this.userService = userService;
            this.appEnvironmentService = appEnvironmentService;
            this._identityRoleService = identityRoleService;
            _groupService = groupService;
            _userGroupsService = userGroupsService;
        }

        [HttpGet]
        [Route("User/Index")]
        public ActionResult Index(Int32 page = 1, Boolean isAjaxCall = false)
        {
            var environmentViewModel = this.SetEnvironmentSession();
            var users = this.userService.GetUsers(page, PageSize, environmentViewModel.SelectedEnvironment);

            var usersViewModel = new UsersViewModel();

            usersViewModel.AddUsers(users);

            if (isAjaxCall)
            {
                return this.PartialView("_PagedUserResults", usersViewModel);
            }

            return this.View(usersViewModel);
        }

        [HttpGet]
        [Route("User/Create")]
        public virtual ActionResult Create()
        {
            var addModel = new AddUserViewModel();
            addModel.UserGroupsRoles = BuildUserGroupsRolesViewModel();
            return this.View(addModel);
        }

        [HttpPost]
        [Route("User/Create")]
        public virtual ActionResult Create(AddUserViewModel user)
        {
            if (ModelState.IsValid)
            {
                this.userService.CreateUser(user.UserName, user.Password, user.FirstName, user.LastName, user.Email);
                var newUser = userService.GetApplicationUser(user.UserName);
                this._identityRoleService.AddRolesToUser(newUser.Id, user.SelectedRoles ?? new List<string>());
                this._userGroupsService.AddGroupsToUser(newUser.Id, user.SelectedGroups ?? new List<int>());

                Logger.Info(string.Format("{0} created", user.UserName));
                return this.RedirectToAction("Index");
            }
            else
            {
                var errorList = this.ModelState.Values.SelectMany(x => x.Errors).ToList();

                if (errorList.Count > 0)
                {
                    this.TempData["comment"] = errorList[0].ErrorMessage;
                }
                else
                {
                    this.TempData["comment"] = "Required fields are empty";
                }

                user.UserGroupsRoles = BuildUserGroupsRolesViewModel();
                return this.View(user);
            }
        }

        [HttpGet]
        [Route("User/Edit/{userName}")]
        [OutputCache(Duration = 0, VaryByParam = "")]
        public virtual ActionResult Edit(String userName)
        {
            var user = this.userService.GetApplicationUser(userName);
            var editModel = new EditUserViewModel(user);
            this.AddPreferredLandingEnvironments(editModel);
            editModel.UserGroupsRoles = BuildUserGroupsRolesViewModel(user);

            return this.View(editModel);
        }

        [HttpPost]
        [Route("User/Edit/{userName}")]
        [ValidateAntiForgeryToken]
        [OutputCache(Duration = 0, VaryByParam = "")]
        public virtual ActionResult Edit(String submitButton, EditUserViewModel editModel)
        {
            var user = this.userService.GetApplicationUser(editModel.UserName);

            if (!this.ModelState.IsValid)
            {
                var errorList = this.ModelState.Values.SelectMany(x => x.Errors).ToList();

                if (errorList.Count > 0)
                {
                    this.TempData["comment"] = errorList[0].ErrorMessage;
                }
                else
                {
                    this.TempData["comment"] = "Required fields are empty";
                }
                this.AddPreferredLandingEnvironments(editModel);
                editModel.UserGroupsRoles = BuildUserGroupsRolesViewModel(user);
                return this.View(editModel);
            }

            if (editModel.IsDeActivated)
            {
                editModel.IsLockedOut = true;
            }

            if (!editModel.IsLockedOut)
            {
                editModel.FailedLogInCount = 0;
            }

            this.userService.Updateuser(
              editModel.UserName,
              editModel.Password,
              editModel.FirstName,
              editModel.LastName,
              editModel.Email,
              editModel.SelectedPrefLandingPage,
              editModel.SelectedPrefEnvironment,
              editModel.Phone,
              editModel.IsLockedOut,
              editModel.FailedLogInCount,
              editModel.IsDeActivated);
            if (editModel.SelectedRoles == null)
                editModel.SelectedRoles = new List<String>();
            if (editModel.SelectedGroups == null)
                editModel.SelectedGroups = new List<Int32>();

            if (System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.User.IsInRole(Role.DSTAdmin))
            {
                this._identityRoleService.AddRolesToUser(user.Id, editModel.SelectedRoles);
                this._userGroupsService.AddGroupsToUser(user.Id, editModel.SelectedGroups);
            }

            Logger.Info(
              string.Format(
                "User {0} changed. New details first name: {1}, last name: {2}, email: {3}, phone: {4}, isLockedOut {5}, landing page: {6}, landing environment: {7} ,Roles: {8}, UserGroupIds:{9}",
                HttpContext.User.Identity.Name,
                editModel.FirstName,
                editModel.LastName,
                editModel.Email,
                editModel.Phone,
                editModel.IsLockedOut,
                editModel.SelectedPrefLandingPage,
                editModel.SelectedPrefLandingPage,
                String.Join(",", editModel.SelectedRoles.ToArray()),
                String.Join(",", editModel.SelectedGroups.ToArray())));

            if (String.IsNullOrEmpty(user.PreferredLandingPage))
            {
                return this.RedirectToAction("Index", "Home");    
            }
            else
            {
                var currentuser = this.userService.GetApplicationUser(HttpContext.User.Identity.Name);
                return RedirectToAction("RedirectToLandingPage", "Session", new { defaultLandingUrl = currentuser.PreferredLandingPage });
            }
            
        }

        [Route("User/Delete/{userName}")]
        [Authorize(Roles = Role.DSTAdmin)]
        [OutputCache(Duration = 0, VaryByParam = "")]
        public virtual ActionResult Delete(String userName)
        {
            var user = this.userService.GetApplicationUser(userName);
            user.IsDeActivated = true;
            user.IsLockedOut = true;

            this.userService.Updateuser(
                user.UserName,
                null,
                user.FirstName,
                user.LastName,
                user.Email,
                user.PreferredLandingPage,
                user.PreferredEnvironment,
                user.Phone,
                user.IsLockedOut,
                user.FailedLogInCount,
                user.IsDeActivated);

            Logger.Info(
             string.Format(
               "User {0} Deactivated",
               HttpContext.User.Identity.Name));

            return this.RedirectToAction("Index", "User");
        }

        [Route("User/Enable/{userName}")]
        [Authorize(Roles = Role.DSTAdmin)]
        [OutputCache(Duration = 0, VaryByParam = "")]
        public virtual ActionResult EnableUser(String userName)
        {
            var user = this.userService.GetApplicationUser(userName);
            user.IsDeActivated = false;
            user.IsLockedOut = false;
            user.FailedLogInCount = 0;

            this.userService.Updateuser(
                user.UserName,
                null,
                user.FirstName,
                user.LastName,
                user.Email,
                user.PreferredLandingPage,
                user.PreferredEnvironment,
                user.Phone,
                user.IsLockedOut,
                user.FailedLogInCount,
                user.IsDeActivated);

            Logger.Info(
            string.Format(
              "User {0} Activated",
              HttpContext.User.Identity.Name));

            return this.RedirectToAction("Index", "User");
        }

        [Route("User/SearchUsers")]
        public ActionResult SearchUsers(string searchCriteria, Int32 page = 1)
        {
            if (TempData.ContainsKey("SearchUser"))
            {
                searchCriteria = this.TempData["SearchUser"].ToString();
            }

            var environmentViewModel = this.SetEnvironmentSession();

            PagedResult<ApplicationUser> users = (String.IsNullOrEmpty(searchCriteria) || searchCriteria.Length < 1) ?
                this.userService.GetUsers(page, PageSize, environmentViewModel.SelectedEnvironment) :
                this.userService.GetUsers(page, PageSize, searchCriteria, environmentViewModel.SelectedEnvironment);

            var usersViewModel = new UsersViewModel();
            usersViewModel.UserSearchValue = searchCriteria;
            usersViewModel.CurrentPage = page.ToString();
            usersViewModel.UserSearchValue = searchCriteria;

            var currentuser = this.userService.GetApplicationUser(HttpContext.User.Identity.Name);
            this.SetDstAdminRole(currentuser, usersViewModel);

            usersViewModel.AddUsers(users);

            return this.PartialView("_PagedUserResults", usersViewModel);
        }

        [Route("User/AssignSearchValue")]
        public ActionResult AssignSearchValue(String searchfield)
        {
            this.TempData["SearchUser"] = searchfield;
            return this.Json(new { Success = true });
        }

        private void SetDstAdminRole(ApplicationUser currentuser, UsersViewModel model)
        {
            if (currentuser.Roles.Select(x => x.Role.Name).ToList().Contains(Role.DSTAdmin))
            {
                model.IsDSTAdmin = true;
            }
        }

        private void AddPreferredLandingEnvironments(EditUserViewModel model)
        {
            var environments = this.appEnvironmentService.GetAppEnvironments(model.Id);

            model.AddAllowedEnvironments(environments);
        }

        private UserGroupsRolesViewModel BuildUserGroupsRolesViewModel(IdentityUser user = null)
        {
            var userGroupsRolesViewModel = new UserGroupsRolesViewModel();
            var groups = this._groupService.GetGroups();
            var availableRoles = this._identityRoleService.GetRoles().ToList();
            var userRoles = (user == null) ? new List<String>() : this._identityRoleService.GetRoles(user.Id).ToList();
            var userGroups = (user == null) ? new List<UserGroup>() : this._userGroupsService.GetUserGroups(user.Id);

            userGroupsRolesViewModel.AvailableRoles = availableRoles;
            userGroupsRolesViewModel.AddGroups(groups);
            userGroupsRolesViewModel.AddUserGroups(userGroups);
            userGroupsRolesViewModel.UserAssignedRoles = userRoles;

            return userGroupsRolesViewModel;
        }

    }
}