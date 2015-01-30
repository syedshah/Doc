namespace Entities
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel.DataAnnotations;
  using System.ComponentModel.DataAnnotations.Schema;
  using System.Data.Entity.Spatial;

  [Table("IndexLayoutFile")]
  public partial class IndexLayoutFile
  {
    public IndexLayoutFile()
    {
      IndexDefinitions = new HashSet<IndexDefinition>();
    }

    [Key]
    public int IndexLayoutID { get; set; }

    public int BravuraRecFileID { get; set; }

    [Required]
    [StringLength(150)]
    public string IndexFileName { get; set; }

    public virtual ICollection<IndexDefinition> IndexDefinitions { get; set; }
  }
}
