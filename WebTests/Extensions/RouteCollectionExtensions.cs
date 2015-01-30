// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RouteCollectionExtensions.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//  
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace WebTests.Extensions
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Reflection;
  using System.Web.Mvc;
  using System.Web.Routing;

  using DocProcessingWorkflow.Controllers;

  public static class RouteCollectionExtensions
  {
    public static void MapMvcAttributeRoutesForTesting(this RouteCollection routes)
    {
      var controllers = (from t in typeof(HomeController).Assembly.GetExportedTypes()
                         where
                             t != null &&
                             t.IsPublic &&
                             t.Name.EndsWith("Controller", StringComparison.OrdinalIgnoreCase) &&
                             !t.IsAbstract &&
                             typeof(IController).IsAssignableFrom(t)
                         select t).ToList();
     
      var mapMvcAttributeRoutesMethod = typeof(RouteCollectionAttributeRoutingExtensions)
          .GetMethod(
              "MapMvcAttributeRoutes",
              BindingFlags.NonPublic | BindingFlags.Static,
              null,
              new Type[] { typeof(RouteCollection), typeof(IEnumerable<Type>) },
              null);

      mapMvcAttributeRoutesMethod = typeof(RouteCollectionAttributeRoutingExtensions).GetMethod(
        "MapMvcAttributeRoutes", BindingFlags.NonPublic | BindingFlags.Static);

      mapMvcAttributeRoutesMethod.Invoke(null, new Object[] { routes, controllers });
    }
  }
}
