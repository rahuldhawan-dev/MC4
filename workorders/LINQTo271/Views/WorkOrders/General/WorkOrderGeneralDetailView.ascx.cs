using System;
using System.Linq;
using System.Web.UI.WebControls;
using LINQTo271.Views.Abstract;
using MMSINC.Common;
using MMSINC.Controls;
using MMSINC.Interface;
using WorkOrders.Library.Permissions;
using WorkOrders.Model;
using SecurityServiceClass = WorkOrders.Library.Permissions.SecurityService;

namespace LINQTo271.Views.WorkOrders.General
{
    /// <summary>
    /// General Detail View.
    /// </summary>
    public partial class WorkOrderGeneralDetailView : WorkOrderDetailView
    {
        #region Constants

        public struct ControlIDs
        {
            public const string INPUT_FORM = "wofvInitialInformation",
                                RESTORATION_FORM = "worRestoration",
                                STREETOPENINGPERMIT_FORM = "woStreetOpeningPermitForm",
                                MAINBREAK_FORM = "woMainBreakForm",
                                ACCOUNT_FORM = "woAccountForm";
        }

        #endregion

        #region Control Declarations

        protected IDetailControl fvWorkOrder;
        protected IObjectContainerDataSource odsWorkOrder;
        protected IDropDownList ddlWorkOrderCancellationReasons;
        protected MvpButton btnCancelOrder;
        protected MvpButton btnMaterialPlanningComplete;
        public IButton CancelOrderButton => btnCancelOrder;
        public IButton MaterialPlanningCompletedOnButton => btnMaterialPlanningComplete;
        protected ISecurityService _securityService;
        protected HyperLink lnkCreateService;

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
            get { return WorkOrderPhase.General; }
        }

        protected ISecurityService SecurityService
        {
            get
            {
                if (_securityService == null)
                    _securityService = SecurityServiceClass.Instance;
                return _securityService;
            }
        }
        #endregion
        
        #region Event handlers

        protected override void Page_Prerender(object sender, EventArgs e)
        {
            base.Page_Prerender(sender, e);

            // verify user is administrator for the current work order's operating center
            btnEdit.Visible = SecurityService.IsAdmin;

            if (fvWorkOrder.CurrentMvpMode == DetailViewMode.Edit)
            {
                btnRefresh.Visible = SecurityService.IsAdmin;
                btnEdit.Visible = false;
            }

            //DeleteButton.Text = Entity.CancelledAt.HasValue ? "Uncancel Order" : "Cancel Order";
            if (Entity != null)
            {
                // bug 3524: only display the "Create Service" button if the work order is a service etype and
                //           also does not have a service attached to it.
                var assetTypeEnum = AssetTypeRepository.GetEnumerationValue(Entity.AssetType);
                lnkCreateService.Visible = (assetTypeEnum == AssetTypeEnum.Service || assetTypeEnum == AssetTypeEnum.SewerLateral) && !Entity.ServiceID.HasValue;
                lnkCreateService.NavigateUrl = string.Format("../../../../../Modules/mvc/FieldOperations/Service/LinkOrNew?workOrderId={0}", Entity.Id);

                // any are assigned for today
                if (Entity.CrewAssignments.Any(x => x.AssignedFor.Date == DateTime.Today))
                {
                    DeleteButton.Visible = CancelOrderButton.Visible = false;
                }
                else
                {
                    DeleteButton.Visible =
                        CancelOrderButton.Visible =
                            // no crew assignments
                            Entity.CrewAssignments == null ||
                            // no started crew assignments
                            Entity.CrewAssignments.All(x => x.DateStarted == null);
                }
                
                if (Entity.CancelledAt.HasValue && CancelOrderButton != null)
                    DeleteButton.Visible = CancelOrderButton.Visible = false;
                if (Entity.DateCompleted.HasValue || Entity.MaterialsUseds.Any())
                    DeleteButton.Visible = CancelOrderButton.Visible = false;
                if (Entity.DateCompleted.HasValue ||
                    Entity.MaterialPlanningCompletedOn.HasValue)
                    MaterialPlanningCompletedOnButton.Visible = false;
            }
        }

        protected override void btnDelete_Click(object sender, EventArgs e)
        {
            if (ddlWorkOrderCancellationReasons.SelectedValue != "")
            {
                Entity.WorkOrderCancellationReasonID = int.Parse(ddlWorkOrderCancellationReasons.SelectedValue);
                Entity.AssignedToContractorOn = null;
                Entity.AssignedContractor = null;
            }
            OnDeleteClicked(new EntityEventArgs<WorkOrder>(Entity));
            fvWorkOrder.DataBind();
        }

        #endregion

        protected void btnPlanningComplete_Click(object sender, EventArgs e)
        {
            Entity.MaterialPlanningCompletedOn = DateTime.Now;
            
            // pretend we updated it, which we did
            OnUpdating(new EntityEventArgs<WorkOrder>(Entity));

            fvWorkOrder.DataBind();
        }
    }
}