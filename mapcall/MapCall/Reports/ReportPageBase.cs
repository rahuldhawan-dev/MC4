using MapCall.Controls;

namespace MapCall.Reports
{
    public abstract class ReportPageBase : TemplatedDetailsViewDataPageBase
    {

        // There are no DetailsViews here, so we don't want view links.
        public override bool AutoGenerateViewColumnInResultsGridView
        {
            get
            {
                return false;
            }
        }

    }

}