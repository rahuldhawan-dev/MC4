using System;
using System.Configuration;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using MMSINC.Authentication;

namespace MapCallIntranet.Helpers
{
    public static class LoginHelper
    {
        public static void DoLogin()
        {
            IAuthenticationService _authserv = DependencyResolver.Current.GetService<IAuthenticationService>();
            _authserv.SignIn(_authserv.GetUserId(ConfigurationManager.AppSettings["IntranetUser"]), isTokenAuthenticated: true);
        }
    }
}