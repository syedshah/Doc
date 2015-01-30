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
  using System.Collections.Generic;

  public class SearchPacksResultsViewModel
  {
    public SearchPacksResultsViewModel()
    {
      this.Packs = new List<PackStoreViewModel>();
    }

    public IList<PackStoreViewModel> Packs { get; set; }

    public void AddPacks(IEnumerable<Entities.ADF.PackStore> packStores)
    {
      foreach (var pull in packStores)
      {
        this.Packs.Add(new PackStoreViewModel(pull));
      }
    }
  }
}