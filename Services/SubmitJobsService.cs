// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SubmitJobsService.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   Submit jobs service
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Services
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using BusinessEngineInterfaces;
  using Entities;
  using Entities.File;
  using Exceptions;
  using FileRepository.Interfaces;
  using ServiceInterfaces;

  public class SubmitJobsService : ISubmitJobsService
  {
    private readonly IEnvironmentTypeService environmentTypeService;

    private readonly IFilePathBuilderEngine filePathBuilderEngine;

    private readonly IInputFileRepository inputFileRepository;

    public SubmitJobsService(
      IEnvironmentTypeService environmentTypeService,
      IFilePathBuilderEngine filePathBuilderEngine,
      IInputFileRepository inputFileRepository)
    {
      this.environmentTypeService = environmentTypeService;
      this.filePathBuilderEngine = filePathBuilderEngine;
      this.inputFileRepository = inputFileRepository;
    }

    public InputFileInfo GetInputFiles(String environment, String manCo, String docType)
    {
      IList<EnvironmentServerEntity> enviroments;

      enviroments = this.environmentTypeService.GetEnvironmentServers(environment);

      if (enviroments.Count > 1)
      {
        throw new DocProcessingException(
          string.Format("More than one processing enviroment available for {0}", environment));
      }

      var path = this.filePathBuilderEngine.BuildInputFileLocationPath(enviroments.Single().ServerName, manCo, docType);

      try
      {
        return this.inputFileRepository.GetInputFileInfo(path);
      }
      catch (Exception e)
      {
        throw new DocProcessingException("Unable to get input files", e);
      }
    }

    public InputFileInfo GetInputFiles(String path, String environment, String manCo, String docType)
    {
      try
      {
        var fileInfo = this.inputFileRepository.GetInputFileInfo(path);

        IList<EnvironmentServerEntity> enviroments;

        enviroments = this.environmentTypeService.GetEnvironmentServers(environment);

        if (enviroments.Count > 1)
        {
          throw new DocProcessingException(
            string.Format("More than one processing enviroment available for {0}", environment));
        }

        var rootPath = this.filePathBuilderEngine.BuildInputFileLocationPath(enviroments.Single().ServerName, manCo, docType);

        var parentFileInfo = this.inputFileRepository.GetInputFileInfo(rootPath);

        fileInfo.Folders = parentFileInfo.Folders;
        fileInfo.Folder = parentFileInfo.Folder;
        return fileInfo;
      }
      catch (Exception e)
      {
        throw new DocProcessingException("Unable to get input files", e);
      }
    }

    public AdditionalInfoFile GetAdditionalInfo(String path, List<String> fileNames)
    {
      try
      {
        return this.inputFileRepository.GetAdditionalInfo(path, fileNames);
      }
      catch (Exception e)
      {
        throw new DocProcessingException("Unable to get additional info", e);
      }
    }
  }
}
