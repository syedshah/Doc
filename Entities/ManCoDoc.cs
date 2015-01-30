// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ManCoDoc.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   ManCoDoc object
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Entities
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel.DataAnnotations;
  using System.ComponentModel.DataAnnotations.Schema;
  using System.Data.Entity.Spatial;

  public partial class ManCoDoc
  {
    public ManCoDoc()
    {
      this.DeploymentHistories = new HashSet<DeploymentHistory>();
      this.GroupDataRights = new HashSet<GroupDataRight>();
      this.IndexDefinitions = new HashSet<IndexDefinition>();
      this.InputFiles = new HashSet<InputFile>();
      this.Jobs = new HashSet<Job>();
      this.JobSubmissions = new HashSet<JobSubmission>();
      this.MediaDefinitions = new HashSet<MediaDefinition>();
    }

    public Int32 ManCoDocID { get; set; }

    public Int32? BravuraDocID { get; set; }

    public Int32 ManCoID { get; set; }

    public Int32 DocumentTypeID { get; set; }

    [Required]
    [StringLength(500)]
    public String PubFileName { get; set; }

    public Int32 EnvironmentID { get; set; }

    [StringLength(10)]
    public String Version { get; set; }

    [StringLength(250)]
    public String VersionComments { get; set; }

    public Boolean Active { get; set; }

    public virtual DocumentType DocumentType { get; set; }

    public virtual EnvironmentType EnvironmentType { get; set; }

    public virtual ICollection<Job> Jobs { get; set; }

    public virtual ICollection<JobSubmission> JobSubmissions { get; set; }

    public virtual ManagementCompany ManagementCompany { get; set; }

    public virtual ICollection<DeploymentHistory> DeploymentHistories { get; set; }

    public virtual ICollection<GroupDataRight> GroupDataRights { get; set; }

    public virtual ICollection<IndexDefinition> IndexDefinitions { get; set; }

    public virtual ICollection<InputFile> InputFiles { get; set; }

    public virtual ICollection<MediaDefinition> MediaDefinitions { get; set; }
  }
}
