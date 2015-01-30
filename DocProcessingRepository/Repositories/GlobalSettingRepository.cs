// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GlobalSettingRepository.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   Doc processing repository
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DocProcessingRepository.Repositories
{
  using System;
  using System.Linq;
  using DocProcessingRepository.Contexts;
  using DocProcessingRepository.Interfaces;
  using EFRepository;
  using Entities;

  public class GlobalSettingRepository : BaseEfRepository<GlobalSetting>, IGlobalSettingRepository
  {
    public GlobalSettingRepository(String connectionString)
      : base(new IdentityDb(connectionString))
    {

    }

    public GlobalSetting Get()
    {
      return (from a in Entities
              select a).FirstOrDefault();
    }
  }
}
