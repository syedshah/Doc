// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AppConfigConfigurationManager.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ConfigurationManagerWrapper
{
  using System;
  using System.Collections.Specialized;
  using System.Configuration;
  using AbstractConfigurationManager;

  public class AppConfigConfigurationManager : IConfigurationManager
  {
    public NameValueCollection AppSettings
    {
      get { return ConfigurationManager.AppSettings; }
    }

    public String AppSetting(String name)
    {
      return ConfigurationManager.AppSettings[name];
    }
  }
}
