using System;
using MMSINC.Utilities.Permissions;
using MapCall.Controls;

namespace MapCall.Modules.FieldServices
{
    public partial class FlushingSchedules : TemplatedDetailsViewDataPageBase
    {
        #region Properties

        protected override DetailsViewDataPageTemplate Template
        {
            get { return template; }
        }

        protected override IModulePermissions ModulePermissions
        {
            get { return Common.Utility.Permissions.Modules.FieldServices.DataLookups; }
        }

        #endregion

        protected void btnExportMeterRoutes_OnClick(object sender, EventArgs e)
        {
            MMSINC.Utility.ExportToExcel(Page, dsFlushingSchedulesMeterRoutes);
        }
    }
}