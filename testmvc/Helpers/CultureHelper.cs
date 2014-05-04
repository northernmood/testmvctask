using System;
using System.Web;

namespace testmvc.Helpers
{
    public static class CultureHelper
    {
        private static string cultureCookieName = "_culture";

        public static string CultureCookieName
        {
            get
            {
                return cultureCookieName;
            }
        }

        public static string GetCurrentCulture(HttpRequestBase httpRequest, string defaultCulture)
        {
            string userCulture = defaultCulture;

            if(httpRequest.RequestContext.RouteData.Values["lang"] != null)
            {
                userCulture = httpRequest.RequestContext.RouteData.Values["lang"] as string;
            }

            return userCulture;
        }

        public static void SetCulture(string culture, HttpRequestBase request, HttpResponseBase response)
        {
            HttpCookie cookie = request.Cookies[CultureCookieName];

            if (cookie != null)
            {
                cookie.Value = culture;
            }
            else
            {
                cookie = new HttpCookie(CultureCookieName);
                cookie.Value = culture;
                cookie.Expires = DateTime.Now.AddYears(1);
            }

            response.Cookies.Add(cookie);
        }
    }
}