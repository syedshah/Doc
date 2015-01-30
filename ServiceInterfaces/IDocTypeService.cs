// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAppEnvironmentService.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ServiceInterfaces
{
  using System;
  using System.Collections.Generic;
  using Entities;

  public interface IDocTypeService
  {
    IList<DocumentType> GetDocTypes(String userId, String manCo);

    DocumentType GetDocType(String code, String name);
  }
}
