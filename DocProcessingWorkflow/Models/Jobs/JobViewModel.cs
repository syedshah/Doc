// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JobViewModel.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   Defines the JobViewModel type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using WebGrease.Css.Extensions;

namespace DocProcessingWorkflow.Models.Jobs
{
    using System;

    using DocProcessingWorkflow.Constants;

    using Entities;

    public class JobViewModel
    {
        public JobViewModel()
        {
            this.EnclosingJobs = new List<EnclosingJobViewModel>();
        }

        public JobViewModel(JobEntity job) : this()
        {  
            this.JobReference = job.JobReference;
            this.Grid = job.Grid;
            this.AllocatorGRID = job.AllocatorGRID;
            this.Authorise = job.HoldAuthorisation;
            this.SubmitDate = job.SubmitDateTime;
            this.ManualSubmissionOnly = job.ManualSubmissionOnly;
            this.JobAuthorise = job.JobAuthorise;
            this.Owner = job.Owner;
            this.Version = job.Version;
            this.Document = job.Document;
            this.Company = job.Company;
            this.Status = job.Status;
            this.Authoriser = job.Authoriser;
            this.Id = job.JobId.ToString();
        }

        public String Id { get; set; }

        public String JobReference { get; set; }

        public String Grid { get; set; }

        public String AllocatorGRID { get; set; }

        public String Company { get; set; }

        public String Document { get; set; }

        public String Version { get; set; }

        public String Owner { get; set; }

        public String SubmitDate { get; set; }

        public String Status { get; set; }

        public Boolean? Authorise { get; set; }

        public Boolean? ManualSubmissionOnly { get; set; }

        public String JobAuthorise { get; set; }

        public String Authoriser { get; set; }

        public List<EnclosingJobViewModel> EnclosingJobs { get; private set; }

        public Boolean Cancelable { get; set; }

        public void AddEnclosingJobs(IEnumerable<EnclosingJob> enclosingJobs)
        {
            this.EnclosingJobs.AddRange(enclosingJobs.Select(job => new EnclosingJobViewModel(job)));

            this.Cancelable =
                this.Status != JobStatusTypes.Cancelled &&
                this.EnclosingJobs.Any(enclosingJob => enclosingJob.PostalDocketNumberMissing);
        }
    }
}