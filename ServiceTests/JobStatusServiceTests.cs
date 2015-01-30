// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JobStatusServiceTests.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   Job status service tests
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ServiceTests
{
  using System;

  using DocProcessingRepository.Interfaces;

  using Exceptions;

  using Moq;

  using NUnit.Framework;

  using Services;

  [Category("Unit")]
  [TestFixture]
  public class JobStatusServiceTests
  {
    private Mock<IJobStatusRepository> jobStatusRepository;
   
    private JobStatusService jobStatusService;

    [SetUp]
    public void SetUp()
    {
      this.jobStatusRepository = new Mock<IJobStatusRepository>();
      this.jobStatusService = new JobStatusService(this.jobStatusRepository.Object);
    }

    [Test]
    public void GivenAValidJobIdAndJobStatus_WhenIWantToUpdateJobStatus_TheJobStatusIsUpdated()
    {
      this.jobStatusRepository.Setup(x => x.UpdateJobStatus(It.IsAny<Int32>(), It.IsAny<Int32>()));

      this.jobStatusService.UpdateJobStatus(It.IsAny<Int32>(), It.IsAny<Int32>());

      this.jobStatusRepository.Verify(x => x.UpdateJobStatus(It.IsAny<Int32>(), It.IsAny<Int32>()), Times.AtLeastOnce);
    }

    [Test]
    [ExpectedException(typeof(DocProcessingException), ExpectedMessage = "Unable to update job status")]
    public void GivenAValidJobIdAndJobStatus_WhenIWantToUpdateJobStatusAndDatabaseIsUnAvailable_AnExceptionIsThrown()
    {
      this.jobStatusRepository.Setup(x => x.UpdateJobStatus(It.IsAny<Int32>(), It.IsAny<Int32>())).Throws<DocProcessingException>();

      this.jobStatusService.UpdateJobStatus(It.IsAny<Int32>(), It.IsAny<Int32>());
    }
  }
}
