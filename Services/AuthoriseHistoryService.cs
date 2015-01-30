// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuthoriseHistoryService.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Services
{
  using System;
  using System.Collections.Generic;

  using DocProcessingRepository.Interfaces;

  using Exceptions;

  using ServiceInterfaces;

  public class AuthoriseHistoryService : IAuthoriseHistoryService
  {
    private readonly IAuthoriseHistoryRepository authoriseHistoryRepository;

    public AuthoriseHistoryService(IAuthoriseHistoryRepository authoriseHistoryRepository)
    {
      this.authoriseHistoryRepository = authoriseHistoryRepository;
    }

    public void InsertAuthorisation(Int32 jobId, String userId, String comment)
    {
      try
      {
        this.authoriseHistoryRepository.InsertAuthorisation(jobId, userId, comment);
      }
      catch (Exception e)
      {
        throw new DocProcessingException("Unable to insert authorisation", e);
      }
    }

    public void InsertAuthorisations(IList<Int32> jobIds, String userId, String comment)
    {
      try
      {
        foreach (var jobId in jobIds)
        {
          this.authoriseHistoryRepository.InsertAuthorisation(jobId, userId, comment);
        }
      }
      catch (Exception e)
      {
        throw new DocProcessingException("Unable to insert authorisations", e);
      }
    }
  }
}
