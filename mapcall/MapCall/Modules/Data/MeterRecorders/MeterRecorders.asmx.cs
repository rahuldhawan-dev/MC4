using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Script.Services;
using System.Web.Services;

namespace MapCall.Modules.Data.MeterRecorders
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    [ScriptService]
    public sealed class MeterRecorders : WebService
    {

        #region Structs

        private struct SQL
        {
            internal const string GET_METER_RECORDER_LOCATION = "" +
                                                               " SELECT TOP 1" +
                                                               "    prem.PremiseID," +
                                                               "    prem.PremiseNumber," +
                                                               "    storage.Name as StorageLocationName," +
                                                               "    storage.MeterRecorderStorageLocationID," +
                                                               "    'HouseNumber' = CASE WHEN (cOrder.PremiseID Is Not Null) THEN prem.ServiceAddressHouseNumber ELSE storage.AddressHouseNumber END," +
                                                               "    'ApartmentNumber' = CASE WHEN (cOrder.PremiseID Is Not Null) THEN prem.ServiceAddressApartmentNumber ELSE storage.AddressApartmentNumber END," +
                                                               "    'Street' = CASE WHEN (cOrder.PremiseID Is Not Null) THEN streetPremise.FullStName ELSE streetStorage.FullStName END," +
                                                               "    'City' = CASE WHEN (cOrder.PremiseID Is Not Null) THEN townPremise.Town ELSE townStorage.Town END," +
                                                               "    'Zip' = CASE WHEN (cOrder.PremiseID Is Not Null) THEN prem.ServiceZip ELSE storage.Zip END," +
                                                               "    'State' = CASE WHEN (cOrder.PremiseID Is Not Null) THEN statePremise.Abbreviation ELSE stateStorage.Abbreviation END" +
                                                               " FROM" +
                                                               "     [MeterRecorderChangeOrders] cOrder" +
                                                               " LEFT JOIN" +
                                                               "     [MeterRecorders] recorders" +
                                                               " ON" +
                                                               "     recorders.MeterRecorderID = cOrder.MeterRecorderID" +
                                                               " LEFT JOIN" +
                                                               "     [Meters] meters" +
                                                               " ON" +
                                                               "     meters.MeterID = cOrder.MeterID" +
                                                               " LEFT JOIN" +
                                                               "     [Premises] prem" +
                                                               " ON" +
                                                               "      prem.PremiseID = cOrder.PremiseID" +
                                                               " LEFT JOIN" +
                                                               "      [MeterRecorderStorageLocations] storage" +
                                                               " ON" +
                                                               "      storage.MeterRecorderStorageLocationID = cOrder.MeterRecorderStorageLocationID" +
                                                               " LEFT JOIN" +
                                                               "     [MeterRecorderWorkDescriptions] workDescript" +
                                                               " ON" +
                                                               "     workDescript.MeterRecorderWorkDescriptionID = cOrder.MeterRecorderWorkDescriptionID" +
                                                               " LEFT JOIN" +
                                                               "     Streets streetPremise" +
                                                               " ON" +
                                                               "     prem.ServiceAddressStreet = streetPremise.StreetID" +
                                                               " LEFT JOIN" +
                                                               "     Towns townPremise" +
                                                               " ON" +
                                                               "     prem.ServiceCity = townPremise.TownID" +
                                                               " LEFT JOIN" +
                                                               "     [States] statePremise" +
                                                               " ON" +
                                                               "     prem.ServiceState = statePremise.StateID" +
                                                               " LEFT JOIN" +
                                                               "     Streets streetStorage" +
                                                               " ON" +
                                                               "     storage.AddressStreetID = streetStorage.StreetID" +
                                                               " LEFT JOIN" +
                                                               "     Towns townStorage" +
                                                               " ON" +
                                                               "     storage.CityID = townStorage.TownID" +
                                                               " LEFT JOIN" +
                                                               "     [States] stateStorage" +
                                                               " ON" +
                                                               "     storage.StateID = stateStorage.StateID" +
                                                               " WHERE " +
                                                               "     cOrder.[MeterRecorderID] = @MeterRecorderID" +
                                                               " AND DatePerformed Is Not Null" +
                                                               " ORDER BY DatePerformed DESC";

        }

        #endregion

        #region Enums

        // For more compact results to send back to the client. This needs to
        // be kept in sync with the client side javascript.
        private enum LookupResults
        {
            NotFound = 0,
            Premise = 1,
            StorageLocation = 2
        }

        #endregion

        #region Fields

        private string _connectionString =
                        ConfigurationManager.ConnectionStrings["MCProd"].ToString();

        #endregion

        #region Private Methods

        private static string TryGetString(IDataRecord reader, string columnName)
        {
            var ordinal = reader.GetOrdinal(columnName);
            return ((reader[ordinal] != DBNull.Value) ? reader.GetString(ordinal) : string.Empty);
        }

        #endregion

        #region Web Methods

        [WebMethod]
        public object GetMeterRecorderCurrentLocation(int meterRecorderId)
        {
            var address = new Dictionary<String, Object>();
            address["MeterRecorderID"] = meterRecorderId.ToString();

            using (var conn = new SqlConnection { ConnectionString = _connectionString })
            {
                conn.Open();

                using (var command = conn.CreateCommand())
                {
                    command.CommandText = SQL.GET_METER_RECORDER_LOCATION;

                    command.Parameters.AddWithValue("MeterRecorderID", meterRecorderId);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader == null || !reader.HasRows)
                        {
                            address["Response"] = LookupResults.NotFound;
                        }
                        else
                        {
                            reader.Read();

                            if (!reader.IsDBNull(reader.GetOrdinal("PremiseID")))
                            {
                                address["Response"] = LookupResults.Premise;
                                address["PremiseID"] = reader.GetInt32(reader.GetOrdinal("PremiseID"));
                                address["PremiseNumber"] = TryGetString(reader, "PremiseNumber");
                            }
                            else if (!reader.IsDBNull(reader.GetOrdinal("MeterRecorderStorageLocationID")))
                            {
                                address["Response"] = LookupResults.StorageLocation;
                                address["MeterRecorderStorageLocationID"] = reader.GetInt32(reader.GetOrdinal("MeterRecorderStorageLocationID"));
                                address["StorageLocationName"] = TryGetString(reader, "StorageLocationName");
                            }

                            address["HouseNumber"] = TryGetString(reader, "HouseNumber");
                            address["AptNumber"] = TryGetString(reader, "ApartmentNumber");
                            address["Street"] = TryGetString(reader, "Street");
                            address["City"] = TryGetString(reader, "City");
                            address["Zip"] = TryGetString(reader, "Zip");
                            address["State"] = TryGetString(reader, "State");

                        }
                    }
                }
            }

            return address;
        }

        #endregion
    }
}
