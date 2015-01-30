// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AppEnvironmentService.cs" company="DST Nexdox">
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

  public class ManCoService : IManCoService
  {
    private readonly IManCoRepository manCoRepository;

    public ManCoService(IManCoRepository manCoRepository)
    {
      this.manCoRepository = manCoRepository;
    }

    public IList<ManagementCompany> GetManCos(String userId)
    {
      try
      {
        return this.manCoRepository.GetManCos(userId);
      }
      catch (Exception e)
      {
        throw new DocProcessingException("Unable to retrieve mancos", e);
      }
    }

    public ManagementCompany GetManCo(String code)
    {
      throw new NotImplementedException();
    }
  }
}
