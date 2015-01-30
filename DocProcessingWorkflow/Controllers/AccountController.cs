// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AccountController.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   Account controller
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DocProcessingWorkflow.Controllers
{
  using System;
  using System.Threading.Tasks;
  using System.Web.Mvc;

  using DocProcessingWorkflow.Filters;
  using DocProcessingWorkflow.Models;
  using DocProcessingWorkflow.Models.User;

  using Logging;
  using Microsoft.AspNet.Identity;

  using ServiceInterfaces;

  public class AccountController : BaseController
  {
    private IUserService userService;

    public AccountController(
      ILogger logger,
      IAppEnvironmentService appEnvironmentService,
      IUserService userService)
      : base(logger, appEnvironmentService, userService)
    {
      this.userService = userService;
    }

    [Route("Account/ChangePassword/{userName}")]
    public ActionResult ChangePassword(String userName)
    {
      if (string.IsNullOrEmpty(userName))
      {
        return this.RedirectToAction("New", "Session");
      }
      else
      {
        this.TempData["username"] = userName;
        return this.RedirectToAction("ChangeCurrent", "Password", userName);
      }
    }

    [ChildActionOnly]
    [AuthorizeLoggedInUser]
    [Route("Account/Summary/")]
    public ActionResult Summary()
    {
      var user = this.userService.GetApplicationUser();

      var modelOut = new UserSummaryViewModel(user);

        return PartialView("_Summary", modelOut);
    }

    public enum ManageMessageId
    {
      ChangePasswordSuccess,
      SetPasswordSuccess,
      RemoveLoginSuccess,
      Error
    }

    private void AddErrors(IdentityResult result)
    {
      foreach (var error in result.Errors)
      {
        ModelState.AddModelError("", error);
      }
    }
  }
}