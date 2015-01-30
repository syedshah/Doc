// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserManagerProvider.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   Wrapper for identity
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace IdentityWrapper.Identity
{
  using System;
  using System.Collections.Generic;
  using System.Data.Entity;
  using DocProcessingRepository.Contexts;
  using Entities;
  using IdentityWrapper.Interfaces;
  using Microsoft.AspNet.Identity;
  using Microsoft.AspNet.Identity.EntityFramework;

  public class UserManagerProvider : IUserManagerProvider, IDisposable
  {
    public UserManagerProvider(String connectionString)
    {
      this._dataDbContext = new IdentityDb(connectionString);
      this.UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(this._dataDbContext));
      this.conString = connectionString;
    }

    public DbContext dataDbContext
    {
      get
      {
        return this._dataDbContext;
      }
      set
      {
        this._dataDbContext = value;
      }
    }

    public UserManager<ApplicationUser> UserManager { get; set; }

    public void Dispose()
    {
      this.UserManager.Dispose();
      this.UserManager = null;
    }

    public System.Security.Claims.ClaimsIdentity CreateIdentity(ApplicationUser user, String authenticationType)
    {
      return this.UserManager.CreateIdentity(user, authenticationType);
    }

    public IdentityResult CreateUser(ApplicationUser user, String password)
    {
      var result = new IdentityResult();

      using (var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new IdentityDb(this.conString))))
      {
        result = userManager.Create(user, password);
      }

      return result;
    }

    public ApplicationUser FindByName(String userName)
    {
      return this.UserManager.FindByName(userName);
    }

    public ApplicationUser Find(String userName, String password)
    {
      return this.UserManager.Find(userName.Trim(), password.Trim());
    }

    public ApplicationUser FindById(String userId)
    {
      return this.UserManager.FindById(userId);
    }

    public IList<String> GetRoles(String userId)
    {
      return this.UserManager.GetRoles(userId);
    }

    public IdentityResult Create(ApplicationUser user, String password)
    {
      return this.UserManager.Create(user, password);
    }

    public IdentityResult RemoveFromRole(String userId, String role)
    {
      return this.UserManager.RemoveFromRole(userId, role);
    }

    public IdentityResult AddToRole(String userId, String role)
    {
      return this.UserManager.AddToRole(userId, role);
    }

    public IdentityResult Update(ApplicationUser user)
    {
      return this.UserManager.Update(user);
    }

    public IdentityResult RemovePassword(String userId)
    {
      return this.UserManager.RemovePassword(userId);
    }

    public IdentityResult AddPassword(String userId, String password)
    {
      return this.UserManager.AddPassword(userId, password);
    }

    public IdentityResult ChangePassword(String userId, String currentPassword, String newPassword)
    {
      return this.UserManager.ChangePassword(userId, currentPassword, newPassword);
    }

    public String HashPassword(String password)
    {
      return this.UserManager.PasswordHasher.HashPassword(password);
    }


    public PasswordVerificationResult VerifyHashedPassword(String hashedPassword, String providedPassword)
    {
      var verificationResult = this.UserManager.PasswordHasher.VerifyHashedPassword(hashedPassword, providedPassword);
      return verificationResult;
    }

    private DbContext _dataDbContext;

    private readonly String conString;
  }
}
