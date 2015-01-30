// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OneStepFileRepository.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   Defines the OneStepFileRepository type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FileRepository.Repositories
{
  using System;
  using System.IO;
  using System.Xml.Serialization;
  using SystemFileAdapter;
  using Entities;
  using FileRepository.Interfaces;
  using FileSystemInterfaces;

  public class SubmitJobFileRepository : BaseFileRepository<Object>, ISubmitJobFileRepository
  {
    public SubmitJobFileRepository(String path, IFileInfoFactory fileInfo, IDirectoryInfo directoryInfo)
      : base(path, fileInfo, directoryInfo)
    {   
    }

    public void SaveTriggerFile(Int32 jobId, SubmitFile submitFile, String path)
    {
      String fileName = String.Format(@"{0}\SubmitFile{1}.xml", path, jobId);

      // add to classes and serialize
      XmlSerializer SubmitFileSerializer = new XmlSerializer(typeof(SubmitFile));

      TextWriter XmlFile = new StreamWriter(fileName);

      SubmitFileSerializer.Serialize(XmlFile, submitFile);
      XmlFile.Close();
    }

    protected override String GenerateFileName()
    {
      return string.Format("{0}.log", this.Path);
    }

    protected override Object InstanceOfClass(String fileName, String filePath)
    {
      return new Object();
    }
  }
}
