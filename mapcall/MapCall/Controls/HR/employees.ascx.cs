using System;
using System.Configuration;
using System.Data;
using System.Text;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using MMSINC;
using MMSINC.Controls;
using MMSINC.Utilities.Permissions;
using MapCall.Common;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Utility.Notifications;
using MapCall.Common.Utility.Permissions.Modules;
using StructureMap;
using MMSINC.Authentication;

namespace MapCall.Controls.HR
{
    public partial class Employees : UserControl, IDataLink
    {
        #region Constants

        public struct EMAIL
        {
            public struct SUBJECTS
            {
                public const string TAILGATE_TALKS = "Tailgate Talk - Attendees",
                                    TRAINING_RECORDS = "Training Record - Attendees";
            }

            public struct PURPOSES
            {
                public const string TAILGATE_TALKS = "Tailgate Talk",
                                    TRAINING_RECORDS = "Training Record";
            }

            public struct MODULES
            {
                public static readonly IModulePermissions TAILGATE_TALKS = Operations.HealthAndSafety,
                                                          TRAINING_RECORDS = HumanResources.Employee;
            }
        }
        
        #endregion

        #region Private Members

        public Employees()
        {
            AllowDelete = true;
            AllowEdit = true;
            AllowAdd = true;
            AllowNotification = false;
        }

        #endregion

        #region Properties

        public int DataLinkID
        {
            get { return ViewState["DataLinkID"] == null ? 0 : Int32.Parse(ViewState["DataLinkID"].ToString()); }
            set { ViewState["DataLinkID"] = value.ToString(); }
        }
        public int DataTypeID
        {
            get { return ViewState["DataTypeID"] == null ? 0 : Int32.Parse(ViewState["DataTypeID"].ToString()); }
            set { ViewState["DataTypeID"] = value.ToString(); }
        }

        public bool AllowDelete { get; set; }
        public bool AllowEdit { get; set; }
        public bool AllowAdd { get; set; }
        public bool AllowNotification { get; set; }

        public SqlDataSource EmailAddressesDataSource { get; set; }
        public SqlDataSource MessageDetailsDataSource { get; set; }
        public string EmailSubject { get; set; }

        #endregion

        #region Private Methods

        private void loadGridView1()
        {
            SqlDataSourceEmployee1.SelectParameters["DataTypeID"].DefaultValue = DataTypeID.ToString();
            SqlDataSourceEmployee1.SelectParameters["DataLinkID"].DefaultValue = DataLinkID.ToString();
            GridView1_Employee.DataBind();
            GridView1_Employee.Visible = true;
        }
        private void SendEmail()
        {
            var sb = new StringBuilder();
            sb.AppendFormat(EmailSubject + "<br/>");
            sb.Append(GetMessage());
            sb.Append("<br/>");

            foreach(GridViewRow gvr in GridView1_Employee.Rows)
            {
                sb.AppendFormat("{0}:{1}<br/>", gvr.Cells[0].Text, gvr.Cells[1].Text);
            }

            const int operatingCenterID = 10;
            RoleModules module = 0;
            string purpose = null;

            switch (EmailSubject)
            {
                case EMAIL.SUBJECTS.TRAINING_RECORDS:
                    module = RoleModules.HumanResourcesEmployee;
                    purpose = EMAIL.PURPOSES.TRAINING_RECORDS;
                    break;
                case EMAIL.SUBJECTS.TAILGATE_TALKS:
                    module = RoleModules.OperationsHealthAndSafety;
                    purpose = EMAIL.PURPOSES.TAILGATE_TALKS;
                    break;
            }

            DependencyResolver.Current.GetService<INotificationService>()
                .Notify(operatingCenterID, module, purpose, sb, EmailSubject);

            btnNotify.Text = "Send Notification(sent)";
        }
        private string GetMessage()
        {
            var result="";
            var dv =
                (DataView)
                MessageDetailsDataSource.Select(DataSourceSelectArguments.Empty);
            if (dv.Count >0)
            {
                for(var x=0;x < dv.Table.Columns.Count;x++)
                    result += string.Format("{0}<br/>",dv.Table.Rows[0][x]);
            }
            dv.Dispose();
            return result;
        }

