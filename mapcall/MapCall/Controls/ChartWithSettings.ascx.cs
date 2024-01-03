using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using dotnetCHARTING;

namespace MapCall.Controls
{
    /// <summary>
    /// Usage: Add the control to the asp.net page.  
    /// Define the following in the pre_render of the Page:
    /// string[] ColumnNames
    /// string[] SeriesNames
    /// SqlDataSource ChartDataSource
    /// </summary>
    public partial class ChartWithSettings : UserControl
    {
        #region Constants

        private struct ScriptURLS
        {
            public const string JQUERY_VALIDATE = "~/scripts/jquery.validate.js";
        }

        #endregion

        #region Private Members

        private bool _loadDefaults = true;

        #endregion

        #region Properties

        /// <summary>
        /// Column names that reside in the data source used to create the x axis names.
        /// </summary>
        public string[] ColumnNames { get; set; }
        /// <summary>
        /// Column names that reside in the data source used to create the series names.  
        /// </summary>
        public string[] SeriesNames { get; set; }

        /// <summary>
        /// SqlDataSource that the chart uses to produce the data
        /// </summary>
        public SqlDataSource ChartDataSource { get; set; }

        public SeriesType SeriesDefault { get; set; }

        public bool ShowSettingsDiv { get; set; }

        public Chart Chart1
        {
            get { return Chart; }
            set { Chart = value; }
        }

        #endregion

        #region Events

        public EventHandler ChartRefreshClick;

        #endregion

        #region Private Methods

        private static ListItem[] GetListItemsByEnum<T>()
        {
            var t = typeof(T);
            var items = new List<ListItem>();
            foreach (var item in Enum.GetNames(t))
            {
                var value = (int)Enum.Parse(t, item);
                items.Add(new ListItem(item, value.ToString()));
            }
            return items.ToArray();

        }

        private void CreateDropDownLists()
        {
            ddlSeriesType.Items.AddRange(GetListItemsByEnum<SeriesType>());
            ddlScale.Items.AddRange(GetListItemsByEnum<Scale>());
            ddlTitleBoxPosition.Items.AddRange(GetListItemsByEnum<TitleBoxPosition>());
            ddlChartType.Items.AddRange(GetListItemsByEnum<ChartType>());
        }

        private void DefaultChartSettings()
        {
            //TODO: make this a setting in web.config?
            Chart.TempDirectory = "~/temp";

            Chart.Type = (ChartType)Enum.Parse(typeof(ChartType), ddlChartType.SelectedValue);
            Chart.Title = txtChartTitle.Text;
            Chart.Depth = 15;
            Chart.Use3D = true;
            Chart.XAxis.ClusterColumns = false;
            Chart.DefaultSeries.DefaultElement.Transparency = 20;
            Chart.DefaultSeries.Type = SeriesDefault;
            Chart.YAxis.Scale = (Scale)Enum.Parse(typeof(Scale), ddlScale.SelectedValue);
            Chart.XAxis.Label.Text = txtXAxisLabel.Text;
            Chart.YAxis.Label.Text = txtYAxisLabel.Text;
            Chart.Width = string.IsNullOrEmpty(txtChartWidth.Text) ? 750 : int.Parse(txtChartWidth.Text);
            Chart.Height = string.IsNullOrEmpty(txtChartHeight.Text) ? 400 : int.Parse(txtChartHeight.Text);
            Chart.DefaultElement.Marker.Visible = cbShowMarkers.Checked;
            Chart.DefaultSeries.Line.Width = 2;
            // Chart.TitleBox.Position = (TitleBoxPosition)Enum.ToObject(typeof(TitleBoxPosition), (int)TitleBoxPosition.FullWithLegend);
            Chart.TitleBox.Position = TitleBoxPosition.FullWithLegend;


            //Control stuff
            pnlChartSettings.Enabled = ShowSettingsDiv;
            ddlSeriesType.SelectedValue = ((int)SeriesDefault).ToString();
            ddlTitleBoxPosition.SelectedValue = ((int)TitleBoxPosition.FullWithLegend).ToString();
            cbUse3d.Checked = Chart.Use3D;
        }

        private SeriesCollection GetChartData()
        {
            var sc = new SeriesCollection();

            foreach (DataRowView o in ChartDataSource.Select(new DataSourceSelectArguments()))
            {
                var s = new Series();
                var sb = new StringBuilder();

                //Build the series names
                for (var i = 0; i < SeriesNames.Length; i++)
                    sb.Append(o.Row[SeriesNames[i]] + " ");

                s.Name = sb.ToString();

                for (var i = 0; i < ColumnNames.Length; i++)
                {
                    var curColName = ColumnNames[i];
                    var curRow = o.Row[curColName];
                    var e = new Element
                                    {
                                        Name = o.DataView.Table.Columns[curColName].ToString(),
                                        YValue = curRow == DBNull.Value ? 0 : double.Parse(curRow.ToString())
                                    };
                    s.Elements.Add(e);
                }

                //Add the series to the collection
                sc.Add(s);
            }

            return sc;
        }

        #endregion

        #region Exposed Methods

        public void OnChartRefreshClick(EventArgs e)
        {
            if (ChartRefreshClick != null)
            {
                ChartRefreshClick(this, e);
            }
        }


        #endregion

        #region Event Handlers

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (_loadDefaults)
            {
                CreateDropDownLists();
                DefaultChartSettings();
                _loadDefaults = false;
            }
            //Fill the chart
            if (ChartDataSource != null)
                Chart.SeriesCollection.Add(GetChartData());

            Page.ClientScript.RegisterClientScriptInclude(Page.GetType(), "jQuery-validate", ResolveClientUrl(ScriptURLS.JQUERY_VALIDATE));
        }


        protected void btnChartRefresh_Click(object sender, EventArgs e)
        {
            OnChartRefreshClick(e);

            //TODO: make this a setting in web.config?
            Chart.TempDirectory = "~/temp";

            Chart.Type = (ChartType)Enum.Parse(typeof(ChartType), ddlChartType.SelectedValue);
            Chart.Title = txtChartTitle.Text;
            Chart.Depth = 15;
            Chart.Use3D = cbUse3d.Checked;
            Chart.XAxis.ClusterColumns = false;
            Chart.DefaultSeries.DefaultElement.Transparency = 20;
            Chart.DefaultSeries.Type = (SeriesType)Enum.Parse(typeof(SeriesType), ddlSeriesType.SelectedValue);
            Chart.YAxis.Scale = (Scale)Enum.Parse(typeof(Scale), ddlScale.SelectedValue);
            Chart.XAxis.Label.Text = txtXAxisLabel.Text;
            Chart.YAxis.Label.Text = txtYAxisLabel.Text;
            Chart.Width = string.IsNullOrEmpty(txtChartWidth.Text) ? 750 : int.Parse(txtChartWidth.Text);
            Chart.Height = string.IsNullOrEmpty(txtChartHeight.Text) ? 400 : int.Parse(txtChartHeight.Text);
            Chart.DefaultElement.Marker.Visible = cbShowMarkers.Checked;
            Chart.DefaultSeries.Line.Width = 2;
            Chart.TitleBox.Position = (TitleBoxPosition)Enum.Parse(typeof(TitleBoxPosition), ddlTitleBoxPosition.SelectedValue);

            _loadDefaults = false;
        }

        #endregion

    }
}