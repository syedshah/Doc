﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JobBuilder.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   Builder for JobBuilder
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace BuildMeA
{
  using Entities;

  public class DocumnetTypeBuilder : Builder<DocumentType>
  {
    public DocumnetTypeBuilder()
    {
      Instance = new DocumentType();
    }
  }
}
