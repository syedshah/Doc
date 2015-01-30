// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IUserManagerProvider.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   Interface for identity wrapper
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace IdentityWrapper.Interfaces
{
  using System;
  using System.Collections.Generic;
  using System.Data.Entity;
  using Entities;
  using Microsoft.AspNet.Identity;

  public interface IUserManagerProvider
  {
    UserManager<ApplicationUser> UserManager { get; set; }

    DbContext dataDbContext { get; set; }

    System.Security.Claims.ClaimsIdentity CreateIdentity(ApplicationUser user, String authenticationType);

    IdentityResult CreateUser(ApplicationUser user, String password);

    ApplicationUser FindByName(String userName);

    ApplicationUser Find(String userName, String password);

    ApplicationUser FindById(String userId);

    IList<String> GetRoles(String userId);

    IdentityResult Create(ApplicationUser user, String password);

    IdentityResult Update(ApplicationUser user);

    IdentityResult RemoveFromRole(String userId, String role);

    IdentityResult AddToRole(String userId, String role);

    IdentityResult RemovePassword(String userId);

    IdentityResult AddPassword(String userId, String password);

    IdentityResult ChangePassword(String userId, String currentPassword, String newPassword);

    String HashPassword(String password);

    PasswordVerificationResult VerifyHashedPassword(String hashedPassword, String providedPassword);
  }
}
