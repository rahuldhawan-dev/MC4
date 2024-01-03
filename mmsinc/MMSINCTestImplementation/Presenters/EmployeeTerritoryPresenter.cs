using MMSINC.Data.Linq;
using MMSINC.Interface;
using MMSINC.Presenter;
using MMSINCTestImplementation.Model;

namespace MMSINCTestImplementation.Presenters
{
    public class EmployeeTerritoryDetailPresenter : DetailPresenter<EmployeeTerritory>
    {
        #region Constructors

        public EmployeeTerritoryDetailPresenter(IDetailView<EmployeeTerritory> view)
            : base(view) { }

        #endregion
    }

    public class EmployeeTerritoryResourcePresenter : ResourcePresenter<EmployeeTerritory>
    {
        #region Constructors

        public EmployeeTerritoryResourcePresenter(IResourceView view, IRepository<EmployeeTerritory> repository)
            : base(view, repository) { }

        #endregion
    }

    public class EmployeeTerritoryChildResourcePresenter : ChildResourcePresenter<EmployeeTerritory>
    {
        #region Constructors

        public EmployeeTerritoryChildResourcePresenter(IChildResourceView<EmployeeTerritory> view,
            IRepository<EmployeeTerritory> repository)
            : base(view, repository) { }

        #endregion

        #region Exposed Methods

        public override void FilterListViews()
        {
            ListView.SetListData(
                Repository.GetFilteredSortedData(
                    empt => empt.Employee == ParentEntity,
                    ListView.SqlSortExpression));
        }

        #endregion
    }

    public class EmployeeTerritoriesListPresenter : ListPresenter<EmployeeTerritory>
    {
        #region Constructors

        public EmployeeTerritoriesListPresenter(IListView<EmployeeTerritory> view)
            : base(view) { }

        #endregion
    }
}
