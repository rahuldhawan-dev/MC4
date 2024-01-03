using MMSINC.Interface;
using MMSINC.Presenter;
using WorkOrders.Model;

namespace WorkOrders.Presenters.SpoilRemovals
{
    public class SpoilRemovalListPresenter : ListPresenter<SpoilRemoval>
    {
        #region Constructors

        public SpoilRemovalListPresenter(IListView<SpoilRemoval> view)
            : base(view)
        {
        }

        #endregion
    }
}
