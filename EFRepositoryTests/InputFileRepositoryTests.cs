// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JobRepositoryTests.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   Tests for Job repository
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace EFRepositoryTests
{
  using System;
  using System.Configuration;
  using System.Linq;
  using System.Transactions;
  using BuildMeA;
  using DocProcessingRepository.Repositories;
  using Entities;
  using FluentAssertions;
  using NUnit.Framework;

  [Category("Integration")]
  [TestFixture]
  public class InputFileRepositoryTests
  {
    private TransactionScope transactionScope;
    private InputFileRepository inputFileRepository;
    
    [SetUp]
    public void SetUp()
    {
      this.transactionScope = new TransactionScope();
      this.inputFileRepository = new InputFileRepository(ConfigurationManager.ConnectionStrings["BBOS"].ConnectionString);
    }

    [TearDown]
    public void TearDown()
    {
      this.transactionScope.Dispose();
    }

    [Test]
    public void WhenItryToInsertAJob_ItIsSavedToTheJobTable()
    {
      Int32 initialCount = this.inputFileRepository.Entities.Count();

      this.inputFileRepository.InsertFile("fileName.txt", "filePath", 30000001, 1);

      this.inputFileRepository.Entities.Count().Should().Be(initialCount + 1);
    }

    [Test]
    public void GivenAPathAndFileName_WhenISearchForInputFiles_IGetOneInputFile()
    {
      var file = this.inputFileRepository.GetInputFile("InputDataFileTest1.inp", @"\\[Processing Server]\Nexdox\NXBNY[nn]\Input\[ManagementCompanyCode]\[DocumentTypeCode]\");

      file.Should().NotBeNull();
    }
  }
}
