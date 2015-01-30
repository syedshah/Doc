// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChangeCurrentPasswordModel.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DocProcessingWorkflow.Models.Password
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using DocProcessingWorkflow.Filters;
    using DocProcessingWorkflow.Resources.Password;

    public class ChangeCurrentPasswordModel
    {
        public ChangeCurrentPasswordModel()
        {

        }

        public ChangeCurrentPasswordModel(String userId, String userName, String firstName, String lastName)
        {
            this.UserId = userId;
            this.UserName = userName;
            this.FirstName = firstName;
            this.LastName = lastName;
        }

        public String UserId { get; set; }

        public String UserName { get; set; }

        public String FirstName { get; set; }

        public String LastName { get; set; }

        [CurrentPasswordValid("UserId")]
        [Required(ErrorMessageResourceName = "CurrentPasswordError", ErrorMessageResourceType = typeof(Change))]
        public String CurrentPassword { get; set; }

        [PasswordValid("UserName", "FirstName", "LastName")]
        [Required(ErrorMessageResourceName = "PasswordError", ErrorMessageResourceType = typeof(Change))]
        [PasswordHistory("UserId")]
        public String NewPassword { get; set; }

        [Required(ErrorMessageResourceName = "ConfirmPasswordError", ErrorMessageResourceType = typeof(Change))]
        [Compare("NewPassword", ErrorMessageResourceName = "ConfirmPasswordCompareError", ErrorMessageResourceType = typeof(Change))]
        public String ConfirmNewPassword { get; set; }
    }
}