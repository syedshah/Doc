// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AdditionalSetupViewModel.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DocProcessingWorkflow.Models.SubmitJobs
{
  using System;
  using System.ComponentModel.DataAnnotations;
  
  public class AdditionalSetupViewModel
  {
    public AdditionalSetupViewModel()
    {
      this.AdditionalFileContent = String.Empty;
    }

    public AdditionalSetupViewModel(Entities.File.AdditionalInfoFile additionalInfoFile)
    {
      this.ShowAdditionalInfo = additionalInfoFile.Warrants > 0;

      this.AdditionalFileContent = additionalInfoFile.AdditionalFileContent;
    }

    public Boolean ShowAdditionalInfo { get; set; }

    [Required]
    public String SortCodeOne { get; set; }

    [Required]
    public String SortCodeTwo { get; set; }

    [Required]
    public String SortCodeThree { get; set; }

    [Required]
    public String AccountNumber { get; set; }

    [Required]
    public String ChequeNumber { get; set; }

    public String AdditionalFileContent { get; set; }

    public String SortCode { get; private set; }
  }
}