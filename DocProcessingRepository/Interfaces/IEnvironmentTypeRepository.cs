// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IEnvironmentTypeRepository.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   Repo for Environment Types
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DocProcessingRepository.Interfaces
{
  using System;
  using System.Collections.Generic;

  using Entities;

  public interface IEnvironmentTypeRepository
  {
    IList<EnvironmentServerEntity> GetEnvironmentServers(String environmentType);
  }
}
