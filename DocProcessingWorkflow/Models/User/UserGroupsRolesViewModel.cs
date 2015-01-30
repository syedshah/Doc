using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Entities;
using Microsoft.AspNet.Identity.EntityFramework;

namespace DocProcessingWorkflow.Models.User
{
    public class UserGroupsRolesViewModel
    {
        public List<GroupViewModel> Groups = new List<GroupViewModel>();

        public List<String> AvailableRoles = new List<string>();

        public List<String> UserAssignedRoles = new List<string>();

        public void AddGroups(IList<Group> groups)
        {
            this.Groups.AddRange(groups.Select(g => new GroupViewModel(g)));
        }

        public void AddUserGroups(IList<UserGroup> userGroups)
        {
            var groupViewModels = this.Groups;
            foreach (var usrGrp in userGroups.Where(usrGrp => groupViewModels != null))
            {
                groupViewModels.Find(g => g.GroupID.Equals(usrGrp.GroupID)).IsAssignedToUser = true;
            }
        }
    }
}