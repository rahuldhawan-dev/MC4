using System;
using System.Configuration;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using MMSINC;

namespace MapCall.Controls.HR
{
    public partial class MeterTestComparisonPoints : UserControl
    {
        #region Private Members

        public MeterTestComparisonPoints()
        {
            AllowAdd = true;
            AllowEdit = true;
            AllowDelete = true;
        }

        #endregion

        #region Properties

        public int MeterTestComparisonMeterID
        {
            get
            {
                return ViewState["MeterTestComparisonMeterID"] == null
                           ? 0
                           : Int32.Parse(ViewState["MeterTestComparisonMeterID"].ToString());
            }
            set { ViewState["MeterTestComparisonMeterID"] = value.ToString(); }
        }

        public bool AllowDelete { get; set; }

        public bool AllowEdit { get; set; }

        public bool AllowAdd { get; set; }

        #endregion

        #region Private Methods

        private void LoadGridView()
        {
            dsMeterTestComparisonPoints.SelectParameters["MeterTestComparisonMeterID"].DefaultValue = MeterTestComparisonMeterID.ToString();
            gvComparisonPoints.DataBind();
            gvComparisonPoints.Visible = true;
        }

        #endregion

        #region Event Handlers

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (MeterTestComparisonMeterID != 0)
            {
                LoadGridView();
            }
            dvMeterTestComparisonPoint.ChangeMode(DetailsViewMode.Insert);
            dvMeterTestComparisonPoint.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            btnAddResult.Visible = AllowAdd;
            gvComparisonPoints.Columns[0].ItemStyle.CssClass = "NoteCell";
            if (MeterTestComparisonMeterID != 0)
            {
                LoadGridView();
            }
            else
            {
                gvComparisonPoints.Visible = false;
            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //TODO: Fragile code here. Position instead of direct lookup.
                var btnDelete = e.Row.Controls[1].FindControl("btnDelete");
                if (btnDelete != null) btnDelete.Visible = AllowDelete;
            }
        }

        protected void SqlDataSource_Deleted(object sender, SqlDataSourceStatusEventArgs e)
        {
            Audit.Insert(
                (int)AuditCategory.DataDelete,
                Page.User.Identity.Name,
                String.Format("Deleted MeterTestResult:{0}", ((IDbDataParameter)e.Command.Parameters["@MeterTestResultID"]).Value),
                ConfigurationManager.ConnectionStrings["MCProd"].ToString()
            );
        }

        #endregion

        protected void btnAddResult_Click(object sender, EventArgs e)
        {
            dvMeterTestComparisonPoint.Visible = true;
            dvMeterTestComparisonPoint.ChangeMode(DetailsViewMode.Insert);
            dvMeterTestComparisonPoint.DataBind();
        }

        protected void dsMeterTestComparisonPoints_Inserting(object sender, EventArgs e)
        {
            dsMeterTestComparisonPoints.InsertParameters["MeterTestComparisonMeterID"].DefaultValue =
                MeterTestComparisonMeterID.ToString();
        }

        protected void dsMeterTestComparisonPoints_Inserted(object sender, DetailsViewInsertedEventArgs e)
        {
            gvComparisonPoints.DataBind();
        }

        protected void gvComparisonPoints_DataBinding(object sender, EventArgs e)
        {
            dsMeterTestComparisonPoints.SelectParameters["MeterTestComparisonMeterID"].DefaultValue =
                MeterTestComparisonMeterID.ToString();
        }
    }
}