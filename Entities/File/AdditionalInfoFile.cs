// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AdditionalInfoFile.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Entities.File
{
  using System;

  public class AdditionalInfoFile
  {
    public String AdditionalFileContent { get; set; }

    public Int32 Warrants { get; set; }

    public String FundInfo { get; set; }
  }
}
