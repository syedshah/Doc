// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JobReportsViewModel.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   Defines the JobReportsViewModel type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DocProcessingWorkflow.Models.Jobs
{
  using System.Collections.Generic;

  using Entities.File;

  public class JobReportsViewModel
  {
    public JobReportsViewModel()
    {
      this.Reports = new List<JobReportViewModel>();
    }

    public IList<JobReportViewModel> Reports { get; set; }

    public void AddJobReports(IList<ReportFile> reportFiles)
    {
      foreach (var reportFile in reportFiles)
      {
        this.Reports.Add(new JobReportViewModel()
                           {
                             ReportFileContent = reportFile.ReportContent,
                             ReportFileName = reportFile.ReportFileName,
                             ReportFilePath = reportFile.ReportFilePath
                           });
      }
    }
  }
}