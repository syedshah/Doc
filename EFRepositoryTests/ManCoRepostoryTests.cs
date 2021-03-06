﻿// --------------------------------------------------------------------------------------------------------------------
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
  public class ManCoRepostoryTests
  {
    private TransactionScope transactionScope;
    private ManCoRepostory manCoRepostory;

    [SetUp]
    public void Setup()
    {
      this.transactionScope = new TransactionScope();
      this.manCoRepostory = new ManCoRepostory(ConfigurationManager.ConnectionStrings["BBOS"].ConnectionString);
    }

    [TearDown]
    public void TearDown()
    {
      this.transactionScope.Dispose();
    }

    [Test]
    public void GivenAUser_WhenIAskForTheManCosTheyHaveAccessTo_IGetManCos()
    {
      var result = this.manCoRepostory.GetManCos("1a323edd-0732-4ef6-b2a2-4647df9dfd74");

      result.Should().Contain(e => e.ManCoName == "Abbey")
        .And.Contain(e => e.ManCoName == "AIM Global");
    }
  }
}
