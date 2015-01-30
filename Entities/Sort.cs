namespace Entities
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel.DataAnnotations;
  using System.ComponentModel.DataAnnotations.Schema;
  using System.Data.Entity.Spatial;

  [Table("Sort")]
  public partial class Sort
  {
    public int SortID { get; set; }

    [StringLength(75)]
    public string SortIndex { get; set; }

    public int DirectionTypeID { get; set; }

    public int SortOrder { get; set; }

    public int IndexDefinitionID { get; set; }

    public virtual DirectionType DirectionType { get; set; }

    public virtual IndexDefinition IndexDefinition { get; set; }
  }
}
