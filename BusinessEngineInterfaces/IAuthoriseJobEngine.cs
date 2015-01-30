// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAuthoriseJobEngine.cs" company="DST Nexdox">
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

  public interface IAuthoriseJobEngine
  {
    void AuthoriseJob(IList<Int32> jobIds, String jobStatusType, String userId, String comment);
  }
}
