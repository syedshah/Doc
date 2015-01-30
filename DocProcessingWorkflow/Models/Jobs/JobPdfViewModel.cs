// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JobPdfViewModel.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   Defines the JobFileViewModel type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DocProcessingWorkflow.Models.Jobs
{
  using System;
  using System.Collections.Generic;

  public class JobPdfViewModel
  {
    public String PdfName { get; set; }

    public String PdfFilePath { get; set; }
  }
}