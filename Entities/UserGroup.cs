namespace Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class UserGroup
    {
        public UserGroup()
        {
            GroupDataRights = new HashSet<GroupDataRight>();
        }

        [Key]
        public int UserGrp_ID { get; set; }

        public int GroupID { get; set; }

        public string GroupName { get; set; }

        [StringLength(128)]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual ICollection<GroupDataRight> GroupDataRights { get; set; }
    }
}
