using System.Web.UI.WebControls;
using MMSINC.DataPages;
using MMSINC.Utilities.Permissions;
using MapCall.Common.Utility.Permissions.Modules;
using MapCall.Controls;

namespace MapCall.Reports.HR.Employee
{
    public partial class EmployeePositionControlReport : ReportPageBase
    {
        protected override DetailsViewDataPageTemplate Template
        {
            get { return template; }
        }

        protected override IModulePermissions ModulePermissions
        {
            get { return HumanResources.Employee; }
        }

        protected override void AddExpressionsToFilterBuilder(IFilterBuilder builder)
        {
            base.AddExpressionsToFilterBuilder(builder);

            // This is the only difference between this page and the EmployeePositionControls page.
            builder.AddExpression(new FilterBuilderExpression("hist.Position_End_Date Is Null"));
        }

        protected override void ApplyFilterBuilder(IFilterBuilder fb)
        {
            base.ApplyFilterBuilder(fb);

            // Sets the default sort order. 
            Template.ResultsGridView.Sort("EmployeePositionControlID", SortDirection.Ascending);
        }
    }
}