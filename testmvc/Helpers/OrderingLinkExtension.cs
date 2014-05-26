using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace testmvc.Helpers
{
    public static class OrderingLinkExtension
    {
        public static MvcHtmlString OrderingLink(this HtmlHelper helper, string text, string orders, string ordered)
        {
            if (ordered == null) ordered = "";

            string tmplt = "<a href=\"#\" onclick=\"loadContent('{0}', 0)\">{1}{2}</a>";
            string result;
            if (ordered.Replace("_desc", "") == orders)
            {
                if (ordered.EndsWith("_desc"))
                    result = string.Format(tmplt, orders, text, " v");
                else
                    result = string.Format(tmplt, orders + "_desc", text, " ^");
            }
            else
                result = string.Format(tmplt, orders, text, "");

            return new MvcHtmlString(result);
        }
    }
}