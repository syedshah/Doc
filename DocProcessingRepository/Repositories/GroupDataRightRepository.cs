// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GroupDataRightRepository.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DocProcessingRepository.Repositories
{
  using System;

  using DocProcessingRepository.Contexts;
  using DocProcessingRepository.Interfaces;
  using EFRepository;
  using Entities;

  public class GroupDataRightRepository : BaseEfRepository<GroupDataRight>, IGroupDataRightRepository
  {
    public GroupDataRightRepository(String connectionString)
      : base(new IdentityDb(connectionString))
    {

    }
  }
}
