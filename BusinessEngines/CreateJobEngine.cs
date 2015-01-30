namespace BusinessEngines
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using System.Linq;
  using System.Xml.Serialization;
  using BusinessEngineInterfaces;
  using DocProcessingRepository.Interfaces;
  using Entities;
  using Exceptions;
  using FileRepository.Interfaces;
  using ServiceInterfaces;

  using IInputFileRepository = DocProcessingRepository.Interfaces.IInputFileRepository;

  public class CreateJobEngine : ICreateJobEngine
  {
    private readonly IJobRepository jobRepository;

    private readonly IManCoDocRepository manCoDocRepository;

    private readonly IEnvironmentTypeService environmentTypeService;

    private readonly IFilePathBuilderEngine filePathBuilderEngine;

    private readonly IInputFileRepository inputFileRepository;

    private readonly ISubmitJobFileRepository submitJobFileRepository;

    private readonly IJobStatusTypeRepository jobStatusTypeRepository;

    public CreateJobEngine(
      IJobRepository jobRepository,
      IManCoDocRepository manCoDocRepository,
      IEnvironmentTypeService environmentTypeService,
      IFilePathBuilderEngine filePathBuilderEngine,
      IInputFileRepository inputFileRepository,
      ISubmitJobFileRepository submitJobFileRepository,
      IJobStatusTypeRepository jobStatusTypeRepository)
    {
      this.jobRepository = jobRepository;
      this.manCoDocRepository = manCoDocRepository;
      this.environmentTypeService = environmentTypeService;
      this.filePathBuilderEngine = filePathBuilderEngine;
      this.inputFileRepository = inputFileRepository;
      this.submitJobFileRepository = submitJobFileRepository;
      this.jobStatusTypeRepository = jobStatusTypeRepository;
    }

    public void SubmitJob(String environment, String manCo, String docTypeCode, String docTypeName, List<String> files, String userId, Boolean allowReprocessing, Boolean requiresAdditionalSetUp, String fundInfo, String sortCode, String accountNumber, String chequeNumber, String selectedFolder)
    {
      String inputPath;
      var enviroments = this.GetEnvironmentInfo(environment, manCo, selectedFolder, out inputPath);

      this.CheckForReprocessing(files, allowReprocessing, inputPath);

      var mancoDoc = this.GetManCoDocInfo(environment, manCo, docTypeCode, docTypeName);

      var jobStatusType = this.GetJobStatusType("Submitted");

      var jobId = this.SaveJob(userId, requiresAdditionalSetUp, fundInfo, sortCode, accountNumber, chequeNumber, mancoDoc, jobStatusType.JobStatusTypeID);

      var submitFile = this.SaveFiles(manCo, docTypeCode, files, inputPath, jobId, mancoDoc, enviroments);

      this.CreateMonitorFile(files, enviroments, submitFile, jobId, inputPath);
    }

    private void CreateMonitorFile(List<String> files, IEnumerable<EnvironmentServerEntity> enviroments, SubmitFile submitFile, Int32 jobId, String inputPath)
    {
      var monitorPath = this.filePathBuilderEngine.BuildOneStepMonitorPath(enviroments.Single().ServerName);
      submitFile.AddFiles(files, inputPath);

      this.submitJobFileRepository.SaveTriggerFile(jobId, submitFile, monitorPath);
    }

    private SubmitFile SaveFiles(
      String manCo, String docTypeCode, IEnumerable<String> files, String inputPath, Int32 jobId, ManCoDoc mancoDoc, IList<EnvironmentServerEntity> enviroments)
    {
      foreach (var file in files)
      {
        try
        {
          this.inputFileRepository.InsertFile(file, inputPath, jobId, mancoDoc.ManCoDocID);
        }
        catch (Exception e)
        {
          throw new DocProcessingException("Unable to insert input file", e);
        }
      }

      var submitFile = new SubmitFile
                         {
                           JobID = jobId.ToString(),
                           ManCoCode = manCo,
                           DocType = docTypeCode,
                           Environment = enviroments.Single().EnvironmentType,
                           EnvironmentID = enviroments.Single().EnvironmentTypeId.ToString()
                         };
      return submitFile;
    }

    private Int32 SaveJob(
      String userId,
      Boolean requiresAdditionalSetUp,
      String fundInfo,
      String sortCode,
      String accountNumber,
      String chequeNumber,
      ManCoDoc mancoDoc,
      Int32 jobStatusTypeId)
    {
      Int32 jobId;

      try
      {
        String additionalInfo = String.Empty;

        if (requiresAdditionalSetUp)
        {
          var additionalSetupInfo = new AdditionalSetupInfo
                                      {
                                        AccountNumber = accountNumber,
                                        ChequeNumber = chequeNumber,
                                        SortCode = sortCode,
                                        FundInfo = fundInfo
                                      };

          var xmlSerializer = new XmlSerializer(additionalSetupInfo.GetType());
          var textWriter = new StringWriter();

          xmlSerializer.Serialize(textWriter, additionalSetupInfo);
          additionalInfo = textWriter.ToString();
        }

        jobId = this.jobRepository.InsertJob(mancoDoc.ManCoDocID, String.Empty, additionalInfo, userId, jobStatusTypeId);
      }
      catch (Exception e)
      {
        throw new DocProcessingException("Unable to insert job", e);
      }

      return jobId;
    }

    private ManCoDoc GetManCoDocInfo(String environment, String manCo, String docTypeCode, String docTypeName)
    {
      ManCoDoc mancoDoc;
      try
      {
        mancoDoc = this.manCoDocRepository.GetManCoDoc(manCo, docTypeCode, docTypeName, environment);
      }
      catch (Exception e)
      {
        throw new DocProcessingException("Unable to retrieve man co doc", e);
      }

      return mancoDoc;
    }

    private void CheckForReprocessing(IEnumerable<String> files, Boolean allowReprocessing, String inputPath)
    {
      if (allowReprocessing)
      {
        return;
      }

      foreach (var file in files)
      {
        try
        {
          var inputFile = this.inputFileRepository.GetInputFile(file, inputPath);

          if (inputFile.Count > 0)
          {
            throw new DocProcessingFileAlreadyProcessedException(
              "Unable to run job as the job contains a file that has already been processed");
          }
        }
        catch (DocProcessingFileAlreadyProcessedException)
        {
          throw;
        }
        catch (Exception e)
        {
          throw new DocProcessingException("Unable to search for input files", e);
        }
      }
    }

    private IList<EnvironmentServerEntity> GetEnvironmentInfo(String environment, String manCo, String docTypeCode, out String inputPath)
    {
      IList<EnvironmentServerEntity> enviroments = this.environmentTypeService.GetEnvironmentServers(environment);

      if (enviroments.Count > 1)
      {
        throw new DocProcessingException(
          String.Format("More than one processing enviroment available for {0}", environment));
      }

      inputPath = this.filePathBuilderEngine.BuildInputFileLocationPath(enviroments.Single().ServerName, manCo, docTypeCode);
      return enviroments;
    }

    private JobStatusTypeEntity GetJobStatusType(String jobStatusTypeDescription)
    {
      try
      {
        return this.jobStatusTypeRepository.GetJobStatusType(jobStatusTypeDescription);
      }
      catch (Exception e)
      {
        throw new DocProcessingException("Unable to get job status type", e);
      }
    }
  }
}
