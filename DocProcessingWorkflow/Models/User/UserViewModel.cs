// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserViewModel.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DocProcessingWorkflow.Models.User
{
  using System;
  using System.ComponentModel;
  using System.ComponentModel.DataAnnotations;
  using System.Security.Principal;

  public class UserViewModel : IPrincipal, IIdentity
  {
    public Int32 Id { get; set; }

    [Required]
    [DisplayName(@"User name")]
    public String UserName { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public String Password { get; set; }

    [Editable(false)]
    public Boolean IsLoggedIn { get; set; }

    public String Name { get; private set; }

    public String AuthenticationType
    {
      get { return "Cookie"; }
    }

    public Boolean IsAuthenticated
    {
      get { return this.IsLoggedIn; }
    }

    public IIdentity Identity
    {
      get { return this; }
    }

    public Boolean IsInRole(String role)
    {
      throw new System.NotImplementedException();
    }
  }
}