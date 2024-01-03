using MMSINC.Data.Linq;
using MMSINC.Interface;
using MMSINC.Presenter;
using MMSINCTestImplementation.Model;

namespace MMSINCTestImplementation.Presenters
{
    public class OrderDetailPresenter : DetailPresenter<Order>
    {
        #region Constructors

        public OrderDetailPresenter(IDetailView<Order> view)
            : base(view) { }

        #endregion
    }

    public class OrderResourcePresenter : ResourcePresenter<Order>
    {
        #region Constructors

        public OrderResourcePresenter(IResourceView view, IRepository<Order> repository)
            : base(view, repository) { }

        #endregion
    }

    public class OrdersListPresenter : ListPresenter<Order>
    {
        #region Constructors

        public OrdersListPresenter(IListView<Order> view)
            : base(view) { }

        #endregion
    }
}
