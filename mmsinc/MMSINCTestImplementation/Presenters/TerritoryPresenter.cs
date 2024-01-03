using MMSINC.Data.Linq;
using MMSINC.Interface;
using MMSINC.Presenter;
using MMSINCTestImplementation.Model;

namespace MMSINCTestImplementation.Presenters
{
    public class TerritoryDetailPresenter : DetailPresenter<Territory>
    {
        #region Constructors

        public TerritoryDetailPresenter(IDetailView<Territory> view)
            : base(view) { }

        #endregion
    }

    public class TerritoryResourcePresenter : ResourcePresenter<Territory>
    {
        #region Constructors

        public TerritoryResourcePresenter(IResourceView view, IRepository<Territory> repository)
            : base(view, repository) { }

        #endregion
    }

    public class TerritoriesListPresenter : ListPresenter<Territory>
    {
        #region Constructors

        public TerritoriesListPresenter(IListView<Territory> view)
            : base(view) { }

        #endregion
    }
}
