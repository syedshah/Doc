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

  using Entities;

  public interface ISubmitJobFileRepository
  {
    void SaveTriggerFile(Int32 jobId, SubmitFile submitFile, String path);
  }
}