        #endregion

        #region Protected Overrides

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            btnAddToggleEmployee.Visible = AllowAdd;
            btnNotify.Visible = AllowNotification;
            btnNotify.Text = "Send Notification";
            GridView1_Employee.Columns[0].ItemStyle.CssClass = "NoteCell";
            if (DataLinkID != 0 && DataTypeID != 0)
            {
                //loadGridView1();
            }
            else
            {
                GridView1_Employee.Visible = false;
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (DataTypeID != 0)
            {
                loadGridView1();
            }
        }

        #endregion

        #region Event Handlers

        protected void Page_PreRender(object sender, EventArgs e)
        {
            //if (DataTypeID != 0)
            //{
            //    loadGridView1();
            //}
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //btnAddToggleEmployee.Visible = AllowAdd;
            //btnNotify.Visible = AllowNotification;
            //btnNotify.Text = "Send Notification";
            //GridView1_Employee.Columns[0].ItemStyle.CssClass = "NoteCell";
            //if (DataLinkID != 0 && DataTypeID != 0)
            //{
            //    loadGridView1();
            //}
            //else
            //{
            //    GridView1_Employee.Visible = false;
            //}
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //TODO: Fragile code here. Position instead of direct lookup.
                var btnDelete = e.Row.Controls[1].FindControl("btnDeleteEmployee");
                if (btnDelete != null) btnDelete.Visible = AllowDelete;
            }
        }
        protected void btnAddEmployee_Click(object sender, EventArgs e)
        {
            var currentUser = DependencyResolver.Current.GetService<IAuthenticationService<User>>().CurrentUser;

            foreach (ListItem x in ddlEmployees.Items)
            {
                if (x.Selected)
                {
                    SqlDataSourceEmployee1.InsertParameters["DataLinkID"].
                        DefaultValue = DataLinkID.ToString();
                    SqlDataSourceEmployee1.InsertParameters["DataTypeID"].
                        DefaultValue = DataTypeID.ToString();
                    SqlDataSourceEmployee1.InsertParameters["CreatedOn"].
                        DefaultValue = DateTime.Now.ToString();
                    SqlDataSourceEmployee1.InsertParameters["CreatedBy"].DefaultValue = currentUser.FullName;
                    SqlDataSourceEmployee1.InsertParameters["tblEmployeeID"].
                        DefaultValue = x.Value;
                    SqlDataSourceEmployee1.Insert();
                    Audit.Insert(
                        (int) AuditCategory.DataInsert,
                        Page.User.Identity.Name,
                        String.Format(
                            "Linked Employee: {0} to DataType : {1}, {2}",
                            ddlEmployees.SelectedValue, DataTypeID.ToString(),
                            DataLinkID.ToString()),
                        ConfigurationManager.ConnectionStrings["MCProd"].
                            ToString()
                        );
                }
            }
            GridView1_Employee.DataBind();
            ddlEmployees.SelectedIndex = -1;
        }
        protected void SqlDataSourceEmployee1_Deleted(object sender, SqlDataSourceStatusEventArgs e)
        {
            Audit.Insert(
                (int)AuditCategory.DataDelete,
                Page.User.Identity.Name,
                String.Format("Deleted Employee Link:{0}", ((IDbDataParameter)e.Command.Parameters["@EmployeeLinkID"]).Value.ToString()),
                ConfigurationManager.ConnectionStrings["MCProd"].ToString()
            );
        }
        protected void btnFilterOpCode_Click(object sender, EventArgs e)
        {
            if (ddlOpCode.SelectedIndex > 2)
            {
                dsEmployees.FilterExpression = String.Format("OpCode = '{0}'",
                                                             ddlOpCode.
                                                                 SelectedValue);
            }
            else
            {
                dsEmployees.FilterExpression = String.Empty;
            }
            ddlEmployees.Items.Clear();
            ddlEmployees.DataBind();
        }
        protected void btnNotify_Click(object sender, EventArgs e)
        {
            SendEmail();
        }

        #endregion
    }
}