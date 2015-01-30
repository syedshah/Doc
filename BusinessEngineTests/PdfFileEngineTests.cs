// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PdfFileEngineTests.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   Pdf File Engine Tests 
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace BusinessEngineTests
{
  using System;
  using System.Collections.Generic;

  using BusinessEngineInterfaces;

  using BusinessEngines;

  using Entities;
  using Entities.File;

  using Exceptions;

  using FileRepository.Interfaces;

  using FluentAssertions;

  using Moq;

  using NUnit.Framework;

  using ServiceInterfaces;

  [Category("Unit")]
  [TestFixture]
  public class PdfFileEngineTests
  {
    private Mock<IPdfFileRepository> pdfFileRepository;

    private Mock<IFilePathBuilderEngine> filePathBuilderEngine;

    private Mock<IEnvironmentTypeService> environmentTypeService;

    private PdfFileEngine pdfFileEngine;

    private IList<PdfFile> pdfFiles;

    private IList<EnvironmentServerEntity> servers;
      
    [SetUp]
    public void SetUp()
    {
      this.pdfFileRepository = new Mock<IPdfFileRepository>();
      this.filePathBuilderEngine = new Mock<IFilePathBuilderEngine>();
      this.environmentTypeService = new Mock<IEnvironmentTypeService>();
      this.pdfFileEngine = new PdfFileEngine(
        this.pdfFileRepository.Object,
        this.filePathBuilderEngine.Object,
        this.environmentTypeService.Object);

      this.pdfFiles = new List<PdfFile>();
      this.pdfFiles.Add(new PdfFile("fileName", "filePath"));

      this.servers = new List<EnvironmentServerEntity>();
      this.servers.Add(new EnvironmentServerEntity() { ServerName = "BravuraDev", EnvironmentType = "Development" });
    }

    [Test]
    public void GivenAPdfFileEngine_WhenIWantToGetPdfFiles_ThePdfFilesAreReturned()
    {
      this.pdfFileRepository.Setup(x => x.GetPdfFiles(It.IsAny<String>())).Returns(this.pdfFiles);
      this.environmentTypeService.Setup(x => x.GetEnvironmentServers(It.IsAny<String>())).Returns(this.servers);
      this.filePathBuilderEngine.Setup(x => x.BuildOneStepOutputDirectory(It.IsAny<String>(), It.IsAny<String>()));

      var result = this.pdfFileEngine.GetPdfFiles(It.IsAny<String>(), It.IsAny<String>());

      result.Should().NotBeNull();
      result.Count.ShouldBeEquivalentTo(1);
      result.ShouldBeEquivalentTo(this.pdfFiles);

      this.environmentTypeService.Verify(x => x.GetEnvironmentServers(It.IsAny<String>()), Times.AtLeastOnce);
      this.pdfFileRepository.Verify(x => x.GetPdfFiles(It.IsAny<String>()), Times.AtLeastOnce);
      this.filePathBuilderEngine.Verify(x => x.BuildOneStepOutputDirectory(It.IsAny<String>(), It.IsAny<String>()), Times.AtLeastOnce);
    }

    [Test]
    [ExpectedException(typeof(DocProcessingException), ExpectedMessage = "More than one processing enviroment available for dev")]
    public void GivenAPdfFileEngine_WhenIWantToGetPdfFilesAndMoreThereIsMoreThanOneServer_AnExceptionIsThrown()
    {
      this.servers.Add(new EnvironmentServerEntity() { ServerName = "BravuraDev2", EnvironmentType = "Development2" });
     
      this.environmentTypeService.Setup(x => x.GetEnvironmentServers("dev")).Returns(this.servers);
     
      this.pdfFileEngine.GetPdfFiles(It.IsAny<String>(), "dev");
    }
  }
}
