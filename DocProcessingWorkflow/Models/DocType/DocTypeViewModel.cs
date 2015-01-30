// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DocTypeViewModel.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DocProcessingWorkflow.Models.DocType
{
  using System;
  using System.ComponentModel.DataAnnotations;

  public class DocTypeViewModel
  {
    public DocTypeViewModel()
    {
    }

    public DocTypeViewModel(Entities.DocumentType docType)
    {
      this.Id = docType.DocumentTypeID;
      this.Code = docType.BravuraDocTypeCode;
      this.Description = docType.DocumentTypeName;
    }

    public Int32 Id { get; set; }

    [Required]
    public String Code { get; set; }

    [Required]
    public String Description { get; set; }
  }
}