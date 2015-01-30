// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DocTypeJsonResponse.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DocProcessingWorkflow.Models.DocType
{
  using System;
  using System.Collections.Generic;

  public class DocTypeJsonResponse
  {
    public Exception exception;

    public String message;

    public String success;

    public String url;

    private List<DocTypeViewModel> docTypes = new List<DocTypeViewModel>();

    public List<DocTypeViewModel> DocTypes
    {
      get { return docTypes; }
      set { docTypes = value; }
    }

    public void AddDocTypes(IList<Entities.DocumentType> docTypes)
    {
      foreach (Entities.DocumentType docType in docTypes)
      {
        var avm = new DocTypeViewModel(docType);
        this.DocTypes.Add(avm);
      }
    }
  }
}