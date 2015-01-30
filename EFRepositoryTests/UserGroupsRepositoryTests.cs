// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserGroupsRepositoryTests.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace EFRepositoryTests
{
    using DocProcessingRepository.Repositories;
    using FluentAssertions;
    using NUnit.Framework;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Transactions;

    [Category("Integration")]
    [TestFixture]
    public class UserGroupsRepositoryTests
    {
        private TransactionScope transactionScope;
        private UserGroupsRepository _userGroupsRepository;
        private const string UserId = "ce45976d-5e5c-4404-af9c-7e80a795c7af";

        [SetUp]
        public void Setup()
        {
            this.transactionScope = new TransactionScope();
            this._userGroupsRepository = new UserGroupsRepository(ConfigurationManager.ConnectionStrings["BBOS"].ConnectionString);
        }

        [TearDown]
        public void TearDown()
        {
            this.transactionScope.Dispose();
        }

        [Test]
        public void GivenUserId_WhenIWantToGetUserGrpups_WhenDatabaseIsAvailable_ISholdGetUserGroupsFromDatabase()
        {
            var userGroups = this._userGroupsRepository.GetUserGroups(UserId);

            userGroups.Should().NotBeNull();
        }

        [Test]
        public void GivenUserId_WhenIWantToAddGrpupsToUser_WhenDatabaseIsAvailable_ISholdBeAbleToAddGroupsToUser()
        {
            this._userGroupsRepository.AddGroupsToUser(UserId, new List<int>() {1, 4 });

            var userGroups = this._userGroupsRepository.GetUserGroups(UserId);

            userGroups.Should().NotBeNull();

            Assert.AreEqual(2, userGroups.Count(), "There is error in adding group to user.");
        }
    }
}
