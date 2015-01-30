// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GlobalSettingBuilder.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   Builder for global settings
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace BuildMeA
{
  using Entities;

  public class GlobalSettingBuilder : Builder<GlobalSetting>
  {
    public GlobalSettingBuilder()
    {
      Instance = new GlobalSetting();
    }
  }
}
