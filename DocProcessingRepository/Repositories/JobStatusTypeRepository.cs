// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JobStatusTypeRepository.cs" company="DST Nexdox">
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

  public class JobStatusTypeRepository : BaseEfRepository<JobStatusType>, IJobStatusTypeRepository
  {
    public JobStatusTypeRepository(String connectionString)
      : base(new IdentityDb(connectionString))
    {
    }

    public JobStatusTypeEntity GetJobStatusType(String jobStatusTypeDescription)
    {
      return this.SelectStoredProcedure<JobStatusTypeEntity>(String.Format("GetJobStatusTypeByDescription {0}", jobStatusTypeDescription));
    }
  }
}
