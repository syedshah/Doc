// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuthoriseHistoryRepository.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DocProcessingRepository.Repositories
{
  using System;

  using DocProcessingRepository.Contexts;
  using DocProcessingRepository.Interfaces;

  using EFRepository;

  using Entities;

  public class AuthoriseHistoryRepository : BaseEfRepository<AuthoriseHistory>, IAuthoriseHistoryRepository
  {
    public AuthoriseHistoryRepository(String connectionString)
      : base(new IdentityDb(connectionString))
    {
    }

    public void InsertAuthorisation(Int32 jobId, String userId, String comment)
    {
      this.ExecuteStoredProcedure(String.Format("CreateAuthoriseHistory {0}, '{1}', '{2}'", jobId, userId, comment));
    }
  }
}
