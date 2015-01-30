namespace ServiceTests
{
  using System;
  using System.Collections.Generic;
  using DocProcessingRepository.Interfaces;
  using Entities;
  using Entities.ADF;

  using Exceptions;
  using FluentAssertions;
  using Moq;
  using NUnit.Framework;
  using ServiceInterfaces;
  using Services;

  [Category("Unit")]
  [TestFixture]
  public class PackStoreServiceTests
  {
    private Mock<IPackStoreRepository> packStoreRepository;
    private IPackStoreService packStoreService;

    [SetUp]
    public void SetUp()
    {
      this.packStoreRepository = new Mock<IPackStoreRepository>();
      this.packStoreService = new PackStoreService(packStoreRepository.Object);
    }

    [Test]
    public void GivenAValidJob_WhenITryToRetrievPulledPacks_ThenThePacksAreRetrieved()
    {
      this.packStoreRepository.Setup(a => a.GetPulledPacks(It.IsAny<Int32>())).Returns(new List<PackStore>());

      var result = this.packStoreService.GetPulledPacks(It.IsAny<Int32>());

      this.packStoreRepository.Verify(x => x.GetPulledPacks(It.IsAny<Int32>()));
      result.Should().NotBeNull();
    }

    [Test]
    public void GivenAValidJob_WhenITryToRetrievPulledPacks_AndDatabaseIsUnavailable_ThenThePacksAreRetrieved()
    {
      this.packStoreRepository.Setup(a => a.GetPulledPacks(It.IsAny<Int32>())).Throws<Exception>();

      Action act = () => this.packStoreService.GetPulledPacks(It.IsAny<Int32>());

      act.ShouldThrow<DocProcessingException>();
    }

    [Test]
    public void GivenAValidClientRefToSearch_WhenITryToRetrievPacks_ThenThePacksAreRetrieved()
    {
      this.packStoreRepository.Setup(a => a.GetNonPulledPacks(It.IsAny<String>(), It.IsAny<Int32>())).Returns(new List<PackStore>());

      var result = this.packStoreService.GetNonPulledPacks(It.IsAny<String>(), It.IsAny<Int32>());

      this.packStoreRepository.Verify(x => x.GetNonPulledPacks(It.IsAny<String>(), It.IsAny<Int32>()));
      result.Should().NotBeNull();
    }

    [Test]
    public void GivenAValidClientRefToSearch_WhenITryToRetrievPacks_AndDatabaseIsUnavailable_ThenThePacksAreRetrieved()
    {
      this.packStoreRepository.Setup(a => a.GetNonPulledPacks(It.IsAny<String>(), It.IsAny<Int32>())).Throws<Exception>();

      Action act = () => this.packStoreService.GetNonPulledPacks(It.IsAny<String>(), It.IsAny<Int32>());

      act.ShouldThrow<DocProcessingException>();
    }

    [Test]
    public void GivenAValidClientRefAndPullStatus_WhenITryToUpdateThePullStatus_ThePullStatusIsUpdated()
    {
      this.packStoreRepository.Setup(a => a.UpdatePullStatus(It.IsAny<String>(), It.IsAny<Boolean>()));

      this.packStoreService.UpdatePullStatus(It.IsAny<String>(), It.IsAny<Boolean>());

      this.packStoreRepository.Verify(x => x.UpdatePullStatus(It.IsAny<String>(), It.IsAny<Boolean>()));
    }

    [Test]
    public void GivenAValidClientRefAndPullStatus_WhenITryToUpdateThePullStatus_AndDatabaseIsUnavailable_ThenThePacksAreRetrieved()
    {
      this.packStoreRepository.Setup(a => a.UpdatePullStatus(It.IsAny<String>(), It.IsAny<Boolean>())).Throws<Exception>();

      Action act = () => this.packStoreService.UpdatePullStatus(It.IsAny<String>(), It.IsAny<Boolean>());

      act.ShouldThrow<DocProcessingException>().WithMessage("Unable to update pull status");
    }
  }
}
