// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IHttpContextBaseProvider.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace IdentityWrapper.Interfaces
{
  using System.Web;

  public interface IHttpContextBaseProvider
  {
    HttpContextBase HttpContext { get; }
  }
}