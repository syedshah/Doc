// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EmailService.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Services
{
  using System;

  using BusinessEngineInterfaces;

  using Exceptions;

  using ServiceInterfaces;

  public class EmailService : IEmailService
  {
    private readonly IEmailEngine emailEngine;

    public EmailService(IEmailEngine emailEngine)
    {
      this.emailEngine = emailEngine;
    }

    public void SendEmail(String from, String to, String subject, String body, String mailServer)
    {
      try
      {
        this.emailEngine.SendEmail(from, to, subject, body, mailServer);
      }
      catch (Exception e)
      {
        throw new DocProcessingException("Unable to send authorise email", e);
      }
    }
  }
}
