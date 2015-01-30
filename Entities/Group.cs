// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Group.cs" company="DST Nexdox">
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

  [Table("Group")]
  public class Group
  {
    public Group()
    {
      this.ApplicationUsers = new HashSet<ApplicationUser>();
      this.GroupDataRights = new HashSet<GroupDataRight>();
    }

    public Int32 GroupID { get; set; }

    [Required]
    [StringLength(50)]
    public String GroupName { get; set; }

    public Int32 GroupTypeID { get; set; }

    public virtual GroupType GroupType { get; set; }

    public virtual ICollection<ApplicationUser> ApplicationUsers { get; set; }

    public virtual ICollection<GroupDataRight> GroupDataRights { get; set; }
  }
}
