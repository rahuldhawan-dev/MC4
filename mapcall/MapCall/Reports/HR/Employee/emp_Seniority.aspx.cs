using System;
using MMSINC.Page;
using MMSINC.Utilities.Permissions;
using MapCall.Common.Utility.Permissions.Modules;
using Microsoft.Reporting.WebForms;

namespace MapCall.Modules.HR.Reports
{
    public partial class emp_Seniority : DataElementRolePage
    {
        #region Properties

        protected override IModulePermissions ModulePermissions
        {
            get
            {
                return HumanResources.Employee;
            }
        }

        #endregion

        protected override void Page_Load(object sender, EventArgs e)
        {
            if (!CanView)
            {
                Response.Write(String.Format("Access Denied => {0} : {1}", ModulePermissions.Application, ModulePermissions.Module));
                Response.End();
            }
        }
        protected void btnView_Click(object sender, EventArgs e)
        {
            ObjectDataSource1.SelectParameters["LocalID"].DefaultValue = ddlLocal.SelectedValue;

            ReportViewer1.Reset();
            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DSHumanResources_emp_Seniority", "ObjectDataSource1"));
            ReportViewer1.LocalReport.ReportPath = "Reports\\emp_Seniority.rdlc";

            ReportParameter p1 = new ReportParameter("LocalID", ddlLocal.SelectedValue);
            ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { p1 });
            ReportViewer1.LocalReport.Refresh();
            ReportViewer1.Visible = true;
        }
    }
}
