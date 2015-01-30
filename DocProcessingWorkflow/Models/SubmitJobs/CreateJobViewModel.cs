// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SubmitJobsViewModel.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   View model for displaying submit jobs index page
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DocProcessingWorkflow.Models.SubmitJobs
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel.DataAnnotations;
  using System.Text.RegularExpressions;

  public class CreateJobViewModel : IValidatableObject
  {
    public CreateJobViewModel()
    {
      this.ChosenFiles = new List<string>();
      AdditionalSetupViewModel = new AdditionalSetupViewModel();
    }

    public String ManCo { get; set; }

    [Required]
    public String DocTypeCode { get; set; }

    [Required]
    public String DocTypeName { get; set; }

    public List<String> ChosenFiles { get; set; }

    public Boolean AdditionalInfoRequired { get; set; }

    public Boolean AllowReprocessing { get; set; }

    [Required]
    public String SelectedFolder { get; set; }

    public AdditionalSetupViewModel AdditionalSetupViewModel { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
      if (String.IsNullOrEmpty(this.ManCo))
      {
        yield return new ValidationResult("Management company is required");
      }

      if (String.IsNullOrEmpty(this.DocTypeCode))
      {
        yield return new ValidationResult("Document type code is required");
      }

      if (this.ChosenFiles.Count == 0)
      {
        yield return new ValidationResult("No files have been chosen");
      }

      if (this.AdditionalInfoRequired && this.AdditionalSetupViewModel.AdditionalFileContent != null)
      {
        if (this.AdditionalSetupViewModel.AccountNumber == "00000000" || String.IsNullOrEmpty(this.AdditionalSetupViewModel.AccountNumber) || this.AdditionalSetupViewModel.AccountNumber.Length != 8 || Regex.IsMatch(this.AdditionalSetupViewModel.AccountNumber, @"^[a-zA-Z]+$"))
        {
          yield return new ValidationResult("Account number is not valid");  
        }

        if (this.AdditionalSetupViewModel.ChequeNumber == "000000" || this.AdditionalSetupViewModel.ChequeNumber.Length != 6 || String.IsNullOrEmpty(this.AdditionalSetupViewModel.ChequeNumber) || Regex.IsMatch(this.AdditionalSetupViewModel.ChequeNumber, @"^[a-zA-Z]+$"))
        {
          yield return new ValidationResult("Cheque number is not valid");
        }

        if (this.AdditionalSetupViewModel.ChequeNumber.Length < 6)
        {
          this.AdditionalSetupViewModel.ChequeNumber = this.AdditionalSetupViewModel.ChequeNumber.PadLeft(6, '0');
        }

        if ((this.AdditionalSetupViewModel.SortCodeOne == "00" && this.AdditionalSetupViewModel.SortCodeTwo == "00" && this.AdditionalSetupViewModel.SortCodeThree == "00") ||
          String.IsNullOrEmpty(this.AdditionalSetupViewModel.SortCodeOne) || String.IsNullOrEmpty(this.AdditionalSetupViewModel.SortCodeTwo) || String.IsNullOrEmpty(this.AdditionalSetupViewModel.SortCodeThree) ||
          this.AdditionalSetupViewModel.SortCodeOne.Length < 2 || this.AdditionalSetupViewModel.SortCodeTwo.Length < 2 || this.AdditionalSetupViewModel.SortCodeThree.Length < 2 ||
          Regex.IsMatch(this.AdditionalSetupViewModel.SortCodeOne, @"^[a-zA-Z]+$") || Regex.IsMatch(this.AdditionalSetupViewModel.SortCodeTwo, @"^[a-zA-Z]+$") || Regex.IsMatch(this.AdditionalSetupViewModel.SortCodeThree, @"^[a-zA-Z]+$"))
        {
            yield return new ValidationResult("Sort code is not valid");
        }
      }
    }
  }
}