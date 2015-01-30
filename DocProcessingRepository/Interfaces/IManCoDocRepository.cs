// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IManCoRepository.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DocProcessingRepository.Interfaces
{
    using System;
    using Entities;
    using System.Collections;
    using System.Collections.Generic;

    public interface IManCoDocRepository
    {
        ManCoDoc GetManCoDoc(String manCo, String docTypeCode, String docTypeName, String environment);
        IList<ManCoDoc> GetManCoDocsByManCoCodeEnvironment(String manCoCode, String environment,string userID);
        void PromoteMancoDocs(String manCoDocIds, String sourceAppEnvironMent, String targetAppEnvironment, String userID, String comment);
    }
}
