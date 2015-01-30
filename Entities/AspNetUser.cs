namespace Entities
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel.DataAnnotations;
  using System.ComponentModel.DataAnnotations.Schema;
  using System.Data.Entity.Spatial;

  public partial class AspNetUser
  {
    public AspNetUser()
    {
      ApprovalHistories = new HashSet<ApprovalHistory>();
      AspNetUserClaims = new HashSet<AspNetUserClaim>();
      AspNetUserLogins = new HashSet<AspNetUserLogin>();
      PasswordHistories = new HashSet<PasswordHistory>();
      Jobs = new HashSet<Job>();
      JobSubmissions = new HashSet<JobSubmission>();
      MediaDefinitions = new HashSet<MediaDefinition>();
      UserGroups = new HashSet<UserGroup>();
      AspNetRoles = new HashSet<AspNetRole>();
    }

    [Key]
    public String Id { get; set; }

    [StringLength(256)]
    public string UserName { get; set; }

    public string PasswordHash { get; set; }

    public string SecurityStamp { get; set; }

    [StringLength(256)]
    public string FirstName { get; set; }

    [StringLength(256)]
    public string LastName { get; set; }

    [StringLength(256)]
    public string Title { get; set; }

    [StringLength(256)]
    public string Email { get; set; }

    public string Comment { get; set; }

    public DateTime? LastLoginDate { get; set; }

    public DateTime? LastPasswordChangedDate { get; set; }

    public bool? IsApproved { get; set; }

    public bool? IsLockedOut { get; set; }

    public int? FailedLogInCount { get; set; }

    [Required]
    [StringLength(128)]
    public string Discriminator { get; set; }

    [StringLength(250)]
    public string PreferredLandingPage { get; set; }

    [StringLength(250)]
    public string PreferredEnvironment { get; set; }

    public virtual ICollection<ApprovalHistory> ApprovalHistories { get; set; }

    public virtual ICollection<AspNetUserClaim> AspNetUserClaims { get; set; }

    public virtual ICollection<AspNetUserLogin> AspNetUserLogins { get; set; }

    public virtual ICollection<PasswordHistory> PasswordHistories { get; set; }

    public virtual ICollection<Job> Jobs { get; set; }

    public virtual ICollection<JobSubmission> JobSubmissions { get; set; }

    public virtual ICollection<MediaDefinition> MediaDefinitions { get; set; }

    public virtual ICollection<UserGroup> UserGroups { get; set; }

    public virtual ICollection<AspNetRole> AspNetRoles { get; set; }
  }
}
