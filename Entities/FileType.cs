namespace Entities
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel.DataAnnotations;
  using System.ComponentModel.DataAnnotations.Schema;
  using System.Data.Entity.Spatial;

  [Table("FileType")]
  public partial class FileType
  {
    public FileType()
    {
      InputFiles = new HashSet<InputFile>();
    }

    public Int32 FileTypeID { get; set; }

    [Column("FileType")]
    [StringLength(75)]
    public String FileType1 { get; set; }

    public virtual ICollection<InputFile> InputFiles { get; set; }
  }
}
