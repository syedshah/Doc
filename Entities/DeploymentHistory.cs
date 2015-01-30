namespace Entities
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel.DataAnnotations;
  using System.ComponentModel.DataAnnotations.Schema;
  using System.Data.Entity.Spatial;

  [Table("DeploymentHistory")]
  public partial class DeploymentHistory
  {
    [Key]
    public Int32 DeployHistID { get; set; }

    public Int32 ManCoDocID { get; set; }

    public DateTime PromoteDate { get; set; }

    [StringLength(250)]
    public String Label { get; set; }

    public String UserID { get; set; }

    [StringLength(500)]
    public String Comment { get; set; }

    public virtual DocumentVersion DocumentVersion { get; set; }

    public virtual ManCoDoc ManCoDoc { get; set; }
  }
}
