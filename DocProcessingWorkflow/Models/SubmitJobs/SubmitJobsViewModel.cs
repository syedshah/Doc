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
  using System.Linq;

  using DocProcessingWorkflow.Models.DocType;
  using DocProcessingWorkflow.Models.ManCo;

  public class SubmitJobsViewModel
  {
    public SubmitJobsViewModel()
    {
      this.Files = new InputFileViewModel();
    }

    public String SelectedManCoId { get; set; }

    public String SelectedDocTypeId { get; set; }

    public Boolean AllowReprocessing { get; set; }

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

    public InputFileViewModel Files { get; set; }

    private List<ManCoViewModel> namCos = new List<ManCoViewModel>();

    private List<DocTypeViewModel> docTypes = new List<DocTypeViewModel>();

    public List<DocTypeViewModel> DocTypes
    {
      get
      {
        return docTypes;
      }
      set
      {
        docTypes = value;
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