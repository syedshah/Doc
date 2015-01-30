// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PasswordHistoryService.cs" company="DST Nexdox">
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
  using System.Linq;
  using DocProcessingRepository.Interfaces;
  using Encryptions;
  using Entities;
  using Exceptions;
  using ServiceInterfaces;

  public class PasswordHistoryService : IPasswordHistoryService
  {
    private readonly IPasswordHistoryRepository passwordHistoryRepository;

    public PasswordHistoryService(
    IPasswordHistoryRepository passwordHistoryRepository)
    {
      this.passwordHistoryRepository = passwordHistoryRepository;
    }

    public Boolean IsPasswordInHistory(String userId, String passwordHash)
    {
      try
      {
        var passwordHistory = this.GetPasswordHistoryByLastItems(userId, 12);

        var passwordList = this.GetEncryptedPasswordList(passwordHistory);

        if (passwordList.Contains(DocProcessingEncryption.Encrypt(passwordHash)))
        {
          return true;
        }
        else
        {
          return false;
        }
      }
      catch (Exception e)
      {
        throw new DocProcessingException("Unable to validate password in password history", e);
      }
    }

    public IList<PasswordHistory> GetPasswordHistoryByLastItems(String userId, Int32 lastNumberOfPasswords)
    {
      try
      {
        return this.passwordHistoryRepository.GetPasswordHistoryByLastItems(userId, lastNumberOfPasswords);
      }
      catch (Exception e)
      {
        throw new DocProcessingException("Unable to get password history", e);
      }
    }

    private IList<String> GetEncryptedPasswordList(IEnumerable<PasswordHistory> passwordHistory)
    {
      var passwordList = new List<String>();

      passwordHistory.ToList().ForEach(x => passwordList.Add(x.PasswordHash));

      return passwordList;
    }
  }
}
