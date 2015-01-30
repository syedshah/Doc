// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InputFileInfo.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Entities.File
{
  using System;
  using System.Collections.Generic;

  public class InputFileInfo
  {
    public IList<String> Files { get; set; }

    public String Folder { get; set; }

    public IList<String> Folders { get; set; }
  }
}
