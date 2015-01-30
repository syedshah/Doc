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
  using ServiceInterfaces;
  using Services;

  [Category("Unit")]
  [TestFixture]
  public class ManCoServiceTests
  {
    private Mock<IManCoRepository> manCoRepository;
    private IManCoService manCoService;

    [SetUp]
    public void SetUp()
    {
      this.manCoRepository = new Mock<IManCoRepository>();
      this.manCoService = new ManCoService(manCoRepository.Object);
    }

    [Test]
    public void GivenAValidUserId_WhenITryToRetrieveManCos_ThenTheManCosAreRetrieved()
    {
      this.manCoRepository.Setup(a => a.GetManCos(It.IsAny<String>())).Returns(new List<ManagementCompany>());

      var result = this.manCoService.GetManCos(It.IsAny<String>());

      this.manCoRepository.Verify(x => x.GetManCos(It.IsAny<String>()));
      result.Should().NotBeNull();
    }

    [Test]
    public void GivenAValidUserId_WhenITryToRetrieveManCos_AndDatabaseIsUnavailable_ThenADocProcessingExceptionIsThrown()
    {
      this.manCoRepository.Setup(a => a.GetManCos(It.IsAny<String>())).Throws<Exception>();

      Action act = () => this.manCoService.GetManCos(It.IsAny<String>());

      act.ShouldThrow<DocProcessingException>();
    }
  }
}
