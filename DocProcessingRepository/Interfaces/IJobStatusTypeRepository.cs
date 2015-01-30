// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IJobStatusTypeRepository.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DocProcessingRepository.Interfaces
{
  using System;

  using Entities;

  using Repository;

  public interface IJobStatusTypeRepository : IRepository<JobStatusType>
  {
    JobStatusTypeEntity GetJobStatusType(String jobStatusTypeDescription);
  }
}
