namespace Entities
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel.DataAnnotations;
  using System.ComponentModel.DataAnnotations.Schema;
  using System.Data.Entity.Spatial;

  [Table("GroupType")]
  public partial class GroupType
  {
    public GroupType()
    {
      Groups = new HashSet<Group>();
    }

    public int GroupTypeID { get; set; }

    [Required]
    [StringLength(50)]
    public string GroupTypeName { get; set; }

    public virtual ICollection<Group> Groups { get; set; }
  }
}
