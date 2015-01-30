// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JobStatusRepository.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DocProcessingRepository.Repositories
{
  using System;

  using DocProcessingRepository.Contexts;
  using DocProcessingRepository.Interfaces;

  using EFRepository;

  using Entities;

  public class JobStatusRepository : BaseEfRepository<JobStatus>, IJobStatusRepository
  {
    public JobStatusRepository(String connectionString)
      : base(new IdentityDb(connectionString))
    {
    }

    public void UpdateJobStatus(Int32 jobId, Int32 jobStatusTypeId)
    {
      this.ExecuteStoredProcedure(String.Format("UpdateJobStatus {0}, {1}", jobId, jobStatusTypeId));
    }

    public void InsertJobStatus(Int32 jobId, Int32 jobStatusTypeId)
    {
      this.ExecuteStoredProcedure(String.Format("InsertJobStatus {0}, {1}", jobId, jobStatusTypeId));
    }
  }
}
