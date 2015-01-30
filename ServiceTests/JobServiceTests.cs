// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JobServiceTests.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   Job service tests
// </summary>
// --------------------------------------------------------------------------------------------------------------------


namespace ServiceTests
{
    using BusinessEngineInterfaces;
    using DocProcessingRepository.Interfaces;
    using Entities;
    using Exceptions;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using Services;
    using System;
    using System.Collections.Generic;
    using System.Text;

    [Category("Unit")]
    [TestFixture]
    public class JobServiceTests
    {
        private Mock<IJobRepository> jobRepository;
        private Mock<ICreateJobEngine> createJobEngine;
        private Mock<IEnclosingJobRepository> enclosingJobRepository;

        private JobService jobService;

        [SetUp]
        public void SetUp()
        {
            this.jobRepository = new Mock<IJobRepository>();
            this.createJobEngine = new Mock<ICreateJobEngine>();
            this.enclosingJobRepository=new Mock<IEnclosingJobRepository>();
            this.jobService = new JobService(this.jobRepository.Object, this.createJobEngine.Object, this.enclosingJobRepository.Object);
        }

        [Test]
        public void WhenIWantToGetJobById_AndDatabaseIsAvailable_IGetTheJobById()
        {
            this.jobRepository.Setup(x => x.GetJobById(It.IsAny<Int32>(), It.IsAny<String>())).Returns(new JobEntity());

            var result = this.jobService.GetJobById(It.IsAny<Int32>(), It.IsAny<String>());

            result.Should().NotBeNull();

            this.jobRepository.Verify(x => x.GetJobById(It.IsAny<Int32>(), It.IsAny<String>()), Times.AtLeastOnce);
        }

        [Test]
        [ExpectedException(typeof(DocProcessingException), ExpectedMessage = "Unable to retrieve job")]
        public void WhenIWantToGetJobById_AndDatabaseIsUnAvailable_ADocProcessingExceptionIsThrown()
        {

            this.jobRepository.Setup(x => x.GetJobById(It.IsAny<Int32>(), It.IsAny<String>())).Throws<DocProcessingException>();

            this.jobService.GetJobById(It.IsAny<Int32>(), It.IsAny<String>());

            this.jobRepository.Verify(x => x.GetJobById(It.IsAny<Int32>(), It.IsAny<String>()), Times.AtLeastOnce);
        }

        [Test]
        public void WhenIWantToGetJobs_AndDatabaseIsAvailable_IGetTheJobs()
        {
            this.jobRepository.Setup(x => x.GetJobs(It.IsAny<String>(), It.IsAny<String>())).Returns(new List<JobEntity>());

            var result = this.jobService.GetJobs(It.IsAny<String>(), It.IsAny<String>());

            result.Should().NotBeNull();

            this.jobRepository.Verify(x => x.GetJobs(It.IsAny<String>(), It.IsAny<String>()), Times.AtLeastOnce);
        }

        [Test]
        [ExpectedException(typeof(DocProcessingException), ExpectedMessage = "Unable to retrieve jobs")]
        public void WhenIWantToGetJobs_AndDatabaseIsUnAvailable_ADocProcessingExceptionIsThrown()
        {
            this.jobRepository.Setup(x => x.GetJobs(It.IsAny<String>(), It.IsAny<String>())).Throws<DocProcessingException>();

            this.jobService.GetJobs(It.IsAny<String>(), It.IsAny<String>());

            this.jobRepository.Verify(x => x.GetJobs(It.IsAny<String>(), It.IsAny<String>()), Times.AtLeastOnce);
        }

        [Test]
        public void WhenIWantToGetPagedJobs_AndDatabaseIsAvailable_IGetTheJobs()
        {
            this.jobRepository.Setup(x => x.GetJobs(It.IsAny<Int32>(), It.IsAny<Int32>(), It.IsAny<String>(), It.IsAny<String>())).Returns(new PagedResult<JobEntity>());

            var result = this.jobService.GetJobs(It.IsAny<Int32>(), It.IsAny<Int32>(), It.IsAny<String>(), It.IsAny<String>());

            result.Should().NotBeNull();

            this.jobRepository.Verify(x => x.GetJobs(It.IsAny<Int32>(), It.IsAny<Int32>(), It.IsAny<String>(), It.IsAny<String>()), Times.AtLeastOnce);
        }

