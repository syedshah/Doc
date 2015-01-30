// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HomeController.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DocProcessingWorkflow.Controllers
{
  using System.Web.Mvc;

  using DocProcessingWorkflow.Filters;

  [AuthorizeLoggedInUser]
  public class HomeController : Controller
  {
    public ActionResult Index()
    {
      return View();
    }

    public ActionResult About()
    {
      ViewBag.Message = "Your application description page.";

      return View();
    }

    public ActionResult Contact()
    {
      ViewBag.Message = "Your contact page.";

      return View();
    }
  }
}