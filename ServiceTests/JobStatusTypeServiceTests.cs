// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JobStatusServiceTests.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   Job status service tests
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Entities;
using FluentAssertions;

namespace ServiceTests
{
    using System;
    using DocProcessingRepository.Interfaces;
    using Moq;
    using NUnit.Framework;
    using Services;

    [Category("Unit")]
    [TestFixture]
    public class JobStatusTypeServiceTests
    {
        private Mock<IJobStatusTypeRepository> jobStatusTypeRepository;

        private JobStatusTypeService jobStatusTypeService;

        [SetUp]
        public void SetUp()
        {
            this.jobStatusTypeRepository = new Mock<IJobStatusTypeRepository>();
            this.jobStatusTypeService = new JobStatusTypeService(this.jobStatusTypeRepository.Object);
        }

        [Test]
        public void GivenAValidJobStatusTypeDescription_WhenICallGetJobStatusType_IShouldGetJobStatusType()
        {
            this.jobStatusTypeRepository.Setup(x => x.GetJobStatusType(It.IsAny<String>()))
                .Returns(new JobStatusTypeEntity());

            var result = this.jobStatusTypeService.GetJobStatusType(It.IsAny<String>());

            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(JobStatusTypeEntity));
            this.jobStatusTypeRepository.Verify(x=>x.GetJobStatusType(It.IsAny<String>()),Times.AtLeastOnce);
        }
    }
}
