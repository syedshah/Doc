// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PdfFileEngine.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   Pdf File Engine object
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace BusinessEngines
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  using BusinessEngineInterfaces;

  using Entities.File;

  using Exceptions;

  using FileRepository.Interfaces;

  using ServiceInterfaces;

  public class PdfFileEngine : IPdfFileEngine
  {
    private readonly IPdfFileRepository pdfFileRepository;

    private readonly IFilePathBuilderEngine filePathBuilderEngine;

    private readonly IEnvironmentTypeService environmentTypeService;

    public PdfFileEngine(
       IPdfFileRepository pdfFileRepository,
      IFilePathBuilderEngine filePathBuilderEngine,
      IEnvironmentTypeService environmentTypeService)
    {
      this.pdfFileRepository = pdfFileRepository;
      this.filePathBuilderEngine = filePathBuilderEngine;
      this.environmentTypeService = environmentTypeService;
    }

    public IList<PdfFile> GetPdfFiles(String grid, String environment)
    {
      IList<PdfFile> pdfFiles = new List<PdfFile>();

      var serverName = this.GetEnvironmentServer(environment);
      var pdfInformationFile = this.GetPdfInformationFilePath(serverName, grid);

      pdfFiles = this.pdfFileRepository.GetPdfFiles(pdfInformationFile);

      return pdfFiles;
    }

    private String GetEnvironmentServer(String environment)
    {
      var servers = this.environmentTypeService.GetEnvironmentServers(environment);

      if (servers.Count > 1)
      {
        throw new DocProcessingException(String.Format("More than one processing enviroment available for {0}", environment));
      }

      return servers.Single().ServerName;
    }

    private String GetPdfInformationFilePath(String serverName, String grid)
    {
      var path = this.filePathBuilderEngine.BuildOneStepOutputDirectory(serverName, grid);
      path = String.Format(path + "\\{0}-PDFFiles.web", grid);
      return path;
    }
  }
}
