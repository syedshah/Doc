// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IJobStatusService.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ServiceInterfaces
{
  using System;

  public interface IJobStatusService
  {
    void UpdateJobStatus(Int32 jobId, Int32 jobStatusTypeId);
  }
}
