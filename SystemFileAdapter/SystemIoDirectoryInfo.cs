// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SystemIoDirectoryInfo.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   Defines the SystemIoDirectoryInfo type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SystemFileAdapter
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using System.Linq;

  using FileSystemInterfaces;

  public class SystemIoDirectoryInfo : IDirectoryInfo
  {
    public IEnumerable<FileInfo> EnumerateFiles(String path, String filter)
    {
      var di = new DirectoryInfo(path);
      FileInfo[] inputFiles = di.GetFiles();

      return inputFiles.Where(f => filter == "*.*" || f.Extension == filter);
    }
  }
}
