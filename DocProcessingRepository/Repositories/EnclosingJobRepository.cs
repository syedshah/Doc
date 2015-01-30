// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnclosingJobRepository.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   Repo for EnclosingJob Types
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DocProcessingRepository.Repositories
{
    using DocProcessingRepository.Contexts;
    using DocProcessingRepository.Interfaces;
    using EFRepository;
    using Entities;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class EnclosingJobRepository : BaseEfRepository<EnclosingJob>, IEnclosingJobRepository
    {
        public EnclosingJobRepository(String connectionString)
            : base(new IdentityDb(connectionString))
        {
        }

        public IList<Entities.EnclosingJob> GetEnclosingJobsByJobId(int jobId)
        {
            return this.GetEntityListByStoreProcedure<EnclosingJob>(String.Format("GetEnclosingJobsByJobId '{0}'", jobId));
        }

        public void UpdateEnclosingJobDocketNumber(int enclosingJobId, string postalDocketNumber)
        {
            this.ExecuteStoredProcedure(string.Format("exec UpdateEnclosingJobDocketNumber {0},'{1}'", enclosingJobId, postalDocketNumber));
        }
    }
}
