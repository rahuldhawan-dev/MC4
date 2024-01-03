using System;
using System.Text;
using System.Web.UI.WebControls;
using MMSINC.Page;
using MMSINC.Utilities.Permissions;
using MapCall.Common.Utility.Permissions.Modules;
using Microsoft.Reporting.WebForms;

namespace MapCall.Modules.Reports
{
    public partial class emp_SickBank : DataElementRolePage
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

        #region Private Methods

        private string selectedOperatingCenters(ListBox lb)
        {
            var sb = new StringBuilder();
            foreach (ListItem li in lb.Items)
                if (li.Selected) sb.Append(li.Value + ',');
            return sb.ToString().TrimEnd(',');
        }

        #endregion

        #region Event Handlers

        protected void btnView_Click(object sender, EventArgs e)
        {
            odsSickBank.SelectParameters["OpCode"].DefaultValue = selectedOperatingCenters(ddlOpCode);
            odsSickBank.SelectParameters["status"].DefaultValue = ddlStatus.SelectedValue;
            odsSickBank.SelectParameters["businessUnitID"].DefaultValue = ddlBusinessUnit.SelectedValue;

            var rp1 = new ReportParameter("OpCode", selectedOperatingCenters(ddlOpCode));
            var rp2 = new ReportParameter("status", ddlStatus.SelectedValue);
            var rp3 = new ReportParameter("businessUnitID", ddlBusinessUnit.SelectedValue);
            
            rvEmployeeSickBank.LocalReport.DataSources.Clear();
            rvEmployeeSickBank.LocalReport.DataSources.Add(new ReportDataSource("DSHumanResources_emp_SickBank", "odsSickBank"));
            rvEmployeeSickBank.LocalReport.ReportPath = "Reports\\rptEmployeeSickBank.rdlc";
            rvEmployeeSickBank.LocalReport.SetParameters(new ReportParameter[] { rp1, rp2, rp3 });
            rvEmployeeSickBank.LocalReport.Refresh();
            rvEmployeeSickBank.Visible = true;
        }

        #endregion


    }
}
