// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UsersViewModel.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using Microsoft.AspNet.Identity.EntityFramework;

namespace DocProcessingWorkflow.Models.User
{
    using System.Collections.Generic;

    using DocProcessingWorkflow.Models.Helper;

    using Entities;

    public class UsersViewModel
    {
        public UsersViewModel()
        {
            this.AddUserViewModel = new AddUserViewModel();
            PagingInfo = new PagingInfoViewModel();
        }

        public UsersViewModel(IEnumerable<ApplicationUser> users)
        {
            foreach (var user in users)
            {
                var x = new ApplicationUserViewModel(user);
                this._users.Add(x);
            }

            this.AddUserViewModel = new AddUserViewModel();

            PagingInfo = new PagingInfoViewModel();
        }

        public AddUserViewModel AddUserViewModel { get; set; }

        public PagingInfoViewModel PagingInfo { get; set; }

        public String CurrentPage { get; set; }

        public String SearchUser { get; set; }

        public String UserSearchValue { get; set; }

        public Boolean IsDSTAdmin { get; set; }

        private List<ApplicationUserViewModel> _users = new List<ApplicationUserViewModel>();

        public List<ApplicationUserViewModel> Users
        {
            get { return _users; }
            set { _users = value; }
        }

        public void AddUsers(PagedResult<ApplicationUser> users)
        {
            foreach (ApplicationUser user in users.Results)
            {
                var avm = new ApplicationUserViewModel(user);
                Users.Add(avm);
            }

            PagingInfo = new PagingInfoViewModel
            {
                CurrentPage = users.CurrentPage,
                TotalItems = users.TotalItems,
                ItemsPerPage = users.ItemsPerPage,
                TotalPages = users.TotalPages,
                StartRow = users.StartRow,
                EndRow = users.EndRow
            };

            CurrentPage = PagingInfo.CurrentPage.ToString();
        }
    }
}