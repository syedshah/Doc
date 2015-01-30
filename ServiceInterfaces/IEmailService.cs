// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IEmailService.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ServiceInterfaces
{
  using System;

  public interface IEmailService
  {
    void SendEmail(String from, String to, String subject, String body, String mailServer);
  }
}
