// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PasswordHistoryRepositoryTests.cs" company="DST Nexdox">
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
  using System.Transactions;
  using BuildMeA;
  using DocProcessingRepository.Repositories;
  using Entities;
  using FluentAssertions;
  using NUnit.Framework;

  [Category("Integration")]
  [TestFixture]
  public class PasswordHistoryRepositoryTests
  {
    private TransactionScope transactionScope;
    private PasswordHistoryRepository passwordHistoryRepository;
    private ApplicationUserRepository applicationUserRepository;
    private ApplicationUser user;
    private PasswordHistory passwordHistory1;
    private PasswordHistory passwordHistory2;
    private PasswordHistory passwordHistory3;
    private PasswordHistory passwordHistory4;
    private PasswordHistory passwordHistory5;
    private PasswordHistory passwordHistory6;
    private PasswordHistory passwordHistory7;
    private PasswordHistory passwordHistory8;
    private PasswordHistory passwordHistory9;
    private PasswordHistory passwordHistory10;
    private PasswordHistory passwordHistory11;
    private PasswordHistory passwordHistory12;
    private PasswordHistory passwordHistory13;

    [SetUp]
    public void Setup()
    {
      this.transactionScope = new TransactionScope();
      this.passwordHistoryRepository = new PasswordHistoryRepository(ConfigurationManager.ConnectionStrings["BBOS"].ConnectionString);
      this.applicationUserRepository = new ApplicationUserRepository(ConfigurationManager.ConnectionStrings["BBOS"].ConnectionString);
      this.user = BuildMeA.ApplicationUser("user1");
      this.passwordHistory1 = BuildMeA.PasswordHistory(this.user.Id, "password1");
      this.passwordHistory2 = BuildMeA.PasswordHistory(this.user.Id, "password2");
      this.passwordHistory3 = BuildMeA.PasswordHistory(this.user.Id, "password3");
      this.passwordHistory4 = BuildMeA.PasswordHistory(this.user.Id, "password4");
      this.passwordHistory5 = BuildMeA.PasswordHistory(this.user.Id, "password5");
      this.passwordHistory6 = BuildMeA.PasswordHistory(this.user.Id, "password6");
      this.passwordHistory7 = BuildMeA.PasswordHistory(this.user.Id, "password7");
      this.passwordHistory8 = BuildMeA.PasswordHistory(this.user.Id, "password8");
      this.passwordHistory9 = BuildMeA.PasswordHistory(this.user.Id, "password9");
      this.passwordHistory10 = BuildMeA.PasswordHistory(this.user.Id, "password10");
      this.passwordHistory11 = BuildMeA.PasswordHistory(this.user.Id, "password11");
      this.passwordHistory12 = BuildMeA.PasswordHistory(this.user.Id, "password12");
      this.passwordHistory13 = BuildMeA.PasswordHistory(this.user.Id, "password13");
    }

    [TearDown]
    public void TearDown()
    {
      this.transactionScope.Dispose();
    } 

    [Test]
    public void WhenIWantToSavePasswordHistory_ThePasswordHistoryIsSaved()
    {
      this.applicationUserRepository.Create(this.user);

      this.passwordHistoryRepository.Create(this.passwordHistory1);
      this.passwordHistoryRepository.Create(this.passwordHistory2);
      this.passwordHistoryRepository.Create(this.passwordHistory3);

      var passwordHistory = this.passwordHistoryRepository.GetPasswordHistory(this.user.Id, 3);

      passwordHistory.Should().NotBeNull();
      passwordHistory[1].PasswordHash.ShouldAllBeEquivalentTo(this.passwordHistory2.PasswordHash);
      passwordHistory[0].PasswordHash.ShouldAllBeEquivalentTo(this.passwordHistory3.PasswordHash);
    }

    [Test]
    public void WhenIWantToRetrievedPasswordHistoryForACertainNumberOfMonths_ThePasswordHistoryIsReceived()
    {
      this.applicationUserRepository.Create(this.user);

      this.passwordHistoryRepository.Create(this.passwordHistory1);
      this.passwordHistoryRepository.Create(this.passwordHistory2);
      this.passwordHistoryRepository.Create(this.passwordHistory3);
      this.passwordHistoryRepository.Create(this.passwordHistory4);
      this.passwordHistoryRepository.Create(this.passwordHistory5);
      this.passwordHistoryRepository.Create(this.passwordHistory6);
      this.passwordHistoryRepository.Create(this.passwordHistory7);
      this.passwordHistoryRepository.Create(this.passwordHistory8);
      this.passwordHistoryRepository.Create(this.passwordHistory9);
      this.passwordHistoryRepository.Create(this.passwordHistory10);
      this.passwordHistoryRepository.Create(this.passwordHistory11);
      this.passwordHistoryRepository.Create(this.passwordHistory12);
      this.passwordHistoryRepository.Create(this.passwordHistory13);

      this.passwordHistory1.LogDate = new DateTime(2012, 10, 15);
      this.passwordHistory2.LogDate = new DateTime(2012, 11, 15);
      this.passwordHistory3.LogDate = new DateTime(2012, 12, 15);

      this.passwordHistoryRepository.Update(this.passwordHistory1);
      this.passwordHistoryRepository.Update(this.passwordHistory2);
      this.passwordHistoryRepository.Update(this.passwordHistory3);

      var passwordHistory = this.passwordHistoryRepository.GetPasswordHistoryByLastItems(this.user.Id, 12);
      
      passwordHistory[7].PasswordHash.ShouldAllBeEquivalentTo(this.passwordHistory6.PasswordHash);
      passwordHistory[8].PasswordHash.ShouldAllBeEquivalentTo(this.passwordHistory5.PasswordHash);
      passwordHistory[5].Id.ShouldBeEquivalentTo(this.passwordHistory8.Id);
      passwordHistory[4].Id.ShouldBeEquivalentTo(this.passwordHistory9.Id);
      passwordHistory.Should().HaveCount(12);
      passwordHistory.Should().NotContain(this.passwordHistory1);
      passwordHistory.Should().Contain(this.passwordHistory13);
      passwordHistory.Should().Contain(this.passwordHistory2);
      passwordHistory.Should().Contain(this.passwordHistory3);
      passwordHistory.Should().Contain(this.passwordHistory4);
      passwordHistory.Should().Contain(this.passwordHistory5);
      passwordHistory.Should().Contain(this.passwordHistory6);
      passwordHistory.Should().Contain(this.passwordHistory7);
      passwordHistory.Should().Contain(this.passwordHistory8);
    }
  }
}
