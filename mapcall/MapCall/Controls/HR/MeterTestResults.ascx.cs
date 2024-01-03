using System;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;
using MMSINC;
using dotnetCHARTING;
using Orientation = dotnetCHARTING.Orientation;

namespace MapCall.Controls.HR
{
    public partial class MeterTestResults : UserControl
    {
        #region Control Declarations

        protected SqlDataSource dsMeterOutputs;

        #endregion
        
        #region Constructors

        public MeterTestResults()
        {
            AllowDelete = true;
            AllowEdit = true;
            AllowAdd = true;
        }

        #endregion

        #region Properties

        public int MeterTestID
        {
            get {
                return ViewState["MeterTestID"] == null
                           ? 0
                           : Int32.Parse(ViewState["MeterTestID"].ToString());
            }
            set { ViewState["MeterTestID"] = value.ToString(); }
        }

        public bool AllowDelete { get; set; }
        public bool AllowEdit { get; set; }
        public bool AllowAdd { get; set; }
        public int MeterOutputs { get; set; }

        #endregion

        #region Private Methods

        private void LoadGridView()
        {
            SetMeterTestID();
            gvTestResults.Visible = true;
        }
       
        private void SetMeterTestID()
        {
            dsMeterTestResults.SelectParameters["MeterTestID"].DefaultValue =
                MeterTestID.ToString();
            dsMeterTestResults.UpdateParameters["MeterTestID"].DefaultValue =
                MeterTestID.ToString();
        }

        private void SetupChart()
        {
            DefaultChartSettings();

            var data = GetSeriesCollection();
            if (data != null)
            {
                Chart.SeriesCollection.Add(data);
                Chart2.SeriesCollection.Add(data);
            }
        }
        
        private void SetMeterOutputs()
        {
            if (MeterOutputs > 0) return;

            dsMeterOutputs.SelectParameters["MeterTestID"].DefaultValue =
                MeterTestID.ToString();
            var dv =
                (DataView)
                dsMeterOutputs.Select(DataSourceSelectArguments.Empty);
            if (dv.Count>0)
            {
                MeterOutputs = Int32.Parse(dv[0].Row[0].ToString());
            }
            dv.Dispose();
        }

        private SeriesCollection GetSeriesCollection()
        {
            SeriesCollection seriesCollection = new SeriesCollection();

            Series subjectSeries = new Series
            {
                Name = "Subject Meter",
            };

            Series testSeries = new Series
            {
                Name = "Test Meter", 
            };


            foreach (DataRowView row in dsMeterTestResults.Select(new DataSourceSelectArguments()))
            {
                Element subjectElement = new Element();
                double subjectMeterTotalVolume = 0;
                double subjectMeterAccuracy = 0;
                double.TryParse(row["SubjectMeterTotalVolume"].ToString(), out subjectMeterTotalVolume);
                double.TryParse(row["SubjectMeterAccuracy"].ToString(), out subjectMeterAccuracy);

                subjectElement.YValue = subjectMeterTotalVolume;
                subjectElement.XValue = subjectMeterAccuracy;
                subjectSeries.AddElements(subjectElement);

                Element testElement = new Element();
                double testMeterVolume = 0;
                double testMeterAccuracy = 0;
                double.TryParse(row["testMeterVolume"].ToString(), out testMeterVolume);
                double.TryParse(row["testMeterAccuracy"].ToString(), out testMeterAccuracy);
                testElement.YValue = testMeterVolume;
                testElement.XValue = testMeterAccuracy;
                
                testSeries.AddElements(testElement);

            }

            seriesCollection.Add(subjectSeries);
            seriesCollection.Add(testSeries);


            return seriesCollection;
        }

