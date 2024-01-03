using LINQTo271.Views.Abstract;
using MMSINC.Interface;
using WorkOrders.Model;

namespace LINQTo271.Views.Restorations
{
    public partial class RestorationReadOnlyRPCView : WorkOrdersResourceRPCView<Restoration>
    {
        #region Constrol Declarations

        protected IDetailView<Restoration> rdvRestoration;

        #endregion

        #region Properties

        public override IListView<Restoration> ListView
        {
            get { return null; }
        }

        public override IDetailView<Restoration> DetailView
        {
            get { return rdvRestoration; }
        }

        public override ISearchView<Restoration> SearchView
        {
            get { return null; }
        }

        #endregion
    }
}
