// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReportFileService.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Services
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  using BusinessEngineInterfaces;

  using Entities.File;

  using Exceptions;

  using FileRepository.Interfaces;

  using ServiceInterfaces;

  public class ReportFileService : IReportFileService
  {
    private readonly IReportFileRepository reportFileRepository;

    private readonly IFilePathBuilderEngine filePathBuilderEngine;

    private readonly IEnvironmentTypeService environmentTypeService;

    public ReportFileService(
      IReportFileRepository reportFileRepository,
      IFilePathBuilderEngine filePathBuilderEngine,
      IEnvironmentTypeService environmentTypeService)
    {
      this.reportFileRepository = reportFileRepository;
      this.filePathBuilderEngine = filePathBuilderEngine;
      this.environmentTypeService = environmentTypeService;
    }

    public IList<ReportFile> GetReportFiles(String grid, String environment)
    {
      var servers = this.environmentTypeService.GetEnvironmentServers(environment);

      if (servers.Count > 1)
      {
        throw new DocProcessingException(string.Format("More than one processing enviroment available for {0}", environment));
      }

      try
      {
        var oneStepOutputDirectoryPath = this.filePathBuilderEngine.BuildOneStepOutputDirectory(servers.Single().ServerName, grid);
        return this.reportFileRepository.GetReportFiles(oneStepOutputDirectoryPath);
      }
      catch (Exception e)
      {
        throw new DocProcessingException("Unable to get report files", e);
      }
    }

    public ReportFile GetReportFile(String filePath)
    {
      try
      {
        return this.reportFileRepository.GetReportFile(filePath);
      }
      catch (Exception e)
      {
        throw new DocProcessingException("Unable to get report", e);
      }
    }
  }
}
