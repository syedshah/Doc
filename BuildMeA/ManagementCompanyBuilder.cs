// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ManagementCompanyBuilder.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------


namespace BuildMeA
{
  using Entities;

  public class ManagementCompanyBuilder : Builder<ManagementCompany>
  {
    public ManagementCompanyBuilder()
    {
      Instance = new ManagementCompany();
    }
  }
}
