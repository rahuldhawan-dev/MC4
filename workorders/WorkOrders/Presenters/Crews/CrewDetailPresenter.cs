using MMSINC.Interface;
using MMSINC.Presenter;
using WorkOrders.Model;

namespace WorkOrders.Presenters.Crews
{
    public class CrewDetailPresenter : DetailPresenter<Crew>
    {
        #region Constructors

        public CrewDetailPresenter(IDetailView<Crew> view)
            : base(view)
        {
        }

        #endregion
    }
}
