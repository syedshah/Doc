namespace Entities
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel.DataAnnotations;
  using System.ComponentModel.DataAnnotations.Schema;
  using System.Data.Entity.Spatial;

  [Table("Finishing")]
  public partial class Finishing
  {
    public Finishing()
    {
      MediaDefinitions = new HashSet<MediaDefinition>();
    }

    [Key]
    public int FinishingDefID { get; set; }

    [Required]
    [StringLength(75)]
    public String FinishingDescription { get; set; }

    [StringLength(10)]
    public String Unknown { get; set; }

    public virtual ICollection<MediaDefinition> MediaDefinitions { get; set; }
  }
}
