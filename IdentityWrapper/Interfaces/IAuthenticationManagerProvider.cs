// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAuthenticationManagerProvider.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace IdentityWrapper.Interfaces
{
  using System;
  using System.Security.Claims;
  using Microsoft.Owin.Security;

  public interface IAuthenticationManagerProvider
  {
    IAuthenticationManager AuthenticationManager { get; }

    ClaimsPrincipal User { get; }

    void SignOut();

    void SignOut(String authenticationType);

    void SignIn(AuthenticationProperties authProps, ClaimsIdentity identity);
  }
}
