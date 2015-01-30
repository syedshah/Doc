namespace Entities
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel.DataAnnotations;
  using System.ComponentModel.DataAnnotations.Schema;
  using System.Data.Entity.Spatial;

  [Table("MediaDefinition")]
  public partial class MediaDefinition
  {
    public MediaDefinition()
    {
      EnclosingJobs = new HashSet<EnclosingJob>();
      MediaStocks = new HashSet<MediaStock>();
    }

    [Key]
    public int MediaDefID { get; set; }

    [Required]
    [StringLength(75)]
    public string QueueName { get; set; }

    [StringLength(75)]
    public string QueueNameSymbollic { get; set; }

    public int ManCoDocID { get; set; }

    public int? IndexDefinitionID { get; set; }

    [StringLength(100)]
    public string DefinitionName { get; set; }

    public bool Hold { get; set; }

    public int? BatchSizeSheets { get; set; }

    public int MailsortSettingID { get; set; }

    public int FinishingDefID { get; set; }

    public int? SortID { get; set; }

    public bool? PDF_Only { get; set; }

    public DateTime? createDate { get; set; }

    [StringLength(128)]
    public string createUserID { get; set; }

    public virtual ApplicationUser AspNetUser { get; set; }

    public virtual ICollection<EnclosingJob> EnclosingJobs { get; set; }

    public virtual Finishing Finishing { get; set; }

    public virtual MailsortSetting MailsortSetting { get; set; }

    public virtual ManCoDoc ManCoDoc { get; set; }

    public virtual ICollection<MediaStock> MediaStocks { get; set; }
  }
}
