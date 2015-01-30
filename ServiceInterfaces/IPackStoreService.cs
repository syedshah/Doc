// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IPackStoreService.cs" company="DST Nexdox">
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
  using Entities.ADF;

  public interface IPackStoreService
  {
    IList<PackStore> GetPulledPacks(Int32 jobId);

    IList<PackStore> GetNonPulledPacks(String searchCriteria, Int32 jobId);

    void UpdatePullStatus(String clientReference, Boolean pullStatus);
  }
}
