namespace Entities
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel.DataAnnotations;
  using System.ComponentModel.DataAnnotations.Schema;
  using System.Data.Entity.Spatial;

  [Table("EnvironmentType")]
  public partial class EnvironmentType
  {
    public EnvironmentType()
    {
      DocumentVersions = new HashSet<DocumentVersion>();
      ManCoDocs = new HashSet<ManCoDoc>();
    }

    [Key]
    public Int32 EnvironmentID { get; set; }

    [Column("EnvironmentType")]
    [Required]
    [StringLength(30)]
    public String EnvironmentType1 { get; set; }

    [StringLength(150)]
    public String ProcessingServerName { get; set; }

    [StringLength(150)]
    public String ADF_DB_Name { get; set; }

    [StringLength(150)]
    public String ADF_DB_ServerName { get; set; }

    [StringLength(50)]
    public String BravuraDOCSEnvironmentType { get; set; }

    [StringLength(1)]
    public String BravuraDOCSEnvironmentChar { get; set; }

    [StringLength(1)]
    public String EnvironmentChar { get; set; }

    public virtual ICollection<DocumentVersion> DocumentVersions { get; set; }

    public virtual ICollection<ManCoDoc> ManCoDocs { get; set; }

    public virtual AppEnvironment AppEnvironment { get; set; }

    public Int32 AppEnvironmentID { get; set; }
  }
}
