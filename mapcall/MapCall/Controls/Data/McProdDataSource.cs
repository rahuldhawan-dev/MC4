using System.Configuration;
using System.Web.UI.WebControls;

namespace MapCall.Controls.Data
{
    public class McProdDataSource : SqlDataSource
    {

        #region Constructors

        public McProdDataSource() 
        {
            base.ConnectionString = ConfigurationManager.ConnectionStrings["MCProd"].ToString();
        }

        #endregion

    }
}
