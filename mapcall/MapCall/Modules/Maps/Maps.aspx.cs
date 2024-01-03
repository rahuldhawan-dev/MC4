using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using MapCall.Common.Controls;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;
using MMSINC.DataPages;
using StructureMap;
using ControlExtensions = MMSINC.ClassExtensions.ControlExtensions;

namespace MapCall.Modules.Maps
{

    // DANGER DANGER WILL ROBINSON
    // SELECT COMMAND AND SQL CONNECTION INFO IS STORED IN VIEWSTATE UNENCRYPTED
    public partial class Maps : AssetLatLonPage
    {
        #region Structs

        private struct ViewStateKeys
        {
            public const string FILTER = "filter";
            public const string REFERRER = "referrer";
            public const string SELECT_COMMAND = "SelectCommand";
            public const string SQL_CONNECTION_STRING = "SqlConnectionString";
        }

        private struct SQL
        {
            public const string FLUSHING_SCHEDULES =
                "SELECT DISTINCT [FlushingScheduleID], [Coordinates].[Latitude] as [Lat], [Coordinates].[Longitude] as [Lon], " +
                "'FlushingSchedule', 'icon' + cast([Coordinates].[iconID] as varchar(15)), [FlushingScheduleID] AS description, radius from [FlushingSchedules] as fs " +
                "inner join [Coordinates] on fs.[CoordinateID] = [Coordinates].[CoordinateID] " +
                "LEFT JOIN [OperatingCenters] oc ON fs.OperatingCenterID = oc.OperatingCenterID ";

            public const string METER_ROUTES =
                "SELECT DISTINCT [MeterRouteID], [Coordinates].[Latitude] as [Lat], [Coordinates].[Longitude] as [Lon], " +
                "'MeterRoute', 'icon' + cast([Coordinates].[iconID] as varchar(15)), [MeterRouteID] AS description, radius from [MeterRoutes] mr " +
                "inner join [Coordinates] on mr.[CoordinateID] = [Coordinates].[CoordinateID] " +
                "LEFT JOIN [OperatingCenters] oc ON mr.OperatingCenterID = oc.OperatingCenterID ";
        }

        #endregion

        #region Private Members

        private string strURL, _mapDataLayerId;
        private IFilterBuilder _filterBuilder;

        #endregion

        #region Properties

        public string filter
        {
            get
            {
                if (_filterBuilder != null)
                {
                    return _filterBuilder.BuildFilter(); ;
                }
                else
                {
                    object obj = ViewState[ViewStateKeys.FILTER];
                    return (obj == null) ? String.Empty : obj.ToString();
                }
            }
            set { ViewState[ViewStateKeys.FILTER] = value; }
        }

        public string OriginalReferrerUrl
        {
            get
            {
                if (_filterBuilder != null) { return _filterBuilder.OriginatingPageUrl; }
                else
                {
                    var referrer = ViewState[ViewStateKeys.REFERRER];
                    return ((referrer == null) ? String.Empty : referrer.ToString());
                }
            }
            set
            {
                // Because the page posts back on load for some reason, this needs
                // to be saved to ViewState initially, and then never on post back.
                // If set on post back, the referrer will always be Maps.aspx.
                if (!IsPostBack)
                {
                    ViewState[ViewStateKeys.REFERRER] = value;
                }
            }
        }

        public string SelectCommand
        {
            get
            {
                if (_filterBuilder != null)
                {
                    return _filterBuilder.SelectCommand;
                }
                else
                {
                    object obj = ViewState[ViewStateKeys.SELECT_COMMAND];
                    return (obj == null) ? String.Empty : obj.ToString();
                }

            }
            set { ViewState[ViewStateKeys.SELECT_COMMAND] = value; }
        }
        public string SqlConnectionString
        {
            get
            {
                if (_filterBuilder != null)
                {
                    return _filterBuilder.ConnectionString;
                }
                else
                {
                    object obj = ViewState[ViewStateKeys.SQL_CONNECTION_STRING];
                    return (obj == null) ? String.Empty : obj.ToString();
                }

            }
            set { ViewState[ViewStateKeys.SQL_CONNECTION_STRING] = value; }
        }

        #endregion

        #region Event Handlers

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            // Required for DataPageBase map links.
            CheckForSearchQuery();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (String.IsNullOrEmpty(filter))
            {
                filter = " WHERE ";
            }

