namespace Entities
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel.DataAnnotations;
  using System.ComponentModel.DataAnnotations.Schema;
  using System.Data.Entity.Spatial;

  [Table("StockType")]
  public partial class StockType
  {
    public StockType()
    {
      Stocks = new HashSet<Stock>();
    }

    public int StockTypeID { get; set; }

    [Column("StockType")]
    [Required]
    [StringLength(50)]
    public string StockType1 { get; set; }

    public virtual ICollection<Stock> Stocks { get; set; }
  }
}
