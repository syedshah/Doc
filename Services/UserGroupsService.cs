using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocProcessingRepository.Interfaces;
using DocProcessingRepository.Repositories;
using ServiceInterfaces;

namespace Services
{
    public class UserGroupsService : IUserGroupsService
    {
        private readonly IUserGroupsRepository _userGroupsRepository;

        public UserGroupsService(IUserGroupsRepository userGroupsRepository)
        {
            this._userGroupsRepository = userGroupsRepository;
        }

        public IList<Entities.UserGroup> GetUserGroups(String userId)
        {
            return this._userGroupsRepository.GetUserGroups(userId);
        }

        public void AddGroupsToUser(string userId, IList<int> groupIdsList)
        {
            this._userGroupsRepository.AddGroupsToUser(userId, groupIdsList);
        }
    }
}
