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

  public class DocProcessingException : Exception
  {
    public DocProcessingException()
    {
    }

    public DocProcessingException(string message)
      : base(message)
    {
    }

    public DocProcessingException(string message, Exception inner)
      : base(message, inner)
    {
    }

    protected DocProcessingException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}
