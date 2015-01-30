// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAppEnvironmentRepostory.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DocProcessingRepository.Interfaces
{
  using System;
  using System.Collections.Generic;
  using Entities;

  public interface IAppEnvironmentRepository
  {
    IList<AppEnvironment> GetAppEnvironments(String userId);
    IList<AppEnvironment> GetAppEnvironments();
  }
}
