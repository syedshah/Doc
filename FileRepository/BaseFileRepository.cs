// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseFileRepository.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   Defines the BaseFileRepository type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FileRepository
{
  using System;
  using System.Collections.Generic;
  using System.IO;

  using SystemFileAdapter;

  using FileSystemInterfaces;

  public abstract class BaseFileRepository<T> where T : class
  {
    protected readonly String Path;

    protected readonly IFileInfoFactory FileInfoFactory;

    protected readonly IDirectoryInfo DirectoryInfo;

    protected BaseFileRepository(String path, IFileInfoFactory fileInfo, IDirectoryInfo directoryInfo)
    {
      this.Path = path;
      this.FileInfoFactory = fileInfo;
      this.DirectoryInfo = directoryInfo;
    }

    public IEnumerable<FileInfo> Entities
    {
      get
      {
        return this.DirectoryInfo.EnumerateFiles(this.Path, "*.*");
      }
    }

    public Byte[] Create(Byte[] entity)
    {
      var fileName = this.GenerateFileName();
      try
      {
        IFileInfo fileInfo = this.FileInfoFactory.CreateFileInfo(fileName);

        using (var stream = fileInfo.Create())
        {
          stream.Write(entity, 0, entity.Length);
        }
        return entity;
      }
      catch (IOException e)
      {
        throw new Exception(String.Format("Unable to create entry with filename: {0}", fileName));
      }
      catch (Exception e)
      {
        throw new Exception("Unable to create entity", e);
      }
    }

    public void Delete(String fileName)
    {
      if (File.Exists(fileName))
      {
        File.Delete(fileName);
      }
    }

    public IList<T> GetFiles(String filePath, String extension) 
    {
      String[] files = new String[] { };

      if (Directory.Exists(filePath))
      {
        files = Directory.GetFiles(filePath, extension);
      }

      var genericFiles = new List<T>();

      foreach (var file in files)
      {
        genericFiles.Add(this.InstanceOfClass(this.GetFileName(file), file));
      }

      return genericFiles;
    }

    public void Dispose()
    {
    }

    private String GetFileName(String filePath)
    {
      var filePathArray = filePath.Split(Convert.ToChar("\\"));
      var fileName = filePathArray[filePathArray.Length - 1];

      return fileName;
    }

    protected abstract T InstanceOfClass(String fileName, String filePath);

    protected abstract String GenerateFileName();
  }
}
