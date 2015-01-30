// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InputFile.cs" company="DST Nexdox">
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
  using System.ComponentModel.DataAnnotations;
  using System.ComponentModel.DataAnnotations.Schema;
  using System.Data.Entity.Spatial;

  [Table("InputFile")]
  public partial class InputFile
  {
    [Key]
    public int FilenameID { get; set; }

    [StringLength(250)]
    public string Filename { get; set; }

    [StringLength(250)]
    public string FilePath { get; set; }

    public DateTime? ReceivedDate { get; set; }

    public int? JobID { get; set; }

    public int? ManCoDocID { get; set; }

    public int? FileTypeID { get; set; }

    public virtual FileType FileType { get; set; }

    public virtual Job Job { get; set; }

    public virtual ManCoDoc ManCoDoc { get; set; }
  }
}
