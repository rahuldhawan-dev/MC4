﻿using System;
using MMSINC.Controls;
using MMSINC.Interface;

namespace LINQTo271.Controls.WorkOrders
{
    public partial class WorkOrderRestorationForm : WorkOrderDetailControlBase, IWorkOrderRestorationForm
    {
        #region Control Declarations

        protected IGridView gvRestorations;
        protected IObjectDataSource odsRestorations;

        #endregion

        #region Private Methods

        protected override void SetDataSource(int workOrderID)
        {
            odsRestorations.SelectParameters["WorkOrderID"].DefaultValue =
                workOrderID.ToString();
        }

        #endregion

        #region Event Handlers

        #region Page Events

        protected void Page_Load(object sender, EventArgs e)
        {
            var visible = (CurrentMvpMode != DetailViewMode.ReadOnly);
            gvRestorations.AutoGenerateSelectButton = false;
            gvRestorations.AutoGenerateDeleteButton = visible;
            gvRestorations.AutoGenerateEditButton = false;
        }

        protected override void Page_Prerender(object sender, EventArgs e)
        {
            base.Page_Prerender(sender, e);

            if (gvRestorations.IFooterRow != null)
            {
                gvRestorations.IFooterRow.Visible = (CurrentMvpMode != DetailViewMode.ReadOnly);
            }
        }

        #endregion

        #endregion
    }

    public interface IWorkOrderRestorationForm : IWorkOrderDetailControl
    {
    }
}
