// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserService.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Services
{
    using System;
    using DocProcessingRepository.Interfaces;
    using Encryptions;
    using Entities;
    using Exceptions;
    using IdentityWrapper.Interfaces;
    using Microsoft.AspNet.Identity;
    using Microsoft.Owin.Security;
    using ServiceInterfaces;

    public class UserService : IUserService
    {
        private readonly IUserManagerProvider userManagerProvider;
        private readonly IPasswordHistoryRepository passwordHistoryRepository;
        private readonly IApplicationUserRepository applicationUserRepository;
        private readonly IGlobalSettingRepository globalSettingRepository;
        private readonly IAuthenticationManagerProvider authenticationManagerProvider;
        private GlobalSetting globalSetting;

        public UserService(
          IUserManagerProvider userManagerProvider,
          IPasswordHistoryRepository passwordHistoryRepository,
          IApplicationUserRepository applicationUserRepository,
          IGlobalSettingRepository globalSettingRepository,
          IAuthenticationManagerProvider authenticationManagerProvider)
        {
            this.userManagerProvider = userManagerProvider;
            this.passwordHistoryRepository = passwordHistoryRepository;
            this.applicationUserRepository = applicationUserRepository;
            this.globalSettingRepository = globalSettingRepository;
            this.authenticationManagerProvider = authenticationManagerProvider;
        }

        public void CreateUser(String userName, String password, String firstName, String lastName, String email)
        {
            var user = new ApplicationUser
            {
                UserName = userName,
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                IsApproved = true,
                IsLockedOut = false,
                LastPasswordChangedDate = DateTime.Now
            };

            var result = this.userManagerProvider.Create(user, password);

            if (result.Succeeded)
            {
                this.StorePasswordInHistory(user.Id, password);
            }

            ////this.ProcessRoles(user.Id, identityRoleIds, "create");
        }

        public PagedResult<ApplicationUser> GetUsers(Int32 pageNumber, Int32 numberOfItems, String environment)
        {
            try
            {
                return this.applicationUserRepository.GetUsers(pageNumber, numberOfItems, environment);
            }
            catch (Exception e)
            {
                throw new DocProcessingException("Unable to get users", e);
            }
        }

        public PagedResult<ApplicationUser> GetUsers(Int32 pageNumber, Int32 numberOfItems, String searchCriteria, String environment)
        {
            try
            {
                return this.applicationUserRepository.GetUsers(pageNumber, numberOfItems, searchCriteria, environment);
            }
            catch (Exception e)
            {
                throw new DocProcessingException("Unable to get users", e);
            }
        }

        public ApplicationUser GetApplicationUser()
        {
            try
            {
                var user = this.authenticationManagerProvider.User;

                return this.userManagerProvider.FindByName(user.Identity.GetUserName());
            }
            catch (Exception e)
            {
                throw new DocProcessingException("Unable to get user", e);
            }
        }

        public ApplicationUser GetApplicationUserById(String userId)
        {
            try
            {
                return this.userManagerProvider.FindById(userId);
            }
            catch (Exception e)
            {
                throw new DocProcessingException("Unable to change password", e);
            }
        }

        public ApplicationUser GetApplicationUser(String userName)
        {
            try
            {
                return this.userManagerProvider.FindByName(userName);
            }
            catch (Exception e)
            {
                throw new DocProcessingException("Unable to get user", e);
            }
        }

        public ApplicationUser GetApplicationUser(String userName, String password)
        {
            try
            {
                return this.userManagerProvider.Find(userName, password);
            }
            catch (Exception e)
            {
                throw new DocProcessingException("Unable to get user", e);
            }
        }

        public Boolean CheckForPassRenewal(DateTime passwordLastChanged, DateTime lastLogin)
        {
            try
            {
                this.GetGlobalSettings();

                Int32 passwordExpiresDays = this.globalSetting.PasswordExpDays;
                Boolean newUserPasswordReset = this.globalSetting.NewUserPasswordReset;

                if (newUserPasswordReset && (lastLogin == DateTime.MinValue))
                {
                    return true;
                }
                else if ((passwordExpiresDays > 0) && ((DateTime.Now - passwordLastChanged).TotalDays >= passwordExpiresDays))
                {
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                throw new DocProcessingException("Unable to check for password renewal", e);
            }
        }

        public void SignIn(ApplicationUser user, Boolean isPersistent)
        {
            this.authenticationManagerProvider.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            var identity = this.userManagerProvider.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
            this.authenticationManagerProvider.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
        }

        public void UpdateUserLastLogindate(String userId)
        {
            try
            {
                this.applicationUserRepository.UpdateUserlastLogindate(userId);
            }
            catch (Exception e)
            {
                throw new DocProcessingException("Unable to update user last login date", e);
            }
        }

        public void UpdateUserFailedLogin(String userId)
        {
            try
            {
                this.GetGlobalSettings();
                var user = applicationUserRepository.UpdateUserFailedLogin(userId);
                if (user.FailedLogInCount >= globalSetting.MaxLogInAttempts)
                {
                    applicationUserRepository.DeactivateUser(userId);
                }
            }
            catch (Exception e)
            {
                throw new DocProcessingException("Unable to update failed login count", e);
            }
        }

        public Boolean IsLockedOut(String userId)
        {
            try
            {
                return this.applicationUserRepository.IsLockedOut(userId);
            }
            catch (Exception e)
            {
                throw new DocProcessingException("Unable to check if user is locked out", e);
            }
        }

        public void ChangePassword(String userId, String currentPassword, String newPassword)
        {
            try
            {
                this.userManagerProvider.ChangePassword(userId, currentPassword, newPassword);

                this.applicationUserRepository.UpdateUserLastPasswordChangedDate(userId);

                this.StorePasswordInHistory(userId, newPassword);
            }
            catch (Exception e)
            {
                throw new DocProcessingException("Unable to change password", e);
            }
        }

        public void SignOut()
        {
            this.authenticationManagerProvider.SignOut();
        }

        public void Updateuser(String userName, String password, String firstName, String lastName, String email, String preferedLandingPage, String preferedEnvironment, String phone, Boolean isLockedOut, Int32 failedLogInCount, bool isDeActivated)
        {
            var user = this.userManagerProvider.FindByName(userName);

            user.FirstName = firstName;
            user.LastName = lastName;
            user.Email = email;
            user.PreferredLandingPage = preferedLandingPage;
            user.PreferredEnvironment = preferedEnvironment;
            user.Phone = phone;
            user.IsLockedOut = isLockedOut;
            user.FailedLogInCount = failedLogInCount;
            user.IsDeActivated = isDeActivated;

            if (password != null)
            {
                this.userManagerProvider.RemovePassword(user.Id);
                var result = this.userManagerProvider.AddPassword(user.Id, password);
                user.LastPasswordChangedDate = DateTime.Now;
                if (result.Succeeded)
                {
                    this.StorePasswordInHistory(user.Id, password);
                }
            }
            this.userManagerProvider.Update(user);
        }

        private void StorePasswordInHistory(String userId, String password)
        {
            password = DocProcessingEncryption.Encrypt(password);

            this.passwordHistoryRepository.Create(
              new PasswordHistory
                {
                    PasswordHash = password,
                    UserId = userId,
                    LogDate = DateTime.Now
                });
        }

        private void GetGlobalSettings()
        {
            this.globalSetting = this.globalSettingRepository.Get();
        }
    }
}
