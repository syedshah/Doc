// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IConfigurationManager.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace AbstractConfigurationManager
{
  using System;
  using System.Collections.Specialized;

  public interface IConfigurationManager
  {
    String AppSetting(String name);

    NameValueCollection AppSettings { get; }
  }
}
