// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApplicationUserBuilder.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace BuildMeA
{
  using System;

  using Entities;

  public class ApplicationUserBuilder : Builder<ApplicationUser>
  {
    public ApplicationUserBuilder()
    {
      Instance = new ApplicationUser();
      Instance.LastPasswordChangedDate = DateTime.Now;
      Instance.LastLoginDate = DateTime.Now;
    }
  }
}

