// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuthenticationManagerProvider.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace IdentityWrapper.Identity
{
  using System;
  using System.Security.Claims;
  using System.Web;
  using IdentityWrapper.Interfaces;
  using Microsoft.Owin.Security;

  public class AuthenticationManagerProvider : IAuthenticationManagerProvider
  {
    private readonly IHttpContextBaseProvider httpContextBaseProvider;
    private readonly IAuthenticationManager authenticationManager;
    private readonly ClaimsPrincipal user;

    public AuthenticationManagerProvider(IHttpContextBaseProvider httpContextBaseProvider)
    {
      this.httpContextBaseProvider = httpContextBaseProvider;
      this.authenticationManager = this.httpContextBaseProvider.HttpContext.GetOwinContext().Authentication;
      this.user = this.authenticationManager.User;
    }

    public IAuthenticationManager AuthenticationManager
    {
      get
      {
        return this.authenticationManager;
      }
    }

    public ClaimsPrincipal User
    {
      get
      {
        return this.user;
      }
    }

    public void SignOut()
    {
      this.authenticationManager.SignOut();
    }

    public void SignOut(String authenticationType)
    {
      this.AuthenticationManager.SignOut(authenticationType);
    }

    public void SignIn(AuthenticationProperties authProps, ClaimsIdentity identity)
    {
      this.AuthenticationManager.SignIn(authProps, identity);
    }
  }
}
