// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GlobalSettingRepository.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   Doc processing repository
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DocProcessingRepository.Repositories
{
  using System;
  using System.Collections.Generic;
  using DocProcessingRepository.Contexts;
  using DocProcessingRepository.Interfaces;
  using EFRepository;
  using Entities.ADF;

  public class PackStoreRepository : BaseEfRepository<PackStore>, IPackStoreRepository
  {
    public PackStoreRepository(String connectionString)
      : base(new AfdDb(connectionString))
    {

    }

    public IList<PackStore> GetPulledPacks(Int32 jobId)
    {
      return this.GetEntityListByStoreProcedure<PackStore>(String.Format("GetPulledPackStores '{0}'", jobId));
    }

    public IList<PackStore> GetNonPulledPacks(String searchCriteria, Int32 jobId)
    {
      return this.GetEntityListByStoreProcedure<PackStore>(String.Format("GetNonPulledPackStoresBySearch '{0}', '{1}'", searchCriteria, jobId));
    }

    public void UpdatePullStatus(String clientReference, Boolean pullStatus)
    {
      this.ExecuteStoredProcedure(String.Format("UpdatePackStorePulledStatus '{0}', '{1}'", clientReference, pullStatus));
    }
  }
}
