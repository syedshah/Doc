// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ManCoDocServiceTests.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   ManCoDocServiceTests object
// </summary>
// --------------------------------------------------------------------------------------------------------------------


using System.Data.SqlClient;
using DocProcessingRepository.Interfaces;
using Entities;
using Exceptions;
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
    public class ManCoDocServiceTests
    {
        private Mock<IManCoDocRepository> manCoDocRepository;
        private ManCoDocService manCoDocService;

        [SetUp]
        public void Setup()
        {
            this.manCoDocRepository = new Mock<IManCoDocRepository>();
            this.manCoDocService = new ManCoDocService(this.manCoDocRepository.Object);
        }

        [Test]
        public void GivenAValidData_WhenITryToGetManCoDocsByMancoCodeAndEnvironment_ThenIGetManCoDocs()
        {
            this.manCoDocRepository.Setup(x=>x.GetManCoDocsByManCoCodeEnvironment(It.IsAny<String>(),It.IsAny<String>(),It.IsAny<String>())).Returns(new List<ManCoDoc>());

            var manCoDocs = this.manCoDocService.GetManCoDocsByManCoCodeEnvironment(It.IsAny<String>(),It.IsAny<String>(), It.IsAny<String>());

            manCoDocs.Should().NotBeNull();

            this.manCoDocRepository.Verify(x => x.GetManCoDocsByManCoCodeEnvironment(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>()),Times.AtLeastOnce);
        }

        [Test]
        public void GivenAValidData_WhenITryToGetManCoDocsByMancoCodeAndEnvironment_AndDatabaseIsUnavailable_ThenADocProcessingExceptionIsReturned()
        {
            this.manCoDocRepository.Setup(x => x.GetManCoDocsByManCoCodeEnvironment(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>()))
                .Throws<DocProcessingException>();

            Action act = () => this.manCoDocService.GetManCoDocsByManCoCodeEnvironment(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>());

            act.ShouldThrow<DocProcessingException>();
        }


        [Test]
        public void GivenAValidData_WhenITryToPromoteManCoDocs_ThenManCoDocsShouldBePromotedToTargetEnvironment()
        {
            this.manCoDocRepository.Setup(x => x.PromoteMancoDocs(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>()));

            this.manCoDocService.PromoteMancoDocs(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>());

            this.manCoDocRepository.Verify(x => x.PromoteMancoDocs(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>()), Times.Once);
        }

        [Test]
        public void GivenAValidData_WhenITryToPromoteManCoDocs_AndDatabaseIsUnavailable_ThenADocProcessingExceptionIsReturned()
        {
            this.manCoDocRepository.Setup(x => x.PromoteMancoDocs(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>())).Throws<DocProcessingException>();

            Action act = () => this.manCoDocService.PromoteMancoDocs(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>());

            act.ShouldThrow<DocProcessingException>();
        }
    }
}
