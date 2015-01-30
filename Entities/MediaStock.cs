namespace Entities
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel.DataAnnotations;
  using System.ComponentModel.DataAnnotations.Schema;
  using System.Data.Entity.Spatial;

  [Table("MediaStock")]
  public partial class MediaStock
  {
    public MediaStock()
    {
      MaterialConsumptions = new HashSet<MaterialConsumption>();
    }

    public int MediaStockID { get; set; }

    public int StockID { get; set; }

    public int MediaDefID { get; set; }

    public DateTime PublishDate { get; set; }

    public int StationNumber { get; set; }

    public virtual ICollection<MaterialConsumption> MaterialConsumptions { get; set; }

    public virtual MediaDefinition MediaDefinition { get; set; }

    public virtual Stock Stock { get; set; }
  }
}
