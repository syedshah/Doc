// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ManCoDocRepostoryTests.cs" company="DST Nexdox">
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
    using System;
    using System.Configuration;
    using System.Linq;
    using System.Transactions;

    [Category("Integration")]
    [TestFixture]
    public class ManCoDocRepostoryTests
    {
        private TransactionScope transactionScope;
        private ManCoDocRepository manCoDocRepository;

        [SetUp]
        public void Setup()
        {
            this.transactionScope = new TransactionScope();
            this.manCoDocRepository = new ManCoDocRepository(ConfigurationManager.ConnectionStrings["BBOS"].ConnectionString);
        }

        [TearDown]
        public void TearDown()
        {
            this.transactionScope.Dispose();
        }

        [Test]
        public void GivenADocTypeManCoAndEnvironment_WhenIAskForTheManCoDoc_IGetTheManCoDoc()
        {
            var result = this.manCoDocRepository.GetManCoDoc("AY", "DIS", "GTA Distribution Tax Vouchers", "Test");

            result.ManCoID.Should().Be(1);
        }

        [Test]
        public void GiveAValidData_WhenDatabaseIsAvailable_IShouldBeAbleToPromoteMancoDocs_FromOneEnvironmentToAnother()
        {
            string manCoDocIds = "10,13";

            Int32 mancoDocsCount = this.manCoDocRepository.Entities.Count();

            this.manCoDocRepository.PromoteMancoDocs(manCoDocIds, "Test", "DryRun", "88fbb257-3de9-4640-a763-d1b0585726f7", "Test");

            this.manCoDocRepository.Entities.Count().ShouldBeEquivalentTo(mancoDocsCount + manCoDocIds.Split(',').Length);
        }

        [Test]
        public void GivenAMancoCodeEnvironmentUserID_WhenDatabaseIsAvailable_IShouldGetMancoDocs()
        {
            var result = this.manCoDocRepository.GetManCoDocsByManCoCodeEnvironment("BA", "Test", "1a323edd-0732-4ef6-b2a2-4647df9dfd74");

            result.Count().Should().BeGreaterThan(0);
            result.GroupBy(x => x.ManCoID).Count().Should().Be(1);
        }
    }
}
