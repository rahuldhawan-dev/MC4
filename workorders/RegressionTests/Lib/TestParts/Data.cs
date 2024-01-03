using System;
using System.Configuration;
using System.Data.SqlClient;

namespace RegressionTests.Lib.TestParts
{
    public static class Data
    {
        public const string MAP_ID = "01d4ebf78acc489695b930d5bc2850f3";

        private static string ConnectionString
        { 
            get
            {
                return ConfigurationManager.ConnectionStrings["MCProd"].ConnectionString;
            } 
        }

        private static void ExecuteSql(string sql)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                using (var cmd = new SqlCommand(sql,conn))
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }   
        }

        private static int ExecuteSqlWithSingleReturn(string sql)
        {
            int ret = 0;
            using (var conn = new SqlConnection(ConnectionString))
            {
                using (var cmd = new SqlCommand(sql, conn))
                {
                    conn.Open();
                    var rdr = cmd.ExecuteReader();
                    if (rdr.HasRows)
                    {
                        while (rdr.Read())
                        {
                            ret = rdr.GetInt32(0);
                        }
                    }
                    conn.Close();
                }
            }
            return ret;
        }

        public static void CreateTAndDFacilities()
        {
            ExecuteSql(
                "update tblFacilities set DepartmentID = 1 where RecordID = 193");
        }

        public static void RollbackTAndDFacilities()
        {
            ExecuteSql(
                "update tblFacilities set DepartmentID = 3 where RecordID = 193");
        }

        public static void FixMaterial()
        {
            ExecuteSql("update materials set isActive = 1 where MaterialId = 2;" +
                       "insert into OperatingCenterStockedMaterials values(10, 2, 5.61);");
        }

        public static void ToggleSAP(bool enabled)
        {
            ExecuteSql(" UPDATE OperatingCenters " +
                       " SET SAPEnabled = " + (enabled ? "1" : "0") + ", " +
                       " SAPWorkOrdersEnabled = " + (enabled ? "1" : "0") + ", " +
                       " MapId = '" + MAP_ID + "'"); // map id because it's checked in tests and can be configured through the site
        }

        public static void ToggleMarkoutsEditable(bool editable)
        {
            ExecuteSql(" UPDATE OperatingCenters " +
                       " SET MarkoutsEditable = " + (editable ? "1" : "0"));
        }

        public static string CreateOrderForEquipment(int operatingCenter, int town, int? townSection, string streetNumber, int street, int crossStreet, int assetType, int equipment, int requestedBy, int purpose, int priority, int workdescription, string orcom, int markoutRequirement, int createdBy)
        {
            var sql = "INSERT INTO WorkOrders(" +
                    "OperatingCenterID," +
                    "TownID," +
                    "TownSectionID, " +
                      "StreetNumber, " +
                      "StreetID, " +
                      "NearestCrossStreetID," +
                      "AssetTypeID," +
                      "EquipmentID," +
                      "RequesterID," +
                      "PurposeID," +
                      "PriorityID," +
                      "WorkDescriptionID," +
                      "ORCOMServiceOrderNumber," +
                      "MarkoutRequirementID," +
                      "CreatorID," +
                      "TrafficControlRequired," +
                      "StreetOpeningPermitRequired," + 
                      "Latitude, Longitude) " +
                "VALUES(" +
                      $"{operatingCenter}, " +
                      $"{town}," +
                      $"{townSection}," +
                      $"'{streetNumber}'," +
                      $"{street}," +
                      $"{crossStreet}," +
                      $"{assetType}," +
                      $"{equipment}," +
                      $"{requestedBy}," +
                      $"{purpose}," +
                      $"{priority}," +
                      $"{workdescription}," +
                      $"'{orcom}'," +
                      $"{markoutRequirement}," +
                      $"{createdBy}," +
                      "0," +
                      "0," +
                      "1," +
                      "2" +
                ");SELECT MAX(WorkOrderID) FROM WorkOrders;";

            return ExecuteSqlWithSingleReturn(sql).ToString();
        }

        public static void ToggleSAPErrorCode(Types.WorkOrder order, string error)
        {
            ExecuteSql($"Update WorkOrders SET SAPErrorCode = '{error}' WHERE WorkOrderID = {order.WorkOrderID}");
        }
        public static void ToggleSOPRequired(Types.WorkOrder order, bool required)
        {
            ExecuteSql($"Update WorkOrders SET StreetOpeningPermitRequired = '{(required ? "1" : "2")}' WHERE WorkOrderID = {order.WorkOrderID}");
        }

        public static void CreateAlertForWorkOrder(Types.WorkOrder order)
        {
            ExecuteSql($"INSERT INTO EchoshoreLeakAlerts(" +
                       $"  PointOfInterestStatusId, CoordinateId, EchoshoreSiteId, " +
                       $"  PersistedCorrelatedNoiseId, Hydrant1Id, Hydrant1Text, Hydrant2Id, Hydrant2Text, " +
                       $"  DatePCNCreated, DistanceFromHydrant1, DistanceFromHydrant2, FieldInvestigationRecommendedOn) " +
                       $"VALUES(1, 1, 1, 1, 15, 'HYD-1', 16, 'HYD-2', '08/30/2020', 100, 200, '08/30/2022');" +
                       $"UPDATE WorkOrders SET EchoshoreLeakAlertId = @@Identity where WorkOrderID = {order.WorkOrderID}");
        }
    }
}