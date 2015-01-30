// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PasswordController.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DocProcessingWorkflow.Controllers
{
    using System.Web.Mvc;
    using DocProcessingWorkflow.Models.Password;
    using Logging;
    using ServiceInterfaces;

    public class PasswordController : BaseController
    {
        private readonly IUserService userService;

        public PasswordController(
          IUserService userService,
          ILogger logger,
          IAppEnvironmentService appEnvironmentService)
            : base(logger, appEnvironmentService, userService)
        {
            this.userService = userService;
        }

        public ActionResult Index()
        {
            return this.View();
        }

        [Route("Password/ChangeCurrent")]
        public ActionResult ChangeCurrent()
        {
            var user = this.userService.GetApplicationUser(TempData["username"].ToString());

            var changeCurrentPasswordModel = new ChangeCurrentPasswordModel(user.Id, user.UserName, user.FirstName, user.LastName);

            ViewBag.LastLoginDate = user.LastLoginDate;

            return this.View(changeCurrentPasswordModel);
        }

        [HttpPost]
        [Route("Password/ChangeCurrent")]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeCurrent(ChangeCurrentPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                this.userService.ChangePassword(model.UserId, model.CurrentPassword, model.NewPassword);

                var user = this.userService.GetApplicationUserById(model.UserId);

                this.userService.SignIn(user, false);

                this.userService.UpdateUserLastLogindate(model.UserId);

                Logger.Info(string.Format("{0} changed password", HttpContext.User.Identity.Name));

                return this.RedirectToAction("Index", "Home");
            }
            else
            {
                this.TempData["message"] = "Please correct the errors and try again.";
                return this.View(model);
            }
        }
    }
}