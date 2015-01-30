// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IManCoRepository.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DocProcessingRepository.Interfaces
{
  using System;
  using System.Collections.Generic;
  using Entities;

  public interface IManCoRepository
  {
    IList<ManagementCompany> GetManCos(String userId);
  }
}
