// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JobStatusService.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Services
{
  using System;

  using DocProcessingRepository.Interfaces;

  using Exceptions;

  using ServiceInterfaces;

  public class JobStatusService : IJobStatusService
  {
    private readonly IJobStatusRepository jobStatusRepository;

    public JobStatusService(IJobStatusRepository jobStatusRepository)
    {
      this.jobStatusRepository = jobStatusRepository;
    }

    public void UpdateJobStatus(Int32 jobId, Int32 jobStatusTypeId)
    {
      try
      {
        this.jobStatusRepository.UpdateJobStatus(jobId, jobStatusTypeId);
      }
      catch (Exception e)
      {
        throw new DocProcessingException("Unable to update job status", e);
      }
    }
  }
}
