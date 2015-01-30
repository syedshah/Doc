// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FakeResponse.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   Defines the FakeResponse type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace WebTests.Helpers
{
  using System;
  using System.Collections.Specialized;
  using System.Web;

  using Moq;

  public class FakeResponse : HttpResponseBase
  {
    // Routing calls this to account for cookieless sessions
    // It's irrelevant for the test, so just return the path unmodified
    private readonly HttpCookieCollection cookies = new HttpCookieCollection();

    private readonly NameValueCollection headers = new NameValueCollection();

    public override Int32 StatusCode { get; set; }

    public override HttpCookieCollection Cookies
    {
      get
      {
        return this.cookies;
      }
    }

    public override HttpCachePolicyBase Cache
    {
      get
      {
        var mock = new Mock<HttpCachePolicyBase>();
        return mock.Object;
      }
    }

    public override NameValueCollection Headers
    {
      get
      {
        return this.headers;
      }
    }

    public override String ApplyAppPathModifier(String x)
    {
      return x;
    }

    public override void AddHeader(String name, String value)
    {
      this.headers.Add(name, value);
    }
  }
}
