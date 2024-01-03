using System;
using WorkOrders.Model;

namespace LINQTo271.Views.Abstract
{
    public abstract class CrewAssignmentRPCListView : WorkOrdersListView<CrewAssignment>
    {
        #region Constants

        public struct URLs
        {
            public const string SEARCH =
                                    "~/Views/WorkOrders/Scheduling/WorkOrderSchedulingResourceView.aspx";
        }

        #endregion

        #region Private Members

        private DateTime? _date;
        private Crew _crew;

        #endregion

        #region Properties

        public virtual String CrewID { get; set; }
        
        public virtual String Date
        {
            get { return _date.Value.ToString(); }
            set { _date = Convert.ToDateTime(value); }
        }

        public virtual Crew Crew
        {
            get
            {
                if (_crew == null)
                    _crew = CrewRepository.GetEntity(CrewID);
                return _crew;
            }
        }

        public virtual DateTime TypedDate
        {
            get { return _date.Value; }
        }

        #endregion

        #region Event Handlers

        protected void btnBackToSearch_Click(object sender, EventArgs e)
        {
            Response.Redirect(URLs.SEARCH);
        }

        #endregion

        #region Exposed Methods

        public override void SetViewControlsVisible(bool visible)
        {
            // noop (for now)
        }

        #endregion
    }
}
