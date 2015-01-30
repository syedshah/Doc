// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ManCoViewModel.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   View model for mancos
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DocProcessingWorkflow.Models.ManCo
{
  using System;
  using System.ComponentModel.DataAnnotations;

  public class ManCoViewModel
  {
    public ManCoViewModel(Entities.ManagementCompany manCo)
    {
      this.Id = manCo.ManCoID;
      this.Code = manCo.ManCoCode;
      this.Description = manCo.ManCoName;
    }

    public Int32 Id { get; set; }

    [Required]
    public String Code { get; set; }

    [Required]
    public String Description { get; set; }
  }
}