using MapCall.Controls;

namespace MapCall.Modules.Management
{
    public partial class WebLinkCategories : LookUpDataPageBase
    {
        protected override LookupControl LookupControl
        {
            get { return lookup; }
        }
    }
}
