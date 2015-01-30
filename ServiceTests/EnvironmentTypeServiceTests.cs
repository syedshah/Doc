// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnvironmentTypeServiceTests.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ServiceTests
{
  using System;
  using System.Collections.Generic;

  using DocProcessingRepository.Interfaces;

  using Entities;

  using Exceptions;

  using FluentAssertions;

  using Moq;

  using NUnit.Framework;

  using Services;

  [Category("Unit")]
  [TestFixture]
  public class EnvironmentTypeServiceTests
  {
    private Mock<IEnvironmentTypeRepository> environmentTypeRepository;

    private EnvironmentTypeService environmentTypeService;

    private IList<EnvironmentServerEntity> servers;

    [SetUp]
    public void SetUp()
    {
      this.environmentTypeRepository = new Mock<IEnvironmentTypeRepository>();
      this.environmentTypeService = new EnvironmentTypeService(this.environmentTypeRepository.Object);
      this.servers = new List<EnvironmentServerEntity>();
      this.servers.Add(new EnvironmentServerEntity() { ServerName = "Bravura Server" });
    }

    [Test]
    public void GivenAValidEnvironmentType_WhenIWantToGetServersAndDatabaseIsAvailable_IGetTheEnvironmentservers()
    {
      this.environmentTypeRepository.Setup(x => x.GetEnvironmentServers(It.IsAny<String>())).Returns(this.servers);

      var result = this.environmentTypeService.GetEnvironmentServers(It.IsAny<String>());

      result.Should().NotBeNull();
      result.Count.ShouldBeEquivalentTo(1);
      this.environmentTypeRepository.Verify(x => x.GetEnvironmentServers(It.IsAny<String>()), Times.AtLeastOnce);
    }

    [Test]
    [ExpectedException(typeof(DocProcessingException), ExpectedMessage = "Unable to get environment servers")]
    public void GivenAValidEnvironmentType_WhenIWantToGetServersAndDatabaseIsUnAvailable_ADocProcessingExceptionIsThrown()
    {
      this.environmentTypeRepository.Setup(x => x.GetEnvironmentServers(It.IsAny<String>())).Throws<DocProcessingException>();

      this.environmentTypeService.GetEnvironmentServers(It.IsAny<String>());

      this.environmentTypeRepository.Verify(x => x.GetEnvironmentServers(It.IsAny<String>()), Times.AtLeastOnce);
    }
  }
}