        private void DefaultChartSettings()
        {
            var charts = new[] {Chart, Chart2};
            foreach (var chart in charts)
            {
                chart.TempDirectory = "~/temp";
                chart.Type = ChartType.ComboHorizontal;

                chart.Title = string.Empty;

                chart.Use3D = false;

                chart.DefaultSeries.Type = SeriesType.Line;
                chart.Height = 300;
                chart.Width = 470;
                chart.DefaultElement.Marker.Visible = true;
                chart.DefaultSeries.Line.Width = 2;

                chart.XAxis.Label.Text = "Volume";
                chart.YAxis.Label.Text = "Accuracy %";

                //move legend to top
                chart.LegendBox.Orientation = Orientation.Top;
                chart.LegendBox.Template = "%Name%Icon";
                chart.Palette =
                    new[]
                        {
                            Color.FromArgb(49, 255, 49),
                            Color.FromArgb(255, 99, 49),
                            Color.FromArgb(0, 156, 255)
                        };
            }
            Chart2.YAxis.ScaleRange.ValueHigh = 102;
            Chart2.YAxis.ScaleRange.ValueLow = 98;

        }


        #endregion

        #region Event Handlers

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (MeterTestID != 0)
            {
                LoadGridView();
                SetMeterOutputs();
            }
            dvMeterTestResult.ChangeMode(DetailsViewMode.Insert);
            dvMeterTestResult.DataBind();
            gvTestResults.DataBind();
            SetupChart();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            btnAddResult.Visible = AllowAdd;
            gvTestResults.Columns[0].ItemStyle.CssClass = "NoteCell";
            if (MeterTestID != 0)
            {
                LoadGridView();
                SetMeterOutputs();
            }
            else
            {
                gvTestResults.Visible = false;
            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow) return;

            //TODO: Fragile code here. Position instead of direct lookup.
            var btnDelete = e.Row.Controls[1].FindControl("btnDelete");
            if (btnDelete != null) btnDelete.Visible = AllowDelete;
            var btnEdit = e.Row.Controls[1].FindControl("btnEdit");
            if (btnEdit != null) btnEdit.Visible = AllowEdit;

            //TODO: Even more fragile
            //TODO: Even more more fragile: MAKE SURE TO REFERENCE THE FULL BOUNDFIELD TYPE NAME
            //                              Or else it assumes it's MapCall.Controls.BoundField 
            //                              for some annoying reason.
            var bf =
                ((System.Web.UI.WebControls.BoundField)
                 (((DataControlFieldCell) (e.Row.Cells[1])).ContainingField));
            bf.ReadOnly = (MeterOutputs != 2);

        }
        
        protected void SqlDataSourceEmployee1_Deleted(object sender, SqlDataSourceStatusEventArgs e)
        {
            Audit.Insert(
                (int)AuditCategory.DataDelete,
                Page.User.Identity.Name,
                String.Format("Deleted MeterTestResult:{0}", ((IDbDataParameter)e.Command.Parameters["@MeterTestResultID"]).Value),
                ConfigurationManager.ConnectionStrings["MCProd"].ToString()
            );
        }

        protected void btnAddResult_Click(object sender, EventArgs e)
        {
            dvMeterTestResult.Visible = true;
            dvMeterTestResult.ChangeMode(DetailsViewMode.Insert);
            dvMeterTestResult.DataBind();
        }

        protected void dsMeterTestResult_Inserting(object sender, EventArgs e)
        {
            // Calculate the two accuracy fields
            // Use the MeterTestID
            // The Comparison Meter will have a list of comparison points. 
            // Using the Subject Meter GPM, retrieve the closest accuracy value
            // from the comparison points table.
            dsMeterTestResult.InsertParameters["MeterTestID"].DefaultValue =
                MeterTestID.ToString();
        }

        protected void dsMeterTestResult_Inserted(object sender, DetailsViewInsertedEventArgs e)
        {
            gvTestResults.DataBind();
        }

        protected void gvTestResults_DataBinding(object sender, EventArgs e)
        {
            SetMeterTestID();
        }

        #endregion

        protected void dvMeterTestResult_DataBinding(object sender, EventArgs e)
        {
            //TODO: Fragile Code - if position changes in aspx, this will be invalid.
            if (dvMeterTestResult.Fields != null)
            {
                dvMeterTestResult.Fields[1].InsertVisible = (MeterOutputs==2);
            }
        }
    }
}