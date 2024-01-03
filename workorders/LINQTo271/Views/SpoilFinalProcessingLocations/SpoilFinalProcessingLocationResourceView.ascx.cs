using LINQTo271.Views.Abstract;
using MMSINC.Controls;
using MMSINC.Interface;
using WorkOrders.Model;

namespace LINQTo271.Views.SpoilFinalProcessingLocations
{
    public partial class SpoilFinalProcessingLocationResourceView : WorkOrdersResourceView<SpoilFinalProcessingLocation>
    {
        #region Control Declarations

        protected IListView<SpoilFinalProcessingLocation> sslListView;
        protected ISearchView<SpoilFinalProcessingLocation> sslSearchView;

        
        #endregion

        #region Properties

        public override IListView<SpoilFinalProcessingLocation> ListView
        {
            get { return sslListView; }
        }

        public override IDetailView<SpoilFinalProcessingLocation> DetailView
        {
            get { return null; }
        }

        public override ISearchView<SpoilFinalProcessingLocation> SearchView
        {
            get { return sslSearchView; }
        }

        public override IButton BackToListButton
        {
            get { return null; }
        }

        #endregion
    }
}