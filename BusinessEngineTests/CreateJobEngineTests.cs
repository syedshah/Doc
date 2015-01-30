// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FilePathBuilderEngineTests.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   Create Job Engine Tests
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace BusinessEngineTests
{
  using System;
  using System.Collections;
  using System.Collections.Generic;

  using BusinessEngineInterfaces;

  using BusinessEngines;

  using DocProcessingRepository.Interfaces;

  using Entities;

  using Exceptions;

  using FileRepository.Interfaces;

  using FluentAssertions;

  using Moq;

  using NUnit.Framework;

  using ServiceInterfaces;

  using IInputFileRepository = DocProcessingRepository.Interfaces.IInputFileRepository;

  [Category("Unit")]
  [TestFixture]
  public class CreateJobEngineTests
  {
    private CreateJobEngine createJobEngine;

    private Mock<IJobRepository> jobRepository;

    private Mock<IManCoDocRepository> manCoDocRepository;

    private Mock<IEnvironmentTypeService> environmentTypeService;

    private Mock<IFilePathBuilderEngine> filePathBuilderEngine;

    private Mock<IInputFileRepository> inputFileRepository;

    private Mock<ISubmitJobFileRepository> submitJobFileRepository;

    private Mock<IJobStatusRepository> jobStatusRepository;

    private Mock<IJobStatusTypeRepository> jobStatusTypeRepository;

    private List<String> files;

    private JobStatusTypeEntity jobStatusTypeEntity;

    [SetUp]
    public void SetUp()
    {
      this.jobRepository = new Mock<IJobRepository>();
      this.manCoDocRepository = new Mock<IManCoDocRepository>();
      this.environmentTypeService = new Mock<IEnvironmentTypeService>();
      this.filePathBuilderEngine = new Mock<IFilePathBuilderEngine>();
      this.inputFileRepository = new Mock<IInputFileRepository>();
      this.submitJobFileRepository = new Mock<ISubmitJobFileRepository>();
      this.jobStatusTypeRepository = new Mock<IJobStatusTypeRepository>();
      this.createJobEngine = new CreateJobEngine(
        this.jobRepository.Object,
        this.manCoDocRepository.Object,
        this.environmentTypeService.Object,
        this.filePathBuilderEngine.Object,
        this.inputFileRepository.Object,
        this.submitJobFileRepository.Object,
        this.jobStatusTypeRepository.Object);

      this.files = new List<String>() { "file1", "file2" };
      this.jobStatusTypeEntity = new JobStatusTypeEntity() { JobStatusDescription = "description", JobStatusTypeID = 2 };

      this.environmentTypeService.Setup(e => e.GetEnvironmentServers(It.IsAny<String>())).Returns(new List<EnvironmentServerEntity>() { new EnvironmentServerEntity() });
      this.inputFileRepository.Setup(i => i.GetInputFile(It.IsAny<String>(), It.IsAny<String>())).Returns(new List<InputFile>());
      this.filePathBuilderEngine.Setup(f => f.BuildInputFileLocationPath(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>())).Returns("path");
      this.manCoDocRepository.Setup(m => m.GetManCoDoc(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>())).Returns(new ManCoDoc());
      this.jobRepository.Setup(i => i.InsertJob(It.IsAny<Int32>(), It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>(), It.IsAny<Int32>())).Returns(1);
      this.filePathBuilderEngine.Setup(b => b.BuildOneStepMonitorPath(It.IsAny<String>())).Returns("path2");
      this.inputFileRepository.Setup(c => c.InsertFile(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<Int32>(), It.IsAny<Int32>()));
      this.submitJobFileRepository.Setup(s => s.SaveTriggerFile(It.IsAny<Int32>(), It.IsAny<SubmitFile>(), It.IsAny<String>()));
      this.jobStatusTypeRepository.Setup(x => x.GetJobStatusType(It.IsAny<String>())).Returns(this.jobStatusTypeEntity);
     
    }

    [Test]
    public void GivenValidJobData_WhenICreateAJob_AndTheFileHasAlreadyBeenProcessed_IGetADocAlreadyProcessedException()
    {
      this.inputFileRepository.Setup(i => i.GetInputFile(It.IsAny<String>(), It.IsAny<String>())).Returns(new List<InputFile>() { new InputFile() });

      Action act = () => this.createJobEngine.SubmitJob(
        It.IsAny<String>(),
        It.IsAny<String>(),
        It.IsAny<String>(),
        It.IsAny<String>(),
        this.files, 
        It.IsAny<String>(),
        false,
        false,
        It.IsAny<String>(),
        It.IsAny<String>(),
        It.IsAny<String>(),
        It.IsAny<String>(),
        It.IsAny<String>());

      act.ShouldThrow<DocProcessingFileAlreadyProcessedException>();
    }

    [Test]
    public void GivenValidJobData_WhenICreateAJob_AndTheFileHasAlreadyBeenProcessed_ButAllowReprocessingIsSelected_TheJobIsSubmitted()
    {
      this.inputFileRepository.Setup(i => i.GetInputFile(It.IsAny<String>(), It.IsAny<String>())).Returns(new List<InputFile>() { new InputFile() });
      

      this.createJobEngine.SubmitJob(
        It.IsAny<String>(),
        It.IsAny<String>(),
        It.IsAny<String>(),
        It.IsAny<String>(),
        this.files,
        It.IsAny<String>(),
        true,
        false,
        It.IsAny<String>(),
        It.IsAny<String>(),
        It.IsAny<String>(),
        It.IsAny<String>(),
        It.IsAny<String>());

      this.jobRepository.Verify(x => x.InsertJob(It.IsAny<Int32>(), It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>(), It.IsAny<Int32>()), Times.Exactly(1));
      this.inputFileRepository.Verify(x => x.InsertFile(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<Int32>(), It.IsAny<Int32>()), Times.Exactly(2));
      this.submitJobFileRepository.Verify(x => x.SaveTriggerFile(It.IsAny<Int32>(), It.IsAny<SubmitFile>(), It.IsAny<String>()), Times.Exactly(1));
    }

    [Test]
    public void GivenValidJobData_WhenICreateAJob_AndAdditionalInfoIsRequredAndSupplied_TheJobIsSubmitted()
    {
      this.createJobEngine.SubmitJob(
        It.IsAny<String>(),
        It.IsAny<String>(),
        It.IsAny<String>(),
        It.IsAny<String>(),
        this.files,
        It.IsAny<String>(),
        false,
        true,
        "fundInfo",
        "sortCode",
        "accountNumber",
        "chequeNumber",
        It.IsAny<String>());

      this.jobRepository.Verify(x => x.InsertJob(It.IsAny<Int32>(), It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>(), It.IsAny<Int32>()), Times.Exactly(1));
      this.inputFileRepository.Verify(x => x.InsertFile(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<Int32>(), It.IsAny<Int32>()), Times.Exactly(2));
      this.submitJobFileRepository.Verify(x => x.SaveTriggerFile(It.IsAny<Int32>(), It.IsAny<SubmitFile>(), It.IsAny<String>()), Times.Exactly(1));
    }

    [Test]
    public void GivenValidJobData_WhenICreateAJob_TheJobIsSubmitted()
    {
      this.createJobEngine.SubmitJob(
        It.IsAny<String>(),
        It.IsAny<String>(),
        It.IsAny<String>(),
        It.IsAny<String>(),
        this.files,
        It.IsAny<String>(),
        false,
        false,
        It.IsAny<String>(),
        It.IsAny<String>(),
        It.IsAny<String>(),
        It.IsAny<String>(),
        It.IsAny<String>());

      this.jobRepository.Verify(x => x.InsertJob(It.IsAny<Int32>(), It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>(), It.IsAny<Int32>()), Times.Exactly(1));
      this.inputFileRepository.Verify(x => x.InsertFile(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<Int32>(), It.IsAny<Int32>()), Times.Exactly(2));
      this.submitJobFileRepository.Verify(x => x.SaveTriggerFile(It.IsAny<Int32>(), It.IsAny<SubmitFile>(), It.IsAny<String>()), Times.Exactly(1));
    }

    [Test]
    public void GivenValidJobData_WhenICreateAJob_AndTheDataBaseIsUnavailable_IGetADocProcessingException()
    {
      this.inputFileRepository.Setup(i => i.GetInputFile(It.IsAny<String>(), It.IsAny<String>())).Throws<DocProcessingException>();

      Action act = () => this.createJobEngine.SubmitJob(
        It.IsAny<String>(),
        It.IsAny<String>(),
        It.IsAny<String>(),
        It.IsAny<String>(),
        this.files,
        It.IsAny<String>(),
        false,
        false,
        It.IsAny<String>(),
        It.IsAny<String>(),
        It.IsAny<String>(),
        It.IsAny<String>(),
        It.IsAny<String>());

      act.ShouldThrow<DocProcessingException>();
    }

    [Test]
    public void GivenValidJobData_WhenICreateAJob_AndTheFileHasAlreadyBeenProcessed_ButAllowReprocessingIsSelected_AndDatabaseIsDown_TheJobIsSubmitted()
    {
      this.inputFileRepository.Setup(i => i.GetInputFile(It.IsAny<String>(), It.IsAny<String>())).Returns(new List<InputFile>() { new InputFile() });
      this.manCoDocRepository.Setup(i => i.GetManCoDoc(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>())).Throws<DocProcessingException>();

      Action act = () =>  this.createJobEngine.SubmitJob(
        It.IsAny<String>(),
        It.IsAny<String>(),
        It.IsAny<String>(),
        It.IsAny<String>(),
        this.files,
        It.IsAny<String>(),
        true,
        false,
        It.IsAny<String>(),
        It.IsAny<String>(),
        It.IsAny<String>(),
        It.IsAny<String>(),
        It.IsAny<String>());

      act.ShouldThrow<DocProcessingException>();
    }
  }
}
