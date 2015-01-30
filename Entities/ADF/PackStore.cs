// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PackStore.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Entities.ADF
{
  using System;
  using System.ComponentModel.DataAnnotations;

  public class PackStore
  {
    [Key]
    public Guid PackID { get; set; }

    public String Name1 { get; set; }

    public String Name2 { get; set; }

    public String Address1 { get; set; }

    public String Address2 { get; set; }

    public String Address3 { get; set; }

    public String Address4 { get; set; }

    public String Address5 { get; set; }

    public String Address6 { get; set; }

    public String PostCode { get; set; }

    public Int32 RecepientRef { get; set; }

    public Int32? OwnerRef { get; set; }

    public Boolean? Remake { get; set; }

    public Int32 JobId { get; set; }

    public String InputFile { get; set; }

    public Int32 PackSeqNo { get; set; }

    public Int32? EnclosingJobID { get; set; }

    public Int32 Pages { get; set; }

    public DateTime CreationDate { get; set; }

    public String Pulled_UserId { get; set; }

    public Boolean Pulled { get; set; }

    public Int32? FBE_ID { get; set; }
  }
}
