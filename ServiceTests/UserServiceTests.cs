// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Class1.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   User service tests
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Security.Claims;
    using DocProcessingRepository.Interfaces;
    using Entities;
    using Exceptions;
    using FluentAssertions;
    using IdentityWrapper.Interfaces;
    using Microsoft.AspNet.Identity;
    using Microsoft.Owin.Security;
    using Moq;
    using NUnit.Framework;
    using Services;

    [Category("Unit")]
    [TestFixture]
    public class UserServiceTests
    {
        private Mock<IUserManagerProvider> userManagerProvider;
        private Mock<IPasswordHistoryRepository> passwordHistoryRepository;
        private Mock<IApplicationUserRepository> applicationUserRepository;
        private Mock<IGlobalSettingRepository> globalSettingRepository;
        private Mock<IAuthenticationManagerProvider> authenticationManagerProvider;
        private UserService userService;

        [SetUp]
        public void SetUp()
        {
            this.userManagerProvider = new Mock<IUserManagerProvider>();
            this.passwordHistoryRepository = new Mock<IPasswordHistoryRepository>();
            this.applicationUserRepository = new Mock<IApplicationUserRepository>();
            this.globalSettingRepository = new Mock<IGlobalSettingRepository>();
            this.authenticationManagerProvider = new Mock<IAuthenticationManagerProvider>();
            this.userService = new UserService(
              this.userManagerProvider.Object,
              this.passwordHistoryRepository.Object,
              this.applicationUserRepository.Object,
              this.globalSettingRepository.Object,
              this.authenticationManagerProvider.Object);
        }

        [Test]
        public void GivenValidParameters_AndDatabaseIsAvailable_WhenIWantToCreateUser_TheUserIsCreated()
        {
            var errors = new String[0];
            var idenResult = new IdentityResult(errors);

            this.userManagerProvider.Setup(x => x.Create(It.IsAny<ApplicationUser>(), It.IsAny<String>())).Returns(idenResult);

            this.passwordHistoryRepository.Setup(x => x.Create(It.IsAny<PasswordHistory>()));
            this.userService.CreateUser("broth", "ageless", "bertrand", "roth", "broth@gmail.com");
            this.userManagerProvider.Verify(x => x.Create(It.IsAny<ApplicationUser>(), It.IsAny<String>()), Times.AtLeastOnce);
        }

        [Test]
        public void GivenValidUsers_WhenItryToRetrieveUsers_AndDatabaseIsUnavailable_ThenAnDocProcessingExceptionIsThrown()
        {
            this.applicationUserRepository.Setup(m => m.GetUsers(It.IsAny<Int32>(), It.IsAny<Int32>(), It.IsAny<String>())).Throws<Exception>();
            Action act = () => this.userService.GetUsers(It.IsAny<Int32>(), It.IsAny<Int32>(), It.IsAny<String>());

            act.ShouldThrow<DocProcessingException>();
        }

        [Test]
        public void GivenValidUsers_WhenItryToRetrieveUsersFromTheDatabase_theusersAreRetireved()
        {
            this.applicationUserRepository.Setup(m => m.GetUsers(It.IsAny<Int32>(), It.IsAny<Int32>(), It.IsAny<String>())).Returns(new PagedResult<ApplicationUser>());
            var result = this.userService.GetUsers(It.IsAny<Int32>(), It.IsAny<Int32>(), It.IsAny<String>());
            result.Should().NotBeNull();
        }

        [Test]
        [ExpectedException(typeof(DocProcessingException), ExpectedMessage = "Unable to get users")]
        public void GivenASearchCriteria_WhenIWantToGetUsers_AndDatabaseIsUnavailable_ThenAnDocProcessingExceptionIsThrown()
        {
            this.applicationUserRepository.Setup(m => m.GetUsers(It.IsAny<Int32>(), It.IsAny<Int32>(), It.IsAny<String>(), It.IsAny<String>())).Throws<DocProcessingException>();
            this.userService.GetUsers(It.IsAny<Int32>(), It.IsAny<Int32>(), It.IsAny<String>(), It.IsAny<String>());

            this.applicationUserRepository.Verify(x => x.GetUsers(It.IsAny<Int32>(), It.IsAny<Int32>(), It.IsAny<String>(), It.IsAny<String>()), Times.AtLeastOnce);
        }

        [Test]
        public void GivenASearchCriteria_WhenIWantToGetUsers_AndDatabaseIsAvailable_IGetTheUsers()
        {
            this.applicationUserRepository.Setup(m => m.GetUsers(It.IsAny<Int32>(), It.IsAny<Int32>(), It.IsAny<String>(), It.IsAny<String>())).Returns(new PagedResult<ApplicationUser>());
            var result = this.userService.GetUsers(It.IsAny<Int32>(), It.IsAny<Int32>(), It.IsAny<String>(), It.IsAny<String>());

            result.Should().NotBeNull();
            this.applicationUserRepository.Verify(x => x.GetUsers(It.IsAny<Int32>(), It.IsAny<Int32>(), It.IsAny<String>(), It.IsAny<String>()), Times.AtLeastOnce);
        }

        [Test]
        public void GivenAUserName_AndDatabaseIsAvailable_WhenIWantToFindAUserByUserNameAndPassword_TheUserIsRetrieved()
        {
            this.userManagerProvider.Setup(x => x.Find(It.IsAny<String>(), It.IsAny<String>())).Returns(new ApplicationUser());
            this.applicationUserRepository.Setup(x => x.IsLockedOut(It.IsAny<String>())).Returns(false);

            var result = this.userService.GetApplicationUser(It.IsAny<String>(), It.IsAny<String>());

            this.userManagerProvider.Verify(x => x.Find(It.IsAny<String>(), It.IsAny<String>()));
            result.Should().NotBeNull();
            result.Should().BeOfType<ApplicationUser>();
        }

        [Test]
        public void GivenAUserName_AndDatabaseIsAvailable_WhenIWantToFindAUserByUserNameAndWrongPassword_TheUserIsNotRetrieved()
        {
            this.userManagerProvider.Setup(x => x.Find(It.IsAny<String>(), It.IsAny<String>())).Returns(It.IsAny<ApplicationUser>());
            this.applicationUserRepository.Setup(x => x.IsLockedOut(It.IsAny<String>())).Returns(false);

            var result = this.userService.GetApplicationUser(It.IsAny<String>(), It.IsAny<String>());

            this.userManagerProvider.Verify(x => x.Find(It.IsAny<String>(), It.IsAny<String>()));
            this.applicationUserRepository.Verify(x => x.IsLockedOut(It.IsAny<String>()), Times.Never);

            result.Should().BeNull();
        }

        [Test]
        public void GivenAUserNameAndPassword_AndDatabaseIsAvailable_AndUserIsLockedOut_WhenIWantToFindAUserByUserNameAndPassword_NullIsRetrieved()
        {
            this.userManagerProvider.Setup(x => x.Find(It.IsAny<String>(), It.IsAny<String>())).Returns(new ApplicationUser { IsLockedOut = true });

            this.applicationUserRepository.Setup(x => x.IsLockedOut(It.IsAny<String>())).Returns(true);
            var result = this.userService.GetApplicationUser(It.IsAny<String>(), It.IsAny<String>());
            this.userManagerProvider.Verify(x => x.Find(It.IsAny<String>(), It.IsAny<String>()));

            result.IsLockedOut.Should().BeTrue();
        }

        [Test]
        [ExpectedException(typeof(DocProcessingException))]
        public void GivenAUserNameAndPassword_AndDatabaseIsUnAvailable_WhenIWantToFindAUserByUserNameAndPassword_AUnityExceptionIsThrown()
        {
            this.userManagerProvider.Setup(x => x.Find(It.IsAny<String>(), It.IsAny<String>())).Throws(new DocProcessingException());
            this.userService.GetApplicationUser(It.IsAny<String>(), It.IsAny<String>());
            this.userManagerProvider.Verify(x => x.Find(It.IsAny<String>(), It.IsAny<String>()));
        }

        [Test]
        public void GivenValidData_WhenMyPasswordHasBeenRenewedWithinTheLast30Days_ThenThePasswordDoesntNeedResetting()
        {
            this.globalSettingRepository.Setup(g => g.Get()).Returns(new GlobalSetting() { PasswordExpDays = 30, NewUserPasswordReset = true });
            Boolean result = this.userService.CheckForPassRenewal(DateTime.Now.AddDays(-29), DateTime.Now.AddDays(-1));
            result.Should().BeFalse();
        }

        [Test]
        public void GivenValidData_WhenMyPasswordHasntBeenRenewedFor30Days_ThenThePasswordNeedsResetting()
        {
            this.globalSettingRepository.Setup(g => g.Get()).Returns(new GlobalSetting() { PasswordExpDays = 30, NewUserPasswordReset = true });
            Boolean result = this.userService.CheckForPassRenewal(DateTime.Now.AddDays(-30), DateTime.Now.AddDays(-1));
            result.Should().BeTrue();
        }

        [Test]
        public void GivenValidData_WhenIAmANewUserAndPasswordRenewalIsSetForNewUsers_ThenThePasswordNeedsResetting()
        {
            this.globalSettingRepository.Setup(g => g.Get()).Returns(new GlobalSetting() { PasswordExpDays = 30, NewUserPasswordReset = true });

            bool result = this.userService.CheckForPassRenewal(It.IsAny<DateTime>(), It.IsAny<DateTime>());
            result.Should().BeTrue();
        }

        [Test]
        public void WhenIWanSignInAUser_TheUserIsSignedIn()
        {
            this.authenticationManagerProvider.Setup(x => x.SignOut(It.IsAny<String>()));

            this.userManagerProvider.Setup(x => x.CreateIdentity(It.IsAny<ApplicationUser>(), It.IsAny<string>())).Returns(new ClaimsIdentity());

            this.authenticationManagerProvider.Setup(x => x.SignIn(It.IsAny<AuthenticationProperties>(), It.IsAny<ClaimsIdentity>()));

            this.userService.SignIn(It.IsAny<ApplicationUser>(), It.IsAny<Boolean>());

            this.authenticationManagerProvider.Verify(x => x.SignOut(It.IsAny<String>()), Times.AtLeastOnce);

            this.userManagerProvider.Verify(x => x.CreateIdentity(It.IsAny<ApplicationUser>(), It.IsAny<String>()), Times.AtLeastOnce);

            this.authenticationManagerProvider.Verify(x => x.SignIn(It.IsAny<AuthenticationProperties>(), It.IsAny<ClaimsIdentity>()), Times.AtLeastOnce);
        }

        [Test]
        public void GivenAUserName_AndDatabaseIsAvailable_AndUserIsNotLockedOut_WhenIWantToFindAUserByUserName_TheUserIsRetrieved()
        {
            this.userManagerProvider.Setup(x => x.FindByName(It.IsAny<String>())).Returns(new ApplicationUser { IsLockedOut = false });
            var result = this.userService.GetApplicationUser(It.IsAny<String>());
            this.userManagerProvider.Verify(x => x.FindByName(It.IsAny<String>()));
            result.Should().BeOfType<ApplicationUser>();
        }

        [Test]
        [ExpectedException(typeof(DocProcessingException))]
        public void GivenAUserName_AndDatabaseIsUnAvailable_WhenIWantToFindAUserByUserName_AUnityExceptionIsThrown()
        {
            this.userManagerProvider.Setup(x => x.FindByName(It.IsAny<String>())).Throws(new DocProcessingException());

            this.userService.GetApplicationUser(It.IsAny<String>());

            this.userManagerProvider.Verify(x => x.FindByName(It.IsAny<String>()));
        }

        [Test]
        public void GivenAUserName_AndDatabaseIsAvailable_WhenIWantToFindAUser_TheUserIsRetrieved()
        {
            this.userManagerProvider.Setup(x => x.FindByName(It.IsAny<String>())).Returns(new ApplicationUser());

            var result = this.userService.GetApplicationUser(It.IsAny<String>());

            this.userManagerProvider.Verify(x => x.FindByName(It.IsAny<String>()));

            result.Should().NotBeNull();
            result.Should().BeOfType<ApplicationUser>();
        }

        [Test]
        [ExpectedException(typeof(DocProcessingException))]
        public void GivenAUserName_AndDatabaseIsUnAvailable_WhenIWantToFindAUser_ThenAUnityExceptionIsThrown()
        {
            this.userManagerProvider.Setup(x => x.FindByName(It.IsAny<String>())).Throws(new DocProcessingException());

            this.userService.GetApplicationUser(It.IsAny<String>());

            this.userManagerProvider.Verify(x => x.FindByName(It.IsAny<String>()));
        }

        [Test]
        public void GivenAUserId_WhenITryToUpdateTheFailedLogInCount_AndNotReachedTheMaxAttempts_ThenTheFailedLogInCountIsUpdatedAndTheUserIsNotLockedOut()
        {
            this.applicationUserRepository.Setup(c => c.UpdateUserFailedLogin(It.IsAny<String>())).Returns(new ApplicationUser() { FailedLogInCount = 2 });
            this.globalSettingRepository.Setup(g => g.Get()).Returns(new GlobalSetting() { MaxLogInAttempts = 3 });
            this.userService.UpdateUserFailedLogin(It.IsAny<String>());
            this.applicationUserRepository.Verify(s => s.DeactivateUser(It.IsAny<String>()), Times.Never);
        }

        [Test]
        public void GivenAUserId_WhenITryToUpdateTheFailedLogInCount_AndTheDatabaseIsUnavailable_ThenTheFailedLogInCountIsUpdated()
        {
            this.globalSettingRepository.Setup(c => c.Get()).Throws<Exception>();
            Action act = () => this.userService.UpdateUserFailedLogin(It.IsAny<String>());

            act.ShouldThrow<DocProcessingException>();
        }

        [Test]
        public void GivenAUserId_WhenITryToUpdateTheFailedLogInCount_AndTheUserHasReachedTheMaxAttempts_ThenTheUserIsLockedOut()
        {
            this.applicationUserRepository.Setup(c => c.UpdateUserFailedLogin(It.IsAny<String>())).Returns(new ApplicationUser() { FailedLogInCount = 3 });
            this.globalSettingRepository.Setup(g => g.Get()).Returns(new GlobalSetting() { MaxLogInAttempts = 3 });
            this.userService.UpdateUserFailedLogin(It.IsAny<String>());
            this.applicationUserRepository.Verify(s => s.DeactivateUser(It.IsAny<String>()), Times.Once());
        }

        [Test]
        public void GivenAValidUserId_WhenIWantToUpdateUserLastLoginDate_AndDatabaseIsAvailable_TheLastLoginDateIsUpdated()
        {
            this.applicationUserRepository.Setup(x => x.UpdateUserlastLogindate(It.IsAny<String>()));

            this.userService.UpdateUserLastLogindate(It.IsAny<String>());

            this.applicationUserRepository.Verify(x => x.UpdateUserlastLogindate(It.IsAny<String>()));
        }

        [Test]
        [ExpectedException(typeof(DocProcessingException))]
        public void GivenAValidUserId_WhenIWantToUpdateUserLastLoginDate_AndDatabaseIsUnAvailable_ThenAunityExceptionIsThrown()
        {
            this.applicationUserRepository.Setup(x => x.UpdateUserlastLogindate(It.IsAny<String>())).Throws<DocProcessingException>();

            this.userService.UpdateUserLastLogindate(It.IsAny<String>());

            this.applicationUserRepository.Verify(x => x.UpdateUserlastLogindate(It.IsAny<String>()));
        }

        [Test]
        public void GivenAValidUserId_WhenIWantIWantToKnowIfTheUserIsLocledOut_IAmNotifiedIfTheUserIsLockedOut()
        {
            this.applicationUserRepository.Setup(x => x.IsLockedOut(It.IsAny<String>())).Returns(true);

            this.userService.IsLockedOut(It.IsAny<String>());

            this.applicationUserRepository.Verify(x => x.IsLockedOut(It.IsAny<String>()), Times.Once);
        }

        [Test]
        public void GivenAuserId_WhenITryToRetrieveAUser_AndDatabaseIsAvailable_ThenTheUserIsRetrieved()
        {
            this.userManagerProvider.Setup(x => x.FindById(It.IsAny<String>())).Returns(new ApplicationUser());

            var result = this.userService.GetApplicationUserById(It.IsAny<String>());

            result.Should().NotBeNull();

            this.userManagerProvider.Verify(x => x.FindById(It.IsAny<String>()), Times.AtLeastOnce);
        }

        [Test]
        [ExpectedException(typeof(DocProcessingException))]
        public void GivenAuserId_WhenITryToRetrieveAUser_AndDatabaseIsUnAvailable_AUnityExceptionIsThrown()
        {
            this.userManagerProvider.Setup(x => x.FindById(It.IsAny<String>())).Throws<Exception>();

            this.userService.GetApplicationUserById(It.IsAny<String>());

            this.userManagerProvider.Verify(x => x.FindById(It.IsAny<String>()), Times.AtLeastOnce);
        }

        [Test]
        public void WithNoParameter_AndDatabaseIsAvailable_WhenIWantToFindTheCurrentuser_TheUserIsRetrieved()
        {
            var identity = new ClaimsIdentity { };
            identity.AddClaim(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", "broth"));
            var user = new ClaimsPrincipal(identity);

            this.authenticationManagerProvider.SetupGet(x => x.User).Returns(user);

            this.userManagerProvider.Setup(x => x.FindByName(It.IsAny<String>()))
                  .Returns(new ApplicationUser());

            var result = this.userService.GetApplicationUser();

            this.authenticationManagerProvider.VerifyGet(x => x.User, Times.AtLeastOnce());

            this.userManagerProvider.Verify(x => x.FindByName(It.IsAny<String>()), Times.AtLeastOnce);

            result.Should().NotBeNull();
        }

        [Test]
        [ExpectedException(typeof(DocProcessingException))]
        public void WithNoParameter_AndDatabaseIsUnAvailable_WhenIWantToFindTheCurrentuser_AUnityExceptionIsThrown()
        {
            this.authenticationManagerProvider.SetupGet(x => x.User).Returns(new ClaimsPrincipal());

            this.userManagerProvider.Setup(x => x.FindByName(It.IsAny<string>())).Throws(new DocProcessingException());

            this.userService.GetApplicationUser();

            this.authenticationManagerProvider.VerifyGet(x => x.User, Times.AtLeastOnce());

            this.userManagerProvider.Verify(x => x.Find(It.IsAny<string>(), It.IsAny<string>()));
        }

        [Test]
        public void GivenAuserIdCurrentPasswordAndNewPassword_WhenITryToChangePassword_AndDatabaseIsAvailable_ThenTheUserPasswordIsChanged()
        {
            this.userManagerProvider.Setup(x => x.ChangePassword("jddj", "currentpassword", "newpassword"));

            this.applicationUserRepository.Setup(x => x.UpdateUserLastPasswordChangedDate(It.IsAny<String>()));

            this.passwordHistoryRepository.Setup(x => x.Create(It.IsAny<PasswordHistory>()));

            this.userService.ChangePassword("jddj", "currentpassword", "newpassword");

            this.userManagerProvider.Verify(x => x.ChangePassword(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>()), Times.AtLeastOnce);

            this.applicationUserRepository.Verify(x => x.UpdateUserLastPasswordChangedDate(It.IsAny<String>()));

            this.passwordHistoryRepository.Verify(x => x.Create(It.IsAny<PasswordHistory>()), Times.AtLeastOnce);
        }

        [Test]
        [ExpectedException(typeof(DocProcessingException))]
        public void GivenAuserIdCurrentPasswordAndNewPassword_WhenITryToChangePassword_AndDatabaseIsUnAvailable_AUnityExceptionIsThrown()
        {
            this.userManagerProvider.Setup(x => x.ChangePassword(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>())).Throws<Exception>();

            this.userService.ChangePassword(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>());

            this.userManagerProvider.Verify(x => x.ChangePassword(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>()), Times.AtLeastOnce);
        }

        [Test]
        public void GivenValidParameters_AndDatabaseIsAvailable_WhenIWantToUpdateUser_TheUserIsUpdated()
        {
            var user = new ApplicationUser("broth");

            user.FirstName = "Bertrand";
            user.LastName = "Roth";
            user.Email = "broth@gmail.com";

            this.userManagerProvider.Setup(x => x.FindByName(It.IsAny<String>())).Returns(user);

            String[] errors = new String[0];
            var idenResult = new IdentityResult(errors);

            this.userManagerProvider.Setup(x => x.Update(It.IsAny<ApplicationUser>())).Returns(idenResult);
            this.userManagerProvider.Setup(x => x.RemovePassword(It.IsAny<String>())).Returns(idenResult);
            this.userManagerProvider.Setup(x => x.AddPassword(It.IsAny<String>(), It.IsAny<String>())).Returns(idenResult);
            this.passwordHistoryRepository.Setup(x => x.Create(It.IsAny<PasswordHistory>()));

            var listIdentityRoleId = new List<String>();
            listIdentityRoleId.Add("tdgdg");
            listIdentityRoleId.Add("tddeeegdg");

            this.userService.Updateuser("broth", "ageless", "bertrand", "roth", "broth@gmail.com", "prefLandingPage", "prefEnvironment", "121212", false, 0, false);
            this.userManagerProvider.Verify(x => x.FindByName(It.IsAny<String>()), Times.Once);
            this.userManagerProvider.Verify(x => x.Update(It.IsAny<ApplicationUser>()), Times.Once);
            this.userManagerProvider.Verify(x => x.RemovePassword(It.IsAny<String>()), Times.Once);
            this.userManagerProvider.Verify(x => x.AddPassword(It.IsAny<String>(), It.IsAny<String>()), Times.Once);
        }
    }
}
