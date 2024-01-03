using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MMSINC.DataPages;
using MMSINC.Utilities.Permissions;
using MapCall.Controls;

namespace MapCall.Modules.Data.Contacts
{
    public partial class TownContactTypes : TemplatedDetailsViewDataPageBase
    {
        protected override IModulePermissions ModulePermissions
        {
            get { return Common.Utility.Permissions.Modules.FieldServices.DataLookups; }
        }

        protected override DetailsViewDataPageTemplate Template
        {
            get { return template; }
        }


        protected override PageModes DefaultPageMode
        {
            get
            {
                return PageModes.Results;
            }
        }

        protected override void OnPageModeChanged(PageModes newMode)
        {
            // Because we're disabling the search page view, we wanna
            // forward any internal changes to the Search page back
            // to the Results page. 
            if (newMode == PageModes.Search)
            {
                PageMode = PageModes.Results;
            }
            else
            {
                base.OnPageModeChanged(newMode);
            }
        }
    }
}