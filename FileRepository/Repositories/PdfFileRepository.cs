// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PdfFileRepository.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   Defines the PdfFileRepository type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FileRepository.Repositories
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using System.Linq;

  using SystemFileAdapter;

  using Entities.File;

  using FileRepository.Interfaces;

  using FileSystemInterfaces;

  public class PdfFileRepository : BaseFileRepository<PdfFile>, IPdfFileRepository
  {
    public PdfFileRepository(String path, IFileInfoFactory fileInfo, IDirectoryInfo directoryInfo)
      : base(path, fileInfo, directoryInfo)
    {
    }

    public IList<PdfFile> GetPdfFiles(String filePath, String extension)
    {
      return this.GetFiles(filePath, extension);
    }

    public IList<PdfFile> GetPdfFiles(String pdfInformationFile)
    {
      return this.GetAllPdfFiles(pdfInformationFile);
    }

    public PdfFile GetPdfFile(String pdfFilePath)
    {
      var pdfFile = new PdfFile();

      var filePath = pdfFilePath.Split(',')[1].Replace("\"", String.Empty);

      //if (File.Exists(pdfFilePath))
      //{
        pdfFile.PdfFilePath = filePath;
        pdfFile.PdfFileName = pdfFilePath.Split(',')[0].Replace("\"", String.Empty);
      //}

      return pdfFile;
    }

    private IList<PdfFile> GetAllPdfFiles(String pdfInformationFile)
    {  
      IList<PdfFile> pdfFiles = new List<PdfFile>();

      var pdfFilePaths = this.GetReportFileContent(pdfInformationFile);

      foreach (var pdfFilePath in pdfFilePaths)
      {
        pdfFiles.Add(this.GetPdfFile(pdfFilePath));
      }

      return pdfFiles;
    }

    private IList<String> GetReportFileContent(String pdfInformationFile)
    {
      IList<String> pdfFileNames = new List<String>();

      if (File.Exists(pdfInformationFile))
      {
        pdfFileNames = File.ReadAllLines(pdfInformationFile).ToList();
      }

      return pdfFileNames;
    }

    protected override PdfFile InstanceOfClass(String fileName, String filePath)
    {
      var pdfFile = new PdfFile(fileName, filePath);

      return pdfFile;
    }

    protected override String GenerateFileName()
    {
      return String.Format("{0}.pdf", this.Path);
    }

    private String GetFileName(String filePath)
    {
      var filePathArray = filePath.Split(Convert.ToChar("\\"));
      var fileName = filePathArray[filePathArray.Length - 1];

      return fileName;
    }
  }
}
