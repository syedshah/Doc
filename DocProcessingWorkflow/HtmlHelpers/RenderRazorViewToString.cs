// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RenderRazorViewToString.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DocProcessingWorkflow.HtmlHelpers
{
  using System;
  using System.IO;
  using System.Web.Mvc;

  public class RenderRazorView
  {
    public static String RenderRazorViewToString(String viewName, Object model, ControllerContext controllerContext, ViewDataDictionary viewData, TempDataDictionary tempData)
    {
      viewData.Model = model;
      using (var sw = new StringWriter())
      {
        var viewResult = ViewEngines.Engines.FindPartialView(controllerContext, viewName);
        var viewContext = new ViewContext(controllerContext, viewResult.View, viewData, tempData, sw);
        viewResult.View.Render(viewContext, sw);
        viewResult.ViewEngine.ReleaseView(controllerContext, viewResult.View);
        return sw.GetStringBuilder().ToString();
      }
    }
  }
}