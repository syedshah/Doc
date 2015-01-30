// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApplicationUserRepositoryTests.cs" company="DST Nexdox">
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
  public class ApplicationUserRepositoryTests
  {
    private ApplicationUser applicationUser;
    private ApplicationUser applicationUser2;
    private ApplicationUser applicationUser3;
    private ApplicationUser applicationUser4;
    private ApplicationUserRepository applicationUserRepository;
    private TransactionScope transactionScope;

    [SetUp]
    public void Setup()
    {
      this.transactionScope = new TransactionScope();
      this.applicationUserRepository = new ApplicationUserRepository(ConfigurationManager.ConnectionStrings["BBOS"].ConnectionString);
      this.applicationUser = BuildMeA.ApplicationUser("tron");
      this.applicationUser2 = BuildMeA.ApplicationUser("tron2");
      this.applicationUser3 = BuildMeA.ApplicationUser("flynn");
      this.applicationUser4 = BuildMeA.ApplicationUser("flynn2");
    }

    [TearDown]
    public void TearDown()
    {
      this.transactionScope.Dispose();
    }

    [Test]
    public void GivenAUserName_WhenITryToSearchForItByUserName_ItIsRetrievedFromTheDatabase()
    {
      this.applicationUserRepository.Create(this.applicationUser);

      var user = this.applicationUserRepository.GetUserByName(this.applicationUser.UserName);

      user.Should().NotBeNull();
      user.UserName.Should().Be(this.applicationUser.UserName);
    }

    [Test]
    public void GivenAListOfUsers_WhenITryToGetPagedUsers_ThePagedUsersAreRetrieved()
    {
      const Int32 PageNumber = 1;
      const Int32 NumOfItems = 2;
      const String Environment = "Test";

      this.applicationUserRepository.Create(this.applicationUser);
      this.applicationUserRepository.Create(this.applicationUser2);
      this.applicationUserRepository.Create(this.applicationUser3);
      this.applicationUserRepository.Create(this.applicationUser4);

      var users = this.applicationUserRepository.GetUsers(PageNumber, NumOfItems, Environment);

      users.Should().NotBeNull();
      users.Results.Count.Should().Be(2);
      users.CurrentPage.ShouldBeEquivalentTo(PageNumber);
      users.ItemsPerPage.ShouldBeEquivalentTo(NumOfItems);
      users.EndRow.ShouldBeEquivalentTo(2);
    }

      [Test]
      public void GivenASearchCriteria_WhenITryToGetPagedUsers_ThePagedUsersAreRetrieved()
      {
          const Int32 PageNumber = 1;
          const Int32 NumOfItems = 1;
          const String Environment = "Test";
          const String searchCriteria = "Test";

          var users = this.applicationUserRepository.GetUsers(PageNumber, NumOfItems, searchCriteria, Environment);
          users.Should().NotBeNull();
          users.Results.Count.Should().BeGreaterOrEqualTo(0);
          users.CurrentPage.ShouldBeEquivalentTo(PageNumber);
          users.ItemsPerPage.ShouldBeEquivalentTo(NumOfItems);
          users.EndRow.ShouldBeEquivalentTo(1);
      }

    [Test]
    public void GivenAUserId_WhenAUserIsNotDeactivated_ItReturnsFalseWhenTryingToCheckTheValueOfIsLockedOut()
    {
      this.applicationUserRepository.Create(this.applicationUser);

      var result = this.applicationUserRepository.IsLockedOut(this.applicationUser.Id);

      result.Should().BeFalse();
    }

    [Test]
    public void GivenAUserId_WhenITryToUnlockAUser_TheUserIsUnlocked()
    {
      ApplicationUser user = BuildMeA.ApplicationUser("usertron");

      user.LastLoginDate = new DateTime(2013, 12, 10);

      this.applicationUserRepository.Create(user);

      this.applicationUserRepository.UpdateUserlastLogindate(user.Id);

      var result = this.applicationUserRepository.GetUserByName(user.UserName);

      result.Should().NotBeNull();
      result.LastLoginDate.Value.ToShortDateString().ShouldBeEquivalentTo(DateTime.Now.ToShortDateString());
    }

    [Test]
    public void GivenAUserId_WhenITryToUpdateTheLastLoginDate_TheFailedLogInCountIsSetToZero()
    {
      ApplicationUser user = BuildMeA.ApplicationUser("usertron");

      this.applicationUserRepository.Create(user);

      this.applicationUserRepository.UpdateUserlastLogindate(user.Id);

      var result = this.applicationUserRepository.GetUserByName(user.UserName);

      result.Should().NotBeNull();
      result.FailedLogInCount.Should().Be(0);
    }

    [Test]
    public void GivenAUserId_WhenITryToUpdateTheFailedLoginCount_TheFailedLogInCountIsIncremented()
    {
      ApplicationUser user = BuildMeA.ApplicationUser("usertron");
      this.applicationUserRepository.Create(user);

      Int32 initialCount = user.FailedLogInCount;
      this.applicationUserRepository.UpdateUserFailedLogin(user.Id);

      var result = this.applicationUserRepository.GetUserByName(user.UserName);
      result.FailedLogInCount.Should().Be(initialCount + 1);
    }

    [Test]
    public void GivenAUserId_WhenITryToDeactivateAUser_TheUserIsLockedOutOfTheSystem()
    {
      this.applicationUserRepository.Create(this.applicationUser);

      this.applicationUserRepository.DeactivateUser(this.applicationUser.Id);

      var user = this.applicationUserRepository.GetUserByName(this.applicationUser.UserName);

      user.Should().NotBeNull();
      user.IsLockedOut.Should().BeTrue();
    }

    [Test]
    public void GivenAUserId_WhenAUserIsDeactivated_ItReturnsTrueWhenTryingToCheckTheValueOfIsLockedOut()
    {
      this.applicationUserRepository.Create(this.applicationUser);

      this.applicationUserRepository.DeactivateUser(this.applicationUser.Id);

      var result = this.applicationUserRepository.IsLockedOut(applicationUser.Id);

      result.Should().BeTrue();
    }

    [Test]
    public void GivenAUserId_WhenITryToUpdateTheLastPasswordChangedDate_TheLastPasswordChangedDateIsUpdated()
    {
      ApplicationUser user;

      user = BuildMeA.ApplicationUser("usertron");

      user.LastPasswordChangedDate = new DateTime(2013, 12, 10);

      this.applicationUserRepository.Create(user);

      this.applicationUserRepository.UpdateUserLastPasswordChangedDate(user.Id);

      var result = this.applicationUserRepository.GetUserByName(user.UserName);

      result.Should().NotBeNull();
      result.LastPasswordChangedDate.ToShortDateString().ShouldBeEquivalentTo(DateTime.Now.ToShortDateString());
    }
  }
}
