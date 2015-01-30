// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JobService.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   JobService object
// </summary>
// --------------------------------------------------------------------------------------------------------------------


namespace Services
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using BusinessEngineInterfaces;
    using DocProcessingRepository.Interfaces;
    using Entities;
    using Exceptions;
    using ServiceInterfaces;

    public class JobService : IJobService
    {
        private readonly IJobRepository jobRepository;
        private readonly ICreateJobEngine createJobEngine;
        private readonly IEnclosingJobRepository _enclosingJobRepository;

        public JobService(IJobRepository jobRepository, ICreateJobEngine createJobEngine)
        {
            this.jobRepository = jobRepository;
            this.createJobEngine = createJobEngine;
        }

        public JobService(IJobRepository jobRepository, ICreateJobEngine createJobEngine, IEnclosingJobRepository enclosingJobRepository)
            : this(jobRepository, createJobEngine)
        {
            this._enclosingJobRepository = enclosingJobRepository;
        }

        public JobEntity GetJobById(int jobId, string userId)
        {
            try
            {
                return this.jobRepository.GetJobById(jobId, userId);
            }
            catch (Exception e)
            {
                throw new DocProcessingException("Unable to retrieve job", e);
            }
        }

        public IList<JobEntity> GetJobs(String userId, String environment)
        {
            try
            {
                return this.jobRepository.GetJobs(userId, environment);
            }
            catch (Exception e)
            {
                throw new DocProcessingException("Unable to retrieve jobs", e);
            }
        }

        public StringBuilder GetJobsReport(String searchCriteria, String userId, String environment)
        {
            try
            {
                return this.jobRepository.GetJobsReport(searchCriteria, userId, environment);
            }
            catch (Exception e)
            {
                throw new DocProcessingException("Unable to retrieve jobs report", e);
            }
        }

        public PagedResult<JobEntity> GetJobs(Int32 pageNumber, Int32 numberOfItems, String userId, String environment)
        {
            try
            {
                return this.jobRepository.GetJobs(pageNumber, numberOfItems, userId, environment);
            }
            catch (Exception e)
            {
                throw new DocProcessingException("Unable to retrieve paged jobs", e);
            }
        }

        public PagedResult<JobEntity> GetJobs(Int32 pageNumber, Int32 numberOfItems, String searchCriteria, String userId, String environment)
        {
            try
            {
                return this.jobRepository.GetJobs(pageNumber, numberOfItems, searchCriteria, userId, environment);
            }
            catch (Exception e)
            {
                throw new DocProcessingException("Unable to retrieve paged jobs", e);
            }
        }

        public IList<JobEntity> GetCompletedJobs(String manCo, String userId)
        {
            try
            {
                return this.jobRepository.GetCompletedJobs(manCo, userId);
            }
            catch (Exception e)
            {
                throw new DocProcessingException("Unable to retrieve jobs", e);
            }
        }

        public void CreateJob(String environment, String manCo, String docTypeCode, String docTypeName, List<String> files, String userId, Boolean allowReprocessing, Boolean requiresAdditionalSetUp, String fundInfo, String sortCode, String accountNumber, String chequeNumber, String seletedFolder)
        {
            try
            {
                this.createJobEngine.SubmitJob(environment, manCo, docTypeCode, docTypeName, files, userId, allowReprocessing, requiresAdditionalSetUp, fundInfo, sortCode, accountNumber, chequeNumber, seletedFolder);
            }
            catch (DocProcessingFileAlreadyProcessedException e)
            {
                throw new DocProcessingFileAlreadyProcessedException("Unable to run job as the job contains a file that has already been processed");
            }
            catch (Exception e)
            {
                throw new DocProcessingException("Unable to insert job", e);
            }
        }

        public void UpdateJobStatus(Int32 jobId, Int32 jobStatusTypeId)
        {
            try
            {
                this.jobRepository.UpdateJobStatus(jobId, jobStatusTypeId);
            }
            catch (Exception e)
            {
                throw new DocProcessingException("Unable to update job status type", e);
            }
        }

        public StringBuilder GetJobReportByJobId(int jobId, string userId)
        {
            var jobEntity = this.jobRepository.GetJobById(jobId, userId);
            var enclosingJobs = this._enclosingJobRepository.GetEnclosingJobsByJobId(jobId);
            StringBuilder csvBuilder = ConvertToExcel(jobEntity, enclosingJobs);

            return csvBuilder;
        }

        private StringBuilder ConvertToExcel(JobEntity job, IEnumerable<EnclosingJob> enclosingJobs)
        {
            var csvBuilder = new StringBuilder();

            csvBuilder.Append("JOB REFERENCE");
            csvBuilder.Append(",");

            csvBuilder.Append("COMPANY");
            csvBuilder.Append(",");

            csvBuilder.Append("VERSION");
            csvBuilder.Append(",");

            csvBuilder.Append("STATUS");
            csvBuilder.Append(",");

            csvBuilder.Append("GRID");
            csvBuilder.Append(",");

            csvBuilder.Append("ALLOCATOR GRID");
            csvBuilder.Append(",");

            csvBuilder.Append("MANCO CODE");
            csvBuilder.Append(",");

            csvBuilder.Append("EnclosingJobID");
            csvBuilder.Append(",");

            csvBuilder.Append("Filename");
            csvBuilder.Append(",");

            csvBuilder.Append("Packs");
            csvBuilder.Append(",");

            csvBuilder.Append("Pages");
            csvBuilder.Append(",");

            csvBuilder.Append("Sheets");
            csvBuilder.Append(",");

            csvBuilder.Append("PostalDocketNumber");
            csvBuilder.Append(",");

            csvBuilder.Append("\n");

            foreach (var enJob in enclosingJobs)
            {
                csvBuilder.Append(job.JobReference);
                csvBuilder.Append(",");

                csvBuilder.Append(job.Company);
                csvBuilder.Append(",");

                csvBuilder.Append(job.Version);
                csvBuilder.Append(",");

                csvBuilder.Append(job.Status);
                csvBuilder.Append(",");

                csvBuilder.Append(job.Grid);
                csvBuilder.Append(",");

                csvBuilder.Append(job.AllocatorGRID);
                csvBuilder.Append(",");

                csvBuilder.Append(job.ManCoCode);
                csvBuilder.Append(",");

                csvBuilder.Append(enJob.EnclosingJobID.ToString());
                csvBuilder.Append(",");

                csvBuilder.Append(enJob.Filename);
                csvBuilder.Append(",");

                csvBuilder.Append(enJob.Packs);
                csvBuilder.Append(",");

                csvBuilder.Append(enJob.Pages);
                csvBuilder.Append(",");

                csvBuilder.Append(enJob.Sheets);
                csvBuilder.Append(",");

                csvBuilder.Append(enJob.PostalDocketNumber);
                csvBuilder.Append(",");

                csvBuilder.Append("\n");
            }
            return csvBuilder;
        }
    }
}
