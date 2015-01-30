// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnvironmentTypeBuilder.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   Builder for Environment Type
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace BuildMeA
{
  using Entities;

  public class EnvironmentTypeBuilder : Builder<EnvironmentType>
  {
    public EnvironmentTypeBuilder()
    {
      Instance = new EnvironmentType();
    }
  }
}
