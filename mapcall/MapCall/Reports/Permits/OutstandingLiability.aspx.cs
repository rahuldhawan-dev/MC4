using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MapCall.Controls;
using MMSINC.Controls;
using MMSINC.DataPages;
using MMSINC.Utilities.Permissions;

namespace MapCall.Reports.Permits
{
    public partial class OutstandingLiability : ReportPageBase
    {
        #region Properties

        protected override IModulePermissions ModulePermissions
        {
            get
            {
                return Common.Utility.Permissions.Modules.FieldServices.WorkManagement;
            }
        }

        protected override DetailsViewDataPageTemplate Template
        {
            get { return template; }
        }

        public decimal TotalPermitFeeNotCleared { get; set; }
        public decimal TotalInspectionFeeNotCleared { get; set; }
        public decimal TotalBondFeeNotCleared { get; set; }

        #endregion

        #region Event Handlers / Private Methods
        
        protected override string RenderGridViewToExcel(IGridView gv)
        {
            var x = RouteHelper.IsExcelExportRoute;
            gv.AllowPaging = false;
            gv.DataBind();
            var ret = base.RenderGridViewToExcel(gv);
            gv.AllowPaging = false;
            return ret;
        }

        protected void rgv_DataBinding(object sender, EventArgs e)
        {
            TotalPermitFeeNotCleared = TotalInspectionFeeNotCleared = TotalBondFeeNotCleared = 0;

        }

        protected void rgv_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var row = ((System.Data.DataRowView)(e.Row.DataItem)).Row;
                if (row[7].ToString().ToLower() == "false")
                {
                    TotalPermitFeeNotCleared += (decimal)row[3];
                    TotalInspectionFeeNotCleared += (decimal)row[4];
                    TotalBondFeeNotCleared += (decimal)row[5];
                }
            }

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[2].Text = "Total Not Cleared:";
                e.Row.Cells[3].Text = string.Format("{0:c}", TotalPermitFeeNotCleared);
                e.Row.Cells[4].Text = string.Format("{0:c}", TotalInspectionFeeNotCleared);
                e.Row.Cells[5].Text = string.Format("{0:c}", TotalBondFeeNotCleared);
                e.Row.Cells[6].Text = string.Format("{0:c}", TotalPermitFeeNotCleared + TotalInspectionFeeNotCleared + TotalBondFeeNotCleared);
            }
        }
        
        #endregion
    }
}
