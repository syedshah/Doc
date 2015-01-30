// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnvironmentTypeService.cs" company="DST Nexdox">
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
  using Entities.ADF;
  using Exceptions;
  using ServiceInterfaces;

  public class PackStoreService : IPackStoreService
  {
    private readonly IPackStoreRepository packStoreRepository;

    public PackStoreService(IPackStoreRepository packStoreRepository)
    {
      this.packStoreRepository = packStoreRepository;
    }

    public IList<PackStore> GetPulledPacks(Int32 jobId)
    {
      try
      {
        return this.packStoreRepository.GetPulledPacks(jobId);
      }
      catch (Exception e)
      {
        throw new DocProcessingException("Unable to get pulled packs", e);
      }
    }

    public IList<PackStore> GetNonPulledPacks(String searchCriteria, Int32 jobId)
    {
      try
      {
        return this.packStoreRepository.GetNonPulledPacks(searchCriteria, jobId);
      }
      catch (Exception e)
      {
        throw new DocProcessingException("Unable to search packs", e);
      }
    }

    public void UpdatePullStatus(String clientReference, Boolean pullStatus)
    {
      try
      {
        this.packStoreRepository.UpdatePullStatus(clientReference, pullStatus);
      }
      catch (Exception e)
      {
        throw new DocProcessingException("Unable to update pull status", e);
      }
    }
  }
}
