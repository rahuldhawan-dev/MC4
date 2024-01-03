using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using MMSINC;
using MMSINC.DataPages;
using MMSINC.Page;
using MMSINC.Utilities.Permissions;
using dotnetCHARTING;
using Orientation = dotnetCHARTING.Orientation;

namespace MapCall.Modules.FieldServices
{
    public partial class MeterRecorders : DataElementPageWithDetailView
    {
        #region Constants

        private const int DEFAULT_CHART_HEIGHT = 275;
        private const string SQL_CONNECTION_STRING_MCPROD = "mcprod";
        private const string SELECT_VIEWSTATE = "AppendedResultsSelectStatement";
        private const int WORK_DESCRIPTION_NEWMETER = 1;

        private struct DataElementConsts
        {
            internal const string DataElementName = "MeterRecorders";
            internal const string DataElementID = "MeterRecorderID";
        }

        private struct QueryFormatStrings
        {
            public const string SELECT_SENSORS =
                "select distinct S.SensorID From [Readings] R inner join [Sensors] S on S.SensorID = R.SensorID inner join [Boards] B on B.BoardID = S.BoardID inner join MeterRecordersBoards mrb on mrb.BoardID = B.BoardID where MeterRecorderID = {0} ";
            public const string SELECT_DATE_TIME_STAMPS_FOR_A_GIVEN_DATE =
                "select distinct datetimestamp from [Readings] where datediff(hh, datetimestamp, '{0}') < 24 and datediff(hh, datetimestamp, '{0}') > 0 order by datetimestamp desc";
            public const string SELECT_SCALED_DATA_BY_SENSOR_ID_AND_DATE_TIME_STAMP =
                "select sensorid, datetimestamp, scaleddata/15 as scaleddata from [Readings] where sensorid = {0} and datetimestamp = '{1}'";
            public const string SELECT_MOST_RECENT_SCALED_DATUM_BY_SENSOR =
                "select top 1 scaleddata/15 as scaleddata, datetimestamp from [Readings] where sensorid = {0} order by datetimestamp desc";
        }

        private struct SqlColumnNames
        {
            public const string SENSOR_ID = "sensorid";
            public const string DATE_TIME_STAMP = "datetimestamp";
            public const string SCALED_DATA = "scaleddata";

        }

        private struct ViewStateNames
        {
            public const string CURRENTLY_SEARCHED_DATE = "CurrentlySearchedDate";
            public const string CHART_HEIGHT = "ChartHeight";
        }

        #endregion

        #region Enumerations

        protected enum InstallationSearchOptions
        {
            Both = 0,
            CurrentlyInstalled = 1,
            CurrentlyUninstalled = 2
        }

        #endregion

        #region Properties

        protected override IModulePermissions ModulePermissions
        {
            get
            {
                return Common.Utility.Permissions.Modules.FieldServices.Meters;
            }
        }

        public override DetailsView DetailView
        {
            get { return DetailsView1; }
        }

        public override SqlDataSource DataSource
        {
            get { return dsDetailView; }
        }

        /// <summary>
        /// Gets or sets the appended where clause added to the results sql source select statement.
        /// </summary>
        /// <remarks>
        /// 
        /// This needs to be stored because the statement is reverted back after posting. If this doesn't
        /// get reset on the select statement, when a user goes to sort a table, it'll only search
        /// with the base select statement, displaying a ton of stuff they don't want.
        /// 
        /// </remarks>
        public string AdjustedResultsSelectStatement
        {
            get
            {
                var rss = ViewState[SELECT_VIEWSTATE];
                return (rss != null ? rss.ToString() : string.Empty);
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    ViewState[SELECT_VIEWSTATE] = value;
                }
                else
                {
                    ViewState.Remove(SELECT_VIEWSTATE);
                }
            }
        }

        #region Chart Properties
        public DateTime CurrentlySearchedDate
        {
            get
            {
                if (ViewState[ViewStateNames.CURRENTLY_SEARCHED_DATE] == null)
                {
                    ViewState[ViewStateNames.CURRENTLY_SEARCHED_DATE] = DateTime.Now;
                }
                return DateTime.Parse(ViewState[ViewStateNames.CURRENTLY_SEARCHED_DATE].ToString());
            }
            set { ViewState[ViewStateNames.CURRENTLY_SEARCHED_DATE] = value; }
        }

