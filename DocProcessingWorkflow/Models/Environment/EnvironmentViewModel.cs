// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnvironmentViewModel.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   view model to display environment options
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DocProcessingWorkflow.Models.Environment
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  public class EnvironmentViewModel
  {
    public EnvironmentViewModel(Entities.ApplicationUser user) 
    {
      this.Environments = new List<String>();
      this.SelectedEnvironment = user.PreferredEnvironment;
    }

    public List<String> Environments { get; set; }

    public String SelectedEnvironment { get; set; }

    public void AddAllowedEnvironments(IList<Entities.AppEnvironment> appEnvironments)
    {
      foreach (var appEnvironment in appEnvironments)
      {
        this.Environments.Add(appEnvironment.Name);
      }

      if (this.SelectedEnvironment == null)
      {
        this.SelectedEnvironment = (from a in appEnvironments orderby a.AppEnvironmentID select a).First().Name;
      }
    }
  }
}