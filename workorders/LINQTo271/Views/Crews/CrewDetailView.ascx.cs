using System.Web.UI.WebControls;
using LINQTo271.Views.Abstract;
using MMSINC.Controls;
using MMSINC.Interface;
using WorkOrders.Model;

namespace LINQTo271.Views.Crews
{
    public partial class CrewDetailView : WorkOrdersDetailView<Crew>
    {
        #region Control Declarations

        protected IDetailControl dvCrew;
        protected IObjectContainerDataSource odsCrew;
        
        #endregion

        #region Constants

        public struct EntityKeys
        {
            public const string OPERATING_CENTER_ID = "OperatingCenterID";
        }

        #endregion

        #region Properties

        public override IDetailControl DetailControl
        {
            get { return dvCrew; }
        }

        public override IObjectContainerDataSource DataSource
        {
            get { return odsCrew; }
        }

        #endregion

        #region Event Handlers

        protected void dvCrew_ItemInserting(object sender, DetailsViewInsertEventArgs e)
        {
            //e.Values[EntityKeys.OPERATING_CENTER_ID] =
            //    Utilities.GetCurrentOperatingCenterID();
        }

        protected void dvCrew_ItemUpdating(object sender, DetailsViewUpdateEventArgs e)
        {
            //e.NewValues[EntityKeys.OPERATING_CENTER_ID] =
                //Utilities.GetCurrentOperatingCenterID();
        }

        #endregion
    }
}
