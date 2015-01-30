// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JobsViewModel.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   Defines the JobsViewModel type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DocProcessingWorkflow.Models.Jobs
{
  using DocProcessingWorkflow.Models.SubmitJobs;

  public class JobsViewModel
  {
    public JobsViewModel()
    {
      this.ViewJobs = new ViewJobsViewModel();
    }

    public ViewJobsViewModel ViewJobs { get; set; }

    public SubmitJobsViewModel SubmitJobs { get; set; }

    public PullsJobsViewModel ManagePulls { get; set; }

    public InsertJobsViewModel ManageInserts { get; set; }
  }
}