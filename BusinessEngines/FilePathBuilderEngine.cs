// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FilePathBuilderEngine.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   File Path Builder Engine object
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace BusinessEngines
{
  using System;

  using BusinessEngineInterfaces;

  public class FilePathBuilderEngine : IFilePathBuilderEngine
  {
    public String BuildOneStepLogFilePath(String server, String gridName)
    {
      return String.Format(@"\\{0}\Nexdox\nmbbs02\Process\{1}\Output\{1}", server, gridName);
    }

    public String BuildInputFileLocationPath(String server, String manCo, String docType)
    {
      return String.Format(@"\\{0}\Nexdox\nmbbs01\Input\{1}\{2}", server, manCo, docType);
    }

    public String BuildOneStepOutputDirectory(String server, String gridName)
    {
      return String.Format(@"\\{0}\Nexdox\nmbbs02\Process\{1}\Output", server, gridName);
    }

    public String BuildOneStepMonitorPath(String server)
    {
      return String.Format(@"\\{0}\Nexdox\nmbbs01\Monitor\", server);
    }
  }
}
