// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IOneStepFileRepository.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   Defines the IOneStepFileRepository type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FileRepository.Interfaces
{
  using System;

  using Entities.File;

  public interface IOneStepFileRepository
  {
    OneStepLog GetOneStepLog(String logPath);
  }
}
