using System.Web.Mvc;
using MMSINC.Data.Linq;
using StructureMap;
using WorkOrders.Model;

namespace LINQTo271.Controls.WorkOrders
{
    public partial class JobSiteCheckListForm : WorkOrderDetailControlBase, IJobSiteCheckListForm
    {
        #region Private Members

        protected IRepository<WorkOrder> _repository;

        #endregion

        public object DataSource
        {
            set { odsWorkOrder.DataSource = value; }
        }

        protected IRepository<WorkOrder> Repository
        {
            get
            {
                if (_repository == null)
                    _repository =
                        DependencyResolver.Current.GetService<IRepository<WorkOrder>>();
                return _repository;
            }
        }

        protected override void SetDataSource(int workOrderID)
        {
             DataSource = Repository.Get(workOrderID).JobSiteCheckLists;
        }
    }

    public interface IJobSiteCheckListForm : IWorkOrderDetailControl
    {

    }
}