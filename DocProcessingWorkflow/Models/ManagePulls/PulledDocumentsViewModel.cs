// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PulledDocumentsViewModel.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   View model for viewing pulls
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DocProcessingWorkflow.Models.ManagePulls
{
  using System;
  using System.Collections.Generic;

  public class PulledDocumentsViewModel
  {
    public PulledDocumentsViewModel()
    {
      this.Pulls = new List<PackStoreViewModel>();
    }

    public Boolean CanAuthorisePullList { get; set; }

    public List<PackStoreViewModel> Pulls { get; set; }

    public String SelectedPull { get; set; }

    public void AddPulls(IEnumerable<Entities.ADF.PackStore> packStores)
    {
      foreach (var pull in packStores)
      {
        this.Pulls.Add(new PackStoreViewModel(pull));
      }
    }
  }
}