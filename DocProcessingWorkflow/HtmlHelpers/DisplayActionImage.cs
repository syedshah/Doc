// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DisplayActionImage.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DocProcessingWorkflow.HtmlHelpers
{
  using System;
  using System.Web.Mvc;

  public static class DisplayActionImage
  {
    public static MvcHtmlString ActionImage(this HtmlHelper html, String href, String imagePath, String alt = null, String cssClass = null, String imgClass = null)
    {
      // build the <img> tag
      var imgBuilder = new TagBuilder("img");
      imgBuilder.MergeAttribute("src", imagePath);
      if (alt != null)
      {
        imgBuilder.MergeAttribute("alt", alt);
      }
      if (imgClass != null)
      {
          imgBuilder.MergeAttribute("class", imgClass);
      }

      String imgHtml = imgBuilder.ToString(TagRenderMode.SelfClosing);

      // build the <a> tag
      var anchorBuilder = new TagBuilder("a");

      anchorBuilder.MergeAttribute("href", href);
      if (cssClass != null)
      {
        anchorBuilder.MergeAttribute("class", cssClass);
      }

      anchorBuilder.InnerHtml = imgHtml; // include the <img> tag inside
      String anchorHtml = anchorBuilder.ToString(TagRenderMode.Normal);

      return MvcHtmlString.Create(anchorHtml);
    }
  }
}