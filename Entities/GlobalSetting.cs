// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GlobalSetting.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   global setting entity
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Entities
{
  using System;

  public class GlobalSetting
  {
    public Int32 Id { get; set; }

    public Int32 MinPasswordLength { get; set; }

    public Int32 MinNonAlphaChars { get; set; }

    public Int32 PasswordExpDays { get; set; }

    public Int32 PassChangeBefore { get; set; }

    public Boolean NewUserPasswordReset { get; set; }

    public Int32 MaxLogInAttempts { get; set; }
  }
}
