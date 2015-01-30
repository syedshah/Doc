// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OneStepFileService.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   OneStepFileService object
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Services
{
  using System;
  using System.Linq;

  using BusinessEngineInterfaces;

  using Entities.File;

  using Exceptions;

  using FileRepository.Interfaces;

  using ServiceInterfaces;

  public class OneStepFileService : IOneStepFileService
  {
    private readonly IOneStepFileRepository oneStepFileRepository;

    private readonly IEnvironmentTypeService environmentTypeService;

    private readonly IFilePathBuilderEngine filePathBuilderEngine;

    public OneStepFileService(
      IOneStepFileRepository oneStepFileRepository,
      IEnvironmentTypeService environmentTypeService,
      IFilePathBuilderEngine filePathBuilderEngine)
    {
      this.oneStepFileRepository = oneStepFileRepository;
      this.environmentTypeService = environmentTypeService;
      this.filePathBuilderEngine = filePathBuilderEngine;
    }

    public OneStepLog GetOneStepLog(String grid, String environment)
    {
      var servers = this.environmentTypeService.GetEnvironmentServers(environment);

      if (servers.Count > 1)
      {
        throw new DocProcessingException(string.Format("More than one processing enviroment available for {0}", environment));
      }

      try
      {
        var logPath = this.filePathBuilderEngine.BuildOneStepLogFilePath(
          servers.Single().ServerName, grid);
        return this.oneStepFileRepository.GetOneStepLog(logPath);
      }
      catch (Exception e)
      {
        throw new DocProcessingException("Unable to retrieve one step log", e);
      }
    }
  }
}
