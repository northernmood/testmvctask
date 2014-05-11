using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testmvc.WebSecurityImpl
{
    public interface IWebSecurityWrapper
    {
        bool Login(string userName, string password, bool persistCookie = false);
        void Logout();

        int CurrentUserId { get; }

        string CreateUserAndAccount(string userName, string password, object propertyValues = null, bool requireConfirmationToken = false);
    }
}
