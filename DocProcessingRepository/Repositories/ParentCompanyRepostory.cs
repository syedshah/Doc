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
  using DocProcessingRepository.Contexts;
  using DocProcessingRepository.Interfaces;
  using EFRepository;
  using Entities;

  public class ParentCompanyRepostory : BaseEfRepository<ParentCompany>, IParentCompanyRepostory
  {
    public ParentCompanyRepostory(String connectionString)
      : base(new IdentityDb(connectionString))
    {
    }
  }
}
