using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Script.Services;
using System.Web.Services;
using AjaxControlToolkit;

namespace MapCall.Modules.Data.StormWater
{
    /// <summary>
    /// Summary description for StormWaterAssets
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    [ScriptService]
    public class StormWaterAssets : WebService
    {
        #region Properties

        public string ConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["MCProd"].ToString();
            }
        }

        #endregion

        #region Helpers

        private static CascadingDropDownNameValue[] GetNameValues(SqlCommand comm, string nameField, string valueField)
        {
            var result = new List<CascadingDropDownNameValue>();

            if (comm.Connection.State != ConnectionState.Open)
            {
                comm.Connection.Open();
            }

            using (var reader = comm.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var cddnv = new CascadingDropDownNameValue();
                        cddnv.name = reader[nameField].ToString();
                        cddnv.value = reader[valueField].ToString();
                        result.Add(cddnv);
                    }
                }
            }
            return result.ToArray();
        }

        private static CascadingDropDownNameValue[] GetNameValuesIncludingStreetNames(SqlCommand comm, string nameField, string valueField)
        {
            var result = new List<CascadingDropDownNameValue>();

            if (comm.Connection.State != ConnectionState.Open)
            {
                comm.Connection.Open();
            }

            using (var reader = comm.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var cddnv = new CascadingDropDownNameValue();
                        var assetNum = reader["AssetNumber"].ToString();
                        var streetName = reader["Street"].ToString();
                        var interStreetName = reader["InterStreet"].ToString();

                        cddnv.name = string.Format("{0}: [{1}, {2}]", assetNum, streetName, interStreetName);
                        cddnv.value = reader["StormWaterAssetID"].ToString();
                        result.Add(cddnv);
                    }
                }
            }
            return result.ToArray();
        }

        #endregion

        [WebMethod, ScriptMethod]
        public CascadingDropDownNameValue[] GetStormWaterAssetTypes(string knownCategoryValues, string category)
        {
            using (var conn = new SqlConnection(ConnectionString))
            using (var com = conn.CreateCommand())
            {
                com.CommandText = "SELECT StormWaterAssetTypeID, Description FROM StormWaterAssetTypes";
                return GetNameValues(com, "Description", "StormWaterAssetTypeID");
            }
        }

        [WebMethod, ScriptMethod]
        public CascadingDropDownNameValue[] GetStormWaterAssetsByStormWaterAssetType(string knownCategoryValues, string category)
        {
            var kv = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues);
            var assetTypeId = kv.ContainsKey("StormWaterAssetType")
                             ? kv["StormWaterAssetType"]
                             : kv["undefined"];
         

            using (var conn = new SqlConnection(ConnectionString))
            using (var com = conn.CreateCommand())
            {
                com.CommandText = "SELECT swa.StormWaterAssetID, swa.AssetNumber, street.FullStName as [Street], interstreet.FullStName as [InterStreet] FROM StormWaterAssets swa"
                                + " LEFT JOIN Streets street ON street.StreetID = swa.StreetID"
                                + " LEFT JOIN Streets interstreet ON interstreet.StreetID = swa.IntersectingStreetID"
                                + " WHERE swa.StormWaterAssetTypeID = @StormWaterAssetTypeID ORDER BY swa.AssetNumber ASC";
                com.Parameters.AddWithValue("StormWaterAssetTypeID", assetTypeId);

                return GetNameValuesIncludingStreetNames(com, "AssetNumber", "StormWaterAssetID");
            }
        }

        [WebMethod, ScriptMethod]
        public CascadingDropDownNameValue[] GetStormWaterAssetsByStormWaterAssetTypeAndOtherStuff(string knownCategoryValues, string category)
        {
            var kv = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues);
            var assetTypeId = kv.ContainsKey("StormWaterAssetType")
                             ? kv["StormWaterAssetType"]
                             : kv["undefined"];

            var streetId = kv.ContainsKey("Street")
                                 ? kv["Street"]
                                 : kv["undefined"];

            var interStreetId = kv.ContainsKey("IntersectingStreet")
                                 ? kv["IntersectingStreet"]
                                 : kv["undefined"];

            using (var conn = new SqlConnection(ConnectionString))
            using (var com = conn.CreateCommand())
            {
                com.CommandText = "SELECT swa.StormWaterAssetID, swa.AssetNumber, street.FullStName as [Street], interstreet.FullStName as [InterStreet] FROM StormWaterAssets swa"
                                + " LEFT JOIN Streets street ON street.StreetID = swa.StreetID"
                                + " LEFT JOIN Streets interstreet ON interstreet.StreetID = swa.IntersectingStreetID"
                                + " WHERE swa.StormWaterAssetTypeID = @StormWaterAssetTypeID"
                                + " AND (isNull(swa.StreetID, 0) in (isNull(@StreetID,0), isNull(@IntersectingStreetID, 0))"
                                + "      OR isNull(swa.IntersectingStreetID, 0) in (isNull(@StreetID,0), isNull(@IntersectingStreetID, 0)))"
                                + " ORDER BY AssetNumber ASC";
                com.Parameters.AddWithValue("StormWaterAssetTypeID", assetTypeId);
                com.Parameters.AddWithValue("StreetID", streetId);
                com.Parameters.AddWithValue("IntersectingStreetID", interStreetId);

                return GetNameValuesIncludingStreetNames(com, "AssetNumber", "StormWaterAssetID");
            }
        }
    }
}
