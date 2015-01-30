// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SystemFileInfoFactory.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   Defines the SystemFileInfoFactory type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SystemFileAdapter
{
  using System;

  using FileSystemInterfaces;

  public class SystemFileInfoFactory : IFileInfoFactory
  {
    public IFileInfo CreateFileInfo(String filename)
    {
      return new SystemIoFileInfo(filename);
    }
  }
}

