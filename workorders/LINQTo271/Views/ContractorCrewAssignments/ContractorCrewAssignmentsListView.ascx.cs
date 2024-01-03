using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LINQTo271.Views.Abstract;
using MMSINC.Controls;
using WorkOrders.Model;
using WorkOrders.Views.CrewAssignments;

namespace LINQTo271.Views.ContractorCrewAssignments
{
    public partial class ContractorCrewAssignmentsListView : WorkOrdersListView<CrewAssignment>, ICrewAssignmentsListView
    {
        #region Properties

        public override IListControl ListControl
        {
            get { return gvCrewAssignments; }
        }

        #endregion

        #region Events
        
        public event CrewAssignmentStartEndEventHandler AssignmentCommand;

        #endregion

        #region Exposed Methods

        public override void SetViewControlsVisible(bool visible)
        {

        }

        public void Redirect(string url)
        {
            IResponse.Redirect(url);
        }

        #endregion
    }
}