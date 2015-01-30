// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SubmitJobsViewModel.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   View model for displaying submit jobs index page
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DocProcessingWorkflow.Models.ManagePulls
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using DocProcessingWorkflow.Models.Jobs;
  using DocProcessingWorkflow.Models.ManCo;

  public class ManagePullsViewModel
  {
    public ManagePullsViewModel()
    {
      this.Pulls = new PulledDocumentsViewModel();
      this.Packs = new SearchPacksResultsViewModel();
    }

    public String SelectedManCoId { get; set; }

    public String SelectedJobId { get; set; }

    public PulledDocumentsViewModel Pulls { get; set; }

    public SearchPacksResultsViewModel Packs { get; set; }

    public String SearchField { get; set; }

    public List<ManCoViewModel> ManCos
    {
      get
      {
        return this.namCos;
      }
      set
      {
        this.namCos = value;
      }
    }

    private List<ManCoViewModel> namCos = new List<ManCoViewModel>();

    private List<CompletedJobsViewModel> completedJobs = new List<CompletedJobsViewModel>();

    public List<CompletedJobsViewModel> CompletedJobs
    {
      get
      {
        return this.completedJobs;
      }
      set
      {
        this.completedJobs = value;
      }
    }

    public void AddManCos(IList<Entities.ManagementCompany> manCos)
    {
      foreach (var mvm in manCos.Select(manCo => new ManCoViewModel(manCo)))
      {
        this.ManCos.Add(mvm);
      }
    }
  }
}