// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JobStatusBuilder.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   Builder for Job Status
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace BuildMeA
{
  using Entities;

  public class JobStatusBuilder : Builder<JobStatus>
  {
    public JobStatusBuilder()
    {
      Instance = new JobStatus();
    }

    public JobStatusBuilder WithJobStatusType(JobStatusType jobStatusType)
    {
      Instance.JobStatusType = jobStatusType;
      return this;
    }
  }
}
