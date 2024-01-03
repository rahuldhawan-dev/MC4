using System.ComponentModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Text;
using System.Web.Script.Services;
using System.Web.Services;

namespace MapCall.Modules.Data.Lookups
{
    /// <summary>
    /// Summary description for Lookups
    /// </summary>
    [WebService(Namespace = "https://mapcall.amwater.com/webservices/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    [ScriptService]
    public class Lookups : WebService
    {
        #region Constants

        internal struct SQL
        {
            internal const string PREMISE_NUMBER_FROM_FIRE_DISTRICT_ID = 
                "Select PremiseNumber from FireDistricts where Id = @FireDistrictID";

            internal const string HYDRANT_MODELS_FOR_MANUFACTURER_ID = 
                "Select HydrantModelID, Name from HydrantModels where ManufacturerID = @ManufacturerID";
        }

        #endregion

        #region Private Members

        private string _ConnectionString;

        #endregion

        #region Properties

        public string ConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(_ConnectionString))
                    _ConnectionString =
                        ConfigurationManager.ConnectionStrings["MCProd"].ToString();
                return _ConnectionString;
            }
        }

        #endregion

        #region Web Methods

        [ScriptMethod, WebMethod]
        public string GetPremiseNumberForFireDistrict(string fireDistrictID)
        {
            using (var conn = new SqlConnection { ConnectionString = ConnectionString})
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = SQL.PREMISE_NUMBER_FROM_FIRE_DISTRICT_ID;
                    cmd.Parameters.AddWithValue("fireDistrictID", fireDistrictID);

                    using(var reader = cmd.ExecuteReader())
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                return reader["PremiseNumber"].ToString();
                            }
                        }
                    }
                }
            }

            return string.Empty;
        }

        [ScriptMethod, WebMethod]
        public string GetHydrantModelsForManufacturer(string manufacturerID)
        {
            using (var conn = new SqlConnection { ConnectionString = ConnectionString })
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = SQL.HYDRANT_MODELS_FOR_MANUFACTURER_ID;
                    cmd.Parameters.AddWithValue("manufacturerID", manufacturerID);
                    var sb = new StringBuilder();

                    using (var reader = cmd.ExecuteReader())
                    {
                        sb.Append("[");
                        while (reader.Read())
                        {
                            sb.AppendFormat("{{\"Text\": \"{0}\", \"Value\": \"{1}\"}},", reader[1], reader[0]);
                        }
                        sb.Remove(sb.Length - 1, 1);
                        sb.Append("]");
                    }
                    Context.Response.Output.Write(sb.ToString());
                    Context.Response.End();
                    return string.Empty;
                }
            }
        }

        #endregion
    }
}
