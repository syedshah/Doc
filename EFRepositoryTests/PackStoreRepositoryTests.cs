// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AppEnvironmentRepostoryTests.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace EFRepositoryTests
{
  using System.Configuration;
  using System.Transactions;
  using DocProcessingRepository.Repositories;
  using FluentAssertions;
  using NUnit.Framework;

  [Category("Integration")]
  [TestFixture]
  public class PackStoreRepositoryTests
  {
    private TransactionScope transactionScope;
    private PackStoreRepository packStoreRepository;

    [SetUp]
    public void Setup()
    {
      this.transactionScope = new TransactionScope();
      this.packStoreRepository = new PackStoreRepository(ConfigurationManager.ConnectionStrings["ADF"].ConnectionString);
    }

    [TearDown]
    public void TearDown()
    {
      this.transactionScope.Dispose();
    }

    [Test]
    public void GivenAJob_WhenIAskForPulledPacks_IGetThePulledPacksForthatJob()
    {
      var result = this.packStoreRepository.GetPulledPacks(1116);

      result.Count.Should().BeGreaterOrEqualTo(1);
      result.Should().NotContain(f => !f.Pulled);
    }

    [Test]
    public void GivenAClientReference_WhenIAskForPacks_IGetPacksThatHaveNotBeenPulled()
    {
      var result = this.packStoreRepository.GetNonPulledPacks("23", 1116);

      result.Count.Should().BeGreaterOrEqualTo(1);
      result.Should().NotContain(f => f.Pulled);
    }
  }
}
