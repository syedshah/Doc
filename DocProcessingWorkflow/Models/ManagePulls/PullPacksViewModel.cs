// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PullPacksViewModel.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DocProcessingWorkflow.Models.ManagePulls
{
  using System;
  using System.Collections.Generic;

  public class PullPacksViewModel
  {
    public Int32 JobId { get; set; }

    public IList<PullPackViewModel> Packs { get; set; }
  }
}