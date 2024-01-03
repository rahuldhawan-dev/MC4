using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Okta.AspNet;
using System.Web.Mvc;
using MapCallIntranet.Configuration;
using MMSINC.Authentication;
using StructureMap;

namespace MapCallIntranet.Controllers
{
    public class AccountController : Controller
    {
        [HttpPost]
        public ActionResult Logout()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                HttpContext.GetOwinContext().Authentication.SignOut(
                    CookieAuthenticationDefaults.AuthenticationType,
                    OktaDefaults.MvcAuthenticationType);
            }

            return RedirectToAction("New", "NearMiss");
        }
    }
}