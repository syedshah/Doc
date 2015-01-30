// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FakeRequest.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   Defines the FakeRequest type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace WebTests.Helpers
{
  using System;
  using System.Collections.Specialized;
  using System.Web;

  public class FakeRequest : HttpRequestBase
  {
    public readonly NameValueCollection Values = new NameValueCollection();

    private readonly HttpCookieCollection cookies = new HttpCookieCollection();

    private Boolean secureConnection = true;

    private Boolean local = true;

    public FakeRequest()
    {
    }

    public override HttpCookieCollection Cookies
    {
      get
      {
        return this.cookies;
      }
    }

    public override Boolean IsLocal
    {
      get
      {
        return this.local;
      }
    }

    public override Boolean IsSecureConnection
    {
      get
      {
        return this.secureConnection;
      }
    }

    public override NameValueCollection Headers
    {
      get
      {
        return this.Values;
      }
    }

    public override String this[String key]
    {
      get
      {
        return this.Values[key];
      }
    }
  }
}
