using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using LINQTo271.Common;
using LINQTo271.Views.Abstract;
using LINQTo271.Views.AssetTypes;
using MMSINC.Common;
using MMSINC.Controls;
using WorkOrders.Model;

namespace LINQTo271.Views.WorkOrders.General
{
    public partial class WorkOrderGeneralSearchView : WorkOrderSearchView
    {
        #region Control Declarations

        protected IListBox lstDescriptionOfWork, lstDocumentType, lstDrivenBy;
        protected IDropDownList ddlAssetType, ddlLastCrewAssigned;
        protected ITextBox txtAssetID, txtSAPNotificationNumber, txtSAPWorkOrderNumber, txtWBSCharged;
        protected IDateRange drDateToSearch;

        protected IDropDownList ddlDateType,
                                ddlPriority,
                                ddlRequestedBy,
                                ddlAcousticMonitoringType,
                                ddlMarkoutRequirement,
                                ddlSOPRequirement,
                                ddlCreatedBy,
                                ddlContractor,
                                ddlHasInvoice,
                                ddlStreetOpeningPermitRequested,
                                ddlStreetOpeningPermitIssued,
                                ddlRequiresInvoice,
                                ddlIsAssignedToContractor,
                                ddlCompleted,
                                ddlCancelled;

        #endregion

        #region Properties

        public override Expression<Func<WorkOrder, bool>> BaseExpression => PredicateBuilder.True<WorkOrder>();

        public AssetTypesJSView AssetTypeIDsScript2
        {
            get { return AssetTypeIDsScript1; }
            set { AssetTypeIDsScript1 = value; }
        }

        public override WorkOrderPhase Phase
        {
            get { return WorkOrderPhase.General; }
        }

        public string AssetID
        {
            get { return txtAssetID.Text; }
        }

        public long? SAPNotificationNumber
        {
            get
            {
                if (String.IsNullOrWhiteSpace(txtSAPNotificationNumber.Text))
                    return null;
                return long.Parse(txtSAPNotificationNumber.Text);
            }
        }

        public long? SAPWorkOrderNumber
        {
            get
            {
                if (String.IsNullOrWhiteSpace(txtSAPWorkOrderNumber.Text))
                    return null;
                return long.Parse(txtSAPWorkOrderNumber.Text);
            }
        }

        public override int? AssetTypeID
        {
            get { return ddlAssetType.GetSelectedValue(); }
        }

        public int? LastCrewAssignedID
        {
            get { return ddlLastCrewAssigned.GetSelectedValue(); }
        }

        public override List<int> DescriptionOfWorkIDs
        {
            get { return lstDescriptionOfWork.GetSelectedValues(); }
        }

        public DateTime? DateToSearch
        {
            get { return drDateToSearch.Date; }
        }

        public DateTime? DateToSearchStart
        {
            get { return drDateToSearch.StartDate; }
        }

        public DateTime? DateToSearchEnd
        {
            get { return drDateToSearch.EndDate; }
        }

        public List<int> DocumentTypeIDs
        {
            get { return lstDocumentType.GetSelectedValues(); }
        }

        public List<int> PurposeIDs
        {
            get { return lstDrivenBy.GetSelectedValues(); }
        }

        protected int? PriorityID
        {
            get { return ddlPriority.GetSelectedValue(); }
        }

        protected int? CreatedByID
        {
            get { return ddlCreatedBy.GetSelectedValue(); }
        }

        protected int? RequestedByID
        {
            get { return ddlRequestedBy.GetSelectedValue(); }
        }

        protected int? AcoustingMonitoringTypeID
        {
            get { return ddlAcousticMonitoringType.GetSelectedValue(); }
        }

        protected int? MarkoutRequirementID
        {
            get { return ddlMarkoutRequirement.GetSelectedValue(); }
        }

        protected bool? SOPRequired
        {
            get { return ddlSOPRequirement.GetBooleanValue(); }
        }

        public bool? StreetOpeningPermitRequested => ddlStreetOpeningPermitRequested.GetBooleanValue();

        public bool? StreetOpeningPermitIssued => ddlStreetOpeningPermitIssued.GetBooleanValue();

        protected bool? HasInvoice
        {
            get { return ddlHasInvoice.GetBooleanValue(); }
        }

