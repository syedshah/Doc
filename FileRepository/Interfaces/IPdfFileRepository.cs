// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IPdfFileRepository.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   Defines the IPdfFileRepository type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FileRepository.Interfaces
{
  using System;
  using System.Collections.Generic;

  using Entities.File;

  public interface IPdfFileRepository
  {
    IList<PdfFile> GetPdfFiles(String filePath, String extension);

    IList<PdfFile> GetPdfFiles(String pdfInformationFile);
      
    PdfFile GetPdfFile(String pdfFilePath);
  }
}
