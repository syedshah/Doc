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

  public class GroupBuilder : Builder<Group>
  {
    public GroupBuilder()
    {
      Instance = new Group();
    }

    public GroupBuilder WithApplicationUser(ApplicationUser applicationUser)
    {
      if (applicationUser != null)
      {
        Instance.ApplicationUsers.Add(applicationUser);
      }
      return this;
    }
  }
}