        protected bool? RequiresInvoice
        {
            get { return ddlRequiresInvoice.GetBooleanValue(); }
        }

        protected bool? Completed
        {
            get
            {
                return
                    ddlCompleted.GetSelectedValue(
                        li =>
                        li.Value == String.Empty
                            ? (bool?)null : bool.Parse(li.Value));
            }
        }

        protected bool? Cancelled
        {
            get
            {
                return
                    ddlCancelled.GetSelectedValue(
                        li =>
                            li.Value == String.Empty
                                ? (bool?)null : bool.Parse(li.Value));
            }
        }

        public int? ContractorID
        {
            get { return ddlContractor.GetSelectedValue(); }
        }

        public bool? IsAssignedToContractor
        {
            get { return ddlIsAssignedToContractor.GetBooleanValue(); }
        }

        protected string AccountCharged => txtWBSCharged?.Text;

        #endregion

        #region Private Methods
        // Not exactly the DRYest code here :/

        private void AddDateReceivedFilter(ExpressionBuilder<WorkOrder> builder)
        {
            if (DateToSearch != null)
            {
                switch (drDateToSearch.SelectedOperator)
                {
                    case "=":
                        builder.And(wo => wo.DateReceived == DateToSearch);
                        break;
                    case ">":
                        builder.And(wo => DateTime.Compare(wo.DateReceived.Value, DateToSearch.Value) > 0);
                        break;
                    case ">=":
                        builder.And(wo => DateTime.Compare(wo.DateReceived.Value, DateToSearch.Value) >= 0);
                        break;
                    case "<":
                        builder.And(wo => DateTime.Compare(wo.DateReceived.Value, DateToSearch.Value) < 0);
                        break;
                    case "<=":
                        builder.And(wo => DateTime.Compare(wo.DateReceived.Value, DateToSearch.Value) <= 0);
                        break;
                }
            }
            else if (DateToSearchStart != null && DateToSearchEnd != null)
                builder.And(
                        wo =>
                        (DateTime.Compare(wo.DateReceived.Value, DateToSearchStart.Value) >= 0 &&
                         DateTime.Compare(wo.DateReceived.Value, DateToSearchEnd.Value) <= 0));
        }

        private void AddCancelledAtFilter(ExpressionBuilder<WorkOrder> builder)
        {
            if (DateToSearch != null)
            {
                switch (drDateToSearch.SelectedOperator)
                {
                    case "=":
                        builder.And(wo => wo.CancelledAt == DateToSearch);
                        break;
                    case ">":
                        builder.And(wo => DateTime.Compare(wo.CancelledAt.Value, DateToSearch.Value) > 0);
                        break;
                    case ">=":
                        builder.And(wo => DateTime.Compare(wo.CancelledAt.Value, DateToSearch.Value) >= 0);
                        break;
                    case "<":
                        builder.And(wo => DateTime.Compare(wo.CancelledAt.Value, DateToSearch.Value) < 0);
                        break;
                    case "<=":
                        builder.And(wo => DateTime.Compare(wo.CancelledAt.Value, DateToSearch.Value) <= 0);
                        break;
                }
            }
            else if (DateToSearchStart != null && DateToSearchEnd != null)
                builder.And(
                        wo =>
                        (DateTime.Compare(wo.CancelledAt.Value, DateToSearchStart.Value) >= 0 &&
                         DateTime.Compare(wo.CancelledAt.Value, DateToSearchEnd.Value) <= 0));
        }

        private void AddDateCompletedFilter(ExpressionBuilder<WorkOrder> builder)
        {
            if (DateToSearch != null)
            {
                switch (drDateToSearch.SelectedOperator)
                {
                    case "=":
                        builder.And(wo => wo.DateCompleted == DateToSearch);
                        break;
                    case ">":
                        builder.And(wo => DateTime.Compare(wo.DateCompleted.Value, DateToSearch.Value) > 0);
                        break;
                    case ">=":
                        builder.And(wo => DateTime.Compare(wo.DateCompleted.Value, DateToSearch.Value) >= 0);
                        break;
                    case "<":
                        builder.And(wo => DateTime.Compare(wo.DateCompleted.Value, DateToSearch.Value) < 0);
                        break;
                    case "<=":
                        builder.And(wo => DateTime.Compare(wo.DateCompleted.Value, DateToSearch.Value) <= 0);
                        break;
                }
            }
            else if (DateToSearchStart != null && DateToSearchEnd != null)
                builder.And(
                        wo =>
                        (DateTime.Compare(wo.DateCompleted.Value, DateToSearchStart.Value) >= 0 &&
                         DateTime.Compare(wo.DateCompleted.Value, DateToSearchEnd.Value) <= 0));
        }

