using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebMatrix.WebData;

namespace testmvc.WebSecurityImpl
{
    public class WebSecurityWrapper : IWebSecurityWrapper
    {
        public bool Login(string userName, string password, bool persistCookie = false)
        {
            return WebSecurity.Login(userName, password, persistCookie);
        }

        public void Logout()
        {
            WebSecurity.Logout();
        }

        public string CreateUserAndAccount(string userName, string password, object propertyValues = null, bool requireConfirmationToken = false)
        {
            return WebSecurity.CreateUserAndAccount(userName, password, propertyValues, requireConfirmationToken);
        }


        public int CurrentUserId
        {
            get { return WebSecurity.CurrentUserId; }
        }
    }
}