// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnvironmentTypeRepository.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   Repo for Environment Types
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

  public class EnvironmentTypeRepository : BaseEfRepository<EnvironmentType>, IEnvironmentTypeRepository
  {
    public EnvironmentTypeRepository(String connectionString)
      : base(new IdentityDb(connectionString))
    {
    }

    public IList<EnvironmentServerEntity> GetEnvironmentServers(String environmentType)
    {
      var servers = this.GetEntityListByStoreProcedure<EnvironmentServerEntity>(String.Format("GetEnvironmentServers '{0}'", environmentType));
      return servers;
    }
  }
}
