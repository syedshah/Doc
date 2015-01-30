namespace Entities
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel.DataAnnotations;
  using System.ComponentModel.DataAnnotations.Schema;
  using System.Data.Entity.Spatial;

  [Table("MaterialConsumption")]
  public partial class MaterialConsumption
  {
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int MaterialConsumptionID { get; set; }

    public int EnclosingID { get; set; }

    public int MediaStockId { get; set; }

    public int? Qty { get; set; }

    public virtual EnclosingJob EnclosingJob { get; set; }

    public virtual MediaStock MediaStock { get; set; }
  }
}
