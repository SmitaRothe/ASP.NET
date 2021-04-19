using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarRentalSystem.Helper
{
    public static class HtmlExtensions
    {
        public static IHtmlString GetThumbnail(this HtmlHelper helper, byte[] photo,
                                           int thumbWidth = 120, int thumbHeight = 120)
        {
            Image image = ImageConverter.ConvertToThumbnail(photo, thumbWidth, thumbHeight);
            MemoryStream ms = new MemoryStream();
            image.Save(ms, ImageFormat.Jpeg);
            return GetImage(helper, ms.ToArray(), thumbWidth, thumbHeight);

        }

        private static IHtmlString GetImage(HtmlHelper helper, byte[] photo, int?
                                             thumbWidth, int? thumbHeight)
        {
            var builder = new TagBuilder("img");
            builder.MergeAttribute("src", String.Format("data:image/jpeg;base64,{0}",
                                                  Convert.ToBase64String(photo)));
            if (thumbHeight.HasValue)
                builder.MergeAttribute("height", thumbHeight.Value.ToString());
            if (thumbWidth.HasValue)
                builder.MergeAttribute("width", thumbWidth.ToString());
            return MvcHtmlString.Create(builder.ToString(TagRenderMode.SelfClosing));
        }

        public static IHtmlString GetArrow(this HtmlHelper helper, string currentSortBy, string sortBy, string sortDir)
        {
            if (currentSortBy == sortBy)
            {
                if (sortDir == "asc")
                    return helper.Raw(@"<span>&uarr;</span>");
                else
                    return helper.Raw(@"<span>&darr;</span>");
            }
            return helper.Raw("");
        }

    }
}