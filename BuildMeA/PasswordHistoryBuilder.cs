// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PasswordHistoryBuilder.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace BuildMeA
{
  using Entities;

  public class PasswordHistoryBuilder : Builder<PasswordHistory>
  {
    public PasswordHistoryBuilder()
    {
      Instance = new PasswordHistory();    
    }
  }
}