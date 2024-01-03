using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MMSINC.Utilities.Permissions;
using MapCall.Controls;

namespace MapCall.Modules.Engineering
{
    public partial class PipeDataLookupTypes : TemplatedDetailsViewDataPageBase
    {
        protected override DetailsViewDataPageTemplate Template
        {
            get { return template; }
        }

        protected override IModulePermissions ModulePermissions
        {
            get { return Common.Utility.Permissions.Modules.FieldServices.Projects; }
        }
    }
}
