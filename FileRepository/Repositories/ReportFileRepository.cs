// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReportFileRepository.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   Defines the ReportFileRepository type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FileRepository.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    using SystemFileAdapter;

    using Entities.File;

    using FileRepository.Interfaces;

    using FileSystemInterfaces;

    public class ReportFileRepository : BaseFileRepository<ReportFile>, IReportFileRepository
    {
        public ReportFileRepository(String path, IFileInfoFactory fileInfo, IDirectoryInfo directoryInfo)
            : base(path, fileInfo, directoryInfo)
        {
        }

        public IList<ReportFile> GetReportFiles(String rptPath)
        {
            String[] files = new String[] { };

            if (Directory.Exists(rptPath))
            {
                string rptFileName = this.GetReportFileName(rptPath);
                if (File.Exists(rptFileName))
                {
                    files = File.ReadAllLines(rptFileName);
                }
            }

            var reportFiles = this.GetAllReportFiles(files);

            return reportFiles;
        }

        /// <summary>
        /// Get all reports from array of files
        /// </summary>
        /// <param name="files">string array of files </param>
        /// <returns>List of ReportFile</returns>
        private List<ReportFile> GetAllReportFiles(string[] files)
        {
            var reportFiles = new List<ReportFile>();
            String[] fileArray = new String[] { };
            String filePath = string.Empty;

            foreach (var file in files)
            {
                fileArray = file.Split(Convert.ToChar(','));
                filePath = fileArray[fileArray.Length - 1].Replace("\"", "");

                if (File.Exists(filePath))
                {
                    reportFiles.Add(new ReportFile(fileArray[0].Replace("\"", ""), filePath));
                }
            }

            if (reportFiles.Count == 0)
            {
                reportFiles.Add(new ReportFile("No Reports Available", "No Reports Available"));
            }
            return reportFiles;
        }

        /// <summary>
        /// Get report file name {GRID}-ReportFiles.web from output folder
        /// which contains list of report files
        /// </summary>
        /// <param name="outputFolderPath">output folder path</param>
        /// <returns>report file path+name</returns>
        private String GetReportFileName(String outputFolderPath)
        {
            var reportPathArray = outputFolderPath.Split(Convert.ToChar("\\"));
            var reportFileName = reportPathArray.Length > 1 ? reportPathArray[reportPathArray.Length - 2] : string.Empty;
            return string.Format("{0}\\{1}-ReportFiles.web", outputFolderPath, reportFileName);
        }


        public ReportFile GetReportFile(String filePath)
        {
            var reportFile = new ReportFile(this.GetFileName(filePath), filePath, File.ReadAllLines(filePath));

            return reportFile;
        }

        protected override ReportFile InstanceOfClass(String fileName, String filePath)
        {
            var reportFile = new ReportFile(fileName, filePath);

            return reportFile;
        }

        protected override String GenerateFileName()
        {
            return string.Format("{0}.rpt", this.Path);
        }

        private String GetFileName(String filePath)
        {
            var filePathArray = filePath.Split(Convert.ToChar("\\"));
            var fileName = filePathArray[filePathArray.Length - 1];

            return fileName;
        }
    }
}
