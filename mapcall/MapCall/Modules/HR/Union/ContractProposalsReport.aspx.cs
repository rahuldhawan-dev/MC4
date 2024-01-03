using System.Web.UI.WebControls;
using MMSINC.DataPages;
using MMSINC.Utilities.Permissions;
using MapCall.Common.Utility.Permissions.Modules;
using MapCall.Controls;
using MapCall.Reports;

namespace MapCall.Modules.HR.Union
{
    public partial class ContractProposalsReport : ReportPageBase
    {
        #region Properties
        
        protected override IModulePermissions ModulePermissions
        {
            get
            {
                return HumanResources.Proposals;
            }
        }

        protected override DetailsViewDataPageTemplate Template
        {
            get { return template; }
        }

        protected override void ApplyFilterBuilder(IFilterBuilder fb)
        {
            base.ApplyFilterBuilder(fb);

            var yeah = (GridView)this.ResultsGridView;
            yeah.Sort("Management_Or_Union, Printing_Sequence", SortDirection.Ascending);
        }

        #endregion
    }
}
