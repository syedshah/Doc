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
  using System.IO;
  using System.Text;

  using SystemFileAdapter;

  using Entities.File;

  using FileRepository.Interfaces;

  using FileSystemInterfaces;

  public class OneStepFileRepository : BaseFileRepository<OneStepLog>, IOneStepFileRepository
  {
    public OneStepFileRepository(String path, IFileInfoFactory fileInfo, IDirectoryInfo directoryInfo)
      : base(path, fileInfo, directoryInfo)
    {   
    }

    public OneStepLog GetOneStepLog(String logPath)
    {
      var logFileName = this.GenerateFileName(logPath);

      String[] content = new String[] { };

      if (File.Exists(logFileName))
      {
        content = File.ReadAllLines(logFileName);
      }

      var log = new OneStepLog();
      log.LogContent = content;
      log.LogFileName = logFileName;

      return log;
    }

    protected override String GenerateFileName()
    {
      return string.Format("{0}.log", this.Path);
    }

    protected override OneStepLog InstanceOfClass(String fileName, String filePath)
    {
      return new OneStepLog();
    }

    private String GenerateFileName(String logPath)
    {
      return string.Format("{0}.log", logPath);
    }
  }
}
