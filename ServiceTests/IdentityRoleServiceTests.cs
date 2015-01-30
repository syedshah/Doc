// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IdentityRoleServiceTests.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   Job service tests
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocProcessingRepository.Interfaces;
using FluentAssertions;
using IdentityWrapper.Interfaces;
using Microsoft.AspNet.Identity;
using Moq;
using NUnit.Framework;
using ServiceInterfaces;
using Services;

namespace ServiceTests
{
    [Category("Unit")]
    [TestFixture]
    public class IdentityRoleServiceTests
    {
        private Mock<IIdentityRoleRepository> _identityRoleRepository;
        private Mock<IUserManagerProvider> _userManagerProvider;
        private IdentityRoleService _identityRoleService;

        [SetUp]
        public void SetUp()
        {
            this._identityRoleRepository = new Mock<IIdentityRoleRepository>();
            this._userManagerProvider = new Mock<IUserManagerProvider>();
            this._identityRoleService = new IdentityRoleService(_identityRoleRepository.Object, _userManagerProvider.Object);
        }

        [Test]
        public void WhenIWantToGetRoles_WhenDatabaseIsAvailable_IShouldGetRoles()
        {
            this._identityRoleRepository.Setup(x => x.GetRoles()).Returns(new List<string>());

            var roles = this._identityRoleService.GetRoles();

            roles.Should().NotBeNull();

            this._identityRoleRepository.Verify(x => x.GetRoles(), Times.AtLeastOnce);
        }

        [Test]
        public void GivenAUserId_WhenIWantToGetRoles_WhenDatabaseIsAvailable_IShouldGetRolesOftheUser()
        {
            this._userManagerProvider.Setup(x => x.GetRoles(It.IsAny<String>())).Returns(new List<string>());

            var roles = this._identityRoleService.GetRoles(It.IsAny<String>());

            roles.Should().NotBeNull();

            this._userManagerProvider.Verify(x => x.GetRoles(It.IsAny<String>()), Times.AtLeastOnce);
        }

        [Test]
        public void GivenValidRoles_WhenIWantToAddRolesToUser_WhenDatabaseIsAvailable_IShouldBeAbleToAddRolesToUser()
        {
            this._userManagerProvider.Setup(x => x.GetRoles(It.IsAny<String>())).Returns(new List<string>());
            this._userManagerProvider.Setup(x => x.RemoveFromRole(It.IsAny<String>(), It.IsAny<String>()))
                .Returns(new IdentityResult());
            this._userManagerProvider.Setup(x => x.AddToRole(It.IsAny<String>(), It.IsAny<String>()))
                .Returns(new IdentityResult());

            this._identityRoleService.AddRolesToUser(It.IsAny<String>(), new List<String>());

            this._userManagerProvider.Verify(x => x.GetRoles(It.IsAny<String>()), Times.AtLeastOnce);
        }
    }
}
