// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnclosingJobService.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------


namespace Services
{
    using DocProcessingRepository.Interfaces;
    using Entities;
    using ServiceInterfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class EnclosingJobService : IEnclosingJobService
    {
        private readonly IEnclosingJobRepository _enclosingJobRepository;

        public EnclosingJobService(IEnclosingJobRepository enclosingJobRepository)
        {
            this._enclosingJobRepository = enclosingJobRepository;
        }

        public IList<EnclosingJob> GetEnclosingJobsByJobId(int jobId)
        {
            return this._enclosingJobRepository.GetEnclosingJobsByJobId(jobId);
        }

        public void UpdateEnclosingJobDocketNumber(int enclosingJobId, string postalDocketNumber)
        {
            this._enclosingJobRepository.UpdateEnclosingJobDocketNumber(enclosingJobId, postalDocketNumber);
        }
    }
}
