// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JobFilesViewModel.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   Defines the JobFilesViewModel type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DocProcessingWorkflow.Models.Jobs
{
  using System.Collections.Generic;
  using System.Linq;

  using Entities;

  public class JobFilesViewModel
  {
    public JobFilesViewModel()
    {
      this.JobFiles = new List<JobFileViewModel>();
    }

    public IList<JobFileViewModel> JobFiles { get; set; }

    public void AddInputFiles(IList<InputFileEntity> inputFiles)
    {
      inputFiles.ToList().ForEach(x => this.JobFiles.Add(new JobFileViewModel() { FilePath = x.FilePath }));
    }
  }
}