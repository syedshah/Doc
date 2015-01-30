// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AppEnvironmentRepostory.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   Repo for Mancos
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

  public class ManCoRepostory : BaseEfRepository<ManagementCompany>, IManCoRepository
  {
    public ManCoRepostory(String connectionString)
      : base(new IdentityDb(connectionString))
    {
    }

    public IList<ManagementCompany> GetManCos(String userId)
    {
      return this.GetEntityListByStoreProcedure<ManagementCompany>(String.Format("GetManCos '{0}'", userId));
    }
  }
}
