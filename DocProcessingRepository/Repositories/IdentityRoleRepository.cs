// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IdentityRoleRepository.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//  IdentityRole Repository
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
    using Microsoft.AspNet.Identity.EntityFramework;

    public class IdentityRoleRepository : BaseEfRepository<IdentityRole>, IIdentityRoleRepository
    {
        public IdentityRoleRepository(string connectionString)
            : base(new IdentityDb(connectionString))
        {

        }

        public IList<String> GetRoles()
        {
            return Entities.Select(r => r.Name).ToList();
        }
    }
}
