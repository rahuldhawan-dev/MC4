using MMSINC.Interface;
using MMSINC.Presenter;
using WorkOrders.Model;

namespace WorkOrders.Presenters.Crews
{
    public class CrewsListPresenter : ListPresenter<Crew>
    {
        #region Constructors

        public CrewsListPresenter(IListView<Crew> view)
            : base(view)
        {
        }

        #endregion
    }
}
