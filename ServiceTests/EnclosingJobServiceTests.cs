// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnclosingJobServiceTests.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   EnclosingJobServiceTests object
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using DocProcessingRepository.Interfaces;
using Entities;
using FluentAssertions;
using Moq;
using Services;

namespace ServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using NUnit.Framework;

    [Category("Unit")]
    [TestFixture]
    public class EnclosingJobServiceTests
    {
        private Mock<IEnclosingJobRepository> enclosingJobRepository;
        private EnclosingJobService enclosingJobService;

        [SetUp]
        public void SetUp()
        {
            this.enclosingJobRepository = new Mock<IEnclosingJobRepository>();
            this.enclosingJobService = new EnclosingJobService(this.enclosingJobRepository.Object);
        }

        [Test]
        public void GivenAJobId_WhenIcallGetEnclosingJobsByJobId_IShouldGetListOfEnclosingJobs()
        {
            this.enclosingJobRepository.Setup(x => x.GetEnclosingJobsByJobId(It.IsAny<Int32>()))
                .Returns(new List<EnclosingJob>());

            var result = this.enclosingJobService.GetEnclosingJobsByJobId(It.IsAny<Int32>());

            result.Should().NotBeNull();

            result.Should().BeOfType(typeof(List<EnclosingJob>));

            this.enclosingJobRepository.Verify(x => x.GetEnclosingJobsByJobId(It.IsAny<Int32>()), Times.AtLeastOnce);
        }

        [Test]
        public void GivenAValidData_WhenIcallUpdateEnclosingJobDocketNumber_EnclosingJobShouldBeUpdated()
        {
            this.enclosingJobRepository.Setup(
                x => x.UpdateEnclosingJobDocketNumber(It.IsAny<Int32>(), It.IsAny<String>()));

            this.enclosingJobService.UpdateEnclosingJobDocketNumber(It.IsAny<Int32>(), It.IsAny<String>());

            this.enclosingJobRepository.Verify(x => x.UpdateEnclosingJobDocketNumber(It.IsAny<Int32>(), It.IsAny<String>()), Times.AtLeastOnce);
        }
    }
}
