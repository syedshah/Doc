// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GroupServiceTests.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   Job service tests
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ServiceTests
{
    using DocProcessingRepository.Interfaces;
    using Entities;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using Services;
    using System.Collections.Generic;

    [Category("Unit")]
    [TestFixture]
    public class GroupServiceTests
    {
        private Mock<IGroupRepository> _groupRepository;
        private GroupService _groupService;

        [SetUp]
        public void SetUp()
        {
            this._groupRepository = new Mock<IGroupRepository>();
            this._groupService = new GroupService(this._groupRepository.Object);
            this._groupRepository.Setup(x => x.GetGroups()).Returns(new List<Group>());
        }

        [Test]
        public void WhenIWantToGetGroups_AndDatabaseIsAvailable_IShouldBeAbleToGetGroups()
        {
            var groups = this._groupService.GetGroups();
            groups.Should().NotBeNull();
            this._groupRepository.Verify(x=>x.GetGroups(),Times.AtLeastOnce);
        }
        
    }
}
