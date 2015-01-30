// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ManCoDocBuilder.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   ManCoDoc Builder
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace BuildMeA
{
  using Entities;

  public class ManCoDocBuilder : Builder<ManCoDoc>
  {
    public ManCoDocBuilder()
    {
      Instance = new ManCoDoc();
    }
  }
}
