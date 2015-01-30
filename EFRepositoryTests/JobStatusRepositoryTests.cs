// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JobStatusRepositoryTests.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   
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
  public class JobStatusRepositoryTests
  {
    private JobStatusRepository jobStatusRepository;

    private JobStatusTypeRepository jobStatusTypeRepository;

    private ApplicationUserRepository applicationUserRepository;

    private JobRepository jobRepository;
    private TransactionScope transactionScope;

    private JobStatus jobStatus;

    private JobStatusType jobStatusType;

    private Job job;

    private ApplicationUser user;

    [SetUp]
    public void SetUp()
    {
      this.transactionScope = new TransactionScope();
      this.applicationUserRepository = new ApplicationUserRepository(ConfigurationManager.ConnectionStrings["BBOS"].ConnectionString);
      this.jobStatusRepository = new JobStatusRepository(ConfigurationManager.ConnectionStrings["BBOS"].ConnectionString);
      this.jobRepository = new JobRepository(ConfigurationManager.ConnectionStrings["BBOS"].ConnectionString);
      this.jobStatusTypeRepository = new JobStatusTypeRepository(ConfigurationManager.ConnectionStrings["BBOS"].ConnectionString);
      
      this.user = BuildMeA.ApplicationUser("UserName");
      this.applicationUserRepository.Create(user);
      this.job = BuildMeA.Job(DateTime.Now, DateTime.Now, DateTime.Now, String.Empty, "dfsdfjdf");
      this.job.ManCoDocID = 10;
      this.job.UserID = this.user.Id;
      this.jobRepository.Create(this.job);

      this.jobStatusType = BuildMeA.JobStatusType("descrip");
      this.jobStatusTypeRepository.Create(this.jobStatusType);

      this.jobStatus = BuildMeA.JobStatus(DateTime.Now).WithJobStatusType(this.jobStatusType);
      this.jobStatus.JobID = this.job.JobID;
    }

    [TearDown]
    public void TearDown()
    {
      this.transactionScope.Dispose();
    }

    //[Test]
    public void GivenAJobStatusRepository_WhenIWantToUpdateTheJobStatus_TheJobStatusIsUpdated()
    {
      this.jobStatusRepository.Create(this.jobStatus);

      var result1 =
       this.jobStatusRepository.Entities.ToList()
           .Where(x => x.JobStatusID == this.jobStatus.JobStatusID)
           .FirstOrDefault();

      result1.JobStatusTypeID.Should().NotBe(1);
      result1.JobStatusTypeID.Should().Be(this.jobStatusType.JobStatusTypeID);

      this.jobStatusRepository.UpdateJobStatus(this.job.JobID, 1);

      var result2 =
        this.jobStatusRepository.Entities.ToList()
            .Where(x => x.JobID == this.job.JobID)
            .FirstOrDefault();

      result2.JobStatusTypeID.Should().Be(1);
    }
  }
}
