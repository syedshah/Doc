// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AddUserViewModel.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//  User mode used to create users 
// </summary>
// --------------------------------------------------------------------------------------------------------------------


using Entities;

namespace DocProcessingWorkflow.Models.User
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using System.Linq;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Filters;

    public class AddUserViewModel : ExtendedUserViewModel
    {
        public String UserNameHidden { get; set; }

        [Required]
        [PasswordValid("UserName", "FirstName", "LastName")]
        public String Password { get; set; }

        [Required]
        public String ConfirmPassword { get; set; }

        public IList<SelectListItem> Domiciles { get; set; }

        public Int32 DomicileId { get; set; }
    }
}