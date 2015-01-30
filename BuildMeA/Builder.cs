// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Builder.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace BuildMeA
{
  using System;

  public class Builder<T> where T : new()
  {
    protected static T Instance;

    protected Builder()
    {
      new T();
    }

    public Builder<T> With(Action<T> action)
    {
      action(Instance);
      return this;
    }

    public static implicit operator T(Builder<T> builder)
    {
      return builder.This();
    }

    public T This()
    {
      return Instance;
    }
  }
}
