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

    public class InputFileViewModel
    {
        public InputFileViewModel()
        {
            this.Files = new List<String>();
            this.Folders = new List<String>();
            this.AdditionalSetupViewModel = new AdditionalSetupViewModel();
        }

        public InputFileViewModel(String folder, String selectedFolder)//, String fundInfo)
            : this()
        {
            this.Folders.Add(folder);
            this.SelectedFolder = selectedFolder;
            //this.AdditionalSetupViewModel.FundInfo = fundInfo;
        }

        public InputFileViewModel(String folder, String selectedFolder, Entities.DocumentType docType)
            : this(folder, selectedFolder)//, fundInfo)
        {
            this.RequiresAdditionalInfo = docType.AdditionalSetup.GetValueOrDefault();
            //int warrantsValue;

            //if (!string.IsNullOrEmpty(fundInfo) &&
            //    Int32.TryParse(
            //          fundInfo.Split().GetValue(Array.IndexOf(fundInfo.ToLower().Split(), "warrants") - 1).ToString(), out warrantsValue))
            //{
            //    this.ShowAdditionalInfo = warrantsValue > 0;
            //}
        }

        public AdditionalSetupViewModel AdditionalSetupViewModel { get; set; }

        public IList<String> Files { get; set; }

        public IList<String> Folders { get; set; }

        public String SelectedFolder { get; set; }

        public String Test { get; set; }

        public IList<String> ChosenFiles { get; set; }

        public IList<String> AvailableFiles { get; set; }

        public Boolean RequiresAdditionalInfo { get; set; }

        public Boolean ShowAdditionalInfo { get; set; }

        public void AddFiles(IEnumerable<String> fileLocations)
        {
            foreach (var file in fileLocations)
            {
                this.Files.Add(file);
            }
        }

        public void AddSubFolders(IEnumerable<String> subFolders)
        {
            foreach (var subFolder in subFolders)
            {
                this.Folders.Add(subFolder);
            }
        }
    }
}