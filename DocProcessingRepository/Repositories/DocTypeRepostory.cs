// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DocTypeRepostory.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   Repo for Doc Types
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DocProcessingRepository.Repositories
{
  using System;
  using System.Collections.Generic;
  using DocProcessingRepository.Contexts;
  using DocProcessingRepository.Interfaces;
  using EFRepository;
  using Entities;

  public class DocTypeRepostory : BaseEfRepository<DocumentType>, IDocTypeRepostory
  {
    public DocTypeRepostory(String connectionString)
      : base(new IdentityDb(connectionString))
    {
    }

    public IList<DocumentType> GetDocTypes(String userId, String manCo)
    {
      return this.GetEntityListByStoreProcedure<DocumentType>(String.Format("GetDocTypes '{0}', '{1}'", userId, manCo));
    }

    public DocumentType GetDocType(String code, String name)
    {
      return this.SelectStoredProcedure<DocumentType>(String.Format("GetDocType '{0}', '{1}'", code, name));
    }
  }
}
