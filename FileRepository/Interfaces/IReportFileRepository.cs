// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IReportFileRepository.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   Defines the IReportFileRepository type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FileRepository.Interfaces
{
  using System;
  using System.Collections.Generic;

  using Entities.File;

  public interface IReportFileRepository
  {
    IList<ReportFile> GetReportFiles(String rptPath);

    ReportFile GetReportFile(String filePath);
  }
}
