namespace Entities
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel.DataAnnotations;
  using System.ComponentModel.DataAnnotations.Schema;
  using System.Data.Entity.Spatial;

  public partial class AspNetRole
  {
    public AspNetRole()
    {
      AspNetUsers = new HashSet<AspNetUser>();
    }

    public string Id { get; set; }

    [Required]
    public string Name { get; set; }

    public virtual ICollection<AspNetUser> AspNetUsers { get; set; }
  }
}
