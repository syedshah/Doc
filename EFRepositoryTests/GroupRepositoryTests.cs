// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GroupRepositoryTests.cs" company="DST Nexdox">
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
    using System.Configuration;
    using System.Transactions;

    [Category("Integration")]
    [TestFixture]
    public class GroupRepositoryTests
    {
        private TransactionScope transactionScope;
        private GroupRepository _groupRepository;

        [SetUp]
        public void Setup()
        {
            this.transactionScope = new TransactionScope();
            this._groupRepository = new GroupRepository(ConfigurationManager.ConnectionStrings["BBOS"].ConnectionString);
        }

        [TearDown]
        public void TearDown()
        {
            this.transactionScope.Dispose();
        }

        [Test]
        public void WhenIWantToGetJobs_WhenDatabaseIsAvailable_IGetJobsFromTheDatabase()
        {
            var groups = this._groupRepository.GetGroups();

            groups.Should().NotBeNull();
        }
    }
}
