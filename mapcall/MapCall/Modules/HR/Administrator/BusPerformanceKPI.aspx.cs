using System;
using System.Configuration;
using System.Text;
using System.Web.UI.WebControls;
using MMSINC;
using MMSINC.DataPages;
using MMSINC.Page;
using MMSINC.Utilities.Permissions;
using MapCall.Controls.Data;

namespace MapCall.Modules.HR.Administrator
{
    public partial class BusPerformanceKPI : DataElementPageWithRoles
    {
        #region Constants

        public const string RESULTS_SQL =
            @"SELECT
	            l1.[LookupValue] AS [KPI_Status],
	            l2.LookupValue as [BSCArea], 
	            t.Grouping,
	            l.[LookupValue] AS [KPI_Level],
	            t2.[OperatingCenterCode] AS [OpCode],
	            t.[KPI_Measurement],
	            t.[KPI_Target],
                l3.LookupValue as [UnitOfMeasure],
	            t.[Measurement_Frequency],
	            t.[System_Requirements_To_Measure],
	            t.[KPI_ID],
                t.[KPI_Status] AS [KPIStatusID],
                e.FullName as [Employee Responsible]
            FROM
	            [tblBusinessPerformance_KPI] AS t
            LEFT JOIN
	            [Lookup] AS l
            ON
	            t.[KPI_Level] = l.[LookupID]
            LEFT JOIN
	            [Lookup] AS l1
            ON
	            t.[KPI_Status] = l1.[LookupID]
            LEFT JOIN
	            [OperatingCenters] AS t2
            ON
	            t.[OpCode] = t2.[OperatingCenterID]
	        LEFT JOIN 
	            [Lookup] as l2
	        ON
	            l2.LookupID = t.BSCArea
            LEFT JOIN
                [Lookup] as l3
            ON
                l3.LookupID = t.UnitOfMeasure
	        LEFT JOIN    
	            Employees e
	        ON 
	            e.tblEmployeeID = EmployeeResponsible";
        #endregion

        #region Properties

        protected override IModulePermissions ModulePermissions
        {
            get
            {
                return Common.Utility.Permissions.Modules.BusinessPerformance.General;
            }
        }

        #endregion

        #region Event Handlers

        protected override void Page_Load(object sender, EventArgs e)
        {
            if (!CanView)
            {
                pnlSearch.Visible = false;
                lblPermissionErrors.Text = String.Format("Access Denied => {0} : {1}", ModulePermissions.Application, ModulePermissions.Module);
            }
            btnAdd.Visible =
                Notes1.AllowAdd =
                Documents1.AllowAdd = CanAdd;

            CheckQueryString(DataElement1, Documents1, Notes1, pnlDetail, pnlSearch, pnlResults);
            SqlDataSource1.SelectCommand = RESULTS_SQL;
        }
        
        protected override void btnSearch_Click(object sender, EventArgs e)
        {
            var sb = new StringBuilder();
            var items = new StringBuilder();

            foreach (Object ctrl in pnlSearch.Controls)
                if (ctrl is DataField)
                    sb.Append(((IDataField)ctrl).FilterExpression());

            if (lbInitiatives.SelectedIndex > 0)
            {
                items.AppendFormat(" WHERE KPI_ID in (Select KPIID from InitiativesKPIs where InitiativeID in (");
                foreach (ListItem li in lbInitiatives.Items)
                    if (li.Selected)
                        items.AppendFormat(", {0}", li.Value);
                items.Append("))");
            }
            if (lbGoals.SelectedIndex > 0)
            {
                items.AppendFormat(" {0}", (items.Length > 0) ? " AND " : " WHERE ");
                items.AppendFormat(" KPI_ID in (Select KPIID from GoalsKPIs where GoalID in (");
                foreach (ListItem li in lbGoals.Items)
                    if (li.Selected)
                        items.AppendFormat(", {0}", li.Value);
                items.Append("))");
            }

            if (items.Length > 0)
            {
                SqlDataSource1.SelectCommand += items.ToString().Replace("(, ", "(");
            }

            if (sb.Length > 0)
            {
                SqlDataSource1.FilterExpression = sb.ToString().Substring(5);
                this.Filter = SqlDataSource1.FilterExpression;
                hidFilter.Value = this.Filter;
            }
            else
            {
                SqlDataSource1.FilterExpression = String.Empty;
                this.Filter = String.Empty;
                hidFilter.Value = this.Filter;
            }

            //if (ddlOrderBy.SelectedIndex > 0)
            //    GridView1.Sort(ddlOrderBy.SelectedValue, SortDirection.Ascending);

            base.btnSearch_Click(sender, e);
            lblRecordCount.Text = String.Format("Total Records: {0}", GridView1.Rows.Count.ToString());
        }
        
        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataElement1.DataElementID = Int32.Parse(GridView1.SelectedDataKey[0].ToString());
            DataElement1.DataBind();
            DataElement1.DetailsView1.DataBind();
            Audit.Insert(
                (int)AuditCategory.DataView,
                Page.User.Identity.Name,
                String.Format("Viewed {0} ID:{1}", DataElement1.DataElementName, DataElement1.DataElementID),
                ConfigurationManager.ConnectionStrings["MCProd"].ToString()
            );

            Notes1.DataLinkID = DataElement1.DataElementID;
            Notes1.Visible = true;
            Documents1.DataLinkID = DataElement1.DataElementID;
            Documents1.Visible = true;

            pnlSearch.Visible = false;
            pnlResults.Visible = false;
            pnlDetail.Visible = true;
        }
        
        protected void DataElement1_DataBinding(object sender, EventArgs e)
        {
            Notes1.AllowAdd =
                Documents1.AllowAdd = DataElement1.AllowNew = CanAdd;
            Notes1.AllowEdit =
                Documents1.AllowEdit =
                DataElement1.AllowEdit =
                CanEdit;
            Notes1.AllowDelete =
                Documents1.AllowDelete =
                DataElement1.AllowDelete =
                CanDelete;
        }

        protected void btnAddGoal_Click(object sender, EventArgs e)
        {
            dsKPIGoals.Insert();
            gvKPIGoals.DataBind();
        }

        protected void btnAddInitiative_Click(object sender, EventArgs e)
        {
            dsKPIInitiatives.Insert();
            gvKPIInitiatives.DataBind();
        }

        #endregion
    }
}
