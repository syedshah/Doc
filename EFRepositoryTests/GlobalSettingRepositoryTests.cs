// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GlobalSettingRepositoryTests.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   Tests for global setting repository
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace EFRepositoryTests
{
  using System.Configuration;
  using System.Transactions;
  using BuildMeA;
  using DocProcessingRepository.Repositories;
  using Entities;
  using FluentAssertions;
  using NUnit.Framework;

  [Category("Integration")]
  [TestFixture]
  public class GlobalSettingRepositoryTests
  {
    private TransactionScope transactionScope;
    private GlobalSettingRepository globalSettingRepository;
    private GlobalSetting globalSetting;

    [SetUp]
    public void SetUp()
    {
      this.transactionScope = new TransactionScope();
      this.globalSettingRepository = new GlobalSettingRepository(ConfigurationManager.ConnectionStrings["BBOS"].ConnectionString);
      this.globalSetting = BuildMeA.GlobalSetting(5, 0, 30, 12, false);
    }

    [TearDown]
    public void TearDown()
    {
      transactionScope.Dispose();
    }

    [Test]
    public void WhenIWantToRetrieveGlobalSetting_ItIsRetrievedFromTheDatabase()
    {
      this.globalSettingRepository.Create(this.globalSetting);
      var result = this.globalSettingRepository.Get();
      result.Should().NotBeNull();
    }
  }
}
