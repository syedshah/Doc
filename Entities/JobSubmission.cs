// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JobSubmission.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   JobSubmission object 
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Entities
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel.DataAnnotations;
  using System.ComponentModel.DataAnnotations.Schema;
  using System.Data.Entity.Spatial;

  [Table("JobSubmission")]
  public partial class JobSubmission
  {
    public Int32 JobSubmissionID { get; set; }

    public DateTime SubmissionDate { get; set; }

    public Int32 ManCoDocID { get; set; }

    [StringLength(128)]
    public String UserID { get; set; }

    public virtual ApplicationUser AspNetUser { get; set; }

    public virtual ManCoDoc ManCoDoc { get; set; }
  }
}
