using System;
using MMSINC.Controls;
using MMSINC.DataPages;
using MMSINC.Utilities.Permissions;
using MapCall.Controls;

namespace MapCall.Modules.Customer
{
    public partial class MeterRouteDetails : TemplatedDetailsViewDataPageBase
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

        #region Event Handlers

        protected override void AddExpressionsToFilterBuilder(IFilterBuilder builder)
        {
            base.AddExpressionsToFilterBuilder(builder);
            opCntrField.FilterExpression(builder);
        }

        protected override void OnPreRenderComplete(EventArgs e)
        {
            base.OnPreRenderComplete(e);
            var meterRouteId = Request.QueryString["MeterRouteID"];
            var ddl =
                (MvpDropDownList)MMSINC.Utility.GetFirstControlInstance((template.DetailsView), "ddlMeterRoutes");
            if (!String.IsNullOrWhiteSpace(meterRouteId) && ddl != null)
                ddl.SelectedValue = meterRouteId;
        }

        #endregion
    }
}