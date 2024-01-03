using MMSINC.Interface;
using MMSINC.Presenter;
using WorkOrders.Model;

namespace WorkOrders.Presenters.SpoilStorageLocations
{
    public class SpoilStorageLocationSearchPresenter : SearchPresenter<SpoilStorageLocation>
    {
        #region Constructors

        public SpoilStorageLocationSearchPresenter(ISearchView<SpoilStorageLocation> view)
            : base(view)
        {
        }

        #endregion
    }
}
