// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuthoriseHistory.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Entities
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel.DataAnnotations;
  using System.ComponentModel.DataAnnotations.Schema;
  using System.Data.Entity.Spatial;

  [Table("AuthoriseHistory")]
  public partial class AuthoriseHistory
  {
    [Key]
    public Int32 ApprovalID { get; set; }

    public Int32 JobID { get; set; }

    public DateTime ApprovalDate { get; set; }

    [StringLength(500)]
    public String Comment { get; set; }

    [Required]
    [StringLength(128)]
    public String UserID { get; set; }

    public virtual ApplicationUser User { get; set; }

    public virtual Job Job { get; set; }
  }
}
