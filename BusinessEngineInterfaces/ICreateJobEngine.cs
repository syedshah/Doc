// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICreateJobEngine.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace BusinessEngineInterfaces
{
  using System;
  using System.Collections.Generic;

  public interface ICreateJobEngine
  {
    void SubmitJob(
      String environment,
      String manCo,
      String docTypeCode,
      String docTypeName,
      List<String> files,
      String userId,
      Boolean allowReprocessing, 
      Boolean requiresAdditionalSetUp,
      String fundInfo, 
      String sortCode, 
      String accountNumber, 
      String chequeNumber,
      String selectedFolder);
  }
}
