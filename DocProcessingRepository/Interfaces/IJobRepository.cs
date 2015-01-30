// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IJobRepository.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DocProcessingRepository.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Entities;
    using Repository;

    public interface IJobRepository : IRepository<Job>
    {
        JobEntity GetJobById(int jobId, string userId);

        IList<JobEntity> GetJobs(String userId, String environment);

        StringBuilder GetJobsReport(String searchCriteria, String userId, String environment);

        PagedResult<JobEntity> GetJobs(Int32 pageNumber, Int32 numberOfItems, String userId, String environment);

        PagedResult<JobEntity> GetJobs(Int32 pageNumber, Int32 numberOfItems, String searchCriteria, String userId, String environment);

        IList<JobEntity> GetCompletedJobs(String manCo, String userId);

        Int32 InsertJob(Int32 manCoDocID, String grid, String additionalInfo, String userId, Int32 jobStatusTypeId);

        void UpdateJobStatus(Int32 jobId, Int32 jobStatusTypeId);
    }
}
