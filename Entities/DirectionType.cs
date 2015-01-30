namespace Entities
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel.DataAnnotations;
  using System.ComponentModel.DataAnnotations.Schema;
  using System.Data.Entity.Spatial;

  [Table("DirectionType")]
  public partial class DirectionType
  {
    public DirectionType()
    {
      this.Sorts = new HashSet<Sort>();
    }

    public int DirectionTypeID { get; set; }

    [Required]
    [StringLength(50)]
    public String Direction { get; set; }

    public virtual ICollection<Sort> Sorts { get; set; }
  }
}
