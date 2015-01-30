// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SearchPacksViewModel.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DocProcessingWorkflow.Models.ManagePulls
{
  using System;
  using System.ComponentModel.DataAnnotations;

  public class SearchPacksViewModel
  {
    [Required(ErrorMessage = "Please select a job")]
    public Int32 JobId { get; set; }

    [Required(ErrorMessage = "Please select a search criteria")]
    public String SearchCriteria { get; set; }
  }
}