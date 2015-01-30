// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SessionController.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//  Manages user sessions 
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DocProcessingWorkflow.Controllers
{
    using System;
    using System.Web.Mvc;

    using DocProcessingWorkflow.Constants;
    using DocProcessingWorkflow.Models.User;
    using Logging;
    using ServiceInterfaces;

    public class SessionController : BaseController
    {
        private readonly IUserService userService;

        public SessionController(IUserService userService, ILogger logger, IAppEnvironmentService appEnvironmentService)
            : base(logger, appEnvironmentService, userService)
        {
            this.userService = userService;
        }

        [Route("Session/New")]
        public ActionResult New()
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return this.View();
            }

            Logger.Info(string.Format("{0} logged in", HttpContext.User.Identity.Name));
            return this.RedirectToAction("Index", "Home");
        }

        [Route("Session/Create")]
        [ValidateAntiForgeryToken]
        public ActionResult Create(LoginUserViewModel userViewModel, String returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = this.userService.GetApplicationUser(userViewModel.Username.Trim(), userViewModel.Password.Trim());

                if (user != null)
                {
                    if (user.IsLockedOut || user.IsDeActivated)
                    {
                        Logger.Info(string.Format("{0} locked out", userViewModel.Username));
                        return this.RedirectToAction("LockedOut", "Session", new { userName = userViewModel.Username });
                    }

                    if (this.userService.CheckForPassRenewal(user.LastPasswordChangedDate, user.LastLoginDate.GetValueOrDefault()))
                    {
                        return this.RedirectToAction("ChangePassword", "Account", new { userName = userViewModel.Username });
                    }

                    this.userService.SignIn(user, false);
                    this.userService.UpdateUserLastLogindate(user.Id);

                    Logger.Info(string.Format("{0} logged in", user.UserName));
                    return this.RedirectToLocal(returnUrl, user.PreferredLandingPage);
                }
                else
                {
                    user = this.userService.GetApplicationUser(userViewModel.Username);

                    if (user != null)
                    {
                        this.userService.UpdateUserFailedLogin(user.Id);
                        Logger.Info(string.Format("{0} failed log in count now {1}", user.UserName, user.FailedLogInCount + 1));

                        if (this.userService.IsLockedOut(user.Id))
                        {
                            Logger.Info(string.Format("{0} locked out", userViewModel.Username));
                            return this.RedirectToAction("LockedOut", "Session", new { userName = userViewModel.Username });
                        }
                    }

                    Logger.Info(String.Format("{0} failed to log on", userViewModel.Username));
                    this.TempData["message"] = "Login was unsuccessful. Please correct the errors and try again.";

                    return this.View("New");
                }
            }

            return this.View("New");
        }

        [Route("Session/LockedOut")]
        public ActionResult LockedOut()
        {
            return this.View();
        }

        [Route("Session/Remove")]
        public ActionResult Remove()
        {
            Logger.Info(string.Format("{0} logged off", HttpContext.User.Identity.Name));
            this.userService.SignOut();
            Session.Clear();
            return this.RedirectToAction("New");
        }

        private ActionResult RedirectToLocal(String returnUrl, String defaultLandingUrl)
        {
            if (!String.IsNullOrEmpty(returnUrl))
            {
                return this.Redirect(returnUrl);
            }
            else
            {
                if (String.IsNullOrEmpty(defaultLandingUrl))
                {
                    return this.RedirectToAction("Index", "Home");
                }
                else
                {
                    return this.RedirectToLandingPage(defaultLandingUrl);
                }
            }
        }

        [Route("Session/RedirectToLandingPage")]
        public ActionResult RedirectToLandingPage(String defaultLandingUrl)
        {
            RedirectToRouteResult redirectToRouteResult = new RedirectToRouteResult(null);
            switch (defaultLandingUrl)
            {
                case LandingPage.Home:
                    redirectToRouteResult = this.RedirectToAction("Index", "Home");
                    break;
                case LandingPage.JobsManageInserts:
                    redirectToRouteResult = this.RedirectToAction("Index", "Jobs");
                    break;
                case LandingPage.JobsSubmit:
                    redirectToRouteResult = this.RedirectToAction("Index", "SubmitJobs");
                    break;
                case LandingPage.JobsView:
                    redirectToRouteResult = this.RedirectToAction("Index", "Jobs");
                    break;
                default:
                    redirectToRouteResult = this.RedirectToAction("Index", "Home");
                    break;
            }

            return redirectToRouteResult;
        }

        [Route("Session/Expired")]
        public ActionResult Expired()
        {
            this.userService.SignOut();
            Session.Clear();
            return this.View();
        }

        [Route("Session/SessionReset")]
        public void SessionReset()
        {
        }
    }
}