// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IFilePathBuilderEngine.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   File Path Builder Engine object
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace BusinessEngineInterfaces
{
  using System;

  public interface IFilePathBuilderEngine
  {
    String BuildOneStepLogFilePath(String server, String gridName);

    String BuildInputFileLocationPath(String server, String manCo, String docType);

    String BuildOneStepOutputDirectory(String server, String gridName);

    String BuildOneStepMonitorPath(String server);
  }
}
