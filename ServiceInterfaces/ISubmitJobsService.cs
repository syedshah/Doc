// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISubmitJobsService.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ServiceInterfaces
{
  using System;
  using System.Collections.Generic;
  using Entities.File;

  public interface ISubmitJobsService
  {
    InputFileInfo GetInputFiles(String environment, String manCo, String docType);

    InputFileInfo GetInputFiles(String path, String environment, String manCo, String docType);

    AdditionalInfoFile GetAdditionalInfo(String path, List<String> fileNames);
  }
}
