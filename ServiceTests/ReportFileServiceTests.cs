// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReportFileServiceTests.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ServiceTests
{
  using System;
  using System.Collections.Generic;

  using BusinessEngineInterfaces;

  using Entities;
  using Entities.File;

  using Exceptions;

  using FileRepository.Interfaces;

  using FluentAssertions;

  using Moq;

  using NUnit.Framework;

  using ServiceInterfaces;

  using Services;

  [Category("Unit")]
  [TestFixture]
  public class ReportFileServiceTests
  {
    private Mock<IReportFileRepository> reportFileRepository;

    private Mock<IEnvironmentTypeService> environmentTypeService;

    private Mock<IFilePathBuilderEngine> filePathBuilderEngine;

    private ReportFileService reportFileService;

    private String reportPath;

    private IList<ReportFile> reportFiles;

    private IList<EnvironmentServerEntity> servers;

    private ReportFile reportFile;

    [SetUp]
    public void SetUp()
    {
      this.reportFileRepository = new Mock<IReportFileRepository>();
      this.filePathBuilderEngine = new Mock<IFilePathBuilderEngine>();
      this.environmentTypeService = new Mock<IEnvironmentTypeService>();
      this.servers = new List<EnvironmentServerEntity>();
      this.reportFileService = new ReportFileService(
        this.reportFileRepository.Object,
        this.filePathBuilderEngine.Object,
        this.environmentTypeService.Object);
      this.reportPath = "report path";
      this.reportFiles = new List<ReportFile>();
      String[] fileContent1 = new String[1] { "filecontent1" };
      String[] fileContent2 = new String[1] { "filecontent2" };
      this.reportFiles.Add(new ReportFile("filename1", "filepath1", fileContent1));
      this.reportFiles.Add(new ReportFile("filename2", "filepath2", fileContent2));
      this.servers.Add(new EnvironmentServerEntity() { ServerName = "Bravura Server" });
      String[] fileContent = new String[1] { "filecontent" };
      this.reportFile = new ReportFile("filename", "filepath", fileContent);
    }

    [Test]
    public void GivenAValidReportPathDirectory_WhenIWantoGetTheReportFilesAndThereIsOnlyOneServerEnvironmentAndFilesAreAvailable_TheReportFilesAreRetrieved()
    {
      this.environmentTypeService.Setup(x => x.GetEnvironmentServers(It.IsAny<String>())).Returns(this.servers);
      this.filePathBuilderEngine.Setup(x => x.BuildOneStepOutputDirectory(It.IsAny<String>(), It.IsAny<String>())).Returns("OneStepLogDirectory");
      this.reportFileRepository.Setup(x => x.GetReportFiles(It.IsAny<String>())).Returns(this.reportFiles);

      var result = this.reportFileService.GetReportFiles(It.IsAny<String>(), It.IsAny<String>());

      result.Should().NotBeNull();
      result.Count.ShouldBeEquivalentTo(2);

      this.reportFileRepository.Verify(x => x.GetReportFiles(It.IsAny<String>()), Times.AtLeastOnce);
      this.environmentTypeService.Verify(x => x.GetEnvironmentServers(It.IsAny<String>()), Times.AtLeastOnce);
      this.filePathBuilderEngine.Verify(x => x.BuildOneStepOutputDirectory(It.IsAny<String>(), It.IsAny<String>()), Times.AtLeastOnce);
    }

    [Test]
    [ExpectedException(typeof(DocProcessingException), ExpectedMessage = "More than one processing enviroment available for ")]
    public void GivenAValidReportPathDirectory_WhenIWantoGetTheReportFilesAndThereIsMoreThanOneServerEnvironmentAndFilesAreAvailable_AnExceptionIsThrown()
    {
      this.servers.Add(new EnvironmentServerEntity() { ServerName = "Bravura Server2" });
      this.environmentTypeService.Setup(x => x.GetEnvironmentServers(It.IsAny<String>())).Returns(this.servers);
      this.filePathBuilderEngine.Setup(x => x.BuildOneStepOutputDirectory(It.IsAny<String>(), It.IsAny<String>())).Returns("OneStepLogDirectory");
      this.reportFileRepository.Setup(x => x.GetReportFiles(It.IsAny<String>())).Returns(this.reportFiles);

      this.reportFileService.GetReportFiles(It.IsAny<String>(), It.IsAny<String>());

      this.environmentTypeService.Verify(x => x.GetEnvironmentServers(It.IsAny<String>()), Times.AtLeastOnce);
      this.filePathBuilderEngine.Verify(x => x.BuildOneStepOutputDirectory(It.IsAny<String>(), It.IsAny<String>()), Times.AtLeastOnce);
    }

    [Test]
    [ExpectedException(typeof(DocProcessingException), ExpectedMessage = "Unable to get report files")]
    public void GivenAValidReportPathDirectory_WhenIWantoGetTheReportFilesAndFilesAreUnavailable_ThenAnExceptionIsThrown()
    {
      this.environmentTypeService.Setup(x => x.GetEnvironmentServers(It.IsAny<String>())).Returns(this.servers);
      this.filePathBuilderEngine.Setup(x => x.BuildOneStepOutputDirectory(It.IsAny<String>(), It.IsAny<String>())).Returns("OneStepLogDirectory");
      this.reportFileRepository.Setup(x => x.GetReportFiles(It.IsAny<String>())).Throws<DocProcessingException>();

      this.reportFileService.GetReportFiles(It.IsAny<String>(), It.IsAny<String>());
    }

    [Test]
    public void GivenAReportPath_WhenIWantToGetAReportFileAndIsAvailable_AReportFileIsRetrieved()
    {
      this.reportFileRepository.Setup(x => x.GetReportFile(It.IsAny<String>())).Returns(this.reportFile);

      var result = this.reportFileService.GetReportFile(It.IsAny<String>());

      result.Should().NotBeNull();
      result.ReportContent.ShouldBeEquivalentTo(this.reportFile.ReportContent);
      result.ReportFileName.ShouldBeEquivalentTo(this.reportFile.ReportFileName);
      result.ReportFilePath.ShouldBeEquivalentTo(this.reportFile.ReportFilePath);
      this.reportFileRepository.Verify(x => x.GetReportFile(It.IsAny<String>()), Times.AtLeastOnce);
    }

    [Test]
    [ExpectedException(typeof(DocProcessingException), ExpectedMessage = "Unable to get report")]
    public void GivenAReportPath_WhenIWantToGetAReportFileAndIsUnAvailable_AReportFileIsRetrieved()
    {
      this.reportFileRepository.Setup(x => x.GetReportFile(It.IsAny<String>())).Throws<DocProcessingException>();

      this.reportFileService.GetReportFile(It.IsAny<String>());
    }
  }
}
