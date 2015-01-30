// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EmailEngine.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace BusinessEngines
{
  using System;
  using System.Collections.Generic;
  using System.Net.Mail;
  using BusinessEngineInterfaces;

  public class EmailEngine : IEmailEngine
  {
    public void SendEmail(String from, String to, String subject, String body, String mailServer)
    {
      var addresses = new List<String>(to.Split(';'));

      foreach (var address in addresses)
      {
        var emailMessage = new MailMessage(from, address, subject, body) { IsBodyHtml = true };
        var smtpServer = new SmtpClient(mailServer);
        smtpServer.Send(emailMessage);  
      }
    }
  }
}
