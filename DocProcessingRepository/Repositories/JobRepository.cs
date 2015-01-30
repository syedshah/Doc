// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JobRepository.cs" company="DST Nexdox">
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
    using System.Text;
    using DocProcessingRepository.Contexts;
    using DocProcessingRepository.Interfaces;
    using EFRepository;
    using Entities;

    public class JobRepository : BaseEfRepository<Job>, IJobRepository
    {
        public JobRepository(String connectionString)
            : base(new IdentityDb(connectionString))
        {

        }

        public JobEntity GetJobById(int jobId, string userId)
        {
            var job = this.SelectStoredProcedure<JobEntity>(string.Format("exec GetJobById '{0}',{1}", userId, jobId));

            return job;
        }

        public IList<JobEntity> GetJobs(String userId, String environment)
        {
            var jobs = this.GetEntityListByStoreProcedure<JobEntity>(String.Format("exec GetJobs '{0}','{1}'", userId,environment));

            return jobs;
        }

        public StringBuilder GetJobsReport(String searchCriteria, String userId, String environment)
        {
            IList<JobEntity> jobs = new List<JobEntity>();
            if (searchCriteria.Length < 1)
            {
                jobs = this.GetEntityListByStoreProcedure<JobEntity>(String.Format("exec GetJobs '{0}','{1}'", userId,environment)).ToList();
            }
            else
            {
                jobs = this.GetEntityListByStoreProcedure<JobEntity>(String.Format("exec GetJobsBySearchCriteria '{0}', '{1}','{2}'", userId, searchCriteria,environment)).ToList();
            }

            var csvBuilder = new StringBuilder();

            csvBuilder.Append("JOB REFERENCE");
            csvBuilder.Append(",");

            csvBuilder.Append("COMPANY");
            csvBuilder.Append(",");

            csvBuilder.Append("DOCUMENT");
            csvBuilder.Append(",");

            csvBuilder.Append("VERSION");
            csvBuilder.Append(",");

            csvBuilder.Append("OWNER");
            csvBuilder.Append(",");

            csvBuilder.Append("SUBMIT DATE/TIME");
            csvBuilder.Append(",");

            csvBuilder.Append("FINISH DATE/TIME");
            csvBuilder.Append(",");

            csvBuilder.Append("STATUS");
            csvBuilder.Append(",");

            csvBuilder.Append("GRID");
            csvBuilder.Append(",");

            csvBuilder.Append("ALLOCATOR GRID");
            csvBuilder.Append(",");

            csvBuilder.Append("MANCO CODE");
            csvBuilder.Append(",");


            csvBuilder.Append("\n");

            foreach (var job in jobs)
            {
                csvBuilder.Append(job.JobReference);
                csvBuilder.Append(",");

                csvBuilder.Append(job.Company);
                csvBuilder.Append(",");

                csvBuilder.Append(job.Document);
                csvBuilder.Append(",");

                csvBuilder.Append(job.Version);
                csvBuilder.Append(",");

                csvBuilder.Append(job.Owner);
                csvBuilder.Append(",");

                csvBuilder.Append(job.SubmitDateTime);
                csvBuilder.Append(",");

                csvBuilder.Append(job.FinishDate);
                csvBuilder.Append(",");

                csvBuilder.Append(job.Status);
                csvBuilder.Append(",");

                csvBuilder.Append(job.Grid);
                csvBuilder.Append(",");

                csvBuilder.Append(job.AllocatorGRID);
                csvBuilder.Append(",");

                csvBuilder.Append(job.ManCoCode);
                csvBuilder.Append(",");

                csvBuilder.Append("\n");
            }

            return csvBuilder;
        }

        public PagedResult<JobEntity> GetJobs(Int32 pageNumber, Int32 numberOfItems, String userId, String environment)
        {
            var jobs = this.GetEntityListByStoreProcedure<JobEntity>(String.Format("exec GetJobs '{0}','{1}'", userId,environment)).ToList();

            return new PagedResult<JobEntity>
            {
                CurrentPage = pageNumber,
                ItemsPerPage = numberOfItems,
                TotalItems = jobs.Count(),
                Results = jobs.OrderByDescending(c => c.JobId)
                .Skip((pageNumber - 1) * numberOfItems)
                .Take(numberOfItems)
                .ToList(),
                StartRow = ((pageNumber - 1) * numberOfItems) + 1,
                EndRow = (((pageNumber - 1) * numberOfItems) + 1) + (numberOfItems - 1)
            };
        }


        public PagedResult<JobEntity> GetJobs(Int32 pageNumber, Int32 numberOfItems, String searchCriteria, String userId, String environment)
        {
            var jobs = this.GetEntityListByStoreProcedure<JobEntity>(String.Format("exec GetJobsBySearchCriteria '{0}', '{1}', '{2}'", userId, searchCriteria,environment)).ToList();

            return new PagedResult<JobEntity>
            {
                CurrentPage = pageNumber,
                ItemsPerPage = numberOfItems,
                TotalItems = jobs.Count(),
                Results = jobs.OrderByDescending(c => c.JobId)
                .Skip((pageNumber - 1) * numberOfItems)
                .Take(numberOfItems)
                .ToList(),
                StartRow = ((pageNumber - 1) * numberOfItems) + 1,
                EndRow = (((pageNumber - 1) * numberOfItems) + 1) + (numberOfItems - 1)
            };
        }

        public IList<JobEntity> GetCompletedJobs(String manCo, String userId)
        {
            return this.GetEntityListByStoreProcedure<JobEntity>(String.Format("exec GetCompletedJobsByManCo '{0}','{1}'", manCo, userId)).ToList();
        }

        public Int32 InsertJob(Int32 manCoDocID, String grid, String additionalInfo, String userId, Int32 jobStatusTypeId)
        {
            return this.InsertStoredProcedureOut(String.Format("[CreateJob] @JobID = @JobID OUTPUT, @ManCoDocID = '{0}', @GRID = '{1}', @AdditionalSetupInfo = '{2}', @UserID = '{3}', @JobStatusTypeId = {4} ", manCoDocID, grid, additionalInfo, userId, jobStatusTypeId), "JobID");
        }

        public void UpdateJobStatus(Int32 jobId, Int32 jobStatusTypeId)
        {
            this.ExecuteStoredProcedure(String.Format("UpdateJobStatusType {0}, {1}", jobId, jobStatusTypeId));
        }
    }
}
