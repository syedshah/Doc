// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OneStepLogViewModel.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   Defines the OneStepLogViewModel type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DocProcessingWorkflow.Models.Jobs
{
  using System;

  using Entities.File;

  public class OneStepLogViewModel
  {
    public String LogFileName { get; set; }

    public String[] LogContent { get; set; }

    public void AddOneStepLog(OneStepLog oneStepLog)
    {
      this.LogContent = oneStepLog.LogContent;
      this.LogFileName = oneStepLog.LogFileName;
    }
  }
}