// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CurrentPasswordValidAttribute.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DocProcessingWorkflow.Filters
{
  using System;
  using System.ComponentModel.DataAnnotations;
  using System.Configuration;

  using DocProcessingRepository.Repositories;

  using IdentityWrapper.Identity;

  using ServiceInterfaces;

  using Services;

  [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false
    )]
  public class CurrentPasswordValidAttribute : ValidationAttribute
  {
    public IUserService UserService { get; set; }

    public String OtherPropertyName { get; private set; }

    public CurrentPasswordValidAttribute(String userId)
    {
      this.OtherPropertyName = userId;

      this.UserService = this.GetUserService();
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
      var result = ValidationResult.Success;

      if (value != null)
      {
        var userId =
          validationContext.ObjectType.GetProperty(OtherPropertyName).GetValue(validationContext.ObjectInstance, null);

        if (!this.ConfirmUser(value.ToString(), userId.ToString()))
        {
          result = new ValidationResult("This current password provided is wrong");
        }
      }

      return result;
    }

    private Boolean ConfirmUser(String password, String userId)
    {
      var loggedInUser = this.UserService.GetApplicationUserById(userId);

      var confirmUser = this.UserService.GetApplicationUser(loggedInUser.UserName.Trim(), password.Trim());

      if (loggedInUser != confirmUser)
      {
        return false;
      }
      else
      {
        return true;
      }
    }

    private IUserService GetUserService()
    {
      var globalSettingsRepository =
        new GlobalSettingRepository(ConfigurationManager.ConnectionStrings["BBOS"].ConnectionString);
      var applicationUserRepository =
        new ApplicationUserRepository(ConfigurationManager.ConnectionStrings["BBOS"].ConnectionString);
      var passwordHistoryRepository =
        new PasswordHistoryRepository(ConfigurationManager.ConnectionStrings["BBOS"].ConnectionString);
      var userManagerProvider = new UserManagerProvider(ConfigurationManager.ConnectionStrings["BBOS"].ConnectionString);
      var authenticationManagerProvider = new AuthenticationManagerProvider(new HttpContextBaseProvider());

      return new UserService(
        userManagerProvider,
        passwordHistoryRepository,
        applicationUserRepository,
        globalSettingsRepository,
        authenticationManagerProvider);
    }
  }
}