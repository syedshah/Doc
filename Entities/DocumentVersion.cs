// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DocumentVersion.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   DocumentVersion object 
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Entities
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel.DataAnnotations;
  using System.ComponentModel.DataAnnotations.Schema;
  using System.Data.Entity.Spatial;

  [Table("DocumentVersion")]
  public partial class DocumentVersion
  {
    [Key]
    public Int32 DocVersionID { get; set; }

    [Required]
    [StringLength(500)]
    public String PubFileName { get; set; }

    public Int32 EnvntID { get; set; }

    [StringLength(10)]
    public String Version { get; set; }

    [StringLength(50)]
    public String Region { get; set; }

    [StringLength(250)]
    public String VersionComments { get; set; }

    public virtual DeploymentHistory DeploymentHistory { get; set; }

    public virtual EnvironmentType EnvironmentType { get; set; }
  }
}
