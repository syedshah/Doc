// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EmailServiceTests.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ServiceTests
{
  using System;
  using BusinessEngineInterfaces;
  using Exceptions;
  using FluentAssertions;
  using Moq;
  using NUnit.Framework;
  using ServiceInterfaces;
  using Services;

  [Category("Unit")]
  [TestFixture]
  public class EmailServiceTests
  {
    private Mock<IEmailEngine> emailEngine;
    private IEmailService emailService;

    [SetUp]
    public void SetUp()
    {
      this.emailEngine = new Mock<IEmailEngine>();
      this.emailService = new EmailService(this.emailEngine.Object);
    }

    [Test]
    public void GivenAValidData_WhenITryToSendEmail_TheEmailIsSent()
    {
      this.emailEngine.Setup(a => a.SendEmail(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>()));

      this.emailService.SendEmail(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>());

      this.emailEngine.Verify(x => x.SendEmail(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>()));
    }

    [Test]
    public void GivenAValidData_WhenITryToSendEmail_AndThereIsASMPTError_ADocProcessingExceptionIsThrown()
    {
      this.emailEngine.Setup(a => a.SendEmail(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>())).Throws<Exception>();

      Action act = () => this.emailService.SendEmail(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>());

      act.ShouldThrow<DocProcessingException>();
    }
  }
}