            if (PreviousPage != null && Page != PreviousPage)
            {
                // TODO: This will not work with the new DataPageBase classes. 
                OriginalReferrerUrl = Request.UrlReferrer.AbsoluteUri;

                // NOTE: As sure as we already are on this being a terrible way of passing these along, it also
                //       leads to the possibility of sql injection should the hidden input's value actually get
                //       rendered to the page. This may not actually apply in the case of FilterExpressions
                //       seeing as they're not sent as part of the select command to the server, but still. 

                var hidFilter = (HiddenField)MMSINC.Utility.GetFirstControlInstanceIn(PreviousPage, "hidFilter", "pnlResults");

                if (!String.IsNullOrEmpty(hidFilter.Value))
                    filter = String.Format("WHERE {0} AND ", hidFilter.Value);

                var dataSource =
                    ((SqlDataSource)
                     MMSINC.Utility.GetFirstControlInstance(PreviousPage,
                                                            "SqlDataSource1"));
                SelectCommand = dataSource.SelectCommand;
                SqlConnectionString = dataSource.ConnectionString;
            }

            InitializeMaps();
        }

        private void InitializeMaps()
        {

            // Instead of checking for a matching string on the select command,
            // just check to see who the referring url is. Assuming that each
            // page can only ever have one select command that this page looks at,
            // it'd work more consistantly.
            //
            // I would check based on referral url first, then do anything else
            // that involves the parsing the select command instead.
            var referrer = OriginalReferrerUrl;

            // Check for query strings and remove them.
            if (referrer.Contains("?"))
            {
                referrer = referrer.Split('?').First();
            }

            Func<string, bool> refEndsWith = (stringToMatch) => referrer.EndsWith(stringToMatch, StringComparison.OrdinalIgnoreCase);

            // Reffing this so there's not a repeated call to ViewState/null checks.
            var selectCommand = SelectCommand;


            if (_filterBuilder != null)
            {
                if (IsPostBack)
                {
                    return;
                }

                if (refEndsWith("Modules/FieldServices/FlushingSchedules.aspx"))
                {
                    InitializeFlushingSchedulesMap();
                }
                else if (refEndsWith("Modules/Customer/MeterRoutes.aspx"))
                {
                    InitializeMeterRoutesMap();
                }
                else if (refEndsWith("Modules/HR/Union/Locals.aspx"))
                {
                    InitializeLocalUnionsMap();
                }
                else if (refEndsWith("Modules/Engineering/ProjectsRP.aspx"))
                {
                    InitializeProjectsRPMap();
                }

            }
            else
            {
                //Temporary Fix to load multiple tables on the same map the same way.
                //
                string jsonSelectCommand, mapObjectType;

                // Do mapping stuff here that relies on the older way.

                // TODO: Separate these, they got messy.
                if (refEndsWith("Modules/FieldServices/MeterTests.aspx"))
                {
                    mapObjectType = "MeterTest";
                    strURL = "iMeterTest.aspx?recordID=";
                    jsonSelectCommand = String.Format("SELECT DISTINCT mt.[MeterTestID], C.[Latitude] as [Lat], C.[Longitude] as [Lon]," +
                        " 'MeterTest', CASE WHEN (DateTested is not null) THEN 'icon5' ELSE 'icon6' END, [MeterTestID] AS description" +
                        " from [MeterTests] mt" +
                        " inner join [Meters] m on m.MeterID = mt.MeterID" +
                        " inner join [Premises] P on P.PremiseID = m.PremiseID" +
                        " inner join [Coordinates] C on P.[CoordinateID] = C.[CoordinateID]" +
                        " {0} isNull(C.[Latitude], '0') <> '0'", filter);
                }
                else if (refEndsWith("Modules/FieldServices/Meters.aspx"))
                {
                    mapObjectType = "Meter";
                    strURL = "iMeter.aspx?recordID=";

                    // icon21 = Black - Null/Retired
                    // icon22 = Green - Active
                    jsonSelectCommand = String.Format("SELECT DISTINCT m.[MeterID], C.[Latitude] as [Lat], C.[Longitude] as [Lon]," +
                                       " 'Meter'," +
                                       " Icon = CASE m.Status WHEN (1) THEN 'icon22' ELSE 'icon21' END," +
                                       " m.[MeterID] AS description" +
                                       " FROM [Meters] m" +
                                       " inner join Premises P on P.PremiseID = m.PremiseID" +
                                       " inner join Coordinates C on P.CoordinateID = C.CoordinateID" +
                                       " LEFT JOIN Towns njtown ON P.ServiceCity = njtown.TownID" +
                                       " {0} isNull(c.[Latitude], '0') <> '0'", filter);

                }
                else if (refEndsWith("Modules/FieldServices/MeterRecorderStorageLocations.aspx"))
                {
                    mapObjectType = "MeterRecorderStorageLocation";
                    strURL = "iMeterRecorderStorageLocations.aspx?recordID=";
                    jsonSelectCommand = "SELECT" +
                        " storage.MeterRecorderStorageLocationID," +
                        " c.Latitude as [Lat]," +
                        " c.Longitude as [Lon]," +
                        " 'MeterRecorderStorageLocation'," +
                        " 'icon' + cast(c.IconID as varchar(15))," +
                        " storage.MeterRecorderStorageLocationID as description" +
                        " FROM [MeterRecorderStorageLocations] storage INNER JOIN [Coordinates] c ON c.CoordinateID = storage.CoordinateID";

                }
                else if (refEndsWith("Modules/FieldServices/MeterRecorders.aspx"))
                {
                    mapObjectType = "MeterRecorder";
                    strURL = "../FieldServices/MeterRecorders.aspx?arg=";

                    // Recorder Icons:
                    // icon25(Green) = Recorders that aren't crying
                    // icon26(Yellow) = Recorders that are begging to be changed
                    // icon27(Red) = Recorders that haven't made contact with the rest of the 
                    //               world in over 48 hours. 

                    // The icon case needs to check the existance of pending first, 
                    // then they need to check for the bad hours. Otherwise the bad
                    // hours will override the pending icon. 

                    jsonSelectCommand = String.Format(@"SELECT  
                                                        mr.[MeterRecorderID], 
                                                        C.[Latitude] as [Lat], 
                                                        C.[Longitude] as [Lon], 
                                                        'MeterRecorder',
                                                        'icon' + CASE 
				                                                      WHEN (EXISTS(select * from [MeterRecorderChangeOrders] mrco where mrco.MeterRecorderID = mr.MeterRecorderID AND mrco.DatePerformed Is Null)) THEN '26' 
			                                                          WHEN (DATEDIFF(hour, (select max(datetimestamp)
                                                                                            From [Readings] R
                                                                                            inner join [Sensors] S on S.SensorID = R.SensorID
                                                                                            inner join MeterRecordersBoards mrb	on mrb.BoardID = S.BoardID
                                                                                            where mrb.MeterRecorderID = mr.MeterRecorderID), GETDATE()) > 48) THEN '27' 
					                                                    ELSE '25' END,
                                                        mr.[MeterRecorderID] AS description
                                                    FROM 
                                                        [MeterRecorders] mr 
                                                    INNER JOIN
                                                        [Premises] P 
                                                    ON
                                                        P.PremiseID = (Select top 1 PremiseID from [MeterRecorderChangeOrders] mrco where mrco.MeterRecorderID = mr.MeterRecorderID and mrco.PremiseID IS NOT NULL order by CreatedOn Desc) 
                                                    INNER JOIN
                                                        [Coordinates] C 
                                                    ON
                                                        P.[CoordinateID] = C.[CoordinateID] 
                                                    {0}
                                                        isNull(C.[Latitude], '0') <> '0'", filter);
                }
                else if (selectCommand.Contains("from [SampleSites]") || SelectCommand.Contains("FROM [BacterialWaterSamples]"))
                {
                    mapObjectType = "Sample_Site";
                    strURL = "iSampleSite.aspx?recordID=";
                    // HACK: this prevents a 'Ambiguous column name' error, but should be replaced with something less hackish.
                    filter = filter.Replace(" [SampleSiteID] ", " [SampleSites].[Id] ");
                    string strBaseQuery = "select DISTINCT [SampleSites].Id, [Coordinates].Latitude as Lat, " +
                        "[Coordinates].Longitude as Lon, 'SampleSite', 'icon' + cast(iconID as varchar(15)), " +
                        "SampleSites.Id AS description from SampleSites " +
                        "inner join [Coordinates] on [Coordinates].[CoordinateID] = [SampleSites].[CoordinateID] ";
                    if (SelectCommand.Contains("FROM [BacterialWaterSamples]"))
                    {
                        strBaseQuery += "inner join [BacterialWaterSamples] on [SampleSites].[Id] = [BacterialWaterSamples].[SampleSiteID] ";
                    }
                    jsonSelectCommand = String.Format("{0} {1} isNull([Coordinates].Latitude, '0') <> '0'", strBaseQuery, filter);
                }
                else if (selectCommand.Contains("FROM [tblWQ_Complaints]"))
                {
                    mapObjectType = "Complaint";
                    strURL = "iWQComplaint.aspx?recordID=";
                    jsonSelectCommand = String.Format("select [{0}].complaint_number, Coordinates.Latitude as Lat, Coordinates.Longitude as Lon, " +
                        "'Complaint', case (left((select lookupvalue from lookup where lookup.lookupID= [{0}].WQ_Complaint_Type), 3)) " +
                        "WHEN 'Aes' then 'icon5' WHEN 'Inf' Then 'icon18' WHEN 'Med' Then 'icon6' else 'icon4' end, " +
                        "[{0}].complaint_number as [description] from [{0}] left join [Coordinates] on [{0}].CoordinateID = Coordinates.CoordinateID " +
                        "left join [PublicWaterSupplies] as b on [{0}].PWSID = b.[Id] {1} " +
                        "isNull(Coordinates.Latitude, '0') <> '0'", "tblWQ_Complaints", filter);
                }
                else if (selectCommand.Contains("FROM [tblEventManagement]"))
                {
                    mapObjectType = "Event";
                    strURL = "iEvent.aspx?recordID=";
                    jsonSelectCommand = String.Format("select [{0}].Event_ID, Coordinates.Latitude as Lat, Coordinates.Longitude as Lon, " +
                        "'Event', 'icon' + cast([Coordinates].[iconID] as varchar(15)), [{0}].Event_ID AS [Description] FROM [{0}] " +
                        "inner join [Coordinates] on [{0}].[CoordinateID] = [Coordinates].[CoordinateID] " +
                        "{1} IsNull([Coordinates].[Latitude], '0') <> '0'", "tblEventManagement", filter);
                }

                else if (selectCommand.Contains("MeterRecorderHistory"))
                {
                    mapObjectType = "MeterRecorderHistory";
                    strURL = "iMeterRecorderHistory.aspx?recordID=";
                    jsonSelectCommand = String.Format("SELECT DISTINCT [MeterRecorderHistoryID], C.[Latitude] as [Lat], C.[Longitude] as [Lon], " +
                        "'MeterRecorderHistory', 'icon' + cast(C.[iconID] as varchar(15)), [MeterRecorderHistoryID] AS description from [MeterRecorderHistory] " +
                        "inner join [Premises] P on P.PremiseID = MeterRecorderHistory.PremiseID " +
                        "inner join [Coordinates] C on P.[CoordinateID] = C.[CoordinateID] " +
                        "{0} isNull(C.[Latitude], '0') <> '0'", filter);
                }
                else if (selectCommand.Contains("Premises"))
                {
                    mapObjectType = "Premise";
                    strURL = "iPremise.aspx?recordID=";
                    jsonSelectCommand = String.Format("SELECT DISTINCT [PremiseID], [Coordinates].[Latitude] as [Lat], [Coordinates].[Longitude] as [Lon], " +
                        "'Premise', 'icon' + cast([Coordinates].[iconID] as varchar(15)), [PremiseID] AS description from [Premises] " +
                        "inner join [Coordinates] on [Premises].[CoordinateID] = [Coordinates].[CoordinateID] " +
                        "{0} isNull([Coordinates].[Latitude], '0') <> '0'", filter);
                }
                else if (selectCommand.Contains("tblConsecutive"))
                {
                    mapObjectType = "ConsecutiveEstimate";
                    strURL = "iConsecutiveEstimate.aspx?recordID=";
                    jsonSelectCommand = String.Format("SELECT DISTINCT [ConEstID], [Coordinates].[Latitude] as [Lat], [Coordinates].[Longitude] as [Lon], " +
                        "'ConsecutiveEstimate', 'icon' + cast([Coordinates].[iconID] as varchar(15)), [ConEstID] AS description from [tblConsecutive_Estimates] " +
                        "inner join [Coordinates] on [tblConsecutive_Estimates].[CoordinateID] = [Coordinates].[CoordinateID] " +
                        "{0} isNull([Coordinates].[Latitude], '0') <> '0'", filter);
                }
                else if (selectCommand.Contains("[InterconnectionTests] it"))
                {
                    mapObjectType = "InterconnectionTest";
                    strURL = "iInterconnectionTest.aspx?recordID=";
                    jsonSelectCommand = String.Format("SELECT DISTINCT " +
                        "InterconnectionTestID, [Coordinates].[Latitude] as [Lat], [Coordinates].[Longitude] as [Lon], " +
                        "'InterconnectionTest', 'icon' + cast([Coordinates].[iconID] as varchar(15)), InterconnectionTestID AS description " +
                        "from [InterconnectionTests] " +
                        "left join [tblFacilities] on tblFacilities.recordid = [InterconnectionTests].facilityID " +
                        "left join [Coordinates] on [tblFacilities].[CoordinateID] = [Coordinates].[CoordinateID] " +
                        "{0} isNull([Coordinates].[Latitude], '0') <> '0'", filter);
                }
                else if (selectCommand.Contains("[CriticalCustomers] AS c"))
                {
                    mapObjectType = "CriticalCustomers";
                    strURL = "iCriticalCustomers.aspx?recordID=";
                    jsonSelectCommand = String.Format("SELECT DISTINCT [CriticalCustomerID], [Coordinates].[Latitude] as [Lat], [Coordinates].[Longitude] as [Lon], " +
                        "'CriticalCustomer', 'icon' + cast([Coordinates].[iconID] as varchar(15)), [CriticalCustomerID] AS description from [CriticalCustomers] " +
                        "inner join [Coordinates] on [CriticalCustomers].[CoordinateID] = [Coordinates].[CoordinateID] " +
                        "{0} isNull([Coordinates].[Latitude], '0') <> '0'", filter);
                }
                else
                {
                    mapObjectType = "Facility";
                    strURL = "iFacility.aspx?recordID=";
                    jsonSelectCommand = String.Format("SELECT DISTINCT [FacilityID], [Coordinates].[Latitude] as [Lat], [Coordinates].[Longitude] as [Lon], " +
                        "'Facility', 'icon' + cast([Coordinates].[iconID] as varchar(15)), [RecordId] AS description from [tblFacilities] " +
                        "inner join [Coordinates] on [tblFacilities].[CoordinateID] = [Coordinates].[CoordinateID] " +
                        "{0} isNull([Coordinates].[Latitude], '0') <> '0'", filter);
                }
                LoadMarkersJSON(
                    mapObjectType,
                    jsonSelectCommand,
                    SqlConnectionString,
                    true
                );

                _mapDataLayerId = mapObjectType;
            }
        }

        /// <summary>
        /// This gets called after the page loads and starts
        /// retrieving the marker for the map. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn1_Click(object sender, EventArgs e)
        {
            InitializeMaps();
        }

        private static string FormatJsonCommand(string command, string filter)
        {
            const string coord = "isNull([Coordinates].[Latitude], '0') <> '0'";

            command += " WHERE ";

            if (!string.IsNullOrWhiteSpace(filter))
            {
                command += filter + " AND ";
            }

            command += coord;

            return command;
        }

        private void InitializeFlushingSchedulesMap()
        {
            var mapObjectType = "FlushingSchedule";
            strURL = "iFlushingSchedule.aspx?ID=";

            var jsonSelectCommand = FormatJsonCommand(SQL.FLUSHING_SCHEDULES, filter);

            LoadMarkersJSON(
                mapObjectType,
                jsonSelectCommand,
                SqlConnectionString,
                true
            );
        }

        private void InitializeMeterRoutesMap()
        {
            strURL = "iMeterRoute.aspx?recordID=";
            var jsonSelectCommand = FormatJsonCommand(SQL.METER_ROUTES, filter);

            LoadMarkersJSON(
               "MeterRoute",
               jsonSelectCommand,
               SqlConnectionString,
               true
           );
        }

        private void InitializeLocalUnionsMap()
        {
            strURL = "../HR/Union/Locals.aspx?view=";
            var jsonSelectCommand
                 = FormatJsonCommand(@"SELECT 
	                                [LocalBargainingUnits].LocalID,
                                    Coordinates.[Latitude] as [Lat], 
                                    Coordinates.[Longitude] as [Lon], 
                                    'Local',
                                    'icon' + cast(Coordinates.IconID as varchar(15)),
	                                [LocalBargainingUnits].LocalID as 'description'
                                    FROM [LocalBargainingUnits]
                                    LEFT JOIN [tblBargaining_Unit] on [tblBargaining_Unit].BargainingUnitID = [LocalBargainingUnits].BargainingUnitID
	                                LEFT JOIN [Coordinates] on Coordinates.CoordinateID = LocalBargainingUnits.CoordinateID 
                                    ", filter);
            LoadMarkersJSON(
              "Local",
              jsonSelectCommand,
              SqlConnectionString,
              true
          );
        }

        private void InitializeProjectsRPMap()
        {
            //ControlExtensions.FindControl<Panel>(Page, "pnlProjects").Visible = true;
            //ControlExtensions.FindControl<Panel>(Page, "pnlWaterQuality").Visible = false;
            strURL = "../Engineering/ProjectsRP.aspx?hideMenu=true&view=";
            var jsonSelectCommand
                 = FormatJsonCommand(@"SELECT 
	                                rp.RPProjectID as ProjectID,
                                    Coordinates.[Latitude] as [Lat], 
                                    Coordinates.[Longitude] as [Lon], 
                                    'Project RP',
                                    'icon' + cast(iconId as varchar),
	                                rp.RPProjectID as 'description'
                                    FROM RPProjects rp
	                                LEFT JOIN [Coordinates] on Coordinates.CoordinateID = rp.CoordinateID 
                                    ", filter);
            LoadMarkersJSON(
              "ProjectsRP",
              jsonSelectCommand,
              SqlConnectionString,
              true
          );
        }

        protected void btnLoadSampleSites_Click(object sender, EventArgs e)
        {
            LoadSampleSites();
        }
        protected void btnLoadFlushingSChedules_Click(object sender, EventArgs e)
        {
            LoadFlushingSchedules();
        }

        protected void btnLoadMeterRoutes_Click(object sender, EventArgs e)
        {
            LoadMeterRoutes();
        }

        #endregion

        #region Private Methods

        private void CheckForSearchQuery()
        {
            var searchQuery = Request.QueryString[DataPageUtility.QUERY.SEARCH];
            if (String.IsNullOrWhiteSpace(searchQuery))
            {
                return;
            }

            Guid search;

            if (!Guid.TryParse(searchQuery, out search))
            {
                throw new InvalidOperationException("Invalid search parameter");
            }

            // Will be null on older pages. All properties will look 
            // for _filterBuilder values first before going with the
            // old viewstate stuff.
            _filterBuilder = new FilterCache().GetFilterBuilder(search);

        }

        /// <summary>
        /// This method is for backwards compatibility. Everything was just passing any old
        /// string, now we're gonna use a proper DateTime object and return an empty string
        /// if there's bad values.
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private string GetDateString(DateTime? dt)
        {
            if (dt.HasValue)
            {
                return dt.Value.ToString();
            }
            return string.Empty;
        }

        private void LoadSampleSites()
        {
            strURL = "iSampleSite.aspx?recordID=";
            var jsonSelectCommand = "select DISTINCT [SampleSites].Id, [Coordinates].Latitude as Lat, " +
                    "[Coordinates].Longitude as Lon, 'SampleSite', 'icon' + cast(iconID as varchar(15)), " +
                    "SampleSites.Id AS description from SampleSites " +
                    "inner join [Coordinates] on [Coordinates].[CoordinateID] = [SampleSites].[CoordinateID] ";
            LoadMarkersJSON(
                "Sample_Site",
                jsonSelectCommand,
                ConfigurationManager.ConnectionStrings["MCProd"].ToString(),
                false
            );
        }

        private void LoadFlushingSchedules()
        {
            strURL = "iFlushingSchedule.aspx?ID=";     // no easy way to send opCode at this point

            var jsonSelectCommand = String.Format(SQL.FLUSHING_SCHEDULES +
                 " WHERE Latitude >= {0} AND Latitude < {1} AND Longitude > {2} and Longitude < {3}",
                     txtSouthWestLat.Text,
                     txtNorthEastLat.Text,
                     txtSouthWestLng.Text,
                     txtNorthEastLng.Text
                     );

            LoadMarkersJSON(
                "FlushingSchedule",
                jsonSelectCommand,
                ConfigurationManager.ConnectionStrings["MCProd"].ToString(),
                false
            );
        }
        private void LoadMeterRoutes()
        {
            strURL = "iMeterRoute.aspx?recordID=";
            var jsonSelectCommand = String.Format(SQL.METER_ROUTES +
                " WHERE Latitude >= {0} AND Latitude < {1} AND Longitude > {2} and Longitude < {3}",
                    txtSouthWestLat.Text,
                    txtNorthEastLat.Text,
                    txtSouthWestLng.Text,
                    txtNorthEastLng.Text
                    );

            LoadMarkersJSON(
               "MeterRoute",
               jsonSelectCommand,
               ConfigurationManager.ConnectionStrings["MCProd"].ToString(),
               false
           );
        }
        private string GetMarkerScript(string mapObjectType, string sql, string connection, bool isInitialData)
        {
            var json = new Dictionary<string, object>();
            json["center"] = new
            {
                lat = Convert.ToDecimal(ConfigurationManager.AppSettings["DefaultMapCenterLatitude"]),
                lng = Convert.ToDecimal(ConfigurationManager.AppSettings["DefaultMapCenterLongitude"])
            };
            json["layerId"] = mapObjectType;

            var coordinates = new List<JsonCoordinate>();
            json["coordinates"] = coordinates;

            using (var conn = new SqlConnection(connection))
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.Text;

                if (_filterBuilder != null)
                {
                    foreach (var p in _filterBuilder.BuildSqlParameters())
                    {
                        cmd.Parameters.Add(p);
                    }
                }

                conn.Open();
                using (var dr = cmd.ExecuteReader())
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            var jc = new JsonCoordinate
                            {
                               // title = dr[0].ToString(), // TODO: Is this still used?
                                lat = dr[1].ToString(),
                                lng = dr[2].ToString(),
                                type = dr[3].ToString(),

                                // I have no desire to go through and fix every sql command. -Ross 7/30/2014
                                iconId = dr[4].ToString().Replace("icon", ""),
                                url = strURL + Server.HtmlEncode(dr[5].ToString())
                            };
                            coordinates.Add(jc);
                        }

                    }
                }
            }
            var sb = new StringBuilder();

