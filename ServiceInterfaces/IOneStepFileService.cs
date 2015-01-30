// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IOneStepFileService.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   IOne Step File Service object
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ServiceInterfaces
{
  using System;

  using Entities.File;

  public interface IOneStepFileService
  {
    OneStepLog GetOneStepLog(String grid, String environment);
  }
}
