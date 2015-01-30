namespace DocProcessingWorkflow.Models.ManagePulls
{
  using System;
  using System.Collections.Generic;

  using DocProcessingWorkflow.Models.Jobs;

  public class CompletedJobsJsonResponse
  {
    public Exception exception;

    public String message;

    public String success;

    public String url;

    private List<CompletedJobsViewModel> jobs = new List<CompletedJobsViewModel>();

    public List<CompletedJobsViewModel> Jobs
    {
      get { return jobs; }
      set { jobs = value; }
    }

    public void AddJobs(IList<Entities.JobEntity> jobs)
    {
      foreach (Entities.JobEntity docType in jobs)
      {
        var avm = new CompletedJobsViewModel(docType);
        this.jobs.Add(avm);
      }
    }
  }
}