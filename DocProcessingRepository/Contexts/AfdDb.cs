// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IdentityDb.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DocProcessingRepository.Contexts
{
  using System;
  using System.Data.Entity;
  using Entities.ADF;

  public class AfdDb : DbContext
  {
    public AfdDb(String connectionString)
      : base(connectionString)
    {
    }

    public virtual DbSet<PackStore> PackStores { get; set; }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);
    }
  }
}
