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
  public class AppEnvironmentRepostoryTests
  {
    private TransactionScope transactionScope;
    private AppEnvironmentRepository appEnvironmentRepository;

    [SetUp]
    public void Setup()
    {
      this.transactionScope = new TransactionScope();
      this.appEnvironmentRepository = new AppEnvironmentRepository(ConfigurationManager.ConnectionStrings["BBOS"].ConnectionString);
    }

    [TearDown]
    public void TearDown()
    {
      this.transactionScope.Dispose();
    }

    [Test]
    public void GivenAUser_WhenIAskForTheirEnvironment_IGetTheEnvironmentsTheyAreIn()
    {
      var result = this.appEnvironmentRepository.GetAppEnvironments("1a323edd-0732-4ef6-b2a2-4647df9dfd74");

      result.Should().Contain(e => e.Name == "Test");
    }

    [Test]
    public void WhenIAskForAppEnvironments_IGetTheEnvironmentsTheyAreInTheDatabase()
    {
        var result = this.appEnvironmentRepository.GetAppEnvironments();

        result.Should().NotBeNull();
        result.Count.Should().BeGreaterThan(0);
    }
  }
}
