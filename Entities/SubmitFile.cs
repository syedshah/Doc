namespace Entities
{
  using System;
  using System.Collections.Generic;
  using System.Xml.Serialization;

  [Serializable]
  [XmlRootAttribute("SubmitFile", IsNullable = false)]
  public class SubmitFile
  {
    public SubmitFile()
    {
      SuppliedFiles = new List<SuppliedFile>();
    }

    [XmlIgnore]
    public String SubmitFilename;

    public String JobID;
    public String ManCoCode;
    public String DocType;
    public String nmbbs01GRID;
    public String EnvironmentID;
    public String Environment;

    //List of Files Supplied
    [XmlArrayAttribute("Files")]
    public List<SuppliedFile> SuppliedFiles;


    public void AddFiles(List<String> files, String path)
    {
      foreach (var file in files)
      {
        this.SuppliedFiles.Add(new SuppliedFile()
                            {
                              FileName = String.Format(@"{0}\{1}", path, file),
                              FileType = "DataFile"
                            });
      }
    }
  }

  /// <summary>
  /// defintion of files that are supplied with the submit file for manual jobs
  /// </summary>
  [XmlRoot("SuppliedFile", IsNullable = false)]
  public class SuppliedFile
  {
    public String FileType;
    public String FileName;
  }
}
