// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AppEnvironmentRepostory.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   Repo for Mancos
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;

namespace DocProcessingRepository.Repositories
{
    using System;
    using DocProcessingRepository.Contexts;
    using DocProcessingRepository.Interfaces;
    using EFRepository;
    using Entities;

    public class ManCoDocRepository : BaseEfRepository<ManCoDoc>, IManCoDocRepository
    {
        public ManCoDocRepository(String connectionString)
            : base(new IdentityDb(connectionString))
        {
        }

        public ManCoDoc GetManCoDoc(String manCo, String docTypeCode, String docTypeName, String environment)
        {
            return this.SelectStoredProcedure<ManCoDoc>(String.Format("GetManCoDocByManCoDocTypeEnvironment '{0}', '{1}', '{2}', '{3}'", manCo, docTypeCode, docTypeName, environment));
        }

        public void PromoteMancoDocs(String manCoDocIds, String sourceAppEnvironMent, String targetAppEnvironment, String userID, String comment)
        {
            this.ExecuteStoredProcedure(String.Format("PromoteMancoDocs '{0}','{1}','{2}','{3}','{4}'", manCoDocIds, sourceAppEnvironMent, targetAppEnvironment, userID, comment));
        }

        public IList<ManCoDoc> GetManCoDocsByManCoCodeEnvironment(string manCoCode, string environment,string userID)
        {
            return this.GetEntityListByStoreProcedure<ManCoDoc>(String.Format("GetManCoDocsByManCoCodeEnvironment '{0}', '{1}','{2}'", manCoCode, environment,userID));
        }
    }
}
