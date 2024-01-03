using System;
using System.Configuration;
using System.Data.SqlClient;
using MMSINC.Testing.Selenium;
using Selenium;
using Config = MMSINC.Testing.Utilities.RegressionTestConfiguration;

namespace MapCall.Common.Testing.Selenium.TestParts
{
    public static class Login
    {
        #region Constants

        public const string LOGIN_URL = "/impersonate.aspx";

        public struct NecessaryIDs
        {
            public const string BASE = "content_cphContent_";

            public const string USER_NAME_FIELD = BASE + "Login1_UserName",
                                PASSWORD_FIELD = BASE + "Login1_Password",
                                LOGIN_BUTTON = BASE + "Login1_Login";
        }

        public struct Credentials
        {
            public const string ADMIN_USER_NAME = "mcadmin",
                                USER_USER_NAME = "mcuser",
                                DISTRO_USER_NAME = "mcdistro",
                                ADMIN_PASSWORD = "rabbitcamry#1",
                                ADMIN_FULL_NAME = "MapCall Developer";
        }

        #endregion

        #region Public Static Methods

        public static string AsAdmin(ISelenium selenium)
        {
            DoLogin(selenium, Credentials.ADMIN_USER_NAME,
                Credentials.ADMIN_PASSWORD);
            VerifyLoginSuccess(selenium);
            return GetUsersFullName(Credentials.ADMIN_USER_NAME, Credentials.ADMIN_FULL_NAME);
        }

        public static string AsUser(ISelenium selenium)
        {
            DoLogin(selenium, Credentials.USER_USER_NAME,
                Credentials.ADMIN_PASSWORD);
            VerifyLoginSuccess(selenium);
            return GetUsersFullName(Credentials.USER_USER_NAME, Credentials.ADMIN_FULL_NAME);
        }

        public static string AsDistro(ISelenium selenium)
        {
            DoLogin(selenium, Credentials.DISTRO_USER_NAME,
                Credentials.ADMIN_PASSWORD);
            //VerifyDistroLoginSuccess(selenium);

            return GetUsersFullName(Credentials.DISTRO_USER_NAME, Credentials.ADMIN_FULL_NAME);
        }

        #endregion

        #region Private Static Methods

        private static void DoLogin(ISelenium selenium, string userName, string password)
        {
            // sometimes selenium will visit the wrong URL due to some race condition or another.
            // Jason and Ross have both witnessed this issue. Retry the login with a longer timeout
            // just to be on the safe side.
            try
            {
                InnerDoLogin(selenium, userName, password, 5000);
            }
            catch (Exception)
            {
                InnerDoLogin(selenium, userName, password, 10000);
            }
        }

        private static void InnerDoLogin(ISelenium selenium, string userName, string password, int timeOut)
        {
            selenium.Open(Config.GetDevSiteUri() + LOGIN_URL + $"?username={userName}");
            //  selenium.WaitForPageToLoad(timeOut.ToString());
            //selenium.AssertElementPresent(NecessaryIDs.USER_NAME_FIELD);
            // selenium.AssertElementPresent(NecessaryIDs.PASSWORD_FIELD);
            // selenium.Type(NecessaryIDs.USER_NAME_FIELD, userName);
            //  selenium.Type(NecessaryIDs.PASSWORD_FIELD, password);
            //  selenium.ClickAndWaitForPageToLoad(NecessaryIDs.LOGIN_BUTTON, timeOut.ToString());
        }

        private static void VerifyLoginSuccess(ISelenium selenium)
        {
            // this is a very loose verification, so not using constants here
            selenium.AssertTextPresent("Operations");
            // selenium.AssertElementPresent("css=div.container.subContainer > div.header");
        }

        private static void VerifyDistroLoginSuccess(ISelenium selenium)
        {
            selenium.AssertElementPresent("btnHumanResources");
        }

        private static string GetUsersFullName(string username, string defaultNameIfNull)
        {
            // MapCall proper tests don't need this value and doesn't have an app.config configuration for this.
            // Rather than force MapCall proper to implement this(when the project itself should be slowly dying),
            // just return null here and use some default value.
            if (ConfigurationManager.ConnectionStrings["MCProd"] == null)
            {
                return defaultNameIfNull;
            }

            // 271 *does* need this value for a whole mess of tests and is already configured to do direct database
            // manipulations. We need to look this up because of name randomization on tblPermissions.
            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MCProd"].ConnectionString))
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "select Fullname from tblPermissions where UserName = @username";
                var param = cmd.CreateParameter();
                param.ParameterName = "username";
                param.Value = username;
                cmd.Parameters.Add(param);
                conn.Open();
                var result = (string)cmd.ExecuteScalar();
                return result;
            }
        }

        #endregion
    }
}
