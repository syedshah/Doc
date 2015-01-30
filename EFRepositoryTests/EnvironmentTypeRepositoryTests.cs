// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnvironmentTypeRepositoryTests.cs" company="DST Nexdox">
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
  using BuildMeA;
  using DocProcessingRepository.Repositories;

  using Entities;

  using FluentAssertions;

  using NUnit.Framework;

  [Category("Integration")]
  [TestFixture]
  public class EnvironmentTypeRepositoryTests
  {
    private TransactionScope transactionScope;

    private EnvironmentTypeRepository environmentTypeRepository;

    private AppEnvironmentRepository appEnvironmentRepostory;

    private EnvironmentType environmentType;

    private AppEnvironment appEnvironment;

    [SetUp]
    public void SetUp()
    {
      this.transactionScope = new TransactionScope();
      this.environmentTypeRepository = new EnvironmentTypeRepository(ConfigurationManager.ConnectionStrings["BBOS"].ConnectionString);
      this.appEnvironmentRepostory = new AppEnvironmentRepository(ConfigurationManager.ConnectionStrings["BBOS"].ConnectionString);
      this.appEnvironment = BuildMeA.AppEnvironment("CustomAppEnvironment");
      this.environmentType = BuildMeA.EnvironmentType("CustomAppEnvironment", "Server name 1", "adf_db_name 1", 1, "bravura DOCS Environment Type", "b", "e");
    }

    [TearDown]
    public void TearDown()
    {
      this.transactionScope.Dispose();
    }

    [Test]
    public void GivenAnEvironmentTypeRepository_WhenIWantToGetTheEnvironmentServers_IGetAListOfEnvironmentServers()
    {
      this.appEnvironmentRepostory.Create(this.appEnvironment);
      this.environmentType.AppEnvironmentID = this.appEnvironment.AppEnvironmentID;
      this.environmentTypeRepository.Create(this.environmentType);
      var result = this.environmentTypeRepository.GetEnvironmentServers("CustomAppEnvironment");

      result.Should().NotBeNull();
      result.Count.Should().BeGreaterOrEqualTo(1);
    }
  }
}
