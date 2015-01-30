// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApplicationUserRepository.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DocProcessingRepository.Repositories
{
  using System;
  using System.Linq;
  using DocProcessingRepository.Contexts;
  using DocProcessingRepository.Interfaces;
  using EFRepository;
  using Entities;

  using Exceptions;

  public class ApplicationUserRepository : BaseEfRepository<ApplicationUser>, IApplicationUserRepository
  {
    public ApplicationUserRepository(String connectionString)
      : base(new IdentityDb(connectionString))
    {
    }

    public PagedResult<ApplicationUser> GetUsers(Int32 pageNumber, Int32 numberOfItems, String environment)
    {
        var users =
                this.GetEntityListByStoreProcedure<ApplicationUser>(
                    String.Format("exec GetUsersByEnvironment '{0}'", environment)).ToList();

      return new PagedResult<ApplicationUser>
      {
        CurrentPage = pageNumber,
        ItemsPerPage = numberOfItems,
        TotalItems = users.Count(),
        Results = users.OrderBy(c => c.UserName)
        .Skip((pageNumber - 1) * numberOfItems)
        .Take(numberOfItems)
        .ToList(),
        StartRow = ((pageNumber - 1) * numberOfItems) + 1,
        EndRow = (((pageNumber - 1) * numberOfItems) + 1) + (numberOfItems - 1)
      };
    }

    public Boolean IsLockedOut(String userId)
    {
      var user = (from a in Entities where a.Id == userId select a).FirstOrDefault();

      if (user == null)
      {
        throw new DocProcessingException("user id not valid");
      }

      return user.IsLockedOut;
    }

    public void UpdateUserlastLogindate(String userId)
    {
      var user = (from a in Entities where a.Id == userId select a).FirstOrDefault();

      if (user == null)
      {
        throw new DocProcessingException("user id not valid");
      }

      user.LastLoginDate = DateTime.Now;
      user.FailedLogInCount = 0;
      user.IsLockedOut = false;

      this.Update(user);
    }

    public ApplicationUser GetUserByName(String userName)
    {
      return (from a in Entities
              where a.UserName == userName
              select a)
        .FirstOrDefault();
    }

    public ApplicationUser UpdateUserFailedLogin(String userId)
    {
      var user = (from a in Entities where a.Id == userId select a).FirstOrDefault();

      if (user == null)
      {
        throw new DocProcessingException("user id not valid");
      }

      user.FailedLogInCount++;

      this.Update(user);
      return user;
    }

    public void DeactivateUser(String userId)
    {
      var user = (from a in Entities where a.Id == userId select a).FirstOrDefault();

      if (user == null)
      {
        throw new DocProcessingException("user id not valid");
      }

      user.IsLockedOut = true;
      this.Update(user);
    }

    public void UpdateUserLastPasswordChangedDate(String userId)
    {
      var user = (from a in Entities where a.Id == userId select a).FirstOrDefault();

      if (user == null)
      {
        throw new DocProcessingException("user id not valid");
      }

      user.LastPasswordChangedDate = DateTime.Now;

      this.Update(user);
    }

      public PagedResult<ApplicationUser> GetUsers(Int32 pageNumber, Int32 numberOfItems, String searchCriteria,
           String environment)
      {
          var users =
              this.GetEntityListByStoreProcedure<ApplicationUser>(
                  String.Format("exec GetUsersBySearchCriteria '{0}', '{1}'", searchCriteria, environment)).ToList();

          return new PagedResult<ApplicationUser>
          {
              CurrentPage = pageNumber,
              ItemsPerPage = numberOfItems,
              TotalItems = users.Count(),
              Results = users
              .Skip((pageNumber - 1) * numberOfItems)
              .Take(numberOfItems)
              .ToList(),
              StartRow = ((pageNumber - 1) * numberOfItems) + 1,
              EndRow = (((pageNumber - 1) * numberOfItems) + 1) + (numberOfItems - 1)
          };
      }
  }
}
