// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DocumentTypeRepositoryTests.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace EFRepositoryTests
{
  using System.Configuration;
  using System.Transactions;
  using DocProcessingRepository.Repositories;
  using Entities;
  using FluentAssertions;
  using NUnit.Framework;

  [Category("Integration")]
  [TestFixture]
  public class DocumentTypeRepositoryTests
  {
    private TransactionScope transactionScope;
    private DocTypeRepostory docTypeRepostory;
    private DocumentType documentType;

    [SetUp]
    public void Setup()
    {
      this.transactionScope = new TransactionScope();
      this.docTypeRepostory = new DocTypeRepostory(ConfigurationManager.ConnectionStrings["BBOS"].ConnectionString);
      this.documentType = BuildMeA.BuildMeA.DocumentType("code", "name", "typeName", false, false);
    }

    [TearDown]
    public void TearDown()
    {
      this.transactionScope.Dispose();
    }

    [Test]
    public void GivenAUserAndManCo_WhenIAskForTheDocTypes_IGetDocTypesManCos()
    {
      var result = this.docTypeRepostory.GetDocTypes("1a323edd-0732-4ef6-b2a2-4647df9dfd74", "AY");

      result.Should().Contain(e => e.BravuraDocTypeCode == "DIS");
    }

    [Test]
    public void GivenADocType_WhenITryToSaveToTheDatabase_ItIsSavedToTheDatabase()
    {
      this.docTypeRepostory.Create(this.documentType);

      DocumentType docType = this.docTypeRepostory.GetDocType("code", "name");

      docType.Should().NotBeNull();
    }
  }
}
