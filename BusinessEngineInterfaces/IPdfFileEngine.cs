// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IPdfFileEngine.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   Pdf File Engine object
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace BusinessEngineInterfaces
{
  using System;
  using System.Collections.Generic;

  using Entities.File;

  public interface IPdfFileEngine
  {
    IList<PdfFile> GetPdfFiles(String grid, String environment);
  }
}
