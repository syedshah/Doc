// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IUserService.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ServiceInterfaces
{
  using System;

  using Entities;

  public interface IUserService
  {
    void CreateUser(String userName, String password, String firstName, String lastName, String email);

    PagedResult<ApplicationUser> GetUsers(Int32 pageNumber, Int32 numberOfItems, String environment);

    PagedResult<ApplicationUser> GetUsers(Int32 pageNumber, Int32 numberOfItems, String searchCriteria, String environment);

    ApplicationUser GetApplicationUser();

    ApplicationUser GetApplicationUserById(String userId);

    ApplicationUser GetApplicationUser(String userName);

    ApplicationUser GetApplicationUser(String userName, String password);

    Boolean CheckForPassRenewal(DateTime passwordLastChanged, DateTime lastLogin);

    void SignIn(ApplicationUser user, Boolean isPersistent);

    void UpdateUserLastLogindate(String userId);

    void UpdateUserFailedLogin(String userId);

    Boolean IsLockedOut(String userId);

    void ChangePassword(String userId, String currentPassword, String newPassword);

    void SignOut();

    void Updateuser(String userName, String password, String firstName, String lastName, String email, String preferedLandingPage, String preferedEnvironment, String phone, Boolean isLockedOut, Int32 failedLogInCount,bool isDeActivated);
  }
}
