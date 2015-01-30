// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JobStatusTypeEntity.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   Job Status Type Entity object 
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Entities
{
  using System;

  public class JobStatusTypeEntity
  {
    public Int32 JobStatusTypeID { get; set; }

    public String JobStatusDescription { get; set; }
  }
}
