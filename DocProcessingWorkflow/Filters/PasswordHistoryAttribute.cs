// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PasswordHistoryAttribute.cs" company="DST Nexdox">
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
  using ServiceInterfaces;
  using Services;

  [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
  public class PasswordHistoryAttribute : ValidationAttribute
  {
    public IPasswordHistoryService PasswordHistoryService { get; set; }

    public PasswordHistoryAttribute(String userId)
    {
      OtherPropertyName = userId;

      var passwordHistoryRepository = new PasswordHistoryRepository(ConfigurationManager.ConnectionStrings["BBOS"].ConnectionString);

      PasswordHistoryService = new PasswordHistoryService(passwordHistoryRepository);
    }

    public String OtherPropertyName { get; private set; }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
      var result = ValidationResult.Success;

      if (value != null)
      {
        var userId =
            validationContext.ObjectType.GetProperty(OtherPropertyName).GetValue(validationContext.ObjectInstance, null);

        if (PasswordHistoryService.IsPasswordInHistory(userId.ToString(), value.ToString()))
        {
          result = new ValidationResult("This password has already been used recently, please choose another password");
        }
      }

      return result;
    }
  }
}