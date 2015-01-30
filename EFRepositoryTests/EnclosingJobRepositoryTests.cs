// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnclosingJobRepositoryTests.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------


namespace EFRepositoryTests
{
    using System.Configuration;
    using System.Linq;
    using System.Transactions;
    using DocProcessingRepository.Repositories;
    using FluentAssertions;
    using NUnit.Framework;

    [Category("Integration")]
    [TestFixture]
    public class EnclosingJobRepositoryTests
    {
        private TransactionScope transactionScope;
        private EnclosingJobRepository enclosingJobRepository;
        private int testJobID;

        [SetUp]
        public void SetUp()
        {
            this.transactionScope = new TransactionScope();
            this.enclosingJobRepository = new EnclosingJobRepository(ConfigurationManager.ConnectionStrings["BBOS"].ConnectionString);
            this.testJobID = 40000152;
        }

        [TearDown]
        public void TearDown()
        {
            this.transactionScope.Dispose();
        }

        [Test]
        public void GivenAJobID_WhenTheDatabaseIsAvailable_WhenICallGetEnclosingJobsByJobId_IShouldGetAllEnclosingJobsAssociatedToAJob()
        {
            var result = this.enclosingJobRepository.GetEnclosingJobsByJobId(testJobID);

            result.ToList().Count().ShouldBeEquivalentTo(1);
        }

        [Test]
        public void GivenAValidData_WhenTheDatabaseIsAvailable_WhenICallUpdateEnclosingJobDocketNumber_ItShouldUpdated()
        {
            var enclosingJobs = this.enclosingJobRepository.GetEnclosingJobsByJobId(testJobID);

            int testEnclosaingJobId = enclosingJobs.First().EnclosingJobID;

            this.enclosingJobRepository.UpdateEnclosingJobDocketNumber(testEnclosaingJobId, "testDocNumber");

            enclosingJobs = this.enclosingJobRepository.GetEnclosingJobsByJobId(testJobID);

            enclosingJobs.ToList().Find(x => x.EnclosingJobID == testEnclosaingJobId).PostalDocketNumber.ShouldAllBeEquivalentTo("testDocNumber");
        }
    }
}
