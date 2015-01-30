// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuthoriseJobEngine.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace BusinessEngines
{
  using System;
  using System.Collections.Generic;

  using BusinessEngineInterfaces;

  using DocProcessingRepository.Interfaces;

  using Entities;

  using Exceptions;

  using ServiceInterfaces;

  public class AuthoriseJobEngine : IAuthoriseJobEngine
  {
    private readonly IAuthoriseHistoryService authoriseHistoryService;

    private readonly IJobService jobService;

    private readonly IJobStatusTypeRepository jobStatusTypeRepository;

    public AuthoriseJobEngine(
      IAuthoriseHistoryService authoriseHistoryService,
      IJobService jobService,
      IJobStatusTypeRepository jobStatusTypeRepository)
    {
      this.authoriseHistoryService = authoriseHistoryService;

      this.jobService = jobService;
      this.jobStatusTypeRepository = jobStatusTypeRepository;
    }

    public void AuthoriseJob(IList<Int32> jobIds, String jobStatusType, String userId, String comment)
    {
      var jobStatus = this.GetJobStatusType(jobStatusType);

      foreach (var jobId in jobIds)
      {
        this.authoriseHistoryService.InsertAuthorisation(jobId, userId, comment);

        this.jobService.UpdateJobStatus(jobId, jobStatus.JobStatusTypeID);
      }
    }

    private JobStatusTypeEntity GetJobStatusType(String jobStatusTypeDescription)
    {
      try
      {
        return this.jobStatusTypeRepository.GetJobStatusType(jobStatusTypeDescription);
      }
      catch (Exception e)
      {
        throw new DocProcessingException("Unable to get job status type", e);
      }
    }
  }
}
