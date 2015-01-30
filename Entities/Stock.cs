namespace Entities
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel.DataAnnotations;
  using System.ComponentModel.DataAnnotations.Schema;
  using System.Data.Entity.Spatial;

  [Table("Stock")]
  public partial class Stock
  {
    public Stock()
    {
      MediaStocks = new HashSet<MediaStock>();
    }

    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int StockID { get; set; }

    [Required]
    [StringLength(50)]
    public string Code { get; set; }

    [Required]
    [StringLength(255)]
    public string Description { get; set; }

    public int? Weight { get; set; }

    public int? SizeX { get; set; }

    public int? SizeY { get; set; }

    [StringLength(1)]
    public string Facing { get; set; }

    public int? Division { get; set; }

    public int? ReorderLevel { get; set; }

    public int StockTypeID { get; set; }

    [StringLength(50)]
    public string User1 { get; set; }

    [StringLength(50)]
    public string User2 { get; set; }

    [StringLength(50)]
    public string User3 { get; set; }

    public double? Overs { get; set; }

    public int? MakeReady { get; set; }

    public DateTime? DateCreated { get; set; }

    [StringLength(150)]
    public string ChangedBy { get; set; }

    public virtual ICollection<MediaStock> MediaStocks { get; set; }

    public virtual StockType StockType { get; set; }
  }
}
