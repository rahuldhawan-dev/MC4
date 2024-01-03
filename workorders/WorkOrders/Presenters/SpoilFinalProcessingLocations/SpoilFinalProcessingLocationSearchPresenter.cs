using MMSINC.Interface;
using MMSINC.Presenter;
using WorkOrders.Model;

namespace WorkOrders.Presenters.SpoilFinalProcessingLocations
{
    public class SpoilFinalProcessingLocationSearchPresenter : SearchPresenter<SpoilFinalProcessingLocation>
    {
        #region Constructors

        public SpoilFinalProcessingLocationSearchPresenter(ISearchView<SpoilFinalProcessingLocation> view)
            : base(view)
        {
        }

        #endregion
    }
}
