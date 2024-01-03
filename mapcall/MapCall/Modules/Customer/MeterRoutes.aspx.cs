using System;
using MMSINC.DataPages;
using MMSINC.Utilities.Permissions;
using MapCall.Controls;

namespace MapCall.Modules.Customer
{
    public partial class MeterRoutes : TemplatedDetailsViewDataPageBase
    {
        #region Properties

        public override bool AutoAddDataKeyToDetailsView
        {
            get
            {
                return false;
            }
        }

        protected override DetailsViewDataPageTemplate Template
        {
            get
            {
                return template;
            }
        }

        protected override IModulePermissions ModulePermissions
        {
            get { return Common.Utility.Permissions.Modules.FieldServices.DataLookups; }
        }

        #endregion

        #region Private methods

        protected override void AddExpressionsToFilterBuilder(IFilterBuilder builder)
        {
            base.AddExpressionsToFilterBuilder(builder);
            opCntrField.FilterExpression(builder);
        }

        #endregion

        #region Event Handlers

        protected void btnAddMeterRouteDetail_Click(object sender, EventArgs e)
        {
            var val = DetailsView.SelectedValue; //DetailsView1.SelectedValue;
            var path = ResolveClientUrl("~/Modules/Customer/MeterRouteDetails.aspx");

            Response.Redirect(String.Format("{0}?create=&MeterRouteID={1}", path, val));
        }

        #endregion

        protected void btnExportMeterRoutes_OnClick(object sender, EventArgs e)
        {
            MMSINC.Utility.ExportToExcel(Page, dsFlushingSchedulesMeterRoutes); 
        }
    }
}