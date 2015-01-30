// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IPasswordHistoryRepository.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   Interface for Password History Repo
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DocProcessingRepository.Interfaces
{
  using System;
  using System.Collections.Generic;
  using Entities;
  using Repository;

  public interface IPasswordHistoryRepository : IRepository<PasswordHistory>
  {
    IList<PasswordHistory> GetPasswordHistory(String userId, Int32 lastNumberOfRecords);

    IList<PasswordHistory> GetPasswordHistoryByLastItems(String userId, Int32 lastNumberOfPasswords);
  }
}