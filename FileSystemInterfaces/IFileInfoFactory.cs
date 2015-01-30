// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IFileInfoFactory.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   Defines the IFileInfoFactory type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;

namespace FileSystemInterfaces
{
    public interface IFileInfoFactory
  {
    IFileInfo CreateFileInfo(String filename);
  }
}
