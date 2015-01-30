using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using DocProcessingWorkflow.Constants;
using Entities;
using Microsoft.AspNet.Identity.EntityFramework;

namespace DocProcessingWorkflow.Models.User
{
    public class ExtendedUserViewModel : BaseUserViewModel
    {
        public ExtendedUserViewModel()
            : base()
        {

        }
        public ExtendedUserViewModel(ApplicationUser user)
            : base(user)
        {
        }

      public UserGroupsRolesViewModel UserGroupsRoles=new UserGroupsRolesViewModel();

      public List<Int32> SelectedGroups { get; set; }

      public List<String> SelectedRoles { get; set; }
    }
}