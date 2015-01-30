// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BuildMeA.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace BuildMeA
{
  using System;

  public class BuildMeA
  {
    public static ApplicationUserBuilder ApplicationUser(String userName)
    {
      return (ApplicationUserBuilder)new ApplicationUserBuilder().With(
        o =>
        {
          o.UserName = userName;
        });
    }

    public static PasswordHistoryBuilder PasswordHistory(String userId, String passwordHash)
    {
      return (PasswordHistoryBuilder)new PasswordHistoryBuilder().With(
        p =>
        {
          p.UserId = userId;
          p.PasswordHash = passwordHash;
          p.LogDate = DateTime.Now;
        });
    }

    public static GlobalSettingBuilder GlobalSetting(Int32 minPasswordLength, Int32 minNonAlphaChars, Int32 passwordExpDays, Int32 passChangeBefore, Boolean newUserPasswordReset)
    {
      return (GlobalSettingBuilder)new GlobalSettingBuilder().With(
        g =>
        {
          g.MinPasswordLength = minPasswordLength;
          g.MinNonAlphaChars = minNonAlphaChars;
          g.PasswordExpDays = passwordExpDays;
          g.PassChangeBefore = passChangeBefore;
          g.NewUserPasswordReset = newUserPasswordReset;
        });
    }

    public static JobBuilder Job(DateTime createDate, DateTime finishDate, DateTime submissionDate, String userId, String grid)
    {
      return (JobBuilder)new JobBuilder().With(
        j =>
          { 
            j.createDate = createDate;
            j.FinishDate = finishDate;
            j.SubmissionDate = submissionDate;
            j.UserID = userId;
            j.GRID = grid;
          });
    }

    public static JobStatusBuilder JobStatus(DateTime dateTime)
    {
      return (JobStatusBuilder)new JobStatusBuilder().With(
        j =>
          { j.DateTime = dateTime; });
    }

    public static JobStatusTypeBuilder JobStatusType(String jobStatusDescription)
    {
      return (JobStatusTypeBuilder)new JobStatusTypeBuilder().With(
        j =>
          { j.JobStatusDescription = jobStatusDescription; });
    }

    public static ManagementCompanyBuilder ManagementCompany(String manCoName, String manCoCode, String manCoShortName, String rufusDatabaseId)
    {
      return (ManagementCompanyBuilder)new ManagementCompanyBuilder().With(
        m =>
          {
            m.ManCoName = manCoName;
            m.ManCoCode = manCoCode;
            m.ManCoShortName = manCoShortName;
            m.RufusDatabaseID = rufusDatabaseId;
          });
    }

    public static ManCoDocBuilder ManCoDoc(Int32 manCoID, Int32 documentTypeID, String pubFileName, String version, Int32 envntID)
    {
      return (ManCoDocBuilder)new ManCoDocBuilder().With(
        m =>
          { 
            m.ManCoID = manCoID;
            m.DocumentTypeID = documentTypeID;
            m.PubFileName = pubFileName;
            m.Version = version;
            m.EnvironmentID = envntID;
          });
    }

    public static DocumentTypeBuilder DocumentType(String code, String name, String typeName, Boolean additionalSetup, Boolean agentDocument)
    {
      return (DocumentTypeBuilder)new DocumentTypeBuilder().With(
        m =>
          { 
            m.BravuraDocTypeCode = code;
            m.DocumentTypeName = name;
            m.AdditionalSetup = additionalSetup;
            m.AgentDocument = agentDocument;
          });
    }

    public static ParentCompanyBuilder ParentCompany(String parentCompanyName)
    {
      return (ParentCompanyBuilder)new ParentCompanyBuilder().With(
        p =>
          { p.ParentCompany_Name = parentCompanyName; });
    }

    public static AppEnvironmentBuilder AppEnvironment(String Name)
    {
      return (AppEnvironmentBuilder)new AppEnvironmentBuilder().With(a =>
        {
          a.Name = Name;
        });
    }

    public static GroupBuilder Group(String GroupName)
    {
      return (GroupBuilder)new GroupBuilder().With(a =>
        {
          a.GroupName = GroupName;
        });
    }

    public static GroupDataRightBuilder GroupDataright(Int32 manCoId)
    {
      return (GroupDataRightBuilder)new GroupDataRightBuilder().With(a =>
        {
          a.ManCoDocID = manCoId;
        });
    }

    public static EnvironmentTypeBuilder EnvironmentType(String environmentType, String processingServerName, String ADF_DB_Name, Int32 appEnvironmentId, String bravuraDOCSEnvironmentType, String bravuraDOCSEnvironmentChar, String environmentChar)
    {
      return (EnvironmentTypeBuilder)new EnvironmentTypeBuilder().With(a =>
        {
          a.EnvironmentType1 = environmentType;
          a.ProcessingServerName = processingServerName;
          a.ADF_DB_Name = ADF_DB_Name;
          a.AppEnvironmentID = appEnvironmentId;
          a.BravuraDOCSEnvironmentType = bravuraDOCSEnvironmentType;
          a.BravuraDOCSEnvironmentChar = bravuraDOCSEnvironmentChar;
          a.EnvironmentChar = environmentChar;
        });
    }
  }
}
