
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IEnclosingJobRepository.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   Repo for EnclosingJob Types
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocProcessingRepository.Interfaces
{
    public interface IEnclosingJobRepository
    {
        IList<EnclosingJob> GetEnclosingJobsByJobId(Int32 jobId);
        void UpdateEnclosingJobDocketNumber(Int32 enclosingJobId, string postalDocketNumber);
    }
}
