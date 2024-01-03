using System;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using StructureMap;

namespace MMSINC.Authentication
{
    public class BasicAuthHttpModule : IHttpModule
    {
        #region Constants

        public const string REALM = "MapCall";

        #endregion

        #region Properties

        protected static IAuthenticationService AuthenticationService =>
            DependencyResolver.Current.GetService<IAuthenticationService>();

        protected static IMembershipHelper MembershipHelper =>
            DependencyResolver.Current.GetService<IMembershipHelper>();

        #endregion

        #region Private Methods

        private static bool CheckPassword(string username, string password)
        {
            return MembershipHelper.ValidateUser(username, password);
        }

        private static void AuthenticateUser(string credentials)
        {
            try
            {
                var encoding = Encoding.GetEncoding("iso-8859-1");
                credentials = encoding.GetString(Convert.FromBase64String(credentials));

                int separator = credentials.IndexOf(':');
                string name = credentials.Substring(0, separator);
                string password = credentials.Substring(separator + 1);

                if (CheckPassword(name, password))
                {
                    var userId = AuthenticationService.GetUserId(name);
                    AuthenticationService.SignIn(userId, true);
                }
                else
                {
                    // Invalid username or password.
                    HttpContext.Current.Response.StatusCode = 401;
                }
            }
            catch (FormatException)
            {
                // Credentials were not formatted correctly.
                HttpContext.Current.Response.StatusCode = 401;
            }
        }

        #endregion

        #region Event Handlers

        private static void OnApplicationAuthenticateRequest(object sender, EventArgs e)
        {
            var request = HttpContext.Current.Request;
            var authHeader = request.Headers["Authorization"];
            if (authHeader != null)
            {
                var authHeaderVal = AuthenticationHeaderValue.Parse(authHeader);

                // RFC 2617 sec 1.2, "scheme" name is case-insensitive
                if (authHeaderVal.Scheme.Equals("basic",
                        StringComparison.OrdinalIgnoreCase) &&
                    authHeaderVal.Parameter != null)
                {
                    AuthenticateUser(authHeaderVal.Parameter);
                }
            }
        }

        // If the request was unauthorized, add the WWW-Authenticate header 
        // to the response.
        private static void OnApplicationEndRequest(object sender, EventArgs e)
        {
            var response = HttpContext.Current.Response;
            if (response.StatusCode == 401)
            {
                response.Headers.Add("WWW-Authenticate",
                    $"Basic realm=\"{REALM}\"");
            }
        }

        #endregion

        #region Exposed Methods

        public void Init(HttpApplication context)
        {
            // Register event handlers
            context.AuthenticateRequest += OnApplicationAuthenticateRequest;
            context.EndRequest += OnApplicationEndRequest;
        }

        public void Dispose() { }

        #endregion
    }
}
