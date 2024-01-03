using System;
using System.ComponentModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Services;

namespace MapCall.public1
{
    /// <summary>
    /// Summary description for UserService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    public class UserService : WebService
    {

        [WebMethod]
        public bool VerifyToken(Guid tokenId)
        {
            return true;
            //if (tokenId == Guid.Empty)
            //{
            //    return false;
            //}

            //var userName = GetUsernameFromTokenId(tokenId);

            //if (string.IsNullOrEmpty(userName))
            //{
            //    return false;
            //}

            //return System.Web.Security.Membership.GetUser(userName).IsOnline;
        }

        private static string GetUsernameFromTokenId(Guid tokenId)
        {
            var userName = string.Empty;

            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MCProd"].ConnectionString))
            {
                using (var com = conn.CreateCommand())
                {
                    com.CommandText = "SELECT perm.UserName FROM [tblPermissions] perm WHERE TokenID = @TokenID";
                    com.Parameters.AddWithValue("TokenID", tokenId);

                    conn.Open();

                    using (var reader = com.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            userName = reader.GetString(reader.GetOrdinal("UserName"));
                        }
                    }
                }
            }

            return userName;
        }

    }
}
