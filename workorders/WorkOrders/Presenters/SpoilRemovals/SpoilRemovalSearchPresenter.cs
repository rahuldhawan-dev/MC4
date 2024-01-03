using MMSINC.Interface;
using MMSINC.Presenter;
using WorkOrders.Model;

namespace WorkOrders.Presenters.SpoilRemovals
{
    public class SpoilRemovalSearchPresenter : SearchPresenter<SpoilRemoval>
    {
        #region Constructors

        public SpoilRemovalSearchPresenter(ISearchView<SpoilRemoval> view) : base(view)
        {
        }

        #endregion

    }
}
