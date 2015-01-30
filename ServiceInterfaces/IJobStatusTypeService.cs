// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IJobStatusTypeService.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ServiceInterfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Entities;

    public interface IJobStatusTypeService
    {
        JobStatusTypeEntity GetJobStatusType(String jobStatusTypeDescription);
    }
}
