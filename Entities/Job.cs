// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Job.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   Job object
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Entities
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel.DataAnnotations;
  using System.ComponentModel.DataAnnotations.Schema;
  using System.Data.Entity.Spatial;

  [Table("Job")]
  public partial class Job
  {
    public Job()
    {
      this.EnclosingJobs = new HashSet<EnclosingJob>();
      this.JobStatus = new HashSet<JobStatus>();
      this.ApprovalHistories = new HashSet<AuthoriseHistory>();
      this.InputFiles = new HashSet<InputFile>();
    }

    public Int32 JobID { get; set; }

    public Int32 ManCoDocID { get; set; }

    [Required]
    [StringLength(10)]
    public String GRID { get; set; }

    public DateTime createDate { get; set; }

    public DateTime? FinishDate { get; set; }

    public DateTime? SubmissionDate { get; set; }

    [StringLength(500)]
    public String AdditionalSetupInfo { get; set; }

    public String UserID { get; set; }

    public Boolean ManuallySubmitted { get; set; }

    public String AllocatorGrid { get; set; }

    public Int32 JobStatusTypeId { get; set; }

    public JobStatusType JobStatusType { get; set; }

    public virtual ICollection<EnclosingJob> EnclosingJobs { get; set; }

    public virtual ManCoDoc ManCoDoc { get; set; }

    public virtual ICollection<JobStatus> JobStatus { get; set; }

    public virtual ICollection<AuthoriseHistory> ApprovalHistories { get; set; }

    public virtual ICollection<InputFile> InputFiles { get; set; }

    public virtual ApplicationUser User { get; set; }
  }
}
