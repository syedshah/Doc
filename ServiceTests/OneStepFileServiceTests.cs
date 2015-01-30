// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OneStepFileServiceTests.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   OneStep File service tests
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
  public class OneStepFileServiceTests
  {
    private Mock<IOneStepFileRepository> oneStepFileRepository;

    private Mock<IEnvironmentTypeService> environmentTypeService;

    private Mock<IFilePathBuilderEngine> filePathBuilderEngine;

    private OneStepFileService oneStepFileService;

    private OneStepLog oneStepLog;

    private IList<EnvironmentServerEntity> servers;

    [SetUp]
    public void SetUp()
    {
      this.oneStepFileRepository = new Mock<IOneStepFileRepository>();
      this.filePathBuilderEngine = new Mock<IFilePathBuilderEngine>();
      this.environmentTypeService = new Mock<IEnvironmentTypeService>();
      this.servers = new List<EnvironmentServerEntity>();
      this.oneStepFileService = new OneStepFileService(
        this.oneStepFileRepository.Object, 
        this.environmentTypeService.Object,
        this.filePathBuilderEngine.Object);
      String[] content = new String[2] { "first line", "second line" };
      this.oneStepLog = new OneStepLog() { LogContent = content, LogFileName = "LogFileName" };
      this.servers.Add(new EnvironmentServerEntity() { ServerName = "Bravura Server" });
    }

    [Test]
    public void GivenAOneStepFileService_WhenIWantToGetLogsAndThereIsOnlyOneServerEnvironmentAndLogFileIsAvailable_OneStepLogsAreRetrieved()
    {
      this.environmentTypeService.Setup(x => x.GetEnvironmentServers(It.IsAny<String>())).Returns(this.servers);
      this.filePathBuilderEngine.Setup(x => x.BuildOneStepLogFilePath(It.IsAny<String>(), It.IsAny<String>())).Returns("OneStepLogPath");
      this.oneStepFileRepository.Setup(x => x.GetOneStepLog(It.IsAny<String>())).Returns(this.oneStepLog);

      var result = this.oneStepFileService.GetOneStepLog(It.IsAny<String>(), It.IsAny<String>());

      result.Should().NotBeNull();
      result.LogContent.ShouldBeEquivalentTo(this.oneStepLog.LogContent);
      result.LogFileName.ShouldBeEquivalentTo(this.oneStepLog.LogFileName);
      this.oneStepFileRepository.Verify(x => x.GetOneStepLog(It.IsAny<String>()), Times.AtLeastOnce);
      this.environmentTypeService.Verify(x => x.GetEnvironmentServers(It.IsAny<String>()), Times.AtLeastOnce);
      this.filePathBuilderEngine.Verify(x => x.BuildOneStepLogFilePath(It.IsAny<String>(), It.IsAny<String>()), Times.AtLeastOnce);
    }

    [Test]
    [ExpectedException(typeof(DocProcessingException), ExpectedMessage = "More than one processing enviroment available for ")]
    public void GivenAOneStepFileService_WhenIWantToGetLogsAndThereIsMoreThanOneServerEnvironment_AnExceptionIsThrown()
    {
      this.servers.Add(new EnvironmentServerEntity() { ServerName = "Bravura Server2" });
      this.environmentTypeService.Setup(x => x.GetEnvironmentServers(It.IsAny<String>())).Returns(this.servers);
      this.filePathBuilderEngine.Setup(x => x.BuildOneStepLogFilePath(It.IsAny<String>(), It.IsAny<String>())).Returns("One Step Log Path");
      this.oneStepFileRepository.Setup(x => x.GetOneStepLog(It.IsAny<String>())).Returns(this.oneStepLog);

      this.oneStepFileService.GetOneStepLog(It.IsAny<String>(), It.IsAny<String>());

      this.environmentTypeService.Verify(x => x.GetEnvironmentServers(It.IsAny<String>()), Times.AtLeastOnce);
      this.filePathBuilderEngine.Verify(x => x.BuildOneStepLogFilePath(It.IsAny<String>(), It.IsAny<String>()), Times.AtLeastOnce);
    }

    [Test]
    [ExpectedException(typeof(DocProcessingException), ExpectedMessage = "Unable to retrieve one step log")]
    public void GivenAOneStepFileService_WhenIWantToGetLogsAndThereIsOnlyOneServerEnvironmentAndLogFileIsUnAvailable_AnExceptionIsThrown()
    {
      this.environmentTypeService.Setup(x => x.GetEnvironmentServers(It.IsAny<String>())).Returns(this.servers);
      this.filePathBuilderEngine.Setup(x => x.BuildOneStepLogFilePath(It.IsAny<String>(), It.IsAny<String>())).Returns("One Step Log Path");
      this.oneStepFileRepository.Setup(x => x.GetOneStepLog(It.IsAny<String>())).Throws<DocProcessingException>();

      this.oneStepFileService.GetOneStepLog(It.IsAny<String>(), It.IsAny<String>());
    }
  }
}
