// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EditUserViewModel.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   model for editing users
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Web.Mvc.Html;
using Entities;

namespace DocProcessingWorkflow.Models.User
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using DocProcessingWorkflow.Constants;
    using DocProcessingWorkflow.Filters;

    public class EditUserViewModel : ExtendedUserViewModel
    {
        public EditUserViewModel()
            : base()
        {
            this.LandingPages = new List<String> { LandingPage.Home, LandingPage.JobsView, LandingPage.JobsSubmit, LandingPage.JobsManageInserts };
            this.LandingEnvironments = new List<String>();
        }
        public EditUserViewModel(ApplicationUser user)
            : base(user)
        {
            this.LandingPages = new List<String> { LandingPage.Home, LandingPage.JobsView, LandingPage.JobsSubmit, LandingPage.JobsManageInserts };
            this.LandingEnvironments = new List<String>();
            this.SelectedPrefLandingPage = user.PreferredLandingPage;
            this.SelectedPrefEnvironment = user.PreferredEnvironment;
        }

        [PasswordHistory("Id")]
        [PasswordValid("UserName", "FirstName", "LastName")]
        public String Password { get; set; }

        public String ConfirmPassword { get; set; }

        public String SelectedPrefLandingPage { get; set; }

        public String SelectedPrefEnvironment { get; set; }

        public List<String> LandingPages { get; set; }

        public List<String> LandingEnvironments { get; set; }

        public void AddAllowedEnvironments(IList<Entities.AppEnvironment> appEnvironments)
        {
            foreach (var appEnvironment in appEnvironments)
            {
                this.LandingEnvironments.Add(appEnvironment.Name);
            }
        }
    }
}