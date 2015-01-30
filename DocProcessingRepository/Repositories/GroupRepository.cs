// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GroupRepository.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   Doc processing repository
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DocProcessingRepository.Repositories
{
    using System;
    using System.Collections.Generic;
    using Contexts;
    using Interfaces;
    using EFRepository;
    using Entities;

    public class GroupRepository : BaseEfRepository<Group>, IGroupRepository
    {
        public GroupRepository(String connectionString)
            : base(new IdentityDb(connectionString))
        {

        }

        public IList<Group> GetGroups()
        {
            var groups = this.GetEntityListByStoreProcedure<Group>("GetGroups");
            return groups;
        }
    }
}
