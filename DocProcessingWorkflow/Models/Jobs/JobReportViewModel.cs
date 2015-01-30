// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JobReportViewModel.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   Defines the JobReportViewModel type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DocProcessingWorkflow.Models.Jobs
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  using Entities.File;

  public class JobReportViewModel
  {
    public String ReportFileName { get; set; }

    public String[] ReportFileContent { get; set; }

    public String ReportFilePath { get; set; }

    public void AddReportFile(ReportFile reportFile)
    {
      this.ReportFileName = reportFile.ReportFileName;
      this.ReportFileContent = reportFile.ReportContent;
      this.ReportFilePath = reportFile.ReportFilePath;
    }
  }
}