using MMSINC.Interface;
using MMSINC.Presenter;
using WorkOrders.Model;

namespace WorkOrders.Presenters.Materials
{
    public class MaterialSearchPresenter : SearchPresenter<Material>
    {
        #region Constructors

        public MaterialSearchPresenter(ISearchView<Material> view) : base(view) {}

        #endregion

    }
}
