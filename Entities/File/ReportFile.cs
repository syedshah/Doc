// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReportFile.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   One Step Log object 
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Entities.File
{
  using System;

  public class ReportFile
  {
    public ReportFile()
    {
      
    }

    public ReportFile(String fileName, String filePath)
    {
      this.ReportFileName = fileName;
      this.ReportFilePath = filePath;
    }

    public ReportFile(String fileName, String filePath, String[] fileContent)
    {
      this.ReportFileName = fileName;
      this.ReportContent = fileContent;
      this.ReportFilePath = filePath;
    }

    public String ReportFileName { get; set; }

    public String[] ReportContent { get; set; }

    public String ReportFilePath { get; set; }
  }
}
