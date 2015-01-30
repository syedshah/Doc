// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IEmailEngine.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace BusinessEngineInterfaces
{
  using System;

  public interface IEmailEngine
  {
    void SendEmail(String from, String to, String subject, String body, String mailServer);
  }
}
