// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApprovalHistoryRepositoryTests.cs" company="DST Nexdox">
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
  public class AuthoriseHistoryRepositoryTests
  {
    private TransactionScope transactionScope;

    private AuthoriseHistoryRepository authoriseHistoryRepository;
    private JobRepository jobRepository;
    private Job job;
    private ManagementCompany managementCompany;
    private ManCoDoc manCoDoc;
    private ParentCompany parentCompany;
    private ApplicationUser user;

    [SetUp]
    public void SetUp()
    {
      this.transactionScope = new TransactionScope();
      this.authoriseHistoryRepository = new AuthoriseHistoryRepository(ConfigurationManager.ConnectionStrings["BBOS"].ConnectionString);
      this.jobRepository = new JobRepository(ConfigurationManager.ConnectionStrings["BBOS"].ConnectionString);
      this.job = BuildMeA.Job(DateTime.Now, DateTime.Now, DateTime.Now, "jwelkjwejwe", "grid");
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
    public void WhenItryToInsertApproval_ItIsSavedToTheApprovalHistoryTable()
    {
      this.manCoDoc.ManagementCompany = this.managementCompany;
      this.job.ManCoDoc = this.manCoDoc;
      this.jobRepository.Create(this.job); 

      this.authoriseHistoryRepository.InsertAuthorisation(this.job.JobID, this.job.UserID, "comment");

      var result = this.authoriseHistoryRepository.Entities.ToList();

      result.Should().NotBeNull();
      result.Count.Should().BeGreaterOrEqualTo(1);
    }
  }
}
