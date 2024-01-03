using System;
using System.Web.UI.WebControls;
using LINQTo271.Controls.WorkOrders;
using LINQTo271.Views.Abstract;
using MMSINC.Controls;
using MMSINC.Interface;
using WorkOrders.Library.Permissions;
using WorkOrders.Model;
using SecurityServiceClass = WorkOrders.Library.Permissions.SecurityService;

namespace LINQTo271.Views.WorkOrders.Finalization
{
    public partial class WorkOrderFinalizationDetailView : WorkOrderDetailView
    {
        #region Constants

        public struct EntityKeys
        {
            public const string DateCompleted = "DateCompleted",
                                CompletedByID = "CompletedByID";
        }

        #endregion

        #region Private Members

        protected IWorkOrderAdditionalFinalizationInfoForm _woAdditionalFinalizationInfoForm;
        protected IWorkOrderMaterialsUsedForm _workOrderMaterialsUsedForm;
       
        #endregion

        #region Control Declarations

        protected IDetailControl fvWorkOrder;
        protected IObjectContainerDataSource odsWorkOrder;
        protected ISecurityService _securityService;
        protected MvpLinkButton lnkEdit;

        #endregion

        #region Properties

        protected ISecurityService SecurityService
        {
            get
            {
                if (_securityService == null)
                    _securityService = SecurityServiceClass.Instance;
                return _securityService;
            }
        }

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

        public IWorkOrderAdditionalFinalizationInfoForm woAdditionalFinalizationInfoForm
        {
            get
            {
                if (_woAdditionalFinalizationInfoForm == null)
                    _woAdditionalFinalizationInfoForm =
                        fvWorkOrder.FindIControl<IWorkOrderAdditionalFinalizationInfoForm>("woafiAdditionalInfo");
                return _woAdditionalFinalizationInfoForm;
            }
        }

        #endregion

        #region Exposed Methods

        public override void SetViewMode(DetailViewMode newMode)
        {
            DetailControl.ChangeMvpMode(newMode);
            var readOnly = (newMode == DetailViewMode.ReadOnly);
            btnEdit.Visible = readOnly;
            lnkEdit.Visible = readOnly;
            lnkEdit.PostBackUrl = "~/Views/WorkOrders/Finalization/WorkOrderFinalizationResourceRPCPage.aspx?cmd=update&arg=" + fvWorkOrder.DataKey.Value;
            btnSave.Visible = !readOnly;
        }

        protected override void btnSave_Click(object sender, EventArgs e)
        {
            woAdditionalFinalizationInfoForm.UpdateDetailControl();
            base.btnSave_Click(sender, e);
        }

        #endregion

        #region Event Handlers

        protected void fvWorkOrder_ItemUpdating(object sender, FormViewUpdateEventArgs e)
        {
            var dateCompleted = e.NewValues[EntityKeys.DateCompleted];

            if (dateCompleted != null)
                e.NewValues[EntityKeys.CompletedByID] =
                    SecurityService.GetEmployeeID();
        }


        protected override void Page_Prerender(object sender, EventArgs e)
        {
            base.Page_Prerender(sender, e);
            if (fvWorkOrder.CurrentMvpMode == DetailViewMode.Edit)
            {
                btnEdit.Visible = false;
            }

            if (Entity != null)
            {
                // bug 3524: only display the "Create Service" button if the work order is a service etype and
                //           also does not have a service attached to it.
                var assetTypeEnum = AssetTypeRepository.GetEnumerationValue(Entity.AssetType);
                lnkCreateService.Visible = (assetTypeEnum == AssetTypeEnum.Service || assetTypeEnum == AssetTypeEnum.SewerLateral) && !Entity.ServiceID.HasValue;
                lnkCreateService.NavigateUrl = $"/Modules/mvc/FieldOperations/Service/LinkOrNew?workOrderId={Entity.Id}";
                lnkCreateService.Text = "Create Service"; 
            }
        }

        #endregion
    }
}
