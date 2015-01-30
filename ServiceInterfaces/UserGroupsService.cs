using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;

namespace ServiceInterfaces
{
    public interface IUserGroupsService
    {
        IList<UserGroup> GetUserGroups(String userId);
        void AddGroupsToUser(String userId, IList<Int32> groupIdsList);
    }
}
