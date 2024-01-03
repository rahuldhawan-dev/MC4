using System;
using System.Collections.Generic;
using System.Linq;
using MMSINC.Controls;
using MMSINC.Utilities.Permissions;
using MapCall.Controls;

namespace MapCall.Modules.HR.Administrator
{
    public partial class BusPerformanceInitiatives : TemplatedDetailsViewDataPageBase
    {
        #region Properties

        protected override DetailsViewDataPageTemplate Template
        {
            get { return template; }
        }

        protected override IModulePermissions ModulePermissions
        {
            get
            {
                return Common.Utility.Permissions.Modules.BusinessPerformance.General;
            }
        }

        #endregion

        #region Private Methods

        protected override IEnumerable<IDataLink> GetIDataLinkControls()
        {
            var c = base.GetIDataLinkControls().ToList();
            c.Add(Hyperlinks1);
            return c;
        }

        #endregion


        #region Event Handlers
     
        protected void btnAddGoal_Click(object sender, EventArgs e)
        {
            dsInitiativeGoals.Insert();
            gvInitiativeGoals.DataBind();
        }

        #endregion
    }
}
