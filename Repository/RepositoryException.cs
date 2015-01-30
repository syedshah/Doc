// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RepositoryException.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Repository
{
  using System;
  using System.Runtime.Serialization;

  [Serializable]
  public class RepositoryException : Exception
  {
    public RepositoryException()
    {
    }

    public RepositoryException(String message)
      : base(message)
    {
    }

    public RepositoryException(String message, Exception inner)
      : base(message, inner)
    {
    }

    protected RepositoryException(
        SerializationInfo info,
        StreamingContext context)
      : base(info, context)
    {
    }
  }
}
