// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApplicationUser.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   User object user for identity
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Entities
{
    using System;
    using System.Collections.Generic;

    using Microsoft.AspNet.Identity.EntityFramework;

    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            this.Groups = new List<Group>();
            ////ManCos = new List<ApplicationUserManCo>();
            ////Domiciles = new List<ApplicationUserDomicile>();
        }

        public ApplicationUser(String userName)
            : base(userName)
        {
            ////ManCos = new List<ApplicationUserManCo>();
            ////Domiciles = new List<ApplicationUserDomicile>();
        }

        /////public virtual IList<ApplicationUserDomicile> Domiciles { get; set; }

        ////public virtual IList<ApplicationUserManCo> ManCos { get; set; }

        public enum ValidLandingPages
        {
            Undefined,
            Home,
            JobsView,
            JobsSubmit,
            JobsManageInserts
        };

        public enum ValidEnvironemnts
        {
            Undefined,
            Development,
            Uat,
            Production
        };

        public virtual String FirstName { get; set; }

        public String LastName { get; set; }

        public String Title { get; set; }

        public String Email { get; set; }

        public String Comment { get; set; }

        public virtual DateTime? LastLoginDate { get; set; }

        public virtual DateTime LastPasswordChangedDate { get; set; }

        public virtual Boolean IsApproved { get; set; }

        public virtual Boolean IsLockedOut { get; set; }

        public virtual Boolean IsDeActivated { get; set; }

        public Int32 FailedLogInCount { get; set; }

        public virtual String PreferredLandingPage { get; set; }

        public String PreferredEnvironment { get; set; }

        public String Phone { get; set; }

        public virtual IList<Group> Groups { get; set; }
    }
}
