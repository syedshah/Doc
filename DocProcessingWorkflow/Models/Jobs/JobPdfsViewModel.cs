// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JobPdfsViewModel.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   Defines the JobPdfsViewModel type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DocProcessingWorkflow.Models.Jobs
{
  using System.Collections.Generic;
  using System.Linq;

  using Entities.File;

  public class JobPdfsViewModel
  {
    public JobPdfsViewModel()
    {
      this.Pdfs = new List<JobPdfViewModel>();
    }

    public IList<JobPdfViewModel> Pdfs { get; set; }

    public void AddPdfs(IList<PdfFile> pdfFiles)
    {
      pdfFiles.ToList().ForEach(x => this.Pdfs.Add(new JobPdfViewModel() { PdfFilePath = x.PdfFilePath, PdfName = x.PdfFileName }));
    }
  }
}