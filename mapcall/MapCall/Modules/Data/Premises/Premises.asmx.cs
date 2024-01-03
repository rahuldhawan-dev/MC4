using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Script.Services;
using System.Web.Services;

namespace MapCall.Modules.Data.Premises
{
    /// <summary>
    /// Summary description for Premises
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    [ScriptService]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class Premises : WebService
    {
        #region Constants

        private struct SQL
        {
            internal const string PREMISE_NUMBERS =
                "select distinct top {0} PremiseNumber from Premises WHERE PremiseNumber like @premNum ORDER BY PremiseNumber";
            internal const string PREMISE_NUMBERS_WITH_ID =
                "select distinct top {0} PremiseID, PremiseNumber from Premises WHERE PremiseNumber like @premNum ORDER BY PremiseNumber";

            internal const string PREMISE_ADDRESS_BY_ID =
                "SELECT" +
                " t.Town as ServiceCityText, " +
                " s.FullStName as ServiceAddressStreetText, " +
                " st.State as State," +
                " p.ServiceAddressHouseNumber, " +
                " p.ServiceAddressApartmentNumber, " +
                " p.ServiceZip, " +
                " p.PremiseNumber " +
                " FROM  " +
                "     [Premises] p " +
                " LEFT JOIN [States] st                ON p.ServiceState = st.StateID" +
                " LEFT JOIN Towns t                    ON p.ServiceCity = t.TownID " +
                " LEFT JOIN Streets s                  ON p.ServiceAddressStreet = s.StreetID " +
                " WHERE PremiseID = @premiseId";

            internal const string PREMISE_ADDRESS_BY_METERRECORDERID =
                "SELECT DISTINCT" +
                " prem.PremiseID" +
                    " FROM" +
                    "     [MeterRecorders] rec" +
                    " LEFT JOIN" +
                    "     [MeterRecorderHistory] hist" +
                    " ON" +
                    "     hist.MeterRecorderID = rec.MeterRecorderID" +
                    " LEFT JOIN" +
                    "     [Meters] meter" +
                    " ON" +
                    "     meter.MeterID = hist.MeterID" +
                    " LEFT JOIN" +
                    "     [Premises] prem" +
                    " ON" +
                    "     prem.PremiseID = meter.PremiseID" +
                    " WHERE hist.DateInstalled = (SELECT" +
                    "                                 MAX(DateInstalled)" +
                    "                             FROM" +
                    "                                 MeterRecorderHistory mrh2" +
                    "                             WHERE" +
                    "                                 mrh2.MeterRecorderID = hist.MeterRecorderID)" +
                    "  AND " +
                    "     rec.MeterRecorderID = @meterRecorderId";

            internal const string PREMISE_BY_METERID =
                                    @"
                                    SELECT 
                                        m.MeterID,
                                        prem.PremiseID
                                    FROM 
                                        [Meters] m
                                    LEFT JOIN
                                        Premises prem
                                    ON
                                        m.PremiseID = prem.PremiseID
                                    WHERE
                                        m.MeterID = @MeterID";

        }

        // This needs to be kept the same in the javascript.
        // Use ToString("d") to send this to the client as its numeric value.
        private enum PremiseAddressLookupResponse
        {
            Success = 1,
            NoPremise = 2
        }

        #endregion

        #region Properties

        public string ConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["MCProd"].ToString();
            }
        }

        #endregion

        #region Private Methods

        private static string TryGetString(IDataRecord reader, string columnName)
        {
            var ordinal = reader.GetOrdinal(columnName);
            return ((reader[ordinal] != DBNull.Value) ? reader.GetString(ordinal) : string.Empty);
        }

        private static int? TryGetInt(IDataRecord reader, string columnName)
        {
            int? value = null;
            var ordinal = reader.GetOrdinal(columnName);

            if (reader[ordinal] != DBNull.Value)
            {
                value = reader.GetInt32(ordinal);
            }

            return value;
        }

        #endregion

        #region Web Methods

        [WebMethod, ScriptMethod]
        public string[] GetPremiseNumbers(string q, int limit)
        {
            //const int count = 10;
            var items = new List<string>(limit);
            using (var conn = new SqlConnection { ConnectionString = ConnectionString })
            {
                var p1 = new SqlParameter("premNum", "%" + q + "%");
                conn.Open();
                using (var dr = new SqlCommand {CommandText = String.Format(SQL.PREMISE_NUMBERS, limit),Connection = conn,Parameters = {p1}}.ExecuteReader())
                {
                    while (dr.Read())
                        items.Add(dr[0].ToString());
                }
            }
            return items.ToArray();
        }

        [ScriptMethod(ResponseFormat = ResponseFormat.Json), WebMethod]
        public List<Premise> GetPremiseNumbersWithID(string q, int limit)
        {
            var items = new List<Premise>(limit);
            using (var conn = new SqlConnection { ConnectionString = ConnectionString })
            {
                var p1 = new SqlParameter("premNum", "%" + q + "%");
                conn.Open();
                using(var dr = new SqlCommand { CommandText = String.Format(SQL.PREMISE_NUMBERS_WITH_ID, limit), Connection = conn, Parameters = {p1} }.ExecuteReader())
                {
                    while (dr.Read())
                        items.Add(new Premise {PremiseID = dr[0].ToString(), PremiseNumber = dr[1].ToString()});
                }
            }
            return items;
        }
        
        [ScriptMethod(ResponseFormat = ResponseFormat.Json), WebMethod]
        public object GetCurrentPremiseOnMeterRecorderById(int recorderId)
        {
            using (var conn = new SqlConnection() { ConnectionString = ConnectionString })
            using (var command = conn.CreateCommand())
            {
                conn.Open();

                command.CommandText = SQL.PREMISE_ADDRESS_BY_METERRECORDERID;
                command.Parameters.AddWithValue("meterRecorderId", recorderId);

                using (var reader = command.ExecuteReader())
                {
                    if (!reader.HasRows)
                    {
                        var address = new Dictionary<string, object>();
                        address["Response"] = PremiseAddressLookupResponse.NoPremise;
                        return address;
                    }
                    while (reader.Read())
                    {
                        var premiseId = reader.GetInt32(reader.GetOrdinal("PremiseID"));
                        return GetPremiseAddress(premiseId);
                    }
                }
            }

            return null;
        }

        [ScriptMethod(ResponseFormat = ResponseFormat.Json), WebMethod]
        public object GetPremiseByMeterId(int meterId)
        {
            if (meterId <= 0)
            {
                throw new ArgumentOutOfRangeException("meterId");
            }

            using (var conn = new SqlConnection() { ConnectionString = ConnectionString })
            using (var command = conn.CreateCommand())
            {
                conn.Open();

                command.CommandText = SQL.PREMISE_BY_METERID;
                command.Parameters.AddWithValue("MeterID", meterId);

                using (var reader = command.ExecuteReader())
                {
                    if (!reader.HasRows)
                    {
                        var address = new Dictionary<string, object>();
                        address["Response"] = PremiseAddressLookupResponse.NoPremise;
                        return address;
                    }
                    while (reader.Read())
                    {
                        var pid = TryGetInt(reader, "PremiseID");

                        if (!pid.HasValue)
                        {
                            var address = new Dictionary<string, object>();
                            address["Response"] = PremiseAddressLookupResponse.NoPremise;
                            return address;
                        }

                        return GetPremiseAddress(pid.Value);
                    }
                }
            }

            return null;
        }

        [ScriptMethod(ResponseFormat = ResponseFormat.Json), WebMethod]
        public object GetPremiseAddress(int premiseId)
        {
            var address = new Dictionary<String, Object>();
            address["PremiseID"] = premiseId.ToString();

            using (var conn = new SqlConnection { ConnectionString = ConnectionString })
            using (var command = conn.CreateCommand())
            {
                conn.Open();

                command.CommandText = SQL.PREMISE_ADDRESS_BY_ID;

                command.Parameters.AddWithValue("premiseId", premiseId);

                using (var reader = command.ExecuteReader())
                {
                    if (!reader.HasRows)
                    {
                        address["Response"] = PremiseAddressLookupResponse.NoPremise;
                    }
                    else
                    {
                        address["Response"] = PremiseAddressLookupResponse.Success;

                        while (reader.Read())
                        {
                            address["PremiseNumber"] = TryGetString(reader, "PremiseNumber");
                            address["HouseNumber"] = TryGetString(reader, "ServiceAddressHouseNumber");
                            address["AptNumber"] = TryGetString(reader, "ServiceAddressApartmentNumber");
                            address["Street"] = TryGetString(reader, "ServiceAddressStreetText");
                            address["City"] = TryGetString(reader, "ServiceCityText");
                            address["Zip"] = TryGetString(reader, "ServiceZip");
                            address["State"] = TryGetString(reader, "State");
                        }
                    }
                }
            }

            return address;
        }

        #endregion
    }
    
    public class Premise
    {
        public string PremiseID { get; set; }
        public string PremiseNumber { get; set; }
    }
}
