// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AppEnvironmentRepostory.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   
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

  public class AppEnvironmentRepository : BaseEfRepository<AppEnvironment>, IAppEnvironmentRepository
  {
    public AppEnvironmentRepository(String connectionString)
      : base(new IdentityDb(connectionString))
    {
    }

    public IList<AppEnvironment> GetAppEnvironments(String userId)
    {
      var appEnvironments = this.GetEntityListByStoreProcedure<AppEnvironment>(String.Format("GetAppEnvironmentsByUserId '{0}'", userId));
      return appEnvironments;
    }

    public IList<AppEnvironment> GetAppEnvironments()
    {
        var appEnvironments = this.GetEntityListByStoreProcedure<AppEnvironment>(String.Format("GetAppEnvironments"));
        return appEnvironments;
    }
  }
}
