// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Class1.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Exceptions
{
  using System;
  using System.Runtime.Serialization;

  public class DocProcessingFileAlreadyProcessedException : Exception
  {
    public DocProcessingFileAlreadyProcessedException()
    {
    }

    public DocProcessingFileAlreadyProcessedException(string message)
      : base(message)
    {
    }

    public DocProcessingFileAlreadyProcessedException(string message, Exception inner)
      : base(message, inner)
    {
    }

    protected DocProcessingFileAlreadyProcessedException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}
