// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ManagementCompany.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   ManagementCompany object 
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Entities
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel.DataAnnotations;
  using System.ComponentModel.DataAnnotations.Schema;
  using System.Data.Entity.Spatial;

  [Table("ManagementCompany")]
  public partial class ManagementCompany
  {
    public ManagementCompany()
    {
      ManCoDocs = new HashSet<ManCoDoc>();
    }

    [Key]
    public Int32 ManCoID { get; set; }

    [Required]
    [StringLength(75)]
    public String ManCoName { get; set; }

    [Required]
    [StringLength(50)]
    public String ManCoCode { get; set; }

    [Required]
    [StringLength(50)]
    public String ManCoShortName { get; set; }

    public Int32 ParentCompanyID { get; set; }

    [Required]
    [StringLength(50)]
    public String RufusDatabaseID { get; set; }

    public Boolean? Active { get; set; }

    public virtual ParentCompany ParentCompany { get; set; }

    public virtual ICollection<ManCoDoc> ManCoDocs { get; set; }
  }
}
