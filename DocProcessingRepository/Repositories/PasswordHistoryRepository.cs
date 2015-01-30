// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PasswordHistoryRepository.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   Password history repo
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DocProcessingRepository.Repositories
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using DocProcessingRepository.Contexts;
  using DocProcessingRepository.Interfaces;
  using EFRepository;
  using Entities;

  public class PasswordHistoryRepository : BaseEfRepository<PasswordHistory>, IPasswordHistoryRepository
  {
    public PasswordHistoryRepository(String connectionString)
      : base(new IdentityDb(connectionString))
    {
    }

    public IList<PasswordHistory> GetPasswordHistory(String userId, Int32 lastNumberOfRecords)
    {
      return
          (from a in Entities orderby a.Id descending where a.UserId == userId select a).Take(lastNumberOfRecords)
                                                                                        .ToList();
    }

    public IList<PasswordHistory> GetPasswordHistoryByLastItems(String userId, Int32 lastNumberOfPasswords)
    {
      return
          (from a in Entities
           orderby a.Id descending
           where a.UserId == userId
           select a).Take(lastNumberOfPasswords).ToList();
    }
  }
}