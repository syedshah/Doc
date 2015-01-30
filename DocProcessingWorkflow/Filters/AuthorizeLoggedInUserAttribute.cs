// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuthorizeLoggedInUserAttribute.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DocProcessingWorkflow.Filters
{
  using System.Web.Mvc;

  public class AuthorizeLoggedInUserAttribute : AuthorizeAttribute
  {
    public AuthorizeLoggedInUserAttribute()
    {
    }

    protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
    {
      filterContext.Result = new RedirectResult("~/session/new");
    }
  }
}