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
  public class AppEnvironmentServiceTests
  {
    private Mock<IAppEnvironmentRepository> appEnvironmentRepostory;
    private IAppEnvironmentService appEnvironmentService;

    [SetUp]
    public void SetUp()
    {
      this.appEnvironmentRepostory = new Mock<IAppEnvironmentRepository>();
      this.appEnvironmentService = new AppEnvironmentService(appEnvironmentRepostory.Object);
    }

    [Test]
    public void GivenAValidUserId_WhenITryToRetrieveAppEnvironments_ThenTheAppEnviromentsAreRetrieved()
    {
      this.appEnvironmentRepostory.Setup(a => a.GetAppEnvironments(It.IsAny<String>())).Returns(new List<AppEnvironment>());

      var result = this.appEnvironmentService.GetAppEnvironments(It.IsAny<String>());

      this.appEnvironmentRepostory.Verify(x => x.GetAppEnvironments(It.IsAny<String>()));
      result.Should().NotBeNull();
    }

    [Test]
    public void GivenAValidUserId_WhenITryToRetrieveAppEnviroments_AndDatabaseIsUnavailable_ThenADocProcessingExceptionIsThrown()
    {
      this.appEnvironmentRepostory.Setup(a => a.GetAppEnvironments(It.IsAny<String>())).Throws<Exception>();

      Action act = () => this.appEnvironmentService.GetAppEnvironments(It.IsAny<String>());

      act.ShouldThrow<DocProcessingException>();
    }

    [Test]
    public void WhenITryToRetrieveAppEnvironments_ThenTheAppEnviromentsAreRetrieved()
    {
        this.appEnvironmentRepostory.Setup(a => a.GetAppEnvironments()).Returns(new List<AppEnvironment>());

        var result = this.appEnvironmentService.GetAppEnvironments();

        this.appEnvironmentRepostory.Verify(x => x.GetAppEnvironments());
        result.Should().NotBeNull();
    }
    [Test]
    public void WhenITryToRetrieveAppEnviroments_AndDatabaseIsUnavailable_ThenADocProcessingExceptionIsThrown()
    {
        this.appEnvironmentRepostory.Setup(a => a.GetAppEnvironments()).Throws<Exception>();

        Action act = () => this.appEnvironmentService.GetAppEnvironments();

        act.ShouldThrow<DocProcessingException>();
    }
  }
}
