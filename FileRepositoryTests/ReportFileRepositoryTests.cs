// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReportFileRepositoryTests.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FileRepositoryTests
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    using SystemFileAdapter;

    using Entities.File;

    using FileRepository.Repositories;

    using FileSystemInterfaces;

    using FluentAssertions;

    using NUnit.Framework;

    [Category("Integration")]
    [TestFixture]
    public class ReportFileRepositoryTests
    {
        private const String BaseDirectory = "files";

        private String reportFilePath;

        private IFileInfoFactory fileInfoFactory;

        private IDirectoryInfo directoryInfo;

        private String reportInformationFile;

        private ReportFileRepository reportFileRepository;

        [SetUp]
        public void Setup()
        {
            this.fileInfoFactory = new SystemFileInfoFactory();
            this.directoryInfo = new SystemIoDirectoryInfo();

            this.reportFileRepository = new ReportFileRepository(BaseDirectory, this.fileInfoFactory, this.directoryInfo);

            this.reportInformationFile = "\\\\srv-dg-bravura4.dstoutput.co.uk\\Nexdox\\nmbbs02\\Process\\14315B-103811X-QQRD\\Output";
            this.reportFilePath = "\\\\srv-dg-bravura4.dstoutput.co.uk\\Nexdox\\nmbbs02\\Process\\14315B-103811X-QQRD\\Output\\14315B-103811X-QQRD.log";
        }

        [TearDown]
        public void TearDown()
        {
            
        }

        [Test]
        public void GivenAReportFileRepository_WhenIWantToGetPdfFilesThroughPdfInformationFile_IGetAListOfReportFiles()
        {
            var result = this.reportFileRepository.GetReportFiles(this.reportInformationFile);

            result.Should().NotBeNull();
        }

        [Test]
        public void GivenAReportFileRepository_WhenIWantAReportFileThroughtFilePath_IGetAReportFile()
        {
            var result = this.reportFileRepository.GetReportFile(this.reportFilePath);

            result.Should().NotBeNull();
        }
    }
}
