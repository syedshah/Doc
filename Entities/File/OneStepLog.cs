// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OneStepLog.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   One Step Log object 
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Entities.File
{
  using System;

  public class OneStepLog
  {
    public String LogFileName { get; set; }

    public String[] LogContent { get; set; }

    public String[] Content { get; set; }
  }
}
