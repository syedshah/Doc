namespace Entities
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel.DataAnnotations;
  using System.ComponentModel.DataAnnotations.Schema;
  using System.Data.Entity.Spatial;

  [Table("MailsortSetting")]
  public partial class MailsortSetting
  {
    public MailsortSetting()
    {
      IndexDefinitions = new HashSet<IndexDefinition>();
      MediaDefinitions = new HashSet<MediaDefinition>();
    }

    public int MailsortSettingID { get; set; }

    [StringLength(50)]
    public string TNTDepot { get; set; }

    [StringLength(50)]
    public string TNTMailingHouse { get; set; }

    [StringLength(50)]
    public string TNTClientCode { get; set; }

    [StringLength(50)]
    public string TNTJobReference { get; set; }

    [StringLength(50)]
    public string nexdoxMailsortService { get; set; }

    public bool? largeletter { get; set; }

    public int? FixedWeight_NoOuter { get; set; }

    public virtual ICollection<IndexDefinition> IndexDefinitions { get; set; }

    public virtual ICollection<MediaDefinition> MediaDefinitions { get; set; }
  }
}
