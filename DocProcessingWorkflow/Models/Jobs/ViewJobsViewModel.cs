// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ViewJobsViewModel.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   Defines the ViewJobsViewModel type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DocProcessingWorkflow.Models.Jobs
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  using DocProcessingWorkflow.Models.Helper;

  using Entities;

  public class ViewJobsViewModel
  {
    public ViewJobsViewModel()
    {
      this.Jobs = new List<JobViewModel>();
    }

    public IList<JobViewModel> Jobs { get; set; }

    public PagingInfoViewModel PagingInfo { get; set; }

    public String CurrentPage { get; set; }

    public String SearchField { get; set; }

    public String SearchValue { get; set; }

    public Boolean CanExportResults { get; set; }

    public Boolean CanAuthoriseJobs { get; set; }

    public Boolean CanShowActionMenu { get; set; }

    public Boolean CanChangeStatus { get; set; }

    public void AddJobs(IList<JobEntity> jobs)
    {
      jobs.ToList().ForEach(x =>
        Jobs.Add(new JobViewModel()
                   {
                     Status = x.Status,
                     Company = x.Company,
                     Grid = x.Grid,
                     AllocatorGRID=x.AllocatorGRID,
                     Document = x.Document,
                     Version = x.Version,
                     Owner = x.Owner,
                     SubmitDate = x.SubmitDateTime,
                     Authorise = x.HoldAuthorisation,
                     JobReference = x.JobReference,
                     JobAuthorise = x.JobAuthorise,
                     Authoriser = x.Authoriser,
                     ManualSubmissionOnly = x.ManualSubmissionOnly,
                     Id = x.JobId.ToString()
                   }));
    }

    public void AddJobs(PagedResult<JobEntity> jobs)
    {
      foreach (JobEntity job in jobs.Results)
      {
        var jvm = new JobViewModel(job);       
        this.Jobs.Add(jvm);
      }

      this.PagingInfo = new PagingInfoViewModel
      {
        CurrentPage = jobs.CurrentPage,
        TotalItems = jobs.TotalItems,
        ItemsPerPage = jobs.ItemsPerPage,
        TotalPages = jobs.TotalPages,
        StartRow = jobs.StartRow,
        EndRow = jobs.EndRow
      };

      this.CurrentPage = this.PagingInfo.CurrentPage.ToString();
    }
  }
}