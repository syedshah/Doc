// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InputFileServiceTests.cs" company="DST Nexdox">
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

  using DocProcessingRepository.Interfaces;

  using Entities;

  using Exceptions;

  using FluentAssertions;

  using Moq;

  using NUnit.Framework;

  using Services;

  [Category("Unit")]
  [TestFixture]
  public class InputFileServiceTests
  {
    private Mock<IInputFileRepository> inputFileRepository;

    private InputFileService inputFileService;

    private IList<InputFileEntity> inputFiles;

    [SetUp]
    public void SetUp()
    {
      this.inputFileRepository = new Mock<IInputFileRepository>();
      this.inputFileService = new InputFileService(this.inputFileRepository.Object);
      this.inputFiles = new List<InputFileEntity>();
      this.inputFiles.Add(new InputFileEntity() { FilePath = "filepath1" });
    }

    [Test]
    public void GivenAJobId_WhenIWantToGetTheInputFilesAndDatabaseIsAvailable_TheInputFilesAreRetrieved()
    {
      this.inputFileRepository.Setup(x => x.GetInputFilesByJobId(It.IsAny<Int32>())).Returns(this.inputFiles);

      var result = this.inputFileService.GetInputFilesByJobId(It.IsAny<Int32>());

      result.Should().NotBeNull();
      result.Count.ShouldBeEquivalentTo(this.inputFiles.Count);
      this.inputFileRepository.Verify(x => x.GetInputFilesByJobId(It.IsAny<Int32>()), Times.AtLeastOnce);
    }

    [Test]
    [ExpectedException(typeof(DocProcessingException), ExpectedMessage = "Unable to retrieve input files")]
    public void GivenAJobId_WhenIWantToGetTheInputFilesAndDatabaseIsUnAvailable_AnExceptionIsThrown()
    {
      this.inputFileRepository.Setup(x => x.GetInputFilesByJobId(It.IsAny<Int32>())).Throws<DocProcessingException>();

      this.inputFileService.GetInputFilesByJobId(It.IsAny<Int32>());
    }
  }
}