        public int ChartHeight
        {
            get
            {
                if (ViewState[ViewStateNames.CHART_HEIGHT] == null)
                {
                    ViewState[ViewStateNames.CHART_HEIGHT] = DEFAULT_CHART_HEIGHT;
                }
                return int.Parse(ViewState[ViewStateNames.CHART_HEIGHT].ToString());
            }
            set { ViewState[ViewStateNames.CHART_HEIGHT] = value; }
        }
        #endregion

        #endregion
        
        #region Private Methods


        private void SetResultSelectStatement()
        {
            var installOption = (InstallationSearchOptions)Enum.Parse(typeof(InstallationSearchOptions), ddlInstallOptions.SelectedValue);

            var sb = new StringBuilder();

            switch (installOption)
            {
                case InstallationSearchOptions.Both:

                    sb.Append(" WHERE hist.DateInstalled =");
                    sb.Append(" (SELECT");
                    sb.Append("     MAX(DateInstalled)");
                    sb.Append(" FROM");
                    sb.Append("     MeterRecorderHistory mrh2");
                    sb.Append(" WHERE");
                    sb.Append("     mrh2.MeterRecorderID = hist.MeterRecorderID)");
                    sb.Append(" OR hist.DateInstalled is null");

                    break;

                case InstallationSearchOptions.CurrentlyInstalled:
                    sb.Append(" WHERE hist.DateInstalled =");
                    sb.Append(" (SELECT");
                    sb.Append("      MAX(DateInstalled)");
                    sb.Append(" FROM");
                    sb.Append("      MeterRecorderHistory mrh2");
                    sb.Append(" WHERE");
                    sb.Append(" mrh2.MeterRecorderID = hist.MeterRecorderID)");

                    break;

                case InstallationSearchOptions.CurrentlyUninstalled:
                    sb.Append(" WHERE hist.DateInstalled is null");
                    break;

            }

            SqlDataSource1.SelectCommand += sb.ToString(); ;
            AdjustedResultsSelectStatement = SqlDataSource1.SelectCommand;
        }


        private void ShowDetailView(int id)
        {
            DetailView.ChangeMode(DetailsViewMode.ReadOnly);
            DataSource.SelectParameters[DataElementConsts.DataElementID].DefaultValue = id.ToString();
            DetailView.DataBind();

            Audit.Insert(
                (int)AuditCategory.DataView,
                Page.User.Identity.Name,
                String.Format("Viewed {0} ID:{1}", DataElementConsts.DataElementName, id),
                ConfigurationManager.ConnectionStrings["MCProd"].ToString()
                );


            pnlSearch.Visible = false;
            pnlResults.Visible = false;
            pnlDetail.Visible = true;
        }

        private void ShowDetailView(string param)
        {
            var id = Int32.Parse(param);
            ShowDetailView(id);
        }

        #region Chart Private Methods
        private void DefaultChartSettings()
        {
            //TODO: make this a setting in web.config?
            Chart.TempDirectory = "~/temp";

            Chart.Type = ChartType.ComboHorizontal;
            Chart.Title = "Meter Recorder Readings";

            Chart.Use3D = false;

            Chart.DefaultSeries.Type = SeriesType.Line;
            Chart.Height = ChartHeight;
            Chart.Width = 500;
            Chart.DefaultElement.Marker.Visible = false;
            Chart.DefaultSeries.Line.Width = 2;

            //move legend to top
            Chart.LegendBox.Orientation = Orientation.Top;
            Chart.LegendBox.Template = "%Name%Icon";
            Chart.Palette = new[] { Color.FromArgb(49, 255, 49), Color.FromArgb(255, 99, 49), Color.FromArgb(0, 156, 255) };
        }

        private void SetupChart()
        {
            var data = GetChartData();
            if (data != null)
                Chart.SeriesCollection.Add(data);
        }

        private SeriesCollection GetChartData()
        {
            var sensorIds = GetSensorsForSelectedMeterRecorder();
            if (sensorIds == null || sensorIds.Length == 0)
                return null;

            var dateTimeStamps = GetDateTimeStampsForAGivenDate(CurrentlySearchedDate);
            txtDate.Text = CurrentlySearchedDate.ToShortDateString();

            return GenerateSeriesCollection(sensorIds, dateTimeStamps);
        }

