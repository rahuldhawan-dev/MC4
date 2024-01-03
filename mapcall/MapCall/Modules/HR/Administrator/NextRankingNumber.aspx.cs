using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using MMSINC.Page;
using MMSINC.Utilities.Permissions;
using MapCall.Common.Utility.Permissions.Modules;
using MMSINC.ClassExtensions;

namespace MapCall.Modules.HR.Administrator
{
    public partial class NextRankingNumber : DataElementRolePage
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

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            pnlDetails.Visible = true;
            string query =
                "SELECT IsNull(max(e.Seniority_Ranking), 0) + 1 AS RankingNumber, GetDate() AS RunDate FROM tblEmployee AS e left join OperatingCenters oc on oc.OperatingCenterID = e.OperatingCenterId WHERE oc.OperatingCenterCode = @operatingCenterCode"; 
            
            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MCProd"].ConnectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.AddParameter("operatingCenterCode", ddlOpCode.SelectedValue);

                    using (var rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        rdr.Read();
                        lblResults.Text = string.Format("Next ranking number is {0} as of {1}.", rdr.GetValue(0),
                            rdr.GetValue(1));
                    }
                }
            }
        }
    }
}
