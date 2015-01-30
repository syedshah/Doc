// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DocProcessingDirectoryDoesntExistException.cs" company="DST Nexdox">
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

  public class DocProcessingDirectoryDoesntExistException : Exception
  {
    public DocProcessingDirectoryDoesntExistException()
    {
    }

    public DocProcessingDirectoryDoesntExistException(string message)
      : base(message)
    {
    }

    public DocProcessingDirectoryDoesntExistException(string message, Exception inner)
      : base(message, inner)
    {
    }

    protected DocProcessingDirectoryDoesntExistException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}
