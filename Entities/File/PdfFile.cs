// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PdfFile.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   Pdf File object 
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Entities.File
{
  using System;

  public class PdfFile
  {
    public PdfFile()
    {
      
    }

    public PdfFile(String fileName, String filePath)
    {
      this.PdfFileName = fileName;
      this.PdfFilePath = filePath;
    }

    public String PdfFileName { get; set; }

    public String PdfFilePath { get; set; }
  }
}
