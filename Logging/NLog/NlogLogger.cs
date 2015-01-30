// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NLogLogger.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   Defines the NLogLogger type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Logging.NLog
{
  using System;

  using global::NLog;

  public class NLogLogger : ILogger
  {
    private readonly Logger logger;

    public NLogLogger()
    {
      this.logger = LogManager.GetLogger("DocProcessingWorkflow");
    }

    public void Info(String message)
    {
      this.logger.Info(message);
    }

    public void Warn(String message)
    {
      this.logger.Warn(message);
    }

    public void Debug(String message)
    {
      this.logger.Debug(message);
    }

    public void Error(String message)
    {
      this.logger.Error(message);
    }

    public void Error(Exception x, String path, String url)
    {
      this.Error(x.BuildExceptionMessage(path, url));
    }

    public void Error(String message, Exception x)
    {
      this.logger.ErrorException(message, x);
    }

    public void Fatal(String message)
    {
      this.logger.Fatal(message);
    }

    public void Fatal(String message, Exception x)
    {
      this.logger.Fatal(message, x);
    }

    public void Fatal(Exception x, String path, String url)
    {
      this.Fatal(x.BuildExceptionMessage(path, url));
    }
  }
}
