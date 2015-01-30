// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IGlobalSettingRepository.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DocProcessingRepository.Interfaces
{
  using Entities;

  using Repository;

  public interface IGlobalSettingRepository : IRepository<GlobalSetting>
  {
    GlobalSetting Get();
  }
}
