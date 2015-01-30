// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ParentCompanyBuilder.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   Builder for ParentCompany
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace BuildMeA
{
  using Entities;

  public class ParentCompanyBuilder : Builder<ParentCompany>
  {
    public ParentCompanyBuilder()
    {
      Instance = new ParentCompany();
    }

    public ParentCompanyBuilder WithManCo(ManagementCompany managementCompany)
    {
      if (managementCompany != null)
      {
        Instance.ManagementCompanies.Add(managementCompany);
      }
      return this;
    }
  }
}
