// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IInputFileRepository.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   Repo for Input FILE
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DocProcessingRepository.Interfaces
{
  using System;
  using System.Collections.Generic;

  using Entities;

  public interface IInputFileRepository
  {
    IList<InputFileEntity> GetInputFilesByJobId(Int32 jobId);

    IList<InputFile> GetInputFile(String fileName, String path);

    void InsertFile(String fileName, String filePath, Int32 jobId, Int32 manCoDocId);
  }
}
