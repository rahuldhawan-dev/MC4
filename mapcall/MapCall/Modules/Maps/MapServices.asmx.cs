using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace MapCall.Modules.Maps
{
    /// <summary>
    /// Summary description for MapServices
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    [ScriptService]
    public class MapServices : WebService
    {
        #region Constants

        internal struct SQL
        {
            internal const string ACTIVE_EVENTS =
                "select Id, Latitude, Longitude from Events E left join Coordinates C on c.coordinateID = e.CoordinateID where IsActive=1";
            internal const string FLUSHING_SCHEDULE =
                "select flushingScheduleID, latitude, longitude, radius from FlushingSchedules f inner join coordinates c on c.coordinateID = f.coordinateID where EndDate is null and c.latitude is not null and c.longitude is not null and radius is not null";
            internal const string LEAKS =
                "select WorkOrderID,Latitude,Longitude,case when WorkDescriptionID in (40,41) then 1 else 0 end from WorkOrders where CancelledAt is null and WorkDescriptionID in (27,40,41,43,57,58,68,74,80) and DateCompleted is Null";
            internal const string RECENT_BACTIRESULTS =
                "select b.SampleSiteID, latitude, longitude from BacterialWaterSamples b left join SampleSites s on s.Id=b.SampleSiteID left join coordinates c on c.CoordinateID = s.CoordinateID where dateDiff(HH,sample_date, getDate()) < {0} and c.latitude is not null and c.longitude is not null order by sample_date desc";
            internal const string RECENT_LEAD_COPPERS_RESULTS =
                "select" +
                " samp.Id, c.latitude, c.Longitude" +
                " from" +
                " WaterSamples samp" +
                " join" +
                " SampleIDMatrices matrix on matrix.Id = samp.SampleMatrixId" +
                " join" +
                " SampleSites sites on sites.Id = matrix.SampleSiteID" +
                " join" +
                " Coordinates c on c.CoordinateID = sites.CoordinateID" +
                " where" +
                " dateDiff(HH, SampleDate, getDate()) < {0} and c.latitude is not null and c.longitude is not null";

            internal const string RECENT_HYDRANT_INSPECTIONS =
                "Select h.Id as recID, c.latitude as lat, c.longitude as lon from HydrantInspections i left join Hydrants h on h.Id = i.hydrantId left join coordinates c on c.CoordinateID = h.CoordinateId where dateDiff(HH, i.DateInspected, getDate()) < {0} and isNull(c.latitude, 0) <> 0 and isNull(c.longitude, 0) <> 0 order by i.DateInspected desc";
            internal const string RECENT_MAINBREAKS =
                "select distinct wo.WorkOrderID,wo.Latitude,wo.Longitude,(case when DateCompleted is not null then 3 when (select count(1) from crewAssignments ca where ca.workOrderID = wo.workOrderID and ca.dateStarted is not null and ca.dateEnded is null) > 0 then 2 when (select count(1) from crewAssignments ca where ca.workOrderID = wo.workOrderID and ca.dateStarted is null and ca.dateEnded is null) > 0 then 1 else 0 end) as status from workorders wo where wo.CancelledAt is null and abs(dateDiff(HH,CreatedAt, getDate())) < {0} and isNull(Latitude,0) <> 0 and isNull(Longitude,0) <> 0 and workDescriptionID in (74,80)";
            internal const string RECENT_FRCC_WORKORDERS =
                "select wo.WorkOrderID,wo.Latitude,wo.Longitude,case when (ca.datestarted is null and dateended is null) then 1 when (ca.datestarted is not null and dateended is null) then 2 else 3 end as [status] from workorders wo left join CrewAssignments ca on ca.WorkOrderID = wo.WorkOrderID where wo.CancelledAt is null and abs(dateDiff(HH,AssignedFor, getDate())) < {0} and isNull(Latitude,0) <> 0 and isNull(Longitude,0) <> 0 and RequesterId = 5 order by ca.assignedfor desc";
            internal const string RECENT_SEWER_OVERFLOWS =
                "select SewerOverflowID, c.Latitude, c.Longitude from sewerOverflows so inner join coordinates c on c.coordinateid = so.coordinateid where DateDiff(HH,IncidentDate, getDate()) < {0} and IsNull(c.Latitude,0) <> 0 and IsNull(c.Longitude,0) <> 0";
            internal const string RECENT_VALVE_INSPECTIONS =
                "Select v.Id as recID, c.latitude as lat, c.longitude as lon from ValveInspections i left join Valves v on v.Id = i.valveId left join coordinates c on c.CoordinateID = v.CoordinateId where dateDiff(HH, i.DateInspected, getDate()) < {0} and isNull(c.Latitude, 0) <> 0 and isNull(c.Longitude, 0) <> 0 order by i.DateInspected desc";
            internal const string RECENT_WORKORDERS =
                "select wo.WorkOrderID,wo.Latitude,wo.Longitude,case when (ca.datestarted is null and dateended is null) then 1 when (ca.datestarted is not null and dateended is null) then 2 else 3 end as [status] from workorders wo left join CrewAssignments ca on ca.WorkOrderID = wo.WorkOrderID where wo.CancelledAt is null and abs(dateDiff(HH,AssignedFor, getDate())) < {0} and isNull(Latitude,0) <> 0 and isNull(Longitude,0) <> 0 order by ca.assignedfor desc";
            internal const string RECENT_SANDY_WORKORDERS =
                "select wo.WorkOrderID,wo.Latitude,wo.Longitude,case when (ca.datestarted is null and dateended is null) then 1 when (ca.datestarted is not null and dateended is null) then 2 else 3 end as [status] from workorders wo left join CrewAssignments ca on ca.WorkOrderID = wo.WorkOrderID join WorkOrderPurposes wop on wop.WorkOrderPurposeID = wo.PurposeID where wo.CancelledAt is null and abs(dateDiff(HH,AssignedFor, getDate())) < {0} and isNull(Latitude,0) <> 0 and isNull(Longitude,0) <> 0 and wop.Description = 'Hurricane Sandy'  order by ca.assignedfor desc";
            internal const string RECENT_WQCOMPLAINTS =
                "select C.Id, cord.Latitude, cord.Longitude, case when (charIndex('Aesthetics', wqct.Description) > 0) then 1 when (charindex('medical', wqct.Description)>0) then 2 when (charindex('',wqct.Description)>0) then 3 else 4 end from WaterQualityComplaints C left join Coordinates Cord on c.CoordinateID = Cord.CoordinateID left join WaterQualityComplaintTypes wqct on ComplaintTypeId = wqct.id where DateDiff(HH, DateComplaintReceived, getDate()) < {0} and isNull(Cord.Latitude,0) <> 0 and isNull(Cord.Longitude,0) <> 0";
            internal const string VEHICLES =
                "select vehicleLocationID, vl.Latitude, vl.Longitude, isNull(v.VehicleIconID,0), isNull(vi.Description,'') from tblVehicleLocations vl inner join vehicles v on v.vehicle_label = vl.vehicleID left join VehicleIcons vi on vi.VehicleIconID = v.VehicleIconID";
            internal const string RECENT_ONE_CALL_TICKETS =
                "select t.Id as RequestNum, 40.4509441 as Latitude, -74.1742363 as Longitude, case when r.CompletedAt is null then 1 else 2 end as [status], isNull(t.Street,'') + ' ' + isNull(t.Town,'') as Title from OneCallMarkoutTickets t left join OneCallMarkoutResponses r on t.Id = r.OneCallMarkoutTicketId where abs(dateDiff(HH, t.DateTransmitted, GETDATE())) < {0} ";
        }

        public class MapObject
        {
            internal MapObject() { }

            public object id { get; set; }
            public object lat { get; set; }
            public object lng { get; set; }
            public object opt { get; set; }
            public object title { get; set; }
        }

        #endregion

        #region Private Members

        private static readonly string _connectionString = ConfigurationManager.ConnectionStrings["MCProd"].ToString();

        #endregion

        #region Web Methods

        [WebMethod, ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<MapObject> ActiveEvents()
        {
            return ReturnJson(SQL.ACTIVE_EVENTS);
        }

        [WebMethod, ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<MapObject> FlushingSchedules()
        {
            return ReturnJson(SQL.FLUSHING_SCHEDULE);
        }

        [WebMethod, ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<MapObject> Leaks()
        {
            return ReturnJson(SQL.LEAKS);
        }

        [WebMethod, ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<MapObject> RecentBactiResults(int hours)
        {
            return ReturnJson(string.Format(SQL.RECENT_BACTIRESULTS, hours));
        }

        [WebMethod, ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<MapObject> RecentLeadCoppers(int hours)
        {
            return ReturnJson(string.Format(SQL.RECENT_LEAD_COPPERS_RESULTS, hours));
        }

        [WebMethod, ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<MapObject> RecentHydrantInspections(int hours)
        {
            return ReturnJson(
                string.Format(SQL.RECENT_HYDRANT_INSPECTIONS, hours));
        }

        [WebMethod, ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<MapObject> RecentMainBreaks(int hours)
        {
            return ReturnJson(string.Format(SQL.RECENT_MAINBREAKS,
                                                     hours));
        }

        [WebMethod, ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<MapObject> RecentFRCCWorkOrders(int hours)
        {
            return ReturnJson(string.Format(SQL.RECENT_FRCC_WORKORDERS, hours));
        }

        [WebMethod, ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<MapObject> RecentSewerOverflows(int hours)
        {
            return ReturnJson(string.Format(SQL.RECENT_SEWER_OVERFLOWS,
                             hours));
        }

        [WebMethod, ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<MapObject> RecentValveInspections(int hours)
        {
            return ReturnJson(string.Format(SQL.RECENT_VALVE_INSPECTIONS, hours));
        }

        [WebMethod, ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<MapObject> RecentWaterQualityComplaints(int hours)
        {
            return ReturnJson(string.Format(SQL.RECENT_WQCOMPLAINTS,
                                         hours));
        }

        [WebMethod, ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<MapObject> RecentWorkOrders(int hours)
        {
            return ReturnJson(string.Format(SQL.RECENT_WORKORDERS,
                                                     hours));
        }

        [WebMethod, ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<MapObject> RecentSandyWorkOrders(int hours)
        {
            return ReturnJson(string.Format(SQL.RECENT_SANDY_WORKORDERS,
                                                     hours));
        }

        [WebMethod, ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<MapObject> Vehicles()
        {
            UpdateTrimbleData();
            return ReturnJson(SQL.VEHICLES);
        }

        [WebMethod, ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<MapObject> RecentOneCallTickets(int hours)
        {
            return ReturnJson(string.Format(SQL.RECENT_ONE_CALL_TICKETS, hours));
        }

        #endregion

        #region Private Methods

        private static SqlConnection GetSqlConnection()
        {
            return new SqlConnection(ConfigurationManager.ConnectionStrings["MCProd"].ToString());
        }

        private List<MapObject> ReturnJson(string sql)
        {
            using (var ds = new SqlDataSource(_connectionString, sql))
            using (var dv = (DataView)ds.Select(DataSourceSelectArguments.Empty))
            {
                switch (dv.Table.Columns.Count)
                {
                    case 4:
                        return BuildMapObjectListWithOptionalColumn(dv.Table.Rows);
                    case 5:
                        return BuildMapObjectListWithOptionalAndTitleColumn(dv.Table.Rows);
                    default:
                        return BuildMapObjectListWithoutOptionalColumn(dv.Table.Rows);
                }
            }
        }

        private static List<MapObject> BuildMapObjectListWithOptionalColumn(DataRowCollection dataRowCollection)
        {
            var ret = new List<MapObject>();
            foreach (DataRow row in dataRowCollection)
            {
                ret.Add(new MapObject { id = row[0], lat = row[1], lng = row[2], opt = row[3] });
            }
            return ret;
        }

        private static List<MapObject> BuildMapObjectListWithOptionalAndTitleColumn(DataRowCollection dataRowCollection)
        {
            var ret = new List<MapObject>();
            foreach (DataRow row in dataRowCollection)
            {
                ret.Add(new MapObject { id = row[0], lat = row[1], lng = row[2], opt = row[3], title = row[4] });
            }
            return ret;
        }

        private static List<MapObject> BuildMapObjectListWithoutOptionalColumn(DataRowCollection dataRowCollection)
        {
            var ret = new List<MapObject>();
            foreach (DataRow row in dataRowCollection)
            {
                ret.Add(new MapObject { id = row[0], lat = row[1], lng = row[2], opt = null });
            }
            return ret;
        }

        private static void UpdateTrimbleData()
        {
            if (!UpdatedInTheLastTwoMinutes())
            {
                InsertTrimbleData();
            }
        }

        private static WebClient CreateTrimbleClient()
        {
            const string un = "api34142";
            const string pass = "oYHStLg";

            var webclient = new WebClient();
            webclient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            webclient.UseDefaultCredentials = false;
            webclient.Credentials = new NetworkCredential(un, pass);
            return webclient;
        }

        private static void InsertTrimbleData()
        {
            const string url = "https://www.road.com/apps/API/GetFleetLocation?RECORD=200";

            using (var webclient = CreateTrimbleClient())
            using (var data = webclient.OpenRead(url))
            using (var sr = new StreamReader(data))
            {
                var vehicleXML = XDocument.Parse(sr.ReadToEnd());
                var vehicles = from vehicle in vehicleXML.Descendants("LOCATION_RECORD")
                               select new
                               {
                                   Label = (vehicle != null && vehicle.Element("LABEL") != null) ? vehicle.Element("LABEL").Value : string.Empty,
                                   Lat = (vehicle != null && vehicle.Element("LAT") != null) ? vehicle.Element("LAT").Value : string.Empty,
                                   Lon = (vehicle != null && vehicle.Element("LON") != null) ? vehicle.Element("LON").Value : string.Empty
                               };

                if (vehicles.Any())
                {
                    using (var conn = GetSqlConnection())
                    {
                        using (var cmd = new SqlCommand("DELETE tblVehicleLocations", conn))
                        {
                            conn.Open();
                            cmd.ExecuteScalar();
                            foreach (var vehicle in vehicles)
                            {
                                cmd.CommandText =
                                    string.Format(
                                              "INSERT INTO [tblVehicleLocations] ([VehicleID],[Latitude],[Longitude],[CreatedOn]) VALUES ('{0}','{1}','{2}','{3}')",
                                              vehicle.Label,
                                              vehicle.Lat,
                                              vehicle.Lon,
                                              DateTime.Now
                                    );
                                cmd.ExecuteScalar();
                            }
                        }
                    }
                }
            }
        }

        private static bool UpdatedInTheLastTwoMinutes()
        {
            using (var conn = GetSqlConnection())
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "select top 1 createdOn from tblVehicleLocations";
                conn.Open();
                var result = cmd.ExecuteScalar();
                if (result != null)
                {
                    var start = DateTime.Parse(result.ToString());
                    var end = DateTime.Now;
                    var span = end.Subtract(start);
                    if (span.Minutes > 2)
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }

            return true; // By default.
        }

        #endregion
    }
}
