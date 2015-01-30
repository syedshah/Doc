// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IApplicationUserRepository.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DocProcessingRepository.Interfaces
{
  using System;

  using Entities;

  public interface IApplicationUserRepository
  {
    PagedResult<ApplicationUser> GetUsers(Int32 pageNumber, Int32 numberOfItems, String environment);

    PagedResult<ApplicationUser> GetUsers(Int32 pageNumber, Int32 numberOfItems, String searchCriteria, String environment);

    Boolean IsLockedOut(String userId);

    void UpdateUserlastLogindate(String userId);

    ApplicationUser GetUserByName(String userName);

    ApplicationUser UpdateUserFailedLogin(String userId);

    void DeactivateUser(String userId);

    void UpdateUserLastPasswordChangedDate(String userId);
  }
}