        [Test]
        [ExpectedException(typeof(DocProcessingException), ExpectedMessage = "Unable to retrieve paged jobs")]
        public void WhenIWantToGetPagedJobs_AndDatabaseIsUnAvailable_ADocProcessingExceptionIsThrown()
        {
            this.jobRepository.Setup(x => x.GetJobs(It.IsAny<Int32>(), It.IsAny<Int32>(), It.IsAny<String>(), It.IsAny<String>())).Throws<DocProcessingException>();

            this.jobService.GetJobs(It.IsAny<Int32>(), It.IsAny<Int32>(), It.IsAny<String>(), It.IsAny<String>());

            this.jobRepository.Verify(x => x.GetJobs(It.IsAny<String>(), It.IsAny<String>()), Times.AtLeastOnce);
        }

        [Test]
        public void GivenAValidData_WhenITryToSaveAJob_ThenTheJobisSaved()
        {
            this.createJobEngine.Setup(
              c =>
              c.SubmitJob(
                It.IsAny<String>(),
                It.IsAny<String>(),
                It.IsAny<String>(),
                It.IsAny<String>(),
                It.IsAny<List<String>>(),
                It.IsAny<String>(),
                It.IsAny<Boolean>(),
                It.IsAny<Boolean>(),
                It.IsAny<String>(),
                It.IsAny<String>(),
                It.IsAny<String>(),
                It.IsAny<String>(),
                It.IsAny<String>()));

            this.jobService.CreateJob(
                It.IsAny<String>(),
                It.IsAny<String>(),
                It.IsAny<String>(),
                It.IsAny<String>(),
                It.IsAny<List<String>>(),
                It.IsAny<String>(),
                It.IsAny<Boolean>(),
                It.IsAny<Boolean>(),
                It.IsAny<String>(),
                It.IsAny<String>(),
                It.IsAny<String>(),
                It.IsAny<String>(),
                It.IsAny<String>());

            this.createJobEngine.Verify(x => x.SubmitJob(
              It.IsAny<String>(),
                It.IsAny<String>(),
                It.IsAny<String>(),
                It.IsAny<String>(),
                It.IsAny<List<String>>(),
                It.IsAny<String>(),
                It.IsAny<Boolean>(),
                It.IsAny<Boolean>(),
                It.IsAny<String>(),
                It.IsAny<String>(),
                It.IsAny<String>(),
                It.IsAny<String>(),
                It.IsAny<String>()), Times.Once);
        }

        [Test]
        public void GivenAValidData_WhenITryToSaveAJob_AndDatabaseIsUnavailable_ThenADocProcessingExceptionIsReturned()
        {
            this.createJobEngine.Setup(
              c =>
              c.SubmitJob(
                It.IsAny<String>(),
                It.IsAny<String>(),
                It.IsAny<String>(),
                It.IsAny<String>(),
                It.IsAny<List<String>>(),
                It.IsAny<String>(),
                It.IsAny<Boolean>(),
                It.IsAny<Boolean>(),
                It.IsAny<String>(),
                It.IsAny<String>(),
                It.IsAny<String>(),
                It.IsAny<String>(),
                It.IsAny<String>())).Throws<DocProcessingException>();

            Action act = () => this.jobService.CreateJob(
                It.IsAny<String>(),
                It.IsAny<String>(),
                It.IsAny<String>(),
                It.IsAny<String>(),
                It.IsAny<List<String>>(),
                It.IsAny<String>(),
                It.IsAny<Boolean>(),
                It.IsAny<Boolean>(),
                It.IsAny<String>(),
                It.IsAny<String>(),
                It.IsAny<String>(),
                It.IsAny<String>(),
                It.IsAny<String>());

            act.ShouldThrow<DocProcessingException>();
        }

