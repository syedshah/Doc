using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;

namespace DocProcessingRepository.Interfaces
{
    public interface IUserGroupsRepository
    {
        IList<UserGroup> GetUserGroups(String userId);
        void AddGroupsToUser(String userId, IList<Int32> groupIdsList);
    }
}