        private string[] GetSensorsForSelectedMeterRecorder()
        {
            StringBuilder sb = new StringBuilder();

            using (var sqlConnection = new SqlConnection { ConnectionString = ConfigurationManager.ConnectionStrings[SQL_CONNECTION_STRING_MCPROD].ConnectionString })
            {
                sqlConnection.Open();

                using (var sqlCommand = new SqlCommand(string.Format(QueryFormatStrings.SELECT_SENSORS, Notes1.DataLinkID), sqlConnection))
                {
                    using (var reader = sqlCommand.ExecuteReader())
                    {
                        if (reader != null && reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                sb.Append(reader[SqlColumnNames.SENSOR_ID]);
                                sb.Append(" ");
                            }
                            //Remove the last space
                            sb.Remove(sb.Length - 1, 1);
                        }
                    }
                }
            }

            return sb.Length > 0 ? sb.ToString().Split(' ') : null;
        }

        private static ArrayList GetDateTimeStampsForAGivenDate(DateTime date)
        {
            var dateTimeStamps = new ArrayList();

            using (var sqlConnection = new SqlConnection { ConnectionString = ConfigurationManager.ConnectionStrings[SQL_CONNECTION_STRING_MCPROD].ConnectionString })
            {
                sqlConnection.Open();

                using (var sqlCommand = new SqlCommand(string.Format(QueryFormatStrings.SELECT_DATE_TIME_STAMPS_FOR_A_GIVEN_DATE, date), sqlConnection))
                {
                    using (var reader = sqlCommand.ExecuteReader())
                    {
                        if (reader != null && reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                //Build a set of datetimestamps
                                dateTimeStamps.Add(reader[SqlColumnNames.DATE_TIME_STAMP].ToString());
                            }
                        }
                    }
                }
            }

            return dateTimeStamps;
        }

        private static SeriesCollection GenerateSeriesCollection(ICollection<string> sensors, ArrayList dateTimeStamps)
        {
            SeriesCollection sc = new SeriesCollection();

            using (var sqlConnection = new SqlConnection { ConnectionString = ConfigurationManager.ConnectionStrings[SQL_CONNECTION_STRING_MCPROD].ConnectionString })
            {
                sqlConnection.Open();

                //For each sensor get a list of readings for the datetimes
                foreach (string sensor in sensors)
                {
                    //Get the latest reading for the sensor
                    object[] lastSensorReading = GetLastSensorReading(sensor);
                    //Create a series for each sensor.
                    Series s = new Series { Name = string.Format("Sensor: {0}\nLast Reading: {1}\n{2}", sensor, lastSensorReading[0], lastSensorReading[1]) };

                    foreach (var time in dateTimeStamps)
                    {
                        using (var sqlCommand = new SqlCommand(string.Format(QueryFormatStrings.SELECT_SCALED_DATA_BY_SENSOR_ID_AND_DATE_TIME_STAMP, sensor, time), sqlConnection))
                        {
                            using (var reader = sqlCommand.ExecuteReader())
                            {
                                if (reader == null || !reader.HasRows)
                                {
                                    s.Elements.Add(new Element
                                                      {
                                                          XValue = 0.0,
                                                          YDateTime = DateTime.Parse(time.ToString())
                                                      });
                                }
                                else
                                {
                                    while (reader.Read())
                                    {
                                        Element e = new Element
                                                        {
                                                            XValue =
                                                                double.Parse(
                                                                reader[SqlColumnNames.SCALED_DATA].ToString()),
                                                            YDateTime =
                                                                DateTime.Parse(
                                                                reader[SqlColumnNames.DATE_TIME_STAMP].ToString())
                                                        };
                                        s.Elements.Add(e);
                                    }
                                }
                            }
                        }
                    }

                    //Add the series to the collection
                    sc.Add(s);
                }
            }

            if (sensors.Count <= 1)
                return sc;

            //If there is more than one sensor, sum the values of all the sensors and create a new series line.
            Series sumSeries = new Series { Name = "Sum Of Sensors", };

            //For a given date...
            foreach (string time in dateTimeStamps)
            {
                //Get the scaleddata value for each sensor and get a total.
                double total = 0;

                foreach (string sensor in sensors)
                {
                    using (var sqlConnection = new SqlConnection { ConnectionString = ConfigurationManager.ConnectionStrings[SQL_CONNECTION_STRING_MCPROD].ConnectionString })
                    {
                        sqlConnection.Open();

                        using (var sqlCommand = new SqlCommand(string.Format(QueryFormatStrings.SELECT_SCALED_DATA_BY_SENSOR_ID_AND_DATE_TIME_STAMP, sensor, time), sqlConnection))
                        {
                            using (var reader = sqlCommand.ExecuteReader())
                            {
                                if (reader == null || !reader.HasRows) continue;
                                while (reader.Read())
                                {
                                    total += double.Parse(reader[SqlColumnNames.SCALED_DATA].ToString());
                                }
                            }
                        }
                    }
                }
                //If the total is 0 then there were no reading for any sensors at this datetime. Skip it.
                if (total == 0) continue;

                Element e = new Element { XValue = total, YDateTime = DateTime.Parse(time) };
                sumSeries.Elements.Add(e);
            }

            sc.Add(sumSeries);

            return sc;
        }

        private static object[] GetLastSensorReading(string sensor)
        {
            using (var sqlConnection = new SqlConnection { ConnectionString = ConfigurationManager.ConnectionStrings[SQL_CONNECTION_STRING_MCPROD].ConnectionString })
            {
                sqlConnection.Open();

                using (var sqlCommand = new SqlCommand(string.Format(QueryFormatStrings.SELECT_MOST_RECENT_SCALED_DATUM_BY_SENSOR, sensor), sqlConnection))
                {
                    using (var reader = sqlCommand.ExecuteReader())
                    {
                        if (reader == null || !reader.HasRows)
                            throw new Exception(string.Format("No Data retrieved for sensor {0}.  This should have data.", sensor));
                        while (reader.Read())
                        {
                            object[] row = new object[reader.FieldCount];
                            reader.GetValues(row);

                            return row;
                        }
                    }
                }
            }
            return null;
        }


        #endregion

        #endregion

        #region Event Handlers

        #region Page Events
        
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!CanView)
            {
                pnlSearch.Visible = false;
                lblPermissionErrors.Text = String.Format("Access Denied => {0} : {1}", ModulePermissions.Application, ModulePermissions.Module);
            }
            btnAdd.Visible =
                Notes1.AllowAdd =
                Documents1.AllowAdd = CanAdd;


            // Only reset this if pnlSearch is visible. The other panels use the cached
            // select statement for sorting/mapping/whatever. 
            if (!pnlSearch.Visible && !String.IsNullOrEmpty(AdjustedResultsSelectStatement))
            {
                SqlDataSource1.SelectCommand = AdjustedResultsSelectStatement;
            }

            //Handle "arg" Make sure this gets appropriately switched to "view" and updated
            // on the Maps page when converted to DataPageBase.
            if (!IsPostBack)
            {
                var param = Request.QueryString["arg"];
                if (param != null)
                {
                    var id = 0;
                    if (int.TryParse(param, out id))
                    {
                        ShowDetailView(param);
                    }
                }

            }
        }


        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            // This button shouldn't be visible in Insert mode since the recorder won't
            // exist yet.
            btnCreateChangeOrder.Visible = (DetailsView1.CurrentMode == DetailsViewMode.ReadOnly);

            if (pnlDetail.Visible)
            {
                DefaultChartSettings();
                SetupChart();
            }
        }

        #endregion

        protected void DetailView_DataBound(object sender, EventArgs e)
        {
            var name = Page.User.Identity.Name;

            var canEdit = CanEdit;
            var canDelete = CanDelete;

            Notes1.AllowAdd = Documents1.AllowAdd = CanAdd;
            Notes1.AllowEdit = Documents1.AllowEdit = canEdit;
            Notes1.AllowDelete = Documents1.AllowDelete = canDelete;

            var btnEdit = MMSINC.Utility.GetFirstControlInstance(DetailView, "btnEdit");
            var btnDelete = MMSINC.Utility.GetFirstControlInstance(DetailView, "btnDelete");

            if (btnEdit != null)
                btnEdit.Visible = canEdit;
            if (btnDelete != null)
                btnDelete.Visible = canDelete;
        }
 
        protected void btnCreateChangeOrder_Click(object sender, EventArgs e)
        {
            var val = DetailsView1.SelectedValue;
            var path = ResolveClientUrl("~/Modules/FieldServices/MeterRecorderChangeOrders.aspx");

            Response.Redirect(String.Format("{0}?createNew=&recorderId={1}", path, val));
        }

        protected override void SqlDataSource1_Inserted(object sender, SqlDataSourceStatusEventArgs e)
        {
            base.SqlDataSource1_Inserted(sender, e);

            // This should re-forward the page so that reloading the page doesn't ask about reposting form
            // data and then inserting twice.

            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MCProd"].ToString()))
            {
                conn.Open();
                using (var comm = conn.CreateCommand())
                {
                    comm.CommandText =
                        "INSERT INTO [MeterRecorderChangeOrders] ([MeterRecorderID], [MeterRecorderWorkDescriptionID], [DatePerformed], [MeterRecorderStorageLocationID], [ChangeOrderDescription], [CreatedBy]) VALUES (@MeterRecorderID, @MeterRecorderWorkDescriptionID, @DatePerformed, @MeterRecorderStorageLocationID, @ChangeOrderDescription, @CreatedBy)";

                    var pageParams = e.Command.Parameters;
                    var commParams = comm.Parameters;

                    commParams.Add(new SqlParameter("MeterRecorderID", pageParams["@MeterRecorderID"].Value));
                    commParams.Add(new SqlParameter("MeterRecorderWorkDescriptionID", WORK_DESCRIPTION_NEWMETER));
                    commParams.Add(new SqlParameter("DatePerformed", DateTime.Now));
                    commParams.Add(new SqlParameter("ChangeOrderDescription", "New meter recorder."));
                    commParams.Add(new SqlParameter("CreatedBy", pageParams["@CreatedBy"].Value));
                    commParams.Add(new SqlParameter("MeterRecorderStorageLocationID", pageParams["@MeterRecorderStorageLocationID"].Value));

                    comm.ExecuteNonQuery();
                }
            }
        }

        protected override void btnSearch_Click(object sender, EventArgs e)
        {
            var sb = new StringBuilder();
            foreach (var ctrl in pnlSearch.Controls.OfType<IDataField>())
            {
                sb.Append(ctrl.FilterExpression());
            }

            var selectedManufacturer = ddlManufacturerSearch.SelectedValue;
            if (!string.IsNullOrEmpty(selectedManufacturer))
            {
                sb.Append(" AND MeterRecorderManufacturerID = " + int.Parse(selectedManufacturer));
            }

            SetResultSelectStatement();

            Filter = (sb.Length > 0) ? sb.ToString().Substring(5) : String.Empty;
            SqlDataSource1.FilterExpression = Filter;
            hidFilter.Value = Filter;
            
            base.btnSearch_Click(sender, e);
            lblRecordCount.Text = String.Format("Total Records: {0}",
                                                GridView1.Rows.Count);
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (GridView1.SelectedDataKey != null)
            {
                var id = int.Parse(GridView1.SelectedDataKey.Value.ToString());

                ShowDetailView(id);

                Notes1.DataLinkID = id;
                Notes1.Visible = true;
                Documents1.DataLinkID = id;
                Documents1.Visible = true;
            }
        }

        protected void btnExportReadings_Click(object sender, EventArgs e)
        {
            VerifyRenderingInServerForm(gvReadings);
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename=Streets.xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.xls";

            var sw = new StringWriter();
            var htmlwriter = new HtmlTextWriter(sw);
            gvReadings.AllowPaging = false;
            gvReadings.AllowSorting = false;
            gvReadings.DataBind();

            gvReadings.RenderControl(htmlwriter);
            Response.Write(sw.ToString());
            Response.End();

            htmlwriter.Dispose();
            sw.Dispose();
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            //base.VerifyRenderingInServerForm(control);
        }

        protected void gridViewChangeOrders_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            var row = e.Row;

            if (row.RowType == DataControlRowType.DataRow)
            {
                var datePerformed = DataBinder.Eval(row.DataItem, "DatePerformed");

                if (datePerformed == DBNull.Value)
                {
                    row.CssClass += (row.RowIndex % 2 == 0 ? "pendingRow" : "pendingAltRow");
                }
            }
        }

        protected void gridViewChangeOrders_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            var id = Int32.Parse(gridViewChangeOrders.SelectedDataKey[0].ToString());
            Response.Redirect("~/Modules/FieldServices/MeterRecorderChangeOrders.aspx?view=" + id);
        }

        #region Chart Buttons

        protected void lbDatePrev_Click(object sender, EventArgs e)
        {
            CurrentlySearchedDate = CurrentlySearchedDate.AddDays(-1);
        }

        protected void lbDateNext_Click(object sender, EventArgs e)
        {
            CurrentlySearchedDate = CurrentlySearchedDate.AddDays(1);
        }

        protected void lbToday_Click(object sender, EventArgs e)
        {
            CurrentlySearchedDate = DateTime.Now;
        }

        protected void lbSizePlus_Click(object sender, EventArgs e)
        {
            ChartHeight += 50;
        }

        protected void lbSizeMinus_Click(object sender, EventArgs e)
        {
            ChartHeight -= 50;
        }
        
        protected void btnUpdateChart_Click(object sender, EventArgs e)
        {
            CurrentlySearchedDate = DateTime.Parse(txtDate.Text);
        }

        #endregion

        #endregion
    }
}

