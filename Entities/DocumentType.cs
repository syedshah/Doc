// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DocumentType.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   DocumentType object 
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Entities
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel.DataAnnotations;
  using System.ComponentModel.DataAnnotations.Schema;
  using System.Data.Entity.Spatial;

  [Table("DocumentType")]
  public partial class DocumentType
  {
    public DocumentType()
    {
      ManCoDocs = new HashSet<ManCoDoc>();
    }

    public Int32 DocumentTypeID { get; set; }

    public Int32 BravuraDocTypeID { get; set; }

    [Required]
    [StringLength(50)]
    public String BravuraDocTypeCode { get; set; }

    [Required]
    [StringLength(75)]
    public String DocumentTypeName { get; set; }

    public Boolean? AdditionalSetup { get; set; }

    public Boolean? AgentDocument { get; set; }

    public virtual ICollection<ManCoDoc> ManCoDocs { get; set; }
  }
}
