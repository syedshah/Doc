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
  using System.Collections.Generic;
  using Entities.File;

  public interface IInputFileRepository
  {
    InputFileInfo GetInputFileInfo(String logPath);

    AdditionalInfoFile GetAdditionalInfo(String rootPath, List<String> files);
  }
}
