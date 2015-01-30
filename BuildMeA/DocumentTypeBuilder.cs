// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ManCoDocBuilder.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   ManCoDoc Builder
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace BuildMeA
{
  using Entities;

  public class DocumentTypeBuilder : Builder<DocumentType>
  {
    public DocumentTypeBuilder()
    {
      Instance = new DocumentType();
    }
  }
}
