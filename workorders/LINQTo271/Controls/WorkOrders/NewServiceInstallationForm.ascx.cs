using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using MMSINC.Controls;
using MMSINC.Data.Linq;
using StructureMap;
using WorkOrders.Model;

namespace LINQTo271.Controls.WorkOrders
{
    public partial class NewServiceInstallationForm : WorkOrderDetailControlBase 
    {
        #region Private Members

        protected IRepository<WorkOrder> _repository;
        protected IObjectContainerDataSource odsWorkOrder;
        
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
                    _repository = DependencyResolver.Current.GetService<IRepository<WorkOrder>>();
                return _repository;
            }
        }

        protected override void SetDataSource(int workOrderID)
        {
            DataSource = Repository.Get(workOrderID).NewServiceInstallation;
        }
    }
}