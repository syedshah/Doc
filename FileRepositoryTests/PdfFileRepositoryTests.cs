// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PdfFileRepositoryTests.cs" company="DST Nexdox">
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

  using Entities.File;

  using FileRepository.Repositories;

  using FileSystemInterfaces;

  using FluentAssertions;

  using NUnit.Framework;

  [Category("Integration")]
  [TestFixture]
  public class PdfFileRepositoryTests
  {
    IFileInfoFactory fileInfoFactory;

    private IDirectoryInfo directoryInfo;

    const String BaseDirectory = "files";

    PdfFileRepository pdfFileRepository;

    private String path = "\\\\srv-dg-bravura4.dstoutput.co.uk\\Nexdox\\nmbbs02\\Process\\141756-142156X-JMDD\\Output\\";

    private String pdfInformationFile;

    private IList<PdfFile> pdfFiles;

    [SetUp]
    public void Setup()
    {
      this.fileInfoFactory = new SystemFileInfoFactory();
      this.directoryInfo = new SystemIoDirectoryInfo();

      var di = new DirectoryInfo(BaseDirectory);

      this.pdfFileRepository = new PdfFileRepository(BaseDirectory, this.fileInfoFactory, this.directoryInfo);

      this.pdfFiles = new List<PdfFile>();
      this.pdfFiles.Add(new PdfFile("fileName", "filePath"));
      this.pdfInformationFile = "\\\\srv-dg-bravura4.dstoutput.co.uk\\Nexdox\\nmbbs02\\Process\\141756-142156X-JMDD\\Output\\141756-142156X-JMDD-PDFFiles.web";
    }

    [TearDown]
    public void TearDown()
    {
    }

    [Test]
    public void GivenAPdfFileRepository_WhenIWantToGetPdfFilesThroughPdfInformationFile_IGetAListOfPdfFiles()
    {
      var result = this.pdfFileRepository.GetPdfFiles(this.pdfInformationFile);

      result.Should().NotBeNull();
    }

    [Test]
    public void GivenAPdfFileRepository_WhenIWantToGetAllPdfFilesThroughFilePathAndExtension_ThenIGetAListOfPdfFiles()
    {
      var result = this.pdfFileRepository.GetPdfFiles(path, "*.PDF");

      result.Should().NotBeNull();
    }
  }
}
