// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnvironmentTypeService.cs" company="DST Nexdox">
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
  using DocProcessingRepository.Interfaces;
  using Entities;
  using Exceptions;
  using ServiceInterfaces;

  public class EnvironmentTypeService : IEnvironmentTypeService
  {
    private readonly IEnvironmentTypeRepository environmentTypeRepository;

    public EnvironmentTypeService(IEnvironmentTypeRepository environmentTypeRepository)
    {
      this.environmentTypeRepository = environmentTypeRepository;
    }

    public IList<EnvironmentServerEntity> GetEnvironmentServers(String environmentType)
    {
      try
      {
        return this.environmentTypeRepository.GetEnvironmentServers(environmentType);
      }
      catch (Exception e)
      {
        throw new DocProcessingException("Unable to get environment servers", e);
      }
    }
  }
}
