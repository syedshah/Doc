// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InputFileRepository.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DocProcessingRepository.Repositories
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using DocProcessingRepository.Contexts;
  using DocProcessingRepository.Interfaces;
  using EFRepository;
  using Entities;

  public class InputFileRepository : BaseEfRepository<InputFile>, IInputFileRepository
  {
    public InputFileRepository(String connectionString)
      : base(new IdentityDb(connectionString))
    {
    }

    public IList<InputFileEntity> GetInputFilesByJobId(Int32 jobId)
    {
      return this.GetEntityListByStoreProcedure<InputFileEntity>(String.Format("exec GetInputFileListByJobId {0}", jobId.ToString())).ToList();
    }

    public IList<InputFile> GetInputFile(String fileName, String path)
    {
      return this.GetEntityListByStoreProcedure<InputFile>(String.Format("GetInputFile '{0}', '{1}'", fileName, path));
    }

    public void InsertFile(String fileName, String filePath, Int32 jobId, Int32 manCoDocId)
    {
      this.ExecuteStoredProcedure(String.Format("CreateInputFile '{0}', '{1}', '{2}', '{3}'", fileName, filePath, jobId, manCoDocId));
    }
  }
}
