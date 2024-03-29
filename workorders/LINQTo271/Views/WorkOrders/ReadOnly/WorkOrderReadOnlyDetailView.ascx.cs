﻿using LINQTo271.Views.Abstract;
using MMSINC.Controls;
using MMSINC.Interface;
using WorkOrders.Model;

namespace LINQTo271.Views.WorkOrders.ReadOnly
{
    public partial class WorkOrderReadOnlyDetailView : WorkOrderDetailView
    {
        #region Control Declarations

        protected IDetailControl fvWorkOrder;
        protected IObjectContainerDataSource odsWorkOrder;

        #endregion

        #region Properties

        public override IDetailControl DetailControl
        {
            get { return fvWorkOrder; }
        }

        public override IObjectContainerDataSource DataSource
        {
            get { return odsWorkOrder; }
        }

        public override WorkOrderPhase Phase
        {
            get { return WorkOrderPhase.Finalization; }
        }

        #endregion

        #region Exposed Methods

        public override void SetViewMode(DetailViewMode newMode)
        {
            // noop
        }

        #endregion
    }
}
