// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IJobService.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   IJobService object
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ServiceInterfaces
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using Entities;

    public interface IJobService
    {
        JobEntity GetJobById(int jobId, string userId);

        IList<JobEntity> GetJobs(String userId,String environment);

        StringBuilder GetJobsReport(String searchCriteria, String userId, String environment);

        StringBuilder GetJobReportByJobId(Int32 jobId,String userId);

        PagedResult<JobEntity> GetJobs(Int32 pageNumber, Int32 numberOfItems, String userId, String environment);

        PagedResult<JobEntity> GetJobs(Int32 pageNumber, Int32 numberOfItems, String searchCriteria, String userId, String environment);

        IList<JobEntity> GetCompletedJobs(String manCo, String userId);

        void CreateJob(String environment, String manCo, String docTypeCode, String docTypeName, List<String> files, String userId, Boolean allowReprocessing, Boolean requiresAdditionalSetUp, String fundInfo, String sortCode, String accountNumber, String chequeNumber, String selectedFolder);

        void UpdateJobStatus(Int32 jobId, Int32 jobStatusTypeId);
    }
}