        [Test]
        public void GivenAValidData_WhenITryToSaveAJob_AndTheFileHasAlreadyBeenProcessed_ThenADocProcessingFileAlreadyProcessedExceptionIsReturned()
        {
            this.createJobEngine.Setup(
              c =>
              c.SubmitJob(
                It.IsAny<String>(),
                It.IsAny<String>(),
                It.IsAny<String>(),
                It.IsAny<String>(),
                It.IsAny<List<String>>(),
                It.IsAny<String>(),
                It.IsAny<Boolean>(),
                It.IsAny<Boolean>(),
                It.IsAny<String>(),
                It.IsAny<String>(),
                It.IsAny<String>(),
                It.IsAny<String>(),
                It.IsAny<String>())).Throws<DocProcessingFileAlreadyProcessedException>();

            Action act = () => this.jobService.CreateJob(
                It.IsAny<String>(),
                It.IsAny<String>(),
                It.IsAny<String>(),
                It.IsAny<String>(),
                It.IsAny<List<String>>(),
                It.IsAny<String>(),
                It.IsAny<Boolean>(),
                It.IsAny<Boolean>(),
                It.IsAny<String>(),
                It.IsAny<String>(),
                It.IsAny<String>(),
                It.IsAny<String>(),
                It.IsAny<String>());

            act.ShouldThrow<DocProcessingFileAlreadyProcessedException>();
        }

        [Test]
        public void GivenAValidManCoAndUserId_WhenISearchForCompletedJobs_IGetCompletedJobs()
        {
            this.jobRepository.Setup(x => x.GetCompletedJobs(It.IsAny<String>(), It.IsAny<String>())).Returns(new List<JobEntity>() { new JobEntity() });

            this.jobService.GetCompletedJobs(It.IsAny<String>(), It.IsAny<String>());

            this.jobRepository.Verify(x => x.GetCompletedJobs(It.IsAny<String>(), It.IsAny<String>()), Times.AtLeastOnce);
        }

        [Test]
        public void GivenAValidEnvironmentType_WhenIWantToGetServersAndDatabaseIsUnAvailable_ADocProcessingExceptionIsThrown()
        {
            this.jobRepository.Setup(x => x.GetCompletedJobs(It.IsAny<String>(), It.IsAny<String>())).Throws<DocProcessingException>();

            Action act = () => this.jobService.GetCompletedJobs(It.IsAny<String>(), It.IsAny<String>());

            act.ShouldThrow<DocProcessingException>();
        }

        [Test]
        public void GivenAValidJobIdAndJobStatusTypeId_WhenIWantToUpdateJobStatus_TheJobStatusIsUpdated()
        {
            this.jobRepository.Setup(x => x.UpdateJobStatus(It.IsAny<Int32>(), It.IsAny<Int32>()));

            this.jobService.UpdateJobStatus(It.IsAny<Int32>(), It.IsAny<Int32>());

            this.jobRepository.Verify(x => x.UpdateJobStatus(It.IsAny<Int32>(), It.IsAny<Int32>()), Times.AtLeastOnce);
        }

        [Test]
        public void GivenAValidJobIdAndJobStatusTypeId_WhenIWantToUpdateJobStatusAndDatabaseIsUnAvailable_ADocProcessingExceptionIsThrown()
        {
            this.jobRepository.Setup(x => x.UpdateJobStatus(It.IsAny<Int32>(), It.IsAny<Int32>())).Throws<DocProcessingException>();

            Action act = () => this.jobService.UpdateJobStatus(It.IsAny<Int32>(), It.IsAny<Int32>());

            act.ShouldThrow<DocProcessingException>().WithMessage("Unable to update job status type");
        }

        [Test]
        public void GivenAValidJobIdAndUserId_WhenIWantToGetJobReport_IShouldGetAStringBuilder()
      {
          this.jobRepository.Setup(x => x.GetJobById(It.IsAny<Int32>(), It.IsAny<String>())).Returns(new JobEntity());
            this.enclosingJobRepository.Setup(x => x.GetEnclosingJobsByJobId(It.IsAny<Int32>()))
                .Returns(new List<EnclosingJob>());

           var result= this.jobService.GetJobReportByJobId(It.IsAny<Int32>(), It.IsAny<String>());

            result.Should().NotBeNull();
            result.Should().BeOfType(typeof (StringBuilder));

            this.jobRepository.Verify(x=>x.GetJobById(It.IsAny<Int32>(),It.IsAny<String>()),Times.AtLeastOnce);
            this.enclosingJobRepository.Verify(x=>x.GetEnclosingJobsByJobId(It.IsAny<Int32>()),Times.AtLeastOnce);
      }
    }
}
