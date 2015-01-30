// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserSummaryViewModel.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DocProcessingWorkflow.Models.User
{
  using System;

  using Entities;

  public class UserSummaryViewModel
  {
    public UserSummaryViewModel()
    {
    }

    public UserSummaryViewModel(ApplicationUser user)
    {
      this.FirstName = user.FirstName;

      this.LastName = user.LastName;
    }

    public String FirstName { get; set; }

    public String LastName { get; set; }
  }
}