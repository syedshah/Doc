// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserGroupsRepository.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//  UserGroups Repository
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DocProcessingRepository.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Contexts;
    using Interfaces;
    using EFRepository;
    using Entities;

    public class UserGroupsRepository : BaseEfRepository<UserGroup>, IUserGroupsRepository
    {
        public UserGroupsRepository(String connectionString)
            : base(new IdentityDb(connectionString))
        {
        }

        public IList<UserGroup> GetUserGroups(String userId)
        {
            var userGroups = this.GetEntityListByStoreProcedure<UserGroup>(String.Format("GetGroupsByUserId '{0}'", userId));
            return userGroups;
        }


        public void AddGroupsToUser(string userId, IList<int> groupIdsList)
        {
            this.ExecuteStoredProcedure(String.Format("AddGroupsToUser '{0}','{1}'", userId, String.Join(",", groupIdsList.ToArray())));
        }
    }
}
