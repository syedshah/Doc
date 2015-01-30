// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApprovalHistoryServiceTests.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ServiceTests
{
  using System;
  using System.Collections.Generic;

  using DocProcessingRepository.Interfaces;

  using Exceptions;

  using Moq;

  using NUnit.Framework;

  using Services;

  [Category("Unit")]
  [TestFixture]
  public class AuthoriseHistoryServiceTests
  {
    private Mock<IAuthoriseHistoryRepository> approvalHistoryRepository;

    private AuthoriseHistoryService approvalHistoryService;

    private IList<Int32> jobIds;
    
    [SetUp]
    public void SetUp()
    {
      this.approvalHistoryRepository = new Mock<IAuthoriseHistoryRepository>();
      this.approvalHistoryService = new AuthoriseHistoryService(this.approvalHistoryRepository.Object);
      this.jobIds = new List<Int32>();
      this.jobIds.Add(2);
      this.jobIds.Add(4);
      this.jobIds.Add(9);
    }

    [Test]
    public void WhenIWantToInsertApproval_AndDatabaseIsAvailable_ApprovalIsInserted()
    {
      this.approvalHistoryRepository.Setup(x => x.InsertAuthorisation(It.IsAny<Int32>(), It.IsAny<String>(), It.IsAny<String>()));

      this.approvalHistoryService.InsertAuthorisation(It.IsAny<Int32>(), It.IsAny<String>(), It.IsAny<String>());

      this.approvalHistoryRepository.Verify(
        x => x.InsertAuthorisation(It.IsAny<Int32>(), It.IsAny<String>(), It.IsAny<String>()), Times.AtLeastOnce);
    }

    [Test]
    [ExpectedException(typeof(DocProcessingException), ExpectedMessage = "Unable to insert authorisation")]
    public void WhenIWantToInsertApproval_AndDatabaseIsUnAvailable_AnExceptionIsThrown()
    {
      this.approvalHistoryRepository.Setup(x => x.InsertAuthorisation(It.IsAny<Int32>(), It.IsAny<String>(), It.IsAny<String>())).Throws<DocProcessingException>();

      this.approvalHistoryService.InsertAuthorisation(It.IsAny<Int32>(), It.IsAny<String>(), It.IsAny<String>());
    }

    [Test]
    public void WhenIWantToInsertAuthorisations_AndDatabaseIsAvailable_AuthorisationsAreInserted()
    {
      this.approvalHistoryRepository.Setup(x => x.InsertAuthorisation(It.IsAny<Int32>(), It.IsAny<String>(), It.IsAny<String>()));

      this.approvalHistoryService.InsertAuthorisations(this.jobIds, It.IsAny<String>(), It.IsAny<String>());

      this.approvalHistoryRepository.Verify(
        x => x.InsertAuthorisation(It.IsAny<Int32>(), It.IsAny<String>(), It.IsAny<String>()), Times.AtLeast(this.jobIds.Count));
    }

    [Test]
    [ExpectedException(typeof(DocProcessingException), ExpectedMessage = "Unable to insert authorisations")]
    public void WhenIWantToInsertAuthorisations_AndDatabaseIsUnAvailable_AnExceptionIsThrown()
    {
      this.approvalHistoryRepository.Setup(x => x.InsertAuthorisation(It.IsAny<Int32>(), It.IsAny<String>(), It.IsAny<String>())).Throws<DocProcessingException>();

      this.approvalHistoryService.InsertAuthorisations(It.IsAny<IList<Int32>>(), It.IsAny<String>(), It.IsAny<String>());
    }
  }
}
