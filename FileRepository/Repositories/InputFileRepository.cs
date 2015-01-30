// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OneStepFileRepository.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   Defines the OneStepFileRepository type.
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

  public class InputFileRepository : BaseFileRepository<InputFileInfo>, IInputFileRepository
  {
    public InputFileRepository(String path, IFileInfoFactory fileInfo, IDirectoryInfo directoryInfo)
      : base(path, fileInfo, directoryInfo)
    {   
    }

    public InputFileInfo GetInputFileInfo(String path)
    {
      var inputFileInfo = new InputFileInfo();
      var files = Directory.GetFiles(path).Select(System.IO.Path.GetFileName).Where(s => !s.ToLower().EndsWith(".additionalinfo")).ToList();
      var subDirectories = (from s in Directory.GetDirectories(path, "*", SearchOption.AllDirectories)
                           select s).ToList();

      inputFileInfo.Files = files;
      inputFileInfo.Folders = subDirectories;
      inputFileInfo.Folder = path;

      return inputFileInfo;
    }

    public AdditionalInfoFile GetAdditionalInfo(String path, List<String> files)
    {
      var additionalInfoFile = new AdditionalInfoFile();

      additionalInfoFile.AdditionalFileContent = String.Empty;
      Int32 warr = 0;

      foreach (var file in files)
      {
        var additionalFile = Directory.GetFiles(path).Select(System.IO.Path.GetFileName).Where(s => s.ToLower().Contains(file.ToLower() + ".additionalinfo")).FirstOrDefault();

        if (additionalFile != null)
        {
          String fileData = System.IO.File.ReadAllText(path + "\\" + additionalFile);

            Int32.TryParse(
              fileData.Split().GetValue(Array.IndexOf(fileData.ToLower().Split(), "warrants") - 1).ToString(), out warr);

            additionalInfoFile.Warrants += warr;

            String[] fundInfo = fileData.Split(',');
            additionalInfoFile.FundInfo = fundInfo[0];
        }
      }
      if (additionalInfoFile.Warrants > 0)
      {
        additionalInfoFile.AdditionalFileContent = String.Format("{0}, {1} Warrants", additionalInfoFile.FundInfo, additionalInfoFile.Warrants);
      }

      return additionalInfoFile;
    }

    protected override String GenerateFileName()
    {
      return string.Format("{0}.log", this.Path);
    }

    protected override InputFileInfo InstanceOfClass(String fileName, String filePath)
    {
      return new InputFileInfo();
    }
  }
}
