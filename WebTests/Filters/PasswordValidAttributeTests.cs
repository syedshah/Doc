// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PasswordValidAttributeTests.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//  Tests for Password Validation Attribute
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace WebTests.Filters
{
  using System.ComponentModel.DataAnnotations;

  using DocProcessingWorkflow.Filters;
  using DocProcessingWorkflow.Models.Password;

  using FluentAssertions;

  using NUnit.Framework;

  [Category("Unit")]
  [TestFixture]
  public class PasswordValidAttributeTests
  {
    private PasswordValidAttribute passwordValidAttribute;

    private ChangeCurrentPasswordModel changeCurrentPasswordModel;

    private ValidationContext validationContext;

    [SetUp]
    public void Setup()
    {
      this.passwordValidAttribute = new PasswordValidAttribute("UserName", "FirstName", "LastName");
      this.changeCurrentPasswordModel = new ChangeCurrentPasswordModel("userId", "myUserNames", "firstName", "lastname");
    }

    [Test]
    public void GivenAPasswordValidationAttribute_IfPasswordComplexityIsNotValid_ThenTheValidationShouldFail()
    {
      this.changeCurrentPasswordModel.NewPassword = "wearetheworld";
      this.validationContext = new ValidationContext(this.changeCurrentPasswordModel);

      var result = this.passwordValidAttribute.GetValidationResult(
        this.changeCurrentPasswordModel.NewPassword, this.validationContext);

      result.Should().NotBeNull();
      result.ErrorMessage.ShouldAllBeEquivalentTo("This password is not valid. Please click on the help message to view the password requirements.");
    }

    [Test]
    public void GivenAPasswordValidationAttribute_IfPasswordHasThreeConsecutiveCharacters_ThenTheValidationShouldFail()
    {
      this.changeCurrentPasswordModel.NewPassword = "Wearetttworl5!";
      this.validationContext = new ValidationContext(this.changeCurrentPasswordModel);

      var result = this.passwordValidAttribute.GetValidationResult(
        this.changeCurrentPasswordModel.NewPassword, this.validationContext);

      result.Should().NotBeNull();
      result.ErrorMessage.ShouldAllBeEquivalentTo("This password is not valid. Please click on the help message to view the password requirements.");
    }

    [Test]
    public void GivenAPasswordValidationAttribute_IfAFuzzyMatchOfMyPasswordIsMoreThan85Percent_ThenTheValidationShouldFail()
    {
      this.changeCurrentPasswordModel.NewPassword = "2yUserNames";
      this.validationContext = new ValidationContext(this.changeCurrentPasswordModel);

      var result = this.passwordValidAttribute.GetValidationResult(
        this.changeCurrentPasswordModel.NewPassword, this.validationContext);

      result.Should().NotBeNull();
      result.ErrorMessage.ShouldAllBeEquivalentTo("This password is not valid. Please click on the help message to view the password requirements.");
    }


    [Test]
    public void GivenAPasswordValidationAttribute_IfAThePasswordIsValid_ThenTheValidationShouldPass()
    {
      this.changeCurrentPasswordModel.NewPassword = "Welwyn2!fed";
      this.validationContext = new ValidationContext(this.changeCurrentPasswordModel);

      var result = this.passwordValidAttribute.GetValidationResult(
        this.changeCurrentPasswordModel.NewPassword, this.validationContext);

      result.Should().BeNull();
    }
  }
}
