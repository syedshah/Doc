// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InputFileService.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Services
{
  using System;
  using System.Collections.Generic;

  using DocProcessingRepository.Interfaces;

  using Entities;

  using Exceptions;

  using ServiceInterfaces;

  public class InputFileService : IInputFileService
  {
    private readonly IInputFileRepository inputFileRepository;

    public InputFileService(IInputFileRepository inputFileRepository)
    {
      this.inputFileRepository = inputFileRepository;
    }

    public IList<InputFileEntity> GetInputFilesByJobId(Int32 jobId)
    {
      try
      {
        return this.inputFileRepository.GetInputFilesByJobId(jobId);
      }
      catch (Exception e)
      {
        throw new DocProcessingException("Unable to retrieve input files", e);
      }
    }
  }
}
