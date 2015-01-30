// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JobBuilder.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   Builder for JobBuilder
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace BuildMeA
{
  using Entities;

  public class JobBuilder : Builder<Job>
  {
    public JobBuilder()
    {
      Instance = new Job();
    }
  }
}