        private void AddDateDocumentAttachedFilter(ExpressionBuilder<WorkOrder> builder)
        {
            if (DateToSearch != null)
            {
                switch (drDateToSearch.SelectedOperator)
                {
                    case "=":
                        builder.And(wo => (from dwo in wo.DocumentsWorkOrders
                                           where
                                               DateToSearch.Value.Date ==
                                               dwo.Document.CreatedOn.Date
                                           select dwo).Count() > 0);
                        break;
                    case ">":
                        builder.And(wo => (from dwo in wo.DocumentsWorkOrders
                                           where
                                               DateTime.Compare(
                                                   dwo.Document.CreatedOn.Date,
                                                   DateToSearch.Value.Date) > 0
                                           select dwo).Count() > 0);
                        break;
                    case ">=":
                        builder.And(wo => (from dwo in wo.DocumentsWorkOrders
                                           where
                                               DateTime.Compare(
                                                   dwo.Document.CreatedOn.Date,
                                                   DateToSearch.Value.Date) >= 0
                                           select dwo).Count() > 0);
                        break;
                    case "<":
                        builder.And(wo => (from dwo in wo.DocumentsWorkOrders
                                           where
                                               DateTime.Compare(
                                                   dwo.Document.CreatedOn.Date,
                                                   DateToSearch.Value.Date) < 0
                                           select dwo).Count() > 0);
                        break;
                    case "<=":
                        builder.And(wo => (from dwo in wo.DocumentsWorkOrders
                                           where
                                               DateTime.Compare(
                                                   dwo.Document.CreatedOn.Date,
                                                   DateToSearch.Value.Date) <= 0
                                           select dwo).Count() > 0);
                        break;
                }
            }
            else if (DateToSearchStart != null && DateToSearchEnd != null)
                builder.And(wo => (from dwo in wo.DocumentsWorkOrders
                                   where
                                       (DateTime.Compare(
                                            dwo.Document.CreatedOn.Date,
                                            DateToSearchStart.Value.Date) >= 0 &&
                                        DateTime.Compare(
                                            dwo.Document.CreatedOn.Date,
                                            DateToSearchEnd.Value.Date) <= 0)
                                   select dwo).Count() > 0);
        }

        #endregion

        #region Exposed Methods

