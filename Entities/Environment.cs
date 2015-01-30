// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Environment.cs" company="DST Nexdox">
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

  [Table("AppEnvironment")]
  public partial class AppEnvironment
  {
    public AppEnvironment()
    {
      this.GroupDataRights = new HashSet<GroupDataRight>();
      this.EnvironmentTypes = new HashSet<EnvironmentType>();
    }

    [Key]
    public int AppEnvironmentID { get; set; }

    [Column("Name")]
    [Required]
    [StringLength(50)]
    public String Name { get; set; }

    public virtual ICollection<GroupDataRight> GroupDataRights { get; set; }

    public virtual ICollection<EnvironmentType> EnvironmentTypes { get; set; }
  }
}
