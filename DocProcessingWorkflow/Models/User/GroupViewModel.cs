using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Entities;

namespace DocProcessingWorkflow.Models.User
{
    public class GroupViewModel
    {
        public GroupViewModel(Group group)
        {
            this.GroupID = group.GroupID;
            this.GroupName = group.GroupName;
        }

        public Int32 GroupID { get; set; }

        [Required]
        [StringLength(50)]
        public String GroupName { get; set; }

        public Boolean IsAssignedToUser { get; set; }
    }
}