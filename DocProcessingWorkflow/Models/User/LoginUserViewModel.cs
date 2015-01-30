// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LoginUserViewModel.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   Model for creating users
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DocProcessingWorkflow.Models.User
{
  using System;
  using System.ComponentModel.DataAnnotations;

  public class LoginUserViewModel
  {
    [Required]
    public String Username { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public String Password { get; set; }
  }
}