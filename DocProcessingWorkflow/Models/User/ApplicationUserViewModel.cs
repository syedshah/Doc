// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApplicationUserViewModel.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Reflection;
using DocProcessingWorkflow.Constants;
using IdentityWrapper.Identity;

namespace DocProcessingWorkflow.Models.User
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Entities;

    public class ApplicationUserViewModel : BaseUserViewModel
    {
        public ApplicationUserViewModel(ApplicationUser user)
            : base(user)
        {

        }
    }
}