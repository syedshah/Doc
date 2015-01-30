// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IApprovalHistoryRepository.cs" company="DST Nexdox">
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

  using Repository;

  public interface IAuthoriseHistoryRepository : IRepository<AuthoriseHistory>
  {
    void InsertAuthorisation(Int32 jobId, String userId, String comment);
  }
}
