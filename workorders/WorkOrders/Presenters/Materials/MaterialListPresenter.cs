using MMSINC.Interface;
using MMSINC.Presenter;
using WorkOrders.Model;

namespace WorkOrders.Presenters.Materials
{
    public class MaterialListPresenter : ListPresenter<Material>
    {
        #region Constructors

        public MaterialListPresenter(IListView<Material> view) : base(view) {}

        #endregion
    }
}