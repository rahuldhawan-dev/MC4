using LINQTo271.Views.Abstract;
using MMSINC.Controls;
using MMSINC.Interface;
using WorkOrders.Model;

namespace LINQTo271.Views.RestorationTypeCosts
{
    public partial class RestorationTypeCostResourceView : WorkOrdersResourceView<RestorationTypeCost>
    {
        #region Control Declarations

        protected IListView<RestorationTypeCost> rtcListView;

        #endregion

        #region Properties

        public override IListView<RestorationTypeCost> ListView
        {
            get { return rtcListView; }
        }

        public override IDetailView<RestorationTypeCost> DetailView
        {
            get { return null; }
        }

        public override ISearchView<RestorationTypeCost> SearchView
        {
            get { return null; }
        }

        public override IButton BackToListButton
        {
            get { return null; }
        }

        #endregion
    }
}
