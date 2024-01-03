using MMSINC.Interface;
using MMSINC.Presenter;
using WorkOrders.Model;

namespace WorkOrders.Presenters.SpoilFinalProcessingLocations
{
    public class SpoilFinalProcessingLocationListPresenter : ListPresenter<SpoilFinalProcessingLocation>
    {
        #region Constructors

        public SpoilFinalProcessingLocationListPresenter(IListView<SpoilFinalProcessingLocation> view)
            : base(view)
        {
        }

        #endregion
    }
}
