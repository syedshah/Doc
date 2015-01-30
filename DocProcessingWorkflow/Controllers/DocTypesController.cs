// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DocTypesController.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   Controller for doc types
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using DocProcessingWorkflow.Filters;

namespace DocProcessingWorkflow.Controllers
{
  using System;
  using System.Web.Mvc;
  using DocProcessingWorkflow.Models.DocType;
  using Logging;
  using ServiceInterfaces;

      [AuthorizeLoggedInUser]
  public class DocTypesController : BaseController
  {
    private readonly IDocTypeService docTypeService;
    private readonly IUserService userService;

    public DocTypesController(
        IDocTypeService docTypeService, 
        IUserService userService,
        IAppEnvironmentService appEnvironmentService,
        ILogger logger)
      : base(logger, appEnvironmentService, userService)
    {
      this.docTypeService = docTypeService;

      this.userService = userService;
      this.docTypeService = docTypeService;
    }

    [Route("DocTypes/Search")]
    public ActionResult Search(String manCo)
    {
      var currentUser = this.userService.GetApplicationUser();
      var docTypes = this.docTypeService.GetDocTypes(currentUser.Id, manCo);

      var model = new DocTypeJsonResponse();
      model.AddDocTypes(docTypes);

      return this.Json(model.DocTypes, JsonRequestBehavior.AllowGet);
    }
  }
}