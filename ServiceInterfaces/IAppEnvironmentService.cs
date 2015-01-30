// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAppEnvironmentService.cs" company="DST Nexdox">
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
  using Entities;

  public interface IAppEnvironmentService
  {
      IList<AppEnvironment> GetAppEnvironments(String userId);
    IList<AppEnvironment> GetAppEnvironments();
  }
}
