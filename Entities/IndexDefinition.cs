namespace Entities
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel.DataAnnotations;
  using System.ComponentModel.DataAnnotations.Schema;
  using System.Data.Entity.Spatial;

  [Table("IndexDefinition")]
  public partial class IndexDefinition
  {
    public IndexDefinition()
    {
      Sorts = new HashSet<Sort>();
    }

    public int IndexDefinitionID { get; set; }

    public int ManCoDocID { get; set; }

    [Required]
    [StringLength(150)]
    public string IndexDefinitionName { get; set; }

    [StringLength(150)]
    public string ExstreamSymbolicIndexName { get; set; }

    [StringLength(150)]
    public string ExstreamSymbolicDataName { get; set; }

    public int IndexLayoutID { get; set; }

    public int? MailSortSettingID { get; set; }

    public virtual IndexLayoutFile IndexLayoutFile { get; set; }

    public virtual MailsortSetting MailsortSetting { get; set; }

    public virtual ManCoDoc ManCoDoc { get; set; }

    public virtual ICollection<Sort> Sorts { get; set; }
  }
}
