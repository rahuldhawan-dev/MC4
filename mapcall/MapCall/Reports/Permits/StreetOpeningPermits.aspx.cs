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
    public partial class StreetOpeningPermits : ReportPageBase
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

        #endregion
    }
}