// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AppEnvironmentBuilder.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace BuildMeA
{
  using Entities;

  public class AppEnvironmentBuilder : Builder<AppEnvironment>
  {
    public AppEnvironmentBuilder()
    {
      Instance = new AppEnvironment();
    }

    public AppEnvironmentBuilder WithGroupDataRight(GroupDataRight groupDataRight)
    {
      if (groupDataRight != null)
      {
        Instance.GroupDataRights.Add(groupDataRight);
      }
      return this;
    }
  }
}
