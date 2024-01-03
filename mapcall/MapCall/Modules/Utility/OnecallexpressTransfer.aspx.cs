using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace MapCall.Modules.Utility
{
    public partial class OnecallexpressTransfer : Page
    {
        #region Constants

        private const string GET_TOKENID = "SELECT TokenID from tblPermissions where UserName = @userName";
        private const string ONECALLEXPRESS_LINK = "https://njw.onecallexpress.com/auth.aspx?token={0}";
        private static readonly Guid DEFAULT_TOKENID = new Guid("00000000-0000-0000-0000-000000000000");

        #endregion

        #region Private Methods

        private void OneCallTransfer()
        {
            var tokenID = DEFAULT_TOKENID;
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MCProd"].ConnectionString))
            {
                using (var comm = connection.CreateCommand())
                {
                    comm.CommandText = GET_TOKENID;
                    comm.Parameters.AddWithValue("UserName", Page.User.Identity.Name);
                    connection.Open();
                    using (var reader = comm.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        if (reader.Read())
                            tokenID = reader.GetGuid(reader.GetOrdinal("TokenID"));
                    }
                }
            }
            if (tokenID != DEFAULT_TOKENID)
                Response.Redirect(String.Format(ONECALLEXPRESS_LINK, tokenID));
        }

        #endregion

        #region Event Handlers

        protected void Page_Load(object sender, EventArgs e)
        {
            OneCallTransfer();
        }

        #endregion
    }
}
