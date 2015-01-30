using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using DocProcessingWorkflow.Constants;
using DocProcessingWorkflow.Filters;
using Entities;

namespace DocProcessingWorkflow.Models.User
{
    public class BaseUserViewModel
    {
        public BaseUserViewModel()
        {

        }
        public BaseUserViewModel(ApplicationUser user)
        {
            this.UserName = user.UserName;
            this.FirstName = user.FirstName;
            this.LastName = user.LastName;
            this.Id = user.Id;
            this.Email = user.Email;
            this.IsLockedOut = user.IsLockedOut;
            this.IsDeActivated = user.IsDeActivated;
            this.FailedLogInCount = user.FailedLogInCount;
            this.Phone = user.Phone;
        }

        public String Id { get; set; }

        [Required]
        public String UserName { get; set; }

        [Required]
        public String FirstName { get; set; }

        [Required]
        public String LastName { get; set; }

        [Required]
        public String Email { get; set; }

        public Boolean IsLockedOut { get; set; }

        public Boolean IsDeActivated { get; set; }

        public Int32 FailedLogInCount { get; set; }

        public String Phone { get; set; }
    }
}