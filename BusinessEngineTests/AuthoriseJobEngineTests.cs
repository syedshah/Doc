// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuthoriseJobEngineTests.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   Authorise Job Engine Tests 
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace BusinessEngineTests
{
  using System;
  using System.Collections.Generic;
  using BusinessEngines;

  using DocProcessingRepository.Interfaces;

  using Entities;

  using Moq;
  using NUnit.Framework;
  using ServiceInterfaces;

  [Category("Unit")]
  [TestFixture]
  public class AuthoriseJobEngineTests
  {
    private AuthoriseJobEngine authoriseJobEngine;

    private Mock<IAuthoriseHistoryService> authoriseHistoryService;
    private Mock<IJobService> jobService;

    private Mock<IJobStatusTypeRepository> jobStatusTypeRepository;

    private IList<Int32> jobIds;

    private JobStatusTypeEntity jobStatusTypeEntity;
      
    [SetUp]
    public void SetUp()
    {
      this.authoriseHistoryService = new Mock<IAuthoriseHistoryService>();
      this.jobService = new Mock<IJobService>();
      this.jobStatusTypeRepository = new Mock<IJobStatusTypeRepository>();
      this.authoriseJobEngine = new AuthoriseJobEngine(
        this.authoriseHistoryService.Object, 
        this.jobService.Object,
        this.jobStatusTypeRepository.Object);
      this.jobIds = new List<Int32>();
      this.jobIds.Add(2);
      this.jobIds.Add(7);
      this.jobIds.Add(9);
      this.jobIds.Add(5);
      this.jobStatusTypeEntity = new JobStatusTypeEntity() { JobStatusDescription = "description", JobStatusTypeID = 2 };
    }

    [Test]
    public void GivenValidParemeters_WhenIwantoToAuthoriseJobs_TheJobsAreAuthorised()
    {
      this.authoriseHistoryService.Setup(x => x.InsertAuthorisation(It.IsAny<Int32>(), It.IsAny<String>(), It.IsAny<String>()));
      this.jobStatusTypeRepository.Setup(x => x.GetJobStatusType(It.IsAny<String>())).Returns(this.jobStatusTypeEntity);
      this.jobService.Setup(x => x.UpdateJobStatus(It.IsAny<Int32>(), It.IsAny<Int32>()));

      this.authoriseJobEngine.AuthoriseJob(this.jobIds, It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>());

      this.authoriseHistoryService.Verify(x => x.InsertAuthorisation(It.IsAny<Int32>(), It.IsAny<String>(), It.IsAny<String>()), Times.AtLeast(4));
      this.jobStatusTypeRepository.Verify(x => x.GetJobStatusType(It.IsAny<String>()), Times.AtLeastOnce);
      this.jobService.Verify(x => x.UpdateJobStatus(It.IsAny<Int32>(), It.IsAny<Int32>()));
    }
  }
}
