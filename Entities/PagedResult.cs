// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PagedResult.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Entities
{
  using System;
  using System.Collections.Generic;

  public class PagedResult<T>
  {
    public IList<T> Results { get; set; }

    public Int32 TotalItems { get; set; }

    public Int32 ItemsPerPage { get; set; }

    public Int32 CurrentPage { get; set; }

    public Int32 StartRow { get; set; }

    public Int32 EndRow { get; set; }

    public Int32 TotalPages
    {
      get
      {
        return (Int32)Math.Ceiling((Decimal)this.TotalItems / this.ItemsPerPage);
      }
    }
  }
}
