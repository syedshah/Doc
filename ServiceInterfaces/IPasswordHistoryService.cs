// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IPasswordHistoryService.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   Password history interface
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ServiceInterfaces
{
  using System;
  using System.Collections.Generic;
  using Entities;

  public interface IPasswordHistoryService
  {
    IList<PasswordHistory> GetPasswordHistoryByLastItems(String userId, Int32 lastNumberOfPasswords);

    Boolean IsPasswordInHistory(String userId, String passwordHash);
  }
}
