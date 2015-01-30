// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IInputFileService.cs" company="DST Nexdox">
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

  public interface IInputFileService
  {
    IList<InputFileEntity> GetInputFilesByJobId(Int32 jobId);
  }
}
