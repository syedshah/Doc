// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AppDb.cs" company="DST Nexdox">
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

  using Entities;

  public partial class AppDb : DbContext
  {
    public AppDb(String connectionString)
      : base(connectionString)
    {
    }

    public virtual DbSet<ApprovalHistory> ApprovalHistories { get; set; }

    public virtual DbSet<DocumentType> DocumentTypes { get; set; }

    public virtual DbSet<DocumentVersion> DocumentVersions { get; set; }

    public virtual DbSet<EnclosingJob> EnclosingJobs { get; set; }

    public virtual DbSet<Job> Jobs { get; set; }

    public virtual DbSet<JobStatu> JobStatus { get; set; }

    public virtual DbSet<JobStatusType> JobStatusTypes { get; set; }

    public virtual DbSet<JobSubmission> JobSubmissions { get; set; }

    public virtual DbSet<ManagementCompany> ManagementCompanies { get; set; }

    public virtual DbSet<ManCoDoc> ManCoDocs { get; set; }

    public virtual DbSet<ParentCompany> ParentCompanies { get; set; }

    public virtual DbSet<DeploymentHistory> DeploymentHistories { get; set; }

    public virtual DbSet<DirectionType> DirectionTypes { get; set; }

    public virtual DbSet<EnvironmentType> EnvironmentTypes { get; set; }

    public virtual DbSet<FileType> FileTypes { get; set; }

    public virtual DbSet<Finishing> Finishings { get; set; }

    public virtual DbSet<GlobalSetting> GlobalSettings { get; set; }

    public virtual DbSet<Group> Groups { get; set; }

    public virtual DbSet<GroupDataRight> GroupDataRights { get; set; }

    public virtual DbSet<GroupType> GroupTypes { get; set; }

    public virtual DbSet<IndexDefinition> IndexDefinitions { get; set; }

    public virtual DbSet<IndexLayoutFile> IndexLayoutFiles { get; set; }

    public virtual DbSet<InputFile> InputFiles { get; set; }

    public virtual DbSet<MailsortSetting> MailsortSettings { get; set; }

    public virtual DbSet<MaterialConsumption> MaterialConsumptions { get; set; }

    public virtual DbSet<MediaDefinition> MediaDefinitions { get; set; }

    public virtual DbSet<MediaStock> MediaStocks { get; set; }

    public virtual DbSet<Sort> Sorts { get; set; }

    public virtual DbSet<Stock> Stocks { get; set; }

    public virtual DbSet<StockType> StockTypes { get; set; }

    public virtual DbSet<UserGroup> UserGroups { get; set; }

    //public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
    //public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
    //public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
    //public virtual DbSet<AspNetUser> AspNetUsers { get; set; }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
      modelBuilder.Entity<DocumentType>()
          .HasMany(e => e.ManCoDocs)
          .WithRequired(e => e.DocumentType)
          .WillCascadeOnDelete(false);

      modelBuilder.Entity<Job>()
          .HasMany(e => e.EnclosingJobs)
          .WithRequired(e => e.Job)
          .WillCascadeOnDelete(false);

      modelBuilder.Entity<ManagementCompany>()
          .Property(e => e.RufusDatabaseID)
          .IsUnicode(false);

      modelBuilder.Entity<ManagementCompany>()
          .HasMany(e => e.ManCoDocs)
          .WithRequired(e => e.ManagementCompany)
          .WillCascadeOnDelete(false);

      modelBuilder.Entity<ManCoDoc>()
          .HasMany(e => e.Jobs)
          .WithRequired(e => e.ManCoDoc)
          .WillCascadeOnDelete(false);

      modelBuilder.Entity<ManCoDoc>()
          .HasMany(e => e.JobSubmissions)
          .WithRequired(e => e.ManCoDoc)
          .WillCascadeOnDelete(false);

      modelBuilder.Entity<ParentCompany>()
          .HasMany(e => e.ManagementCompanies)
          .WithRequired(e => e.ParentCompany)
          .WillCascadeOnDelete(false);


      //modelBuilder.Entity<AspNetRole>()
      //   .HasMany(e => e.AspNetUsers)
      //   .WithMany(e => e.AspNetRoles)
      //   .Map(m => m.ToTable("AspNetUserRoles").MapLeftKey("RoleId").MapRightKey("UserId"));

      //modelBuilder.Entity<AspNetUser>()
      //    .HasMany(e => e.ApprovalHistories)
      //    .WithRequired(e => e.AspNetUser)
      //    .HasForeignKey(e => e.UserID)
      //    .WillCascadeOnDelete(false);

      //modelBuilder.Entity<AspNetUser>()
      //    .HasMany(e => e.AspNetUserClaims)
      //    .WithRequired(e => e.AspNetUser)
      //    .HasForeignKey(e => e.User_Id);

      //modelBuilder.Entity<AspNetUser>()
      //    .HasMany(e => e.AspNetUserLogins)
      //    .WithRequired(e => e.AspNetUser)
      //    .HasForeignKey(e => e.UserId);

      ////modelBuilder.Entity<AspNetUser>()
      ////    .HasMany(e => e.PasswordHistories)
      ////    .WithOptional(e => e.AspNetUser)
      ////    .HasForeignKey(e => e.UserId);

      //modelBuilder.Entity<AspNetUser>()
      //    .HasMany(e => e.Jobs)
      //    .WithOptional(e => e.AspNetUser)
      //    .HasForeignKey(e => e.UserID);

      //modelBuilder.Entity<AspNetUser>()
      //    .HasMany(e => e.JobSubmissions)
      //    .WithOptional(e => e.AspNetUser)
      //    .HasForeignKey(e => e.UserID);

      //modelBuilder.Entity<AspNetUser>()
      //    .HasMany(e => e.MediaDefinitions)
      //    .WithOptional(e => e.AspNetUser)
      //    .HasForeignKey(e => e.createUserID);

      //modelBuilder.Entity<AspNetUser>()
      //    .HasMany(e => e.UserGroups)
      //    .WithOptional(e => e.AspNetUser)
      //    .HasForeignKey(e => e.UserId);


      modelBuilder.Entity<DirectionType>()
       .HasMany(e => e.Sorts)
       .WithRequired(e => e.DirectionType)
       .WillCascadeOnDelete(false);

      modelBuilder.Entity<DocumentVersion>()
         .HasOptional(e => e.DeploymentHistory)
         .WithRequired(e => e.DocumentVersion);

      modelBuilder.Entity<EnclosingJob>()
          .HasMany(e => e.MaterialConsumptions)
          .WithRequired(e => e.EnclosingJob)
          .HasForeignKey(e => e.EnclosingID)
          .WillCascadeOnDelete(false);

      modelBuilder.Entity<EnvironmentType>()
          .HasMany(e => e.DocumentVersions)
          .WithRequired(e => e.EnvironmentType)
          .HasForeignKey(e => e.EnvntID)
          .WillCascadeOnDelete(false);

      modelBuilder.Entity<EnvironmentType>()
          .HasMany(e => e.ManCoDocs)
          .WithRequired(e => e.EnvironmentType)
          .HasForeignKey(e => e.EnvntID)
          .WillCascadeOnDelete(false);

      modelBuilder.Entity<Finishing>()
          .Property(e => e.Unknown)
          .IsFixedLength();

      modelBuilder.Entity<Finishing>()
          .HasMany(e => e.MediaDefinitions)
          .WithRequired(e => e.Finishing)
          .WillCascadeOnDelete(false);

      modelBuilder.Entity<Group>()
          .HasMany(e => e.GroupDataRights)
          .WithRequired(e => e.Group)
          .WillCascadeOnDelete(false);

      modelBuilder.Entity<GroupType>()
          .HasMany(e => e.Groups)
          .WithRequired(e => e.GroupType)
          .WillCascadeOnDelete(false);

      modelBuilder.Entity<IndexDefinition>()
          .HasMany(e => e.Sorts)
          .WithRequired(e => e.IndexDefinition)
          .WillCascadeOnDelete(false);

      modelBuilder.Entity<IndexLayoutFile>()
          .HasMany(e => e.IndexDefinitions)
          .WithRequired(e => e.IndexLayoutFile)
          .WillCascadeOnDelete(false);

      modelBuilder.Entity<MailsortSetting>()
         .HasMany(e => e.MediaDefinitions)
         .WithRequired(e => e.MailsortSetting)
         .WillCascadeOnDelete(false);


      modelBuilder.Entity<MediaDefinition>()
        .HasMany(e => e.EnclosingJobs)
        .WithRequired(e => e.MediaDefinition)
        .HasForeignKey(e => e.MediDefID)
        .WillCascadeOnDelete(false);

      modelBuilder.Entity<MediaDefinition>()
          .HasMany(e => e.MediaStocks)
          .WithRequired(e => e.MediaDefinition)
          .WillCascadeOnDelete(false);

      modelBuilder.Entity<MediaStock>()
          .HasMany(e => e.MaterialConsumptions)
          .WithRequired(e => e.MediaStock)
          .WillCascadeOnDelete(false);

      modelBuilder.Entity<Stock>()
          .Property(e => e.Code)
          .IsUnicode(false);

      modelBuilder.Entity<Stock>()
          .Property(e => e.Description)
          .IsUnicode(false);

      modelBuilder.Entity<Stock>()
          .Property(e => e.Facing)
          .IsFixedLength()
          .IsUnicode(false);

      modelBuilder.Entity<Stock>()
          .Property(e => e.User1)
          .IsUnicode(false);

      modelBuilder.Entity<Stock>()
          .Property(e => e.User2)
          .IsUnicode(false);

      modelBuilder.Entity<Stock>()
          .Property(e => e.User3)
          .IsUnicode(false);

      modelBuilder.Entity<Stock>()
          .Property(e => e.ChangedBy)
          .IsUnicode(false);

      modelBuilder.Entity<Stock>()
          .HasMany(e => e.MediaStocks)
          .WithRequired(e => e.Stock)
          .WillCascadeOnDelete(false);

      modelBuilder.Entity<StockType>()
          .Property(e => e.StockType1)
          .IsUnicode(false);

      modelBuilder.Entity<StockType>()
          .HasMany(e => e.Stocks)
          .WithRequired(e => e.StockType)
          .WillCascadeOnDelete(false);

      modelBuilder.Entity<UserGroup>()
          .HasMany(e => e.GroupDataRights)
          .WithRequired(e => e.UserGroup)
          .HasForeignKey(e => e.GroupID)
          .WillCascadeOnDelete(false);
       
    }
  }
}
