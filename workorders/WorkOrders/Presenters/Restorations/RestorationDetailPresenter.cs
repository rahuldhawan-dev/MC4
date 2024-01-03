using MMSINC.Interface;
using MMSINC.Presenter;
using WorkOrders.Model;

namespace WorkOrders.Presenters.Restorations
{
    public class RestorationDetailPresenter : DetailPresenter<Restoration>
    {
        #region Constructors

        public RestorationDetailPresenter(IDetailView<Restoration> view) : base(view)
        {
        }

        #endregion

        #region Exposed Methods

        public override void OnViewInitialized()
        {
            // noop
            // (so it doesn't set view controls invisible, hiding the buttons)
        }

        #endregion
    }
}
