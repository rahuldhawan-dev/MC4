using System;
using MMSINC.Utilities.Permissions;
using MapCall.Controls;
using dotnetCHARTING;

namespace MapCall.Modules.HR.Administrator
{
    public partial class BusPerformanceKPIMeasurement : TemplatedDetailsViewDataPageBase
    {
        // TODO: QueryStringCheck wiring.
        // TODO: Chart

        #region Properties
       
        protected override DetailsViewDataPageTemplate Template
        {
            get { return template; }
        }

        protected override IModulePermissions ModulePermissions
        {
            get { return Common.Utility.Permissions.Modules.BusinessPerformance.General; }
        }

        #endregion

        #region Event Handlers

        protected void Page_PreRender(object sender, EventArgs e)
        {
            //Chart Setup
            cws.ChartDataSource = ResultsDataSource;
            //cws.ChartDataSource.FilterExpression = ;
            cws.ColumnNames = new[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
            cws.SeriesNames = new[] { "KPI_ID", "OpCode", "KPIMeasurementCategory" };
            cws.SeriesDefault = SeriesType.Line;
            cws.ShowSettingsDiv = true;
        }

        #endregion

    }
}
