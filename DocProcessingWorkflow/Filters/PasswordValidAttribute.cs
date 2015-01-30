// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PasswordValidAttribute.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   Filter for validataing password strength
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DocProcessingWorkflow.Filters
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Text.RegularExpressions;

    using Utilities;

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class PasswordValidAttribute : ValidationAttribute
    {
        private String[] fuzzyMatchPropertyNames;

        public PasswordValidAttribute(params String[] fuzzyMatchProperties)
        {
            if (fuzzyMatchProperties.Length >= 1)
            {
                this.fuzzyMatchPropertyNames = fuzzyMatchProperties;
            }
        }

        protected override ValidationResult IsValid(Object value, ValidationContext validationContext)
        {
            var result = ValidationResult.Success;

            if (value != null)
            {
                if (this.IsComplexityNotValid(value.ToString()) || this.IsPasswordHasThreeConsecutiveCharacters(value.ToString()) || this.IsFuzzyMatchOfUserNameAndNames(value.ToString(), validationContext))
                {
                    result =
                      new ValidationResult(
                        "This password is not valid. Please click on the help message to view the password requirements.");
                }
            }

            return result;
        }

        private Boolean IsComplexityNotValid(String password)
        {
            Boolean validationResult = false;

            Match match = Regex.Match(password, @"(?=^.{8,}$)((?=.*\d)|(?=.*\W+))(?![.\n])(?=.*[A-Z])(?=.*[a-z]).*$");

            if (!match.Success)
            {
                validationResult = true;
            }

            return validationResult;
        }

        private Boolean IsFuzzyMatchOfUserNameAndNames(String password, ValidationContext validationContext)
        {
            Boolean validationResult = false;

            foreach (var fuzzyMatchPropertyName in this.fuzzyMatchPropertyNames)
            {
                var propValue =
                  validationContext.ObjectType.GetProperty(fuzzyMatchPropertyName.Trim())
                                   .GetValue(validationContext.ObjectInstance, null);
                if (propValue != null)
                {
                    if (this.IsFuzzyMatchOfValue(password, propValue.ToString()))
                    {
                        validationResult = true;
                        break;
                    }
                }
            }

            return validationResult;
        }

        private Boolean IsFuzzyMatchOfValue(String password, String fuzzyMatchValue)
        {
            var fuzzyPercentage = StringUtil.FuzzyMatch(password, fuzzyMatchValue);

            if (fuzzyPercentage > 85.00)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private Boolean IsPasswordHasThreeConsecutiveCharacters(String password)
        {
            Boolean validationResult = false;

            Match match = Regex.Match(password, @"(.)\1\1");

            if (match.Success)
            {
                validationResult = true;
            }

            return validationResult;
        }
    }
}