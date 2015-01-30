// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDirectoryInfo.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   Defines the IDirectoryInfo type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FileSystemInterfaces
{
  using System;
  using System.Collections.Generic;
  using System.IO;

  public interface IDirectoryInfo
  {
    IEnumerable<FileInfo> EnumerateFiles(String path, String filter);
  }
}

