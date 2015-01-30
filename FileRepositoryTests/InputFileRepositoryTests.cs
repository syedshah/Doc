// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InputFileRepositoryTests.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FileRepositoryTests
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using SystemFileAdapter;
  using FileRepository.Repositories;
  using FileSystemInterfaces;
  using FluentAssertions;
  using NUnit.Framework;

  [Category("Integration")]
  [TestFixture]
  public class InputFileRepositoryTests
  {
    IFileInfoFactory fileInfoFactory;
    private IDirectoryInfo directoryInfo;
    const String BaseDirectory = "files";
    InputFileRepository inputFileRepository;

    private String path = "\\\\srv-dg-bravura4.dstoutput.co.uk\\Nexdox\\nmbbs01\\Input\\AY\\DIS";

    [SetUp]
    public void Setup()
    {
      this.fileInfoFactory = new SystemFileInfoFactory();
      this.directoryInfo = new SystemIoDirectoryInfo();

      var di = new DirectoryInfo(BaseDirectory);

      this.inputFileRepository = new InputFileRepository(BaseDirectory, this.fileInfoFactory, this.directoryInfo);
    }

    [TearDown]
    public void TearDown()
    {

    }

    [Test]
    public void GivenAFileStucture_WhenITryToReadTheDataInTheDirectory_ThenTheFileDataIsCorrect()
    {
      var fileInfoData = this.inputFileRepository.GetInputFileInfo(path);

      fileInfoData.Folder.Should().Be(path);
      fileInfoData.Files.Count.Should().BeGreaterOrEqualTo(1);
      fileInfoData.Folders.Count.Should().BeGreaterOrEqualTo(1);
    }

    [Test]
    public void GivenAFileStucture_WhenITryToReadTheAdditionalFileInfo_ThenTheFileDataIsCorrect()
    {
      String path = "\\\\srv-dg-bravura4.dstoutput.co.uk\\Nexdox\\nmbbs01\\Input\\RI\\RTV";
      List<String> files = new List<string>()
                             {
                               "CRIT00.B3P",
                               "CRIT00_25062014123414.B3P"
                             };


      var additonalFileInfo = this.inputFileRepository.GetAdditionalInfo(path, files);
      
      additonalFileInfo.AdditionalFileContent.Should().Be("RIT - RIT Capital Partners plc, 108 Warrants");
      additonalFileInfo.Warrants.Should().Be(108);
    }

    [Test]
    public void GivenAFileStucture_WhenITryToReadTheAdditionalFileInfo_AndWarrantsIsZero_ThenTheFileDataIsCorrect()
    {
      String path = "\\\\srv-dg-bravura4.dstoutput.co.uk\\Nexdox\\nmbbs01\\Input\\RI\\RTV";
      List<String> files = new List<string>()
                             {
                               "BRIT00.B3P"
                             };


      var additonalFileInfo = this.inputFileRepository.GetAdditionalInfo(path, files);
      
      additonalFileInfo.AdditionalFileContent.Should().Be(String.Empty);
      additonalFileInfo.Warrants.Should().Be(0);
    }
  }
}