            sb.AppendFormat("Maps.loadMapData({0});", new JavaScriptSerializer().Serialize(json));

            return sb.ToString();
        }

        private void LoadMarkersJSON(string mapObjectType, string sql, string connection, bool isInitialData)
        {
            var sb = GetMarkerScript(mapObjectType, sql, connection, isInitialData);
            if (!IsPostBack)
                _renderedMarkerScript = sb;
            else
                ScriptManager.RegisterStartupScript(Page, typeof(string), String.Format("Load{0}MarkersScript", mapObjectType), sb, true);
        }

        private string _renderedMarkerScript;

        public string RenderMarkerScript()
        {
            return _renderedMarkerScript;
        }

        protected string GetMapIcons()
        {
            // I think this method could go in a remote script, allowing for it to be cached instead
            // of having to re-retrieve this every time.
            // -Ross

            var icons = DependencyResolver.Current.GetService<IRepository<MapIcon>>().GetAll()
             .Select(x => new
            {
                id = x.Id,
                url = ResolveUrl("~/images/" + x.FileName),
                width = x.Width,
                height = x.Height,
                offset = x.Offset.Description
            }).ToArray();

            return new JavaScriptSerializer().Serialize(icons);
        }

        protected void Page_Load()
        {
            DataBind();
        }

        #endregion

        #region Private classes

        private class JsonCoordinate
        {
            //public string title { get; set; }
            public string lat { get; set; }
            public string lng { get; set; }
            public string type { get; set; }
            public string iconId { get; set; }
            public string url { get; set; }
        }

        #endregion
    }
}
