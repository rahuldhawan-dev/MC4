using MMSINC.Data.Linq;
using MMSINC.Interface;
using MMSINC.Presenter;
using MMSINCTestImplementation.Model;

namespace MMSINCTestImplementation.Presenters
{
    public class StateDetailPresenter : DetailPresenter<State>
    {
        #region Constructors

        public StateDetailPresenter(IDetailView<State> view)
            : base(view) { }

        #endregion
    }

    public class StateResourcePresenter : ResourcePresenter<State>
    {
        #region Constructors

        public StateResourcePresenter(IResourceView view, IRepository<State> repository)
            : base(view, repository) { }

        #endregion
    }

    public class StatesListPresenter : ListPresenter<State>
    {
        #region Constructors

        public StatesListPresenter(IListView<State> view)
            : base(view) { }

        #endregion
    }
}
