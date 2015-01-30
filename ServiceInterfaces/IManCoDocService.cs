// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IManCoDocService.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   IManCoDocService object
// </summary>
// --------------------------------------------------------------------------------------------------------------------


namespace ServiceInterfaces
{
    using System;
    using System.Collections.Generic;
    using Entities;

    public interface IManCoDocService
    {
        IList<ManCoDoc> GetManCoDocsByManCoCodeEnvironment(String manCoCode, String environment,string userID);
        void PromoteMancoDocs(String manCoDocIds, String sourceAppEnvironMent, String targetAppEnvironment, String userID, String comment);
    }
}
