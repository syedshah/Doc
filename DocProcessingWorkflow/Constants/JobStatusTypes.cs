// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JobStatusTypes.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DocProcessingWorkflow.Constants
{
  using System;

  public static class JobStatusTypes
  {
    public const String Complete = "Complete";

    public const String Processing = "Processing";

    public const String Failed = "Failed";

    public const String WaitingAuthorisation = "WaitingAuthorisation";

    public const String Dispatched = "Dispatched";

    public const String Cancelled = "Cancelled";
  }
}