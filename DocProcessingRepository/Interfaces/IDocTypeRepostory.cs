// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAppEnvironmentRepostory.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DocProcessingRepository.Interfaces
{
  using System;
  using System.Collections.Generic;
  using Entities;

  public interface IDocTypeRepostory
  {
    IList<DocumentType> GetDocTypes(String userId, String manCo);

    DocumentType GetDocType(String code, String name);
  }
}
