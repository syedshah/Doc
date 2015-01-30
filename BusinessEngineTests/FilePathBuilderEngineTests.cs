// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FilePathBuilderEngineTests.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   File Path Builder Engine Tests
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace BusinessEngineTests
{
  using System;

  using BusinessEngines;

  using FluentAssertions;

  using NUnit.Framework;

  [Category("Unit")]
  [TestFixture]
  public class FilePathBuilderEngineTests
  {
    private String server;

    private String gridName;

    private FilePathBuilderEngine filePathBuilderEngine;

    [SetUp]
    public void SetUp()
    {
      this.server = "BravuraServer";
      this.gridName = "141204-154037R-CVXL";
      this.filePathBuilderEngine = new FilePathBuilderEngine();
    }

    [Test]
    public void GivenAServerNameAndGridName_WheneIWantToBuildTheOneStepLogPath_IGetTheRightPath()
    {
      var result = this.filePathBuilderEngine.BuildOneStepLogFilePath(this.server, this.gridName);

      result.Should().NotBeNull();
      result.ShouldBeEquivalentTo(@"\\BravuraServer\Nexdox\nmbbs02\Process\141204-154037R-CVXL\Output\141204-154037R-CVXL");
    }

    [Test]
    public void GivenAServerNameAndGridName_WheneIWantToBuildTheOneStepOutputDirectoryPath_IGetTheRightPath()
    {
      var result = this.filePathBuilderEngine.BuildOneStepOutputDirectory(this.server, this.gridName);

      result.Should().NotBeNull();
      result.ShouldBeEquivalentTo(@"\\BravuraServer\Nexdox\nmbbs02\Process\141204-154037R-CVXL\Output");
    }
  }
}
