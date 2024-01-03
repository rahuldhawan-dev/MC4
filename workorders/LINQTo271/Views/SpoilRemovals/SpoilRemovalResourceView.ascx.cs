using LINQTo271.Views.Abstract;
using MMSINC.Controls;
using MMSINC.Interface;
using WorkOrders.Model;

namespace LINQTo271.Views.SpoilRemovals
{
    public partial class SpoilRemovalResourceView : WorkOrdersResourceView<SpoilRemoval>
    {
        #region Private Members

        protected IListView<SpoilRemoval> srListView;
        protected ISearchView<SpoilRemoval> srSearchView;

        #endregion

        #region Properties

        public override IListView<SpoilRemoval> ListView
        {
            get { return srListView; }
        }

        public override IDetailView<SpoilRemoval> DetailView
        {
            get { return null; }
        }

        public override ISearchView<SpoilRemoval> SearchView
        {
            get { return srSearchView; }
        }

        public override IButton BackToListButton
        {
            get { return null; }
        }

        #endregion
    }
}