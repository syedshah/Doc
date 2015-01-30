// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OneStepFileRepositoryTests.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace FileRepositoryTests
{
  using System;
  using System.IO;

  using SystemFileAdapter;

  using FileRepository.Repositories;

  using FileSystemInterfaces;

  using FluentAssertions;

  using NUnit.Framework;

  [Category("Integration")]
  [TestFixture]
  public class OneStepFileRepositoryTests
  {
    private const String BaseDirectory = "files";

    private IFileInfoFactory fileInfoFactory;

    private IDirectoryInfo directoryInfo;

    private OneStepFileRepository oneStepFileRepository;

    private String path = "\\\\srv-dg-bravura4.dstoutput.co.uk\\Nexdox\\nmbbs02\\Process\\141756-142156X-JMDD\\Output\\141756-142156X-JMDD";

    [SetUp]
    public void Setup()
    {
      this.fileInfoFactory = new SystemFileInfoFactory();
      this.directoryInfo = new SystemIoDirectoryInfo();

      var di = new DirectoryInfo(BaseDirectory);

      this.oneStepFileRepository = new OneStepFileRepository(BaseDirectory, this.fileInfoFactory, this.directoryInfo);
    }

    [TearDown]
    public void TearDown()
    {
    }

    [Test]
    public void GivenAOneStepFileRepository_WhenIWantToGetTheOneStepLog_TheOneStepLogIsRetrieved()
    {
      var result = this.oneStepFileRepository.GetOneStepLog(this.path);

      result.Should().NotBeNull();
    }

    [Test]
    public void GivenAOneStepFileRepository_WhenIWantToGetAllOneStepLogsThroughFilePathAndExtension_ThenIGetAListOfOneStepLogs()
    {
      var result = this.oneStepFileRepository.GetFiles(this.path, "*.LOG");

      result.Should().NotBeNull();
    }
  }
}
