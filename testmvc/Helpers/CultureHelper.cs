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

            HttpCookie cookie = httpRequest.Cookies[CultureCookieName];

            if (cookie != null && !string.IsNullOrEmpty(cookie.Value))
            {
                userCulture = cookie.Value;
            }
            else
            {
                try
                {
                    userCulture = httpRequest.UserLanguages[0].Substring(0, 2);
                }
                catch { }
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