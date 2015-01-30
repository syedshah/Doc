// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JobRepositoryTests.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   Tests for Job repository
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace EFRepositoryTests
{
    using System;
    using System.Configuration;
    using System.Linq;
    using System.Transactions;
    using BuildMeA;
    using DocProcessingRepository.Repositories;
    using Entities;
    using FluentAssertions;
    using NUnit.Framework;

    [Category("Integration")]
    [TestFixture]
    public class JobRepositoryTests
    {
        private TransactionScope transactionScope;
        private JobRepository jobRepository;

        private JobStatusTypeRepository jobStatusTypeRepository;

        private Job job;

        private JobStatusType jobStatusType;

        private ManagementCompany managementCompany;

        private ManCoDoc manCoDoc;

        private ParentCompany parentCompany;

        private ApplicationUser user;

        [SetUp]
        public void SetUp()
        {
            this.transactionScope = new TransactionScope();
            this.jobRepository = new JobRepository(ConfigurationManager.ConnectionStrings["BBOS"].ConnectionString);
            this.jobStatusTypeRepository = new JobStatusTypeRepository(ConfigurationManager.ConnectionStrings["BBOS"].ConnectionString);
            this.job = BuildMeA.Job(DateTime.Now, DateTime.Now, DateTime.Now, "jwelkjwejwe", "grid");
            this.jobStatusType = BuildMeA.JobStatusType("job status descrip");
            this.managementCompany = BuildMeA.ManagementCompany("manconame", "mancocode", "mancoshortname", "rufudbid");
            this.manCoDoc = BuildMeA.ManCoDoc(2, 3, "Pub file name", "version", 2);
            this.parentCompany = BuildMeA.ParentCompany("parent company name");
            this.managementCompany.ParentCompany = this.parentCompany;
            this.user = BuildMeA.ApplicationUser("user");
            this.job.User = this.user;
        }

        [TearDown]
        public void TearDown()
        {
            this.transactionScope.Dispose();
        }

        [Test]
        public void WhenIWantToGetJobById_ItIsRetrievedFromTheDatabase()
        {
            var job = this.jobRepository.GetJobById(30000001, "446740ff-e617-40d0-ac3d-f2e962f9a2ce");

            job.Should().NotBeNull();
            job.JobId.Equals(30000001);         
        }

        [Test]
        public void WhenIWantToGetJobs_ItIsRetrievedFromTheDatabase()
        {
            this.manCoDoc.ManagementCompany = this.managementCompany;
            this.job.ManCoDoc = this.manCoDoc;
            this.jobRepository.Create(this.job);

            const String Environment = "Development";

            var jobs = this.jobRepository.GetJobs(this.user.Id,Environment);

            jobs.Should().NotBeNull();
        }

        [Test]
        public void WhenIWantToGetPagedJobs_ItIsRetrievedFromTheDatabase()
        {
            const Int32 PageNumber = 1;
            const Int32 NumOfItems = 1;
            const String Environment = "Development";

            this.manCoDoc.ManagementCompany = this.managementCompany;
            this.job.ManCoDoc = this.manCoDoc;
            this.jobRepository.Create(this.job);

            var jobs = this.jobRepository.GetJobs(PageNumber, NumOfItems, this.user.Id,Environment);

            jobs.Should().NotBeNull();

            jobs.Results.Count.Should().BeGreaterOrEqualTo(0);
            jobs.CurrentPage.ShouldBeEquivalentTo(PageNumber);
            jobs.ItemsPerPage.ShouldBeEquivalentTo(NumOfItems);
            jobs.EndRow.ShouldBeEquivalentTo(1);
        }

        // [Test]
        public void WhenIWantToGetPagedJobsBySearchCriteria_ItIsRetrievedFromTheDatabase()
        {
            const Int32 PageNumber = 1;
            const Int32 NumOfItems = 1;

            this.manCoDoc.ManagementCompany = this.managementCompany;
            this.job.ManCoDoc = this.manCoDoc;
            this.jobRepository.Create(this.job);

            var jobs = this.jobRepository.GetJobs(PageNumber, NumOfItems, "mancoshortname", this.user.Id);

            jobs.Should().NotBeNull();

            jobs.Results.Count.Should().BeGreaterOrEqualTo(1);
            jobs.CurrentPage.ShouldBeEquivalentTo(PageNumber);
            jobs.ItemsPerPage.ShouldBeEquivalentTo(NumOfItems);
            jobs.EndRow.ShouldBeEquivalentTo(1);
        }

        [Test]
        public void WhenIWantToGetJobsReport_ItIsRetrievedFromTheDatabase()
        {
            const String Environment = "Development";

            this.manCoDoc.ManagementCompany = this.managementCompany;
            this.job.ManCoDoc = this.manCoDoc;
            this.jobRepository.Create(this.job);

            var jobReport = this.jobRepository.GetJobsReport(String.Empty, this.user.Id,Environment);

            jobReport.Should().NotBeNull();
        }

        [Test]
        public void WhenIWantToGetJobsReportWhithSearchString_ItIsRetrievedFromTheDatabase()
        {
            const String Environment = "Development";

            this.manCoDoc.ManagementCompany = this.managementCompany;
            this.job.ManCoDoc = this.manCoDoc;
            this.jobRepository.Create(this.job);

            var jobReport = this.jobRepository.GetJobsReport("mancoshortname", this.user.Id,Environment);

            jobReport.Should().NotBeNull();
        }

        [Test]
        public void WhenItryToInsertAJob_ItIsSavedToTheJobTable()
        {
            this.manCoDoc.ManagementCompany = this.managementCompany;
            this.job.ManCoDoc = this.manCoDoc;

            Int32 initialCount = this.jobRepository.Entities.Count();

            this.jobRepository.InsertJob(1, "grid", "additionalInfo", "1a323edd-0732-4ef6-b2a2-4647df9dfd74", 2);

            this.jobRepository.Entities.Count().Should().Be(initialCount + 1);
        }

        [Test]
        public void GivenAManCoAndUserId_WhenISearchForJobs_IGetJobs()
        {
            var result = this.jobRepository.GetCompletedJobs("SL", "1a323edd-0732-4ef6-b2a2-4647df9dfd74");

            result.Should().NotBeEmpty();
        }

        [Test]
        public void GivenAJobIdAndJobStatusTypeId_WhenIUpdateTheJobStatusType_TheJobStatusIsUpdated()
        {
            this.jobStatusTypeRepository.Create(this.jobStatusType);

            this.manCoDoc.ManagementCompany = this.managementCompany;
            this.job.ManCoDoc = this.manCoDoc;

            var jobId = this.jobRepository.InsertJob(1, "grid", "additionalInfo", "1a323edd-0732-4ef6-b2a2-4647df9dfd74", 2);

            this.jobRepository.UpdateJobStatus(jobId, this.jobStatusType.JobStatusTypeID);

            var result = this.jobRepository.Entities.Where(x => x.JobID == jobId).FirstOrDefault().JobStatusTypeId;

            result.ShouldBeEquivalentTo(this.jobStatusType.JobStatusTypeID);
        }
    }
}
