using System;
using System.Text;
using MMSINC.DataPages;
using MMSINC.Page;
using dotnetCHARTING;

namespace MapCall.Reports.FieldServices
{
    public partial class JobObservationSummary : DataElementPage
    {
        protected override void btnSearch_Click(object sender, EventArgs e)
        {
            var sb = new StringBuilder();
            foreach (Object ctrl in pnlSearch.Controls)
                if (ctrl is IDataField)
                    sb.Append(((IDataField)ctrl).FilterExpression());

            if (sb.Length > 0)
            {
                SqlDataSource1.FilterExpression = sb.ToString().Substring(5);
                Filter = SqlDataSource1.FilterExpression;
                hidFilter.Value = Filter;
            }
            else
            {
                SqlDataSource1.FilterExpression = String.Empty;
                Filter = String.Empty;
                hidFilter.Value = Filter;
            }

            base.btnSearch_Click(sender, e);
            lblRecordCount.Text = String.Format("Total Records: {0}", GridView1.Rows.Count);
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            //Chart Setup
            cws.ChartDataSource = SqlDataSource1;
            cws.ChartDataSource.FilterExpression = hidFilter.Value;
            cws.ColumnNames = new[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
            cws.SeriesNames = new[] { "OpCode", "Year" };
            cws.SeriesDefault = SeriesType.Line;
            cws.ShowSettingsDiv = true;
        }
    }
}
