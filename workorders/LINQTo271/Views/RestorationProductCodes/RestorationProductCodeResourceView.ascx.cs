using System;
using LINQTo271.Views.Abstract;
using MMSINC.Controls;
using MMSINC.Interface;
using WorkOrders.Model;

namespace LINQTo271.Views.RestorationProductCodes
{
    public partial class RestorationProductCodeResourceView : WorkOrdersResourceView<RestorationProductCode>
    {
        #region Control Declarations

        protected IListView<RestorationProductCode> slListView;
        protected ISearchView<RestorationProductCode> slSearchView;

        #endregion

        #region Properties

        public override IListView<RestorationProductCode> ListView
        {
            get { return slListView; }
        }

        public override ISearchView<RestorationProductCode> SearchView
        {
            get { return slSearchView; }
        }

        public override IDetailView<RestorationProductCode> DetailView
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