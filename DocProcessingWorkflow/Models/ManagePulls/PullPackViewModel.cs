// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PullPackViewModel.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DocProcessingWorkflow.Models.ManagePulls
{
  using System;

  public class PullPackViewModel
  {
    public String ClientReference { get; set; }

    public Boolean Selected { get; set; }

    public String Name { get; set; }
  }
}