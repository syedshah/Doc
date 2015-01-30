// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PagingInfoViewModel.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DocProcessingWorkflow.Models.Helper
{
  using System;

  public class PagingInfoViewModel
  {
    public Int32 TotalItems { get; set; }

    public Int32 ItemsPerPage { get; set; }

    public Int32 CurrentPage { get; set; }

    public Int32 TotalPages { get; set; }

    public Int32 StartRow { get; set; }

    public Int32 EndRow { get; set; }
  }
}