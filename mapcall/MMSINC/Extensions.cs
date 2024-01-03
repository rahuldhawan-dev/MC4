using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Principal;
using System.Web.UI.HtmlControls;

namespace MMSINC.Extensions
{
    public static class User
    {
        #region Constants

        private struct SQL
        {
            internal const string BOOLEANS =
                "select IsAdministrator, IsUserAdministrator, isProductionUser, isProductionAdministrator, isProductionOnly from tblPermissions where Username = @Username";
        }

        #endregion

        #region Private Methods

        private static bool CheckProperty(string name, string ordinal)
        {
            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MCProd"].ConnectionString))
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = SQL.BOOLEANS;
                cmd.Parameters.AddWithValue("Username", name);
                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    if (!reader.Read())
                    {
                        throw new InvalidOperationException("The current user is not in the Database: " + name);
                    }
                    return reader.GetBoolean(reader.GetOrdinal(ordinal));
                }
            }
        }

        #endregion

        #region Extension Methods

        /// <summary>
        /// Don't confuse this with IsAdmin.
        /// There are a lot more users in this group. 
        /// Originated from original mapcall's distinction between
        /// regular users and administrators before they just made
        /// everyone administrators 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static bool IsAdministrator(this IPrincipal user)
        {
            return CheckProperty(user.Identity.Name, "IsAdministrator");
        }

        /// <summary>
        /// Users in this role are able to administrate other users. 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static bool IsUserAdministrator(this IPrincipal user)
        {
            return CheckProperty(user.Identity.Name, "IsUserAdministrator");
        }

        /// <summary>
        /// User can access the Production system as a user.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static bool IsProductionUser(this IPrincipal user)
        {
            return CheckProperty(user.Identity.Name, "IsProductionUser");
        }

        /// <summary>
        /// User can access the Production system as an administrator.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static bool IsProductionAdministrator(this IPrincipal user)
        {
            return CheckProperty(user.Identity.Name, "IsProductionAdministrator");
        }

        /// <summary>
        /// User should not have access to anything but the production system.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static bool IsProductionOnly(this IPrincipal user)
        {
            return CheckProperty(user.Identity.Name, "IsProductionOnly");
        }

    #endregion
    }

    public static class Controls
    {
        #region Constants

        private const string CLIENT_CLICK_EVENT_NAME = "onclick";

        #endregion

        #region Extension Methods

        public static void SetClientClickHandler(this HtmlInputButton btn, string script)
        {
            btn.Attributes.Add(CLIENT_CLICK_EVENT_NAME, script);
        }

        #endregion
    }
}
