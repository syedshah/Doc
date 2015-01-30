// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestableBaseController.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   Defines the TestableBaseController type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace WebTests.Controllers
{
  using System;
  using System.Web.Mvc;

  using DocProcessingWorkflow.Controllers;
  using DocProcessingWorkflow.Models;

  using Logging;

  using ServiceInterfaces;

  public class TestableBaseController : BaseController
  {
    public TestableBaseController(ILogger logger, IUserService userService, IAppEnvironmentService appEnvironmentService)
      : base(logger, appEnvironmentService, userService)
    {
    }

    public void OnException(System.Web.Mvc.ExceptionContext filterContext)
    {
      base.OnException(filterContext);
    }

    public JsonResult JsonError(Exception e, ErrorCode errorCode, String displayMessage)
    {
      return base.JsonError(e, errorCode, displayMessage);
    }
  }
}
