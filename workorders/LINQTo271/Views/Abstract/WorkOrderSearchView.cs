using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using LINQTo271.Controls.WorkOrders;
using MMSINC.Common;
using WorkOrders.Model;
using WorkOrders.Views.WorkOrders;

namespace LINQTo271.Views.Abstract
{
    /// <summary>
    /// Abstract search view intended to be the base for all search views in
    /// this project that deal with WorkOrder objects (in various contexts).
    /// </summary>
    public abstract class WorkOrderSearchView : WorkOrdersSearchView<WorkOrder>, IWorkOrderSearchView
    {
        #region Control Declarations

        protected IBaseWorkOrderSearch baseSearch;

        #endregion

        #region Properties

        public override Expression<Func<WorkOrder, bool>> BaseExpression =>
                    new ExpressionBuilder<WorkOrder>(
                        wo => wo.CancelledAt == null);

        // This is used to filter the views so that orders that haven't been submitted to SAP
        // that should have do not appear until they have been fixed.
        public Expression<Func<WorkOrder, bool>> SAPValid => new ExpressionBuilder<WorkOrder>(wo =>
            wo.OperatingCenter.SAPWorkOrdersEnabled == false 
            || (wo.OperatingCenter.SAPWorkOrdersEnabled && wo.OperatingCenter.IsContractedOperations)
            || (wo.OperatingCenter.SAPWorkOrdersEnabled && wo.SAPWorkOrderNumber.HasValue)
        );
        
        //static so that the RPCView can use it
        public static Expression<Func<WorkOrder, bool>> NotRetiredRemovedOrCancelled => new ExpressionBuilder<WorkOrder>(wo => 
            !(wo.AssetTypeID == AssetTypeRepository.Indices.VALVE 
                && (wo.Valve.AssetStatusID == MapCall.Common.Model.Entities.AssetStatus.Indices.RETIRED
                 || wo.Valve.AssetStatusID == MapCall.Common.Model.Entities.AssetStatus.Indices.REMOVED
                 || wo.Valve.AssetStatusID == MapCall.Common.Model.Entities.AssetStatus.Indices.CANCELLED))
            &&
            !(wo.AssetTypeID == AssetTypeRepository.Indices.HYDRANT
                && (wo.Hydrant.AssetStatusID == MapCall.Common.Model.Entities.AssetStatus.Indices.RETIRED
                 || wo.Hydrant.AssetStatusID == MapCall.Common.Model.Entities.AssetStatus.Indices.REMOVED
                 || wo.Hydrant.AssetStatusID == MapCall.Common.Model.Entities.AssetStatus.Indices.CANCELLED))
            &&
            !(wo.AssetTypeID == AssetTypeRepository.Indices.SEWER_OPENING
                && (wo.SewerOpening.AssetStatusID == MapCall.Common.Model.Entities.AssetStatus.Indices.RETIRED
                 || wo.SewerOpening.AssetStatusID == MapCall.Common.Model.Entities.AssetStatus.Indices.REMOVED
                 || wo.SewerOpening.AssetStatusID == MapCall.Common.Model.Entities.AssetStatus.Indices.CANCELLED)
            )
        );
        
        public virtual int? OperatingCenterID
        {
            get { return baseSearch.OperatingCenterID; }
        }

        public virtual int? TownID
        {
            get { return baseSearch.TownID; }
        }

        public virtual int? TownSectionID
        {
            get { return baseSearch.TownSectionID; }
        }

        public virtual int? StreetID
        {
            get { return baseSearch.StreetID; }
        }

        public virtual int? NearestCrossStreetID
        {
            get { return baseSearch.NearestCrossStreetID; }
        }

        public virtual int? AssetTypeID
        {
            get { return baseSearch.AssetTypeID; }
        }

        public virtual int? WorkOrderNumber
        {
            get { return baseSearch.WorkOrderNumber; }
        }

        public virtual List<int> DescriptionOfWorkIDs
        {
            get { return baseSearch.DescriptionOfWorkIDs; }
        }
        
        public virtual string StreetNumber
        {
            get { return baseSearch.StreetNumber; }
        }

        public virtual string ApartmentAddtl
        {
            get { return baseSearch.ApartmentAddtl; }
        }

        #endregion

        #region Abstract Properties

        public abstract WorkOrderPhase Phase { get; }

        #endregion

        #region Private Methods

        protected virtual void ApplyCommonSearchFilters(ExpressionBuilder<WorkOrder> builder)
        {
            if (OperatingCenterID!=null)
                builder.And(wo => wo.OperatingCenterID == OperatingCenterID);
            if (TownID != null)
                builder.And(wo => wo.TownID == TownID);
            if (TownSectionID != null)
                builder.And(wo => wo.TownSectionID == TownSectionID);
            if (StreetID != null)
                builder.And(wo => wo.StreetID == StreetID);
            if (NearestCrossStreetID != null)
                builder.And(wo => wo.NearestCrossStreetID == NearestCrossStreetID);
            if (AssetTypeID != null)
                builder.And(wo => wo.AssetTypeID == AssetTypeID);
            if (DescriptionOfWorkIDs != null)
                builder.And(
                    wo => DescriptionOfWorkIDs.Contains(wo.WorkDescriptionID));
            if(!string.IsNullOrEmpty(StreetNumber))
                builder.And( wo => StreetNumber == wo.StreetNumber);
            if(!string.IsNullOrEmpty(ApartmentAddtl))
                builder.And(wo => ApartmentAddtl == wo.ApartmentAddtl);
        }

        #endregion

        #region Exposed Methods

        public override Expression<Func<WorkOrder, bool>> GenerateExpression()
        {
            var builder = new ExpressionBuilder<WorkOrder>(BaseExpression);
            if (WorkOrderNumber != null)
            {
                builder.And(wo => wo.WorkOrderID == WorkOrderNumber);
            }

            ApplyCommonSearchFilters(builder);
            ApplySearchFilters(builder);
            return builder;
        }

        public virtual void DisplaySearchError(string message)
        {
            baseSearch.DisplaySearchError(message);
        }

        #endregion

        #region Abstract Methods

        protected abstract void ApplySearchFilters(
            ExpressionBuilder<WorkOrder> builder);

        #endregion
    }
}
