// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PasswordHistory.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   Password history entity
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Entities
{
  using System;

  public class PasswordHistory
  {
    public Int32 Id { get; set; }

    public String PasswordHash { get; set; }

    public String UserId { get; set; }

    public ApplicationUser User { get; set; }

    public virtual DateTime LogDate { get; set; }
  }
}
