using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MapCall.Controls;

namespace MapCall.Modules.Admin
{
    public partial class Departments : TemplatedDetailsViewDataPageBase
    {
        protected override DetailsViewDataPageTemplate Template
        {
            get { return template; }
        }

        protected override MMSINC.Utilities.Permissions.IModulePermissions ModulePermissions
        {
            get { return Common.Utility.Permissions.Modules.FieldServices.DataLookups; }
        }
    }
}