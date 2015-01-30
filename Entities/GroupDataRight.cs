namespace Entities
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel.DataAnnotations;
  using System.ComponentModel.DataAnnotations.Schema;
  using System.Data.Entity.Spatial;

  public partial class GroupDataRight
  {
    [Key]
    public int GroupDR_ID { get; set; }

    public int GroupID { get; set; }

    public int ManCoDocID { get; set; }

    public virtual Group Group { get; set; }

    public virtual ManCoDoc ManCoDoc { get; set; }

    public virtual UserGroup UserGroup { get; set; }

    public AppEnvironment AppEnvironment { get; set; }
  }
}
