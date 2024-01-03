using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MapCall.Modules.Data.Manholes
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    [ScriptService]
    public class Overflows : WebService
    {
        #region Constants

        internal struct SQL
        {
            internal const string OVERFLOW_INFORMATION =
                "select * from SewerOverflowDEPInformation where OperatingCenterID = '{0}'";
        }

        #endregion

        #region Private Members

        private string _connectionString =
                        ConfigurationManager.ConnectionStrings["MCProd"].ToString();

        #endregion

        #region Web Methods

        [WebMethod, ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object getSewerOverflowDEPInformation(int operatingCenterID)
        {
            var ds = new SqlDataSource(_connectionString,
                                       string.Format(SQL.OVERFLOW_INFORMATION,
                                                     operatingCenterID));
            var dv = (DataView)ds.Select(DataSourceSelectArguments.Empty);
            if (dv.Table.Rows.Count != 1)
                return null;
            var overflowInformation = new SewerOverflowDEPInformation
            {
                OperatingCenterID = int.Parse(dv.Table.Rows[0]["OperatingCenterID"].ToString()),
                MaximumGallons = int.Parse(dv.Table.Rows[0]["MaximumGallons"].ToString()),
                PrimaryEmail = dv.Table.Rows[0]["PrimaryEmail"].ToString(),
                SecondaryEmail = dv.Table.Rows[0]["SecondaryEmail"].ToString()
            };
            dv.Dispose();
            ds.Dispose();
            return overflowInformation;
        }

        #endregion

    }

    internal class SewerOverflowDEPInformation
    {
        #region Properties

        public int OperatingCenterID { get; set; }
        public int MaximumGallons{ get; set; }
        public string PrimaryEmail { get; set; }
        public string SecondaryEmail { get; set; }

        #endregion
    }
}
