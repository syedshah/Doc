// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JobStatusTypeService.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   Defines the JobStatusTypeService type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using DocProcessingRepository.Interfaces;
    using Entities;
    using Exceptions;
    using ServiceInterfaces;

    public class JobStatusTypeService : IJobStatusTypeService
    {
        private readonly IJobStatusTypeRepository _jobStatusTypeRepository;
        public JobStatusTypeService(IJobStatusTypeRepository jobsyJobStatusTypeRepository)
        {
            this._jobStatusTypeRepository = jobsyJobStatusTypeRepository;
        }

        public JobStatusTypeEntity GetJobStatusType(string jobStatusTypeDescription)
        {
            try
            {
                return this._jobStatusTypeRepository.GetJobStatusType(jobStatusTypeDescription);
            }
            catch (Exception e)
            {
                throw new DocProcessingException("Unable to get job status type", e);
            }
        }
    }
}
