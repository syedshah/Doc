namespace DocProcessingWorkflow.Models.Jobs
{
  using System;
  using System.ComponentModel.DataAnnotations;

  public class CompletedJobsViewModel
  {
    public CompletedJobsViewModel()
    {
    }

    public CompletedJobsViewModel(Entities.JobEntity jobEntity)
    {
      this.Id = jobEntity.JobId;
      this.CompletedDate = jobEntity.FinishDate.GetValueOrDefault().ToString("g");
      this.JobReference = jobEntity.JobReference;
      this.JobDisplay = String.Format("{0} ({1})", this.CompletedDate, this.JobReference);
    }

    public Int32 Id { get; set; }

    public String JobDisplay { get; set; }

    [Required]
    public String CompletedDate { get; set; }

    [Required]
    public String JobReference { get; set; }
  }
}