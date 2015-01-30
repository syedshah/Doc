// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AppEnvironmentService.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Services
{
  using System;
  using System.Collections.Generic;
  using DocProcessingRepository.Interfaces;
  using Entities;
  using Exceptions;
  using ServiceInterfaces;

  public class DocTypeService : IDocTypeService
  {
    private readonly IDocTypeRepostory docTypeRepostory;

    public DocTypeService(IDocTypeRepostory docTypeRepostory)
    {
      this.docTypeRepostory = docTypeRepostory;
    }

    public IList<DocumentType> GetDocTypes(String userId, String manCo)
    {
      try
      {
        return this.docTypeRepostory.GetDocTypes(userId, manCo);
      }
      catch (Exception e)
      {
        throw new DocProcessingException("Unable to retrieve doc types", e);
      }
    }

    public DocumentType GetDocType(String code, String name)
    {
      try
      {
        return this.docTypeRepostory.GetDocType(code, name);
      }
      catch (Exception e)
      {
        throw new DocProcessingException("Unable to retrieve doc type", e);
      }
    }
  }
}
