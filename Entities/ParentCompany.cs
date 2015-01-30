namespace Entities
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel.DataAnnotations;
  using System.ComponentModel.DataAnnotations.Schema;
  using System.Data.Entity.Spatial;

  [Table("ParentCompany")]
  public partial class ParentCompany
  {
    public ParentCompany()
    {
      ManagementCompanies = new HashSet<ManagementCompany>();
    }

    public Int32 ParentCompanyID { get; set; }

    [Required]
    [StringLength(50)]
    public String ParentCompany_Name { get; set; }

    public virtual ICollection<ManagementCompany> ManagementCompanies { get; set; }
  }
}
