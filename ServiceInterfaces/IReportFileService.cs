// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IReportFileService.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   Report File interface
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ServiceInterfaces
{
  using System;
  using System.Collections.Generic;

  using Entities.File;

  public interface IReportFileService
  {
    IList<ReportFile> GetReportFiles(String grid, String environment);

    ReportFile GetReportFile(String filePath);
  }
}
