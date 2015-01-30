// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IEnclosingJobService.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------


namespace ServiceInterfaces
{
    using Entities;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IEnclosingJobService
    {
        IList<EnclosingJob> GetEnclosingJobsByJobId(Int32 jobId);
        void UpdateEnclosingJobDocketNumber(Int32 enclosingJobId, string postalDocketNumber);
    }
}
