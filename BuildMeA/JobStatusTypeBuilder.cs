// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JobStatusTypeBuilder.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   Job Status Type Builder
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace BuildMeA
{
  using Entities;

  public class JobStatusTypeBuilder : Builder<JobStatusType>
  {
    public JobStatusTypeBuilder()
    {
      Instance = new JobStatusType();
    }
  }
}
