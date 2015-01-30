// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RouteConfig.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DocProcessingWorkflow
{
  using System.Web.Mvc;
  using System.Web.Routing;

  public static class RouteConfig
  {
    public static void RegisterRoutes(this RouteCollection routes)
    {
      routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

      ////routes.MapMvcAttributeRoutes();

      routes.MapRoute(
        name: "Default-Home", 
        url: "", 
        defaults: new { controller = "Home", action = "Index" });
    }
  }
}