        protected override void ApplySearchFilters(ExpressionBuilder<WorkOrder> builder)
        {
            switch(ddlDateType.SelectedValue)
            {
                case "DateCompleted":
                    AddDateCompletedFilter(builder);
                    break;
                case "DateReceived":
                    AddDateReceivedFilter(builder);
                    break;
                case "DateDocumentAttached":
                    AddDateDocumentAttachedFilter(builder);
                    break;
                case "CancelledAt":
                    AddCancelledAtFilter(builder);
                    break;
            }

            if (AssetTypeID != null && !String.IsNullOrEmpty(AssetID))
            {
                switch (AssetTypeID)
                {
                    /* Don't Convert these to String.IsNullOrEmpty. It fails SQL Translation. */
                    case AssetTypeRepository.Indices.VALVE:
                        builder.And(
                            wo =>
                            wo.Valve != null &&
                            wo.Valve.ValveNumber != null &&
                            wo.Valve.ValveNumber != string.Empty &&
                            wo.Valve.ValveNumber.Contains(AssetID));
                        break;
                    case AssetTypeRepository.Indices.HYDRANT:
                        builder.And(
                            wo =>
                            wo.Hydrant != null &&
                            wo.Hydrant.HydrantNumber != null &&
                            wo.Hydrant.HydrantNumber != string.Empty &&
                            wo.Hydrant.HydrantNumber.Contains(AssetID));
                        break;
                    case AssetTypeRepository.Indices.SEWER_OPENING:
                        builder.And(
                            wo =>
                            wo.SewerOpening != null &&
                            wo.SewerOpening.OpeningNumber != null &&
                            wo.SewerOpening.OpeningNumber != string.Empty &&
                            wo.SewerOpening.OpeningNumber.Contains(AssetID));
                        break;
                    case AssetTypeRepository.Indices.STORM_CATCH:
                        builder.And(
                            wo => 
                                wo.StormCatch != null &&
                                wo.StormCatch.AssetNumber != null &&
                                wo.StormCatch.AssetNumber != string.Empty && 
                                wo.StormCatch.AssetNumber.Contains(AssetID));
                        break;
                    case AssetTypeRepository.Indices.SERVICE:
                    case AssetTypeRepository.Indices.SEWER_LATERAL:
                        builder.And(wo =>
                            wo.PremiseNumber != null &&
                            wo.PremiseNumber != string.Empty &&
                            wo.PremiseNumber.Contains(AssetID) &&
                            wo.AssetTypeID == AssetTypeID);
                        break;
                    case AssetTypeRepository.Indices.EQUIPMENT:
                        builder.And(wo =>
                            wo.Equipment != null && wo.Equipment.EquipmentID.ToString()!= null &&
                           // wo.Equipment.Identifier != string.Empty &&
                            wo.Equipment.EquipmentID.ToString().Contains(AssetID));
                        break;
                }
            }

            if (SAPNotificationNumber != null)
            {
                builder.And(wo => wo.SAPNotificationNumber == SAPNotificationNumber);
            }

            if (SAPWorkOrderNumber != null)
            {
                builder.And(wo => wo.SAPWorkOrderNumber == SAPWorkOrderNumber);
            }

            if (DocumentTypeIDs != null && DocumentTypeIDs.Count > 0)
            {
                builder.And(wo => (from dwo in wo.DocumentsWorkOrders
                                   where
                                       DocumentTypeIDs.Contains(
                                       dwo.Document.DocumentTypeID)
                                   select dwo).Count() > 0);
            }

            if (PurposeIDs != null && PurposeIDs.Any())
                builder.And(wo => PurposeIDs.Contains(wo.PurposeID));
            if (PriorityID != null)
                builder.And(wo => wo.PriorityID == PriorityID);
            if (RequestedByID != null)
                builder.And(wo => wo.RequesterID == RequestedByID);
            if (AcoustingMonitoringTypeID != null)
                builder.And(wo => wo.AcousticMonitoringTypeId == AcoustingMonitoringTypeID);
            if (MarkoutRequirementID != null)
                builder.And(wo => wo.MarkoutRequirementID == MarkoutRequirementID);
            if (SOPRequired != null)
                builder.And(wo => wo.StreetOpeningPermitRequired == SOPRequired);
            if (StreetOpeningPermitRequested != null)
                builder.And(wo => wo.StreetOpeningPermits.Any() == StreetOpeningPermitRequested);
            if (StreetOpeningPermitIssued != null)
                builder.And(wo => wo.StreetOpeningPermits.Any(x => x.DateIssued != null) == StreetOpeningPermitIssued);
            if (RequiresInvoice != null)
                builder.And(wo => wo.RequiresInvoice == RequiresInvoice);
            if (HasInvoice != null)
                builder.And(wo => wo.WorkOrderInvoices.Any() == HasInvoice);
            if (CreatedByID != null)
                builder.And(wo => wo.CreatorID == CreatedByID);
            if (Completed != null)
                builder.And(wo => (wo.DateCompleted != null) == Completed);
            if (Cancelled != null)
                builder.And(wo => (wo.CancelledAt != null) == Cancelled);
            if (ContractorID != null)
                builder.And(wo => wo.AssignedContractorID == ContractorID);
            if (IsAssignedToContractor != null)
                builder.And(wo => IsAssignedToContractor == (wo.AssignedContractorID != null));
            if (LastCrewAssignedID != null)
                builder.And(
                    wo =>
                    (wo.CrewAssignments.Any() &&
                    wo.CrewAssignments.OrderByDescending(ca => ca.AssignedFor)
                      .First()
                      .CrewID == LastCrewAssignedID.Value));
            if (!string.IsNullOrWhiteSpace(AccountCharged))
            {
                builder.And(wo => wo.AccountCharged == AccountCharged);
            }
        }

        #endregion
    }
}