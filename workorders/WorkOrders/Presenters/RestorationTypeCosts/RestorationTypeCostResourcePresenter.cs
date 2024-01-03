using MMSINC.Data.Linq;
using MMSINC.Interface;
using WorkOrders.Model;
using WorkOrders.Presenters.Abstract;

namespace WorkOrders.Presenters.RestorationTypeCosts
{
    public class RestorationTypeCostResourcePresenter : WorkOrdersAdminResourcePresenter<RestorationTypeCost>
    {
        #region Constructors

        public RestorationTypeCostResourcePresenter(IResourceView view, IRepository<RestorationTypeCost> repository)
            : base(view, repository)
        {
        }

        #endregion
    }
}
