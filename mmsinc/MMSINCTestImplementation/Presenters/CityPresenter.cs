using MMSINC.Data.Linq;
using MMSINC.Interface;
using MMSINC.Presenter;
using MMSINCTestImplementation.Model;

namespace MMSINCTestImplementation.Presenters
{
    public class CityDetailPresenter : DetailPresenter<City>
    {
        #region Constructors

        public CityDetailPresenter(IDetailView<City> view)
            : base(view) { }

        #endregion
    }

    public class CityResourcePresenter : ResourcePresenter<City>
    {
        #region Constructors

        public CityResourcePresenter(IResourceView view, IRepository<City> repository)
            : base(view, repository) { }

        #endregion
    }

    public class CitysListPresenter : ListPresenter<City>
    {
        #region Constructors

        public CitysListPresenter(IListView<City> view)
            : base(view) { }

        #endregion
    }
}
