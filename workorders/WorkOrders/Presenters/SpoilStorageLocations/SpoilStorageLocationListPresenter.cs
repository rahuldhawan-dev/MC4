using MMSINC.Interface;
using MMSINC.Presenter;
using WorkOrders.Model;

namespace WorkOrders.Presenters.SpoilStorageLocations
{
    public class SpoilStorageLocationListPresenter : ListPresenter<SpoilStorageLocation>
    {
        #region Constructors

        public SpoilStorageLocationListPresenter(IListView<SpoilStorageLocation> view)
            : base(view)
        {
        }

        #endregion
    }
}
