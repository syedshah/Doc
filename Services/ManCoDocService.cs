using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ManCoDocService.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   ManCoDocService object
// </summary>
// --------------------------------------------------------------------------------------------------------------------



namespace Services
{
    using DocProcessingRepository.Interfaces;
    using Exceptions;
    using ServiceInterfaces;
    using Entities;

    public class ManCoDocService : IManCoDocService
    {
        private readonly IManCoDocRepository manCoDocRepository;

        public ManCoDocService(IManCoDocRepository _manCoDocRepository)
        {
            this.manCoDocRepository = _manCoDocRepository;
        }

        public void PromoteMancoDocs(String manCoDocIds, String sourceAppEnvironMent, String targetAppEnvironment, String userID, String comment)
        {
            try
            {
                this.manCoDocRepository.PromoteMancoDocs(manCoDocIds, sourceAppEnvironMent, targetAppEnvironment, userID, comment);
            }
            catch (Exception e)
            {
                throw new DocProcessingException("Unable to promote ManCoDocs", e);
            }
        }

        public IList<ManCoDoc> GetManCoDocsByManCoCodeEnvironment(string manCoCode, string environment, string userID)
        {
            try
            {
                return this.manCoDocRepository.GetManCoDocsByManCoCodeEnvironment(manCoCode, environment, userID);
            }
            catch (Exception e)
            {
                throw new DocProcessingException("Unable to get ManCoDocs", e);
            }
        }
    }
}
