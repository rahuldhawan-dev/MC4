using LINQTo271.Views.Abstract;
using MMSINC.Controls;
using MMSINC.Interface;
using WorkOrders.Model;

namespace LINQTo271.Views.Materials
{
    public partial class MaterialsResourceView : WorkOrdersResourceView<Material>
    {
        #region Control Declarations

        protected IListView<Material> slListView;
        protected ISearchView<Material> slSearchView;

        #endregion

        #region Properties

        public override IListView<Material> ListView
        {
            get { return slListView; }
        }

        public override IDetailView<Material> DetailView
        {
            get { return null; }
        }

        public override ISearchView<Material> SearchView
        {
            get { return slSearchView; }
        }

        public override IButton BackToListButton
        {
            get { return null; }
        }

        #endregion
    }
}