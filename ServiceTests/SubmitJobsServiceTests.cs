namespace ServiceTests
{
  using System;
  using System.Collections.Generic;
  using BusinessEngineInterfaces;
  using Entities;
  using Entities.File;
  using Exceptions;
  using FluentAssertions;
  using Moq;
  using NUnit.Framework;
  using ServiceInterfaces;
  using Services;

  [Category("Unit")]
  [TestFixture]
  public class SubmitJobsServiceTests
  {
    private Mock<IEnvironmentTypeService> environmentTypeService;
    private Mock<IFilePathBuilderEngine> filePathBuilderEngine;
    private Mock<FileRepository.Interfaces.IInputFileRepository> inputFileRepository;
    private ISubmitJobsService submitJobsService;

    [SetUp]
    public void SetUp()
    {
      this.environmentTypeService = new Mock<IEnvironmentTypeService>();
      this.filePathBuilderEngine = new Mock<IFilePathBuilderEngine>();
      this.inputFileRepository = new Mock<FileRepository.Interfaces.IInputFileRepository>();
      this.submitJobsService = new SubmitJobsService(this.environmentTypeService.Object, this.filePathBuilderEngine.Object, this.inputFileRepository.Object);
    }

    [Test]
    public void GivenAEnvirommentManCoAndDocType_WhenITryToRetrieFileInfo_IGetFileInfo()
    {
      this.environmentTypeService.Setup(a => a.GetEnvironmentServers(It.IsAny<String>())).Returns(new List<EnvironmentServerEntity>() { new EnvironmentServerEntity() }) ;
      this.filePathBuilderEngine.Setup(
        f => f.BuildInputFileLocationPath(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>())).Returns("path");

      this.inputFileRepository.Setup(f => f.GetInputFileInfo(It.IsAny<String>())).Returns(new InputFileInfo());
      var result = this.submitJobsService.GetInputFiles(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>());

      this.inputFileRepository.Verify(x => x.GetInputFileInfo(It.IsAny<String>()), Times.Once);
      result.Should().NotBeNull();
    }

    [Test]
    public void GivenAEnvirommentManCoAndDocType_WhenITryToRetrieFileInfo_AndThereIsMoreThanOneProcessingEnviroment_ThenIGetADocProcessingException()
    {
      this.environmentTypeService.Setup(a => a.GetEnvironmentServers(It.IsAny<String>())).Returns(new List<EnvironmentServerEntity>() { new EnvironmentServerEntity(), new EnvironmentServerEntity()});
      
      Action act = () => this.submitJobsService.GetInputFiles(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>());

      act.ShouldThrow<DocProcessingException>();
    }

    public void GivenAEnvirommentManCoAndDocType_WhenITryToRetrieFileInfo_AndIAmNotAbleToGetFileInfo_ThenIGetADocProcessingException()
    {
      this.environmentTypeService.Setup(a => a.GetEnvironmentServers(It.IsAny<String>())).Returns(new List<EnvironmentServerEntity>());
      this.filePathBuilderEngine.Setup(
        f => f.BuildInputFileLocationPath(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>())).Returns("path");

      this.inputFileRepository.Setup(f => f.GetInputFileInfo(It.IsAny<String>())).Returns(new InputFileInfo());
      Action act = () => this.submitJobsService.GetInputFiles(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>());

      act.ShouldThrow<DocProcessingException>();
    }

    [Test]
    public void GivenAPathEnvirommentManCoAndDocType_WhenITryToRetrieFileInfo_IGetFileInfo()
    {
      this.environmentTypeService.Setup(a => a.GetEnvironmentServers(It.IsAny<String>())).Returns(new List<EnvironmentServerEntity>() { new EnvironmentServerEntity() });
      this.filePathBuilderEngine.Setup(
        f => f.BuildInputFileLocationPath(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>())).Returns("path");

      this.inputFileRepository.Setup(f => f.GetInputFileInfo(It.IsAny<String>())).Returns(new InputFileInfo());
      var result = this.submitJobsService.GetInputFiles(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>());

      this.inputFileRepository.Verify(x => x.GetInputFileInfo(It.IsAny<String>()), Times.Exactly(2));
      result.Should().NotBeNull();
    }

    [Test]
    public void GivenAPathEnvirommentManCoAndDocType_WhenITryToRetrieFileInfo_AndThereIsMoreThanOneProcessingEnviroment_ThenIGetADocProcessingException()
    {
      this.environmentTypeService.Setup(a => a.GetEnvironmentServers(It.IsAny<String>())).Returns(new List<EnvironmentServerEntity>() { new EnvironmentServerEntity(), new EnvironmentServerEntity() });

      Action act = () => this.submitJobsService.GetInputFiles(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>());

      act.ShouldThrow<DocProcessingException>();
    }

    public void GivenAPathEnvirommentManCoAndDocType_WhenITryToRetrieFileInfo_AndIAmNotAbleToGetFileInfo_ThenIGetADocProcessingException()
    {
      this.environmentTypeService.Setup(a => a.GetEnvironmentServers(It.IsAny<String>())).Returns(new List<EnvironmentServerEntity>());
      this.filePathBuilderEngine.Setup(
        f => f.BuildInputFileLocationPath(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>())).Returns("path");

      this.inputFileRepository.Setup(f => f.GetInputFileInfo(It.IsAny<String>())).Returns(new InputFileInfo());
      Action act = () => this.submitJobsService.GetInputFiles(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>());

      act.ShouldThrow<DocProcessingException>();
    }

    [Test]
    public void GivenAPathAndFiles_WhenITryToRetrieFileInfo_IGetFileInfo()
    {
      this.inputFileRepository.Setup(f => f.GetAdditionalInfo(It.IsAny<String>(), It.IsAny<List<String>>())).Returns(new AdditionalInfoFile());
      var result = this.submitJobsService.GetAdditionalInfo(It.IsAny<String>(), It.IsAny<List<String>>());

      this.inputFileRepository.Verify(x => x.GetAdditionalInfo(It.IsAny<String>(), It.IsAny<List<String>>()), Times.Exactly(1));
      result.Should().NotBeNull();
    }

    [Test]
    public void GivenAPathAndFiles_WhenITryToRetrieFileInfo_AndICannotGetTheInputFileInfo_IGetFileInfo_ThenIGetADocProcessingException()
    {
      this.inputFileRepository.Setup(x => x.GetAdditionalInfo(It.IsAny<String>(), It.IsAny<List<String>>())).Throws<DocProcessingException>();

      Action act = () => this.submitJobsService.GetInputFiles(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>());

      act.ShouldThrow<DocProcessingException>();
    }
  }
}
