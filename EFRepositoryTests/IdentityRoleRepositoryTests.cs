// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IdentityRoleRepositoryTests.cs" company="DST Nexdox">
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
    public class IdentityRoleRepositoryTests
    {
        private TransactionScope transactionScope;
        private IdentityRoleRepository _identityRoleRepository;

        [SetUp]
        public void Setup()
        {
            this.transactionScope = new TransactionScope();
            this._identityRoleRepository = new IdentityRoleRepository(ConfigurationManager.ConnectionStrings["BBOS"].ConnectionString);
        }

        [TearDown]
        public void TearDown()
        {
            this.transactionScope.Dispose();
        }

        [Test]
        public void WhenIWantToGetRoles_WhenDatabaseIsAvailable_IGetRolesFromTheDatabase()
        {
            var groups = this._identityRoleRepository.GetRoles();

            groups.Should().NotBeNull();
        }
    }
}
