using System;
using MMSINC.Page;
using MMSINC.Utilities.Permissions;
using MapCall.Common.Utility.Permissions.Modules;
using Microsoft.Reporting.WebForms;

namespace MapCall.Modules.Reports
{
    public partial class emp_Licenses : DataElementRolePage
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
            odsLicenses.SelectParameters["OpCode"].DefaultValue = ddlOpCode.SelectedValue;

            rvEmployeeLicenses.LocalReport.DataSources.Clear();
            rvEmployeeLicenses.LocalReport.DataSources.Add(new ReportDataSource("DSHumanResources_emp_Licenses", "odsLicenses"));
            rvEmployeeLicenses.LocalReport.ReportPath = "Reports\\rptEmployeeLicenses.rdlc";

            ReportParameter rp1 = new ReportParameter("OpCode", ddlOpCode.SelectedValue);
            rvEmployeeLicenses.LocalReport.SetParameters(new ReportParameter[] { rp1 });
            rvEmployeeLicenses.LocalReport.Refresh();
            rvEmployeeLicenses.Visible = true;
        }
    }
}
