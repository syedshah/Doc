// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAuthoriseHistoryService.cs" company="DST Nexdox">
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

  public interface IAuthoriseHistoryService
  {
    void InsertAuthorisation(Int32 jobId, String userId, String comment);

    void InsertAuthorisations(IList<Int32> jobIds, String userId, String comment);
  }
}
