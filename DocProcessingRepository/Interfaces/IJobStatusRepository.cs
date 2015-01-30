// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IJobStatusRepository.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DocProcessingRepository.Interfaces
{
  using System;

  public interface IJobStatusRepository
  {
    void UpdateJobStatus(Int32 jobId, Int32 jobStatusTypeId);

    void InsertJobStatus(Int32 jobId, Int32 jobStatusTypeId);
  }
}
