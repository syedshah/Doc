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

  public interface IManCoService
  {
    IList<ManagementCompany> GetManCos(String userId);

    ManagementCompany GetManCo(String code);
  }
}
