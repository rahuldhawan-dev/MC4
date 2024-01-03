using LINQTo271.Views.Abstract;
using MMSINC.Controls;
using MMSINC.Interface;
using WorkOrders.Model;

namespace LINQTo271.Views.RestorationAccountingCodes
{
    public partial class RestorationAccountingCodeResourceView : WorkOrdersResourceView<RestorationAccountingCode>
    {
        #region Control Declarations

        protected IListView<RestorationAccountingCode> slListView;
        protected ISearchView<RestorationAccountingCode> slSearchView;

        #endregion

        #region Properties

        public override IListView<RestorationAccountingCode> ListView
        {
            get { return slListView; }
        }

        public override ISearchView<RestorationAccountingCode> SearchView
        {
            get { return slSearchView; }
        }

        public override IDetailView<RestorationAccountingCode> DetailView
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