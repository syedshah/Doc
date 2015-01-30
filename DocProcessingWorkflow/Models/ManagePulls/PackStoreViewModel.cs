// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SearchPackResultViewModel.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DocProcessingWorkflow.Models.ManagePulls
{
  using System;

  public class PackStoreViewModel
  {
    public PackStoreViewModel(Entities.ADF.PackStore packStore)
    {
      this.ClientReference = packStore.RecepientRef.ToString();
      this.Name = String.Format("{0} {1}", packStore.Name1, packStore.Name2);
      this.DisplayName = String.Format("{0} {1}", this.ClientReference, this.Name);
    }

    public Boolean Selected { get; set; }

    public String ClientReference { get; set; }

    public String Name { get; set; }

    public String DisplayName { get; set; }
  }
}