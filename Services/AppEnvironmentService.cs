// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AppEnvironmentService.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Services
{
    using System;
    using System.Collections.Generic;
    using DocProcessingRepository.Interfaces;
    using Entities;
    using Exceptions;
    using ServiceInterfaces;

    public class AppEnvironmentService : IAppEnvironmentService
    {
        private readonly IAppEnvironmentRepository appEnvironmentRepostory;

        public AppEnvironmentService(IAppEnvironmentRepository appEnvironmentRepostory)
        {
            this.appEnvironmentRepostory = appEnvironmentRepostory;
        }

        public IList<AppEnvironment> GetAppEnvironments(String userId)
        {
            try
            {
                return this.appEnvironmentRepostory.GetAppEnvironments(userId);
            }
            catch (Exception e)
            {
                throw new DocProcessingException("Unable to get appenviromnets", e);
            }
        }


        public IList<AppEnvironment> GetAppEnvironments()
        {
            try
            {
                return this.appEnvironmentRepostory.GetAppEnvironments();
            }
            catch (Exception e)
            {
                throw new DocProcessingException("Unable to get appenviromnets", e);
            }
        }
    }
}
