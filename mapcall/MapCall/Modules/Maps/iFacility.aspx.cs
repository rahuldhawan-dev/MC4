using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MapCall.Modules.Maps
{
    public partial class iFacility : Page
    {
        double usage, totalLMP, cost;
        string units, datetime;
      
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                usage += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "lastval"));
                cost += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "cost"));
                totalLMP = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "totalLMP"));
                units = DataBinder.Eval(e.Row.DataItem, "units").ToString();
                datetime = DataBinder.Eval(e.Row.DataItem, "datetimestamp").ToString();
                e.Row.Attributes.Add("title", DataBinder.Eval(e.Row.DataItem, "EquipmentID").ToString());
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[0].Text = "Totals&nbsp;&nbsp;&nbsp;";
                e.Row.Cells[0].Attributes.Add("style", "text-align:right;");
                e.Row.Cells[1].Text = datetime;
                e.Row.Cells[2].Text = String.Format("{0:n2}", usage);
                e.Row.Cells[3].Text = units;
                e.Row.Cells[4].Text = String.Format("{0:n5}", totalLMP);
                e.Row.Cells[5].Text = String.Format("{0:c2}", cost);
            }
        }

    }
}
