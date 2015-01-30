// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PasswordHistoryServiceTests.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   User service tests
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ServiceTests
{
  using System;
  using System.Collections.Generic;
  using DocProcessingRepository.Interfaces;
  using Encryptions;
  using Entities;
  using Exceptions;
  using FluentAssertions;
  using Moq;
  using NUnit.Framework;
  using Services;

  [Category("Unit")]
  [TestFixture]
  public class PasswordHistoryServiceTests
  {
    private Mock<IPasswordHistoryRepository> passwordHistoryRepository;

    private PasswordHistoryService passwordHistoryService;

    [SetUp]
    public void SetUp()
    {
      this.passwordHistoryRepository = new Mock<IPasswordHistoryRepository>();
      this.passwordHistoryService = new PasswordHistoryService(this.passwordHistoryRepository.Object);
    }

    [Test]
    public void WhenIWantToGetPasswordHistorByLastNumberOfItems_AndDatabaseIsAvailable_ThePasswordHistoryIsRetrieved()
    {
      this.passwordHistoryRepository.Setup(x => x.GetPasswordHistoryByLastItems(It.IsAny<String>(), It.IsAny<Int32>()))
          .Returns(new List<PasswordHistory>());

      var result = this.passwordHistoryService.GetPasswordHistoryByLastItems(It.IsAny<String>(), It.IsAny<Int32>());

      this.passwordHistoryRepository.Verify(
        x => x.GetPasswordHistoryByLastItems(It.IsAny<String>(), It.IsAny<Int32>()), Times.AtLeastOnce);
      result.Should().NotBeNull();
    }

    [Test]
    [ExpectedException(typeof(DocProcessingException))]
    public void WhenIWantToGetPasswordHistoryByLastNumberOfItems_AndDatabaseIsUnAvailable_AUnityExceptionIsThrown()
    {
      this.passwordHistoryRepository.Setup(x => x.GetPasswordHistoryByLastItems(It.IsAny<String>(), It.IsAny<Int32>()))
          .Throws<DocProcessingException>();

      this.passwordHistoryService.GetPasswordHistoryByLastItems(It.IsAny<String>(), It.IsAny<Int32>());

      this.passwordHistoryRepository.Verify(
        x => x.GetPasswordHistoryByLastItems(It.IsAny<String>(), It.IsAny<Int32>()), Times.AtLeastOnce);
    }

    [Test]
    public void WhenIWantToCheckIfPasswordIsInHistory_AndDatabaseIsAvailable_IfPasswordIsInHistory_ItShouldReturnTrue()
    {
      var passwordHistoryList = new List<PasswordHistory>();

      passwordHistoryList.Add(new PasswordHistory { UserId = "2w3", PasswordHash = DocProcessingEncryption.Encrypt("gogo") });
      passwordHistoryList.Add(new PasswordHistory { UserId = "2w3", PasswordHash = DocProcessingEncryption.Encrypt("gogi") });
      passwordHistoryList.Add(new PasswordHistory { UserId = "2w3", PasswordHash = DocProcessingEncryption.Encrypt("gaga") });
      passwordHistoryList.Add(new PasswordHistory { UserId = "2w3", PasswordHash = DocProcessingEncryption.Encrypt("gigi") });

      this.passwordHistoryRepository.Setup(x => x.GetPasswordHistoryByLastItems(It.IsAny<String>(), It.IsAny<Int32>()))
              .Returns(passwordHistoryList);

      var result = this.passwordHistoryService.IsPasswordInHistory("2w3", "gigi");

      result.Should().BeTrue();
      this.passwordHistoryRepository.Verify(x => x.GetPasswordHistoryByLastItems(It.IsAny<String>(), It.IsAny<Int32>()), Times.AtLeastOnce);
    }

    [Test]
    public void WhenIWantToCheckIfPasswordIsInHistory_AndDatabaseIsAvailable_IfPasswordIsNotInHistory_ItShouldReturnFalse()
    {
      var passwordHistoryList = new List<PasswordHistory>();

      passwordHistoryList.Add(new PasswordHistory { UserId = "2w3", PasswordHash = DocProcessingEncryption.Encrypt("gogo") });
      passwordHistoryList.Add(new PasswordHistory { UserId = "2w3", PasswordHash = DocProcessingEncryption.Encrypt("gogi") });
      passwordHistoryList.Add(new PasswordHistory { UserId = "2w3", PasswordHash = DocProcessingEncryption.Encrypt("gaga") });
      passwordHistoryList.Add(new PasswordHistory { UserId = "2w3", PasswordHash = DocProcessingEncryption.Encrypt("gigi") });

      this.passwordHistoryRepository.Setup(x => x.GetPasswordHistoryByLastItems(It.IsAny<String>(), It.IsAny<Int32>()))
                .Returns(passwordHistoryList);

      var result = this.passwordHistoryService.IsPasswordInHistory("2w3", "gugi");

      result.Should().BeFalse();
      this.passwordHistoryRepository.Verify(x => x.GetPasswordHistoryByLastItems(It.IsAny<String>(), It.IsAny<Int32>()), Times.AtLeastOnce);
    }

    [Test]
    [ExpectedException(typeof(DocProcessingException))]
    public void WhenIWantToCheckIfPasswordIsInHistory_AndDatabaseIsUnAvailable_AUnityExceptionIsThrown()
    {
      this.passwordHistoryRepository.Setup(x => x.GetPasswordHistory(It.IsAny<String>(), It.IsAny<Int32>()))
                .Throws<DocProcessingException>();

      this.passwordHistoryService.IsPasswordInHistory("2w3", "gugi");

      this.passwordHistoryRepository.Verify(x => x.GetPasswordHistory(It.IsAny<String>(), It.IsAny<Int32>()), Times.AtLeastOnce);
    }
  }
}
