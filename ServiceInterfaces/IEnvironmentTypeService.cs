// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IEnvironmentTypeService.cs" company="DST Nexdox">
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

  public interface IEnvironmentTypeService
  {
    IList<EnvironmentServerEntity> GetEnvironmentServers(String environmentType);
  }
}
