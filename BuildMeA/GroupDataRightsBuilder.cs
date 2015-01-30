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

  public class GroupDataRightBuilder : Builder<GroupDataRight>
  {
    public GroupDataRightBuilder()
    {
      Instance = new GroupDataRight();
    }

    public GroupDataRightBuilder WithManCoDoc(ManCoDoc manCoDoc)
    {
      Instance.ManCoDoc = manCoDoc;
      return this;
    }

    public GroupDataRightBuilder WithGroup(Group group)
    {
      Instance.Group = group;
      return this;
    }

    public GroupDataRightBuilder WithAppEnvironment(AppEnvironment appEnvironment)
    {
      Instance.AppEnvironment = appEnvironment;
      return this;
    }
  }
}
