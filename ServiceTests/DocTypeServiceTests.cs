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
  public class DocTypeServiceTests
  {
    private Mock<IDocTypeRepostory> docTypeRepostory;
    private IDocTypeService docTypeService;

    [SetUp]
    public void SetUp()
    {
      this.docTypeRepostory = new Mock<IDocTypeRepostory>();
      this.docTypeService = new DocTypeService(docTypeRepostory.Object);
    }

    [Test]
    public void GivenAValidUserIdAndManCo_WhenITryToRetrieveDocTypes_ThenTheDocTypesAreRetrieved()
    {
      this.docTypeRepostory.Setup(a => a.GetDocTypes(It.IsAny<String>(), It.IsAny<String>())).Returns(new List<DocumentType>());

      var result = this.docTypeService.GetDocTypes(It.IsAny<String>(), It.IsAny<String>());

      this.docTypeRepostory.Verify(x => x.GetDocTypes(It.IsAny<String>(), It.IsAny<String>()));
      result.Should().NotBeNull();
    }

    [Test]
    public void GivenAValidUserId_WhenITryToRetrieveManCos_AndDatabaseIsUnavailable_ThenADocProcessingExceptionIsThrown()
    {
      this.docTypeRepostory.Setup(a => a.GetDocTypes(It.IsAny<String>(), It.IsAny<String>())).Throws<Exception>();

      Action act = () => this.docTypeService.GetDocTypes(It.IsAny<String>(), It.IsAny<String>());

      act.ShouldThrow<DocProcessingException>();
    }

    [Test]
    public void GivenAValidCode_WhenITryToRetriveTheDocType_ThenIGetTheDocType()
    {
      this.docTypeRepostory.Setup(a => a.GetDocType(It.IsAny<String>(), It.IsAny<String>())).Returns(new DocumentType());

      var result = this.docTypeService.GetDocType(It.IsAny<String>(), It.IsAny<String>());

      this.docTypeRepostory.Verify(x => x.GetDocType(It.IsAny<String>(), It.IsAny<String>()));
      result.Should().NotBeNull();
    }

    [Test]
    public void GivenAValidCode_WhenITryToRetriveTheDocType_AndDatabaseIsUnavailable_ThenIGetTheDocType()
    {
      this.docTypeRepostory.Setup(a => a.GetDocType(It.IsAny<String>(), It.IsAny<String>())).Throws<Exception>();

      Action act = () => this.docTypeService.GetDocType(It.IsAny<String>(), It.IsAny<String>());

      act.ShouldThrow<DocProcessingException>();
    }
  }
}
