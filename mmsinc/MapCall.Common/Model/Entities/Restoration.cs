using System;
using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Mappings;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.Excel;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class Restoration : IEntity, IThingWithOperatingCenter, IThingWithDocuments, IThingWithNotes,
        IThingWithCoordinate
    {
        #region Consts

        public struct StringLengths
        {
            public const int PARTIAL_INVOICE_NUMBER = 12,
                             INITIAL_PO_NUM = 20,
                             PARTIAL_PO_NUM = INITIAL_PO_NUM,
                             FINAL_PO_NUM = PARTIAL_PO_NUM,
                             FINAL_INVOICE_NUMBER = 12,
                             PARTIAL_AND_FINAL_TRAFFIC_CONTROL_INVOICE_NUMBER = 30,
                             WBS_NUMBER = 30;
        }

        public enum RestorationStatusTypes
        {
            Unassigned,
            Assigned,
            CompletedPendingApproval,
            Approved
        }

        #endregion

        #region Properties

        public virtual int Id { get; set; }

        public virtual IList<Document<Restoration>> Documents { get; set; }
        public virtual IList<Note<Restoration>> Notes { get; set; }

        public virtual IList<IDocumentLink> LinkedDocuments => Documents.Cast<IDocumentLink>().ToList();

        public virtual IList<INoteLink> LinkedNotes => Notes.Cast<INoteLink>().ToList();

        [DoesNotExport]
        public virtual string TableName => nameof(Restoration) + "s";

        public virtual Coordinate Coordinate
        {
            get => WorkOrder?.Coordinate;
            set { }
        }

        public virtual MapIcon Icon => WorkOrder?.Icon;

        #region Initial

        public virtual bool AcknowledgedByContractor { get; set; }

        [Multiline]
        public virtual string RestorationNotes { get; set; }

        [View("Type of Restoration")]
        public virtual RestorationType RestorationType { get; set; }

        public virtual WorkOrder WorkOrder { get; set; }

        /// <summary>
        /// This field will only ever have a value when the restoration has been created from the
        /// contractors portal.
        /// </summary>
        public virtual Contractor CreatedByContractor { get; set; }

        public virtual DateTime? CreatedByContractorAt { get; set; }

        public virtual Contractor AssignedContractor { get; set; }
        public virtual DateTime? AssignedContractorAt { get; set; }
        public virtual Town Town { get; set; }
        public virtual OperatingCenter OperatingCenter { get; set; }

        [View("WBS #")]
        public virtual string WBSNumber { get; set; }

        [View("Priority")]
        public virtual RestorationResponsePriority ResponsePriority { get; set; }

        [View("8\" Stab Base By Company Forces")]
        public virtual bool EightInchStabilizeBaseByCompanyForces { get; set; }

        [View(FormatStyle.Currency)]
        public virtual decimal? TotalAccruedCost { get; set; }

        public virtual bool? CompletedByOthers { get; set; }

        [Multiline]
        public virtual string CompletedByOthersNotes { get; set; }

        public virtual bool? TrafficControlRequired { get; set; }
        public virtual string InitialPurchaseOrderNumber { get; set; }

        // Don't use either of these properties if you don't have to specifically.
        // Use EstimatedRestorationFootage instead.
        [View(FormatStyle.DecimalMaxTwoDecimalPlacesWithTrailingZeroes)]
        public virtual decimal? PavingSquareFootage { get; set; }

        [View(FormatStyle.DecimalMaxTwoDecimalPlacesWithTrailingZeroes)]
        public virtual decimal? LinearFeetOfCurb { get; set; }

        /// <summary>
        /// Getting and setting this property will get/set either LinearFeetOfCurb or PavingSquareFootage
        /// depending on the measurement type. 
        /// </summary>
        [View(FormatStyle.DecimalMaxTwoDecimalPlacesWithTrailingZeroes)]
        public virtual decimal? EstimatedRestorationFootage
        {
            get
            {
                switch (MeasurementType)
                {
                    case RestorationMeasurementTypes.LinearFt:
                        return LinearFeetOfCurb;
                    case RestorationMeasurementTypes.SquareFt:
                        return PavingSquareFootage;
                    default:
                        throw new NotSupportedException(MeasurementType.ToString());
                }
            }
            set
            {
                // NOTE: The correct RestorationType/MeasurementType needs to be set on here
                //       in order for the correct property to be set.
                decimal? linFeetCurb = null,
                         pavingSqFoot = null;

                switch (MeasurementType)
                {
                    case RestorationMeasurementTypes.LinearFt:
                        linFeetCurb = value;
                        break;

                    case RestorationMeasurementTypes.SquareFt:
                        pavingSqFoot = value;
                        break;

                    default:
                        throw new NotImplementedException(MeasurementType.ToString());
                }

                LinearFeetOfCurb = linFeetCurb;
                PavingSquareFootage = pavingSqFoot;
            }
        }

        #endregion

        #region Reopened

        public virtual DateTime? DateReopened { get; set; }
        public virtual DateTime? DateRescheduled { get; set; }
        public virtual DateTime? DateRecompleted { get; set; }

        #endregion

        #region Partial

        [View("Invoice Number"), ExcelExportColumn(UsePropertyName = true)]
        public virtual string PartialRestorationInvoiceNumber { get; set; }

        [View("Restoration Date", FormatStyle.Date), ExcelExportColumn(UsePropertyName = true)]
        public virtual DateTime? PartialRestorationDate { get; set; }

        [View("Completed By"), ExcelExportColumn(UsePropertyName = true)]
        public virtual Contractor PartialRestorationCompletedBy { get; set; }

        [View("Traffic Control Cost"), ExcelExportColumn(UsePropertyName = true)]
        public virtual int? PartialRestorationTrafficControlCost { get; set; }

        [View("Traffic Control Invoice Number"), ExcelExportColumn(UsePropertyName = true)]
        public virtual string PartialRestorationTrafficControlInvoiceNumber { get; set; }

        [View(FormatStyle.Currency, ApplyFormatInEditMode = false)] // wot?
        public virtual decimal? PartialRestorationActualCost { get; set; }

        [View("Restoration Methods"), DoesNotExport]
        public virtual IList<RestorationMethod> PartialRestorationMethods { get; set; }

        [View("Partial Restoration Methods"), DoesNotExport] // This is needed for an index view column.
        public virtual string PartialRestorationMethodsDisplay => string.Join(", ", PartialRestorationMethods);

        [View("PO #")]
        public virtual string PartialRestorationPurchaseOrderNumber { get; set; }

        [Multiline, View("Restoration Notes"), ExcelExportColumn(UsePropertyName = true)]
        public virtual string PartialRestorationNotes { get; set; }

        [View("Priority Upcharge Type"), ExcelExportColumn(UsePropertyName = true)]
        public virtual RestorationPriorityUpchargeType PartialRestorationPriorityUpchargeType { get; set; }

        [View("Priority Upcharge", FormatStyle.Currency), ExcelExportColumn(UsePropertyName = true)]
        public virtual decimal? PartialRestorationPriorityUpcharge { get; set; }

        [View("Actual Footage"), ExcelExportColumn(UsePropertyName = true)]
        public virtual int? PartialPavingSquareFootage { get; set; }

        [View("Due Date", FormatStyle.Date), ExcelExportColumn(UsePropertyName = true)]
        public virtual DateTime? PartialRestorationDueDate { get; set; }

        [View("Approved On"), ExcelExportColumn(UsePropertyName = true)]
        public virtual DateTime? PartialRestorationApprovedAt { get; set; }

        [View("Breakout Billing"), Multiline, ExcelExportColumn(UsePropertyName = true)]
        public virtual string PartialRestorationBreakoutBilling { get; set; }

        #endregion

        #region Final

        [View("Invoice Number"), ExcelExportColumn(UsePropertyName = true)]
        public virtual string FinalRestorationInvoiceNumber { get; set; }

        [View("Restoration Date", FormatStyle.Date), ExcelExportColumn(UsePropertyName = true)]
        public virtual DateTime? FinalRestorationDate { get; set; }

        [View("Completed By"), ExcelExportColumn(UsePropertyName = true)]
        public virtual Contractor FinalRestorationCompletedBy { get; set; }

        [View("Traffic Control Cost"), ExcelExportColumn(UsePropertyName = true)]
        public virtual int? FinalRestorationTrafficControlCost { get; set; }

        [View("Traffic Control Invoice Number"), ExcelExportColumn(UsePropertyName = true)]
        public virtual string FinalRestorationTrafficControlInvoiceNumber { get; set; }

        [View(DisplayFormat = "{0:#######0.00}"), ExcelExportColumn(UsePropertyName = true)]
        public virtual decimal? FinalRestorationActualCost { get; set; }

        [View("Restoration Method"), DoesNotExport]
        public virtual IList<RestorationMethod> FinalRestorationMethods { get; set; }

        [Multiline, View("Restoration Notes"), ExcelExportColumn(UsePropertyName = true)]
        public virtual string FinalRestorationNotes { get; set; }

        [View("PO #"), ExcelExportColumn(UsePropertyName = true)]
        public virtual string FinalRestorationPurchaseOrderNumber { get; set; }

        [View("Restoration Methods"), DoesNotExport] // If it does export, it will be slow as hell.
        public virtual string FinalRestorationMethodDisplay => string.Join(", ", FinalRestorationMethods);

        [View("Priority Upcharge Type"), ExcelExportColumn(UsePropertyName = true)]
        public virtual RestorationPriorityUpchargeType FinalRestorationPriorityUpchargeType { get; set; }

        [View("Priority Upcharge", FormatStyle.Currency), ExcelExportColumn(UsePropertyName = true)]
        public virtual decimal? FinalRestorationPriorityUpcharge { get; set; }

        [View("Actual Footage"), ExcelExportColumn(UsePropertyName = true)]
        public virtual int? FinalPavingSquareFootage { get; set; }

        [View("Due Date", FormatStyle.Date), ExcelExportColumn(UsePropertyName = true)]
        public virtual DateTime? FinalRestorationDueDate { get; set; }

        [View("Approved On"), ExcelExportColumn(UsePropertyName = true)]
        public virtual DateTime? FinalRestorationApprovedAt { get; set; }

        #endregion

        #region Who knows

        // the shadown knows - jmd 2018-05-14

        //public virtual int? PartialPavingBreakOutEightInches { get; set; }
        //public virtual int? PartialPavingBreakOutTenInches { get; set; }
        //public virtual int? PartialSawCutting { get; set; }
        //public virtual int? DaysToPartialPaveHole { get; set; }
        //public virtual int? FinalPavingBreakOutEightInches { get; set; }
        //public virtual int? FinalPavingBreakOutTenInches { get; set; }
        //public virtual int? FinalSawCutting { get; set; }
        //public virtual int? DaysToFinalPaveHole { get; set; }
        //public virtual DateTime? DateApproved { get; set; }
        //public virtual int? ApprovedByID { get; set; }
        //public virtual int? RejectedByID { get; set; }
        //public virtual DateTime? DateRejected { get; set; }

        //public virtual bool SawCutByCompanyForces { get; set; }

        #endregion

        #region Logical

        private decimal RestorationSize
        {
            get
            {
                // 271 returned 0 if any part of this property would otherwise cause it
                // to return null. Why, though, is unknown. 
                var value = (RestorationType.MeasurementType == RestorationMeasurementTypes.LinearFt)
                    ? LinearFeetOfCurb
                    : PavingSquareFootage;

                return value.GetValueOrDefault(decimal.Zero);
            }
        }

        /// <summary>
        /// Gets the AccrualValue that is only used by the RestorationAccrualReport.
        /// </summary>
        [View(FormatStyle.Currency)]
        [DoesNotExport] // This is only needed for accrual report, and that'll need a custom export anyway.
        public virtual decimal? AccrualValue
        {
            get
            {
                if (PartialRestorationActualCost == null || PartialRestorationActualCost == 0)
                {
                    return TotalAccruedCost;
                }

                var typeCost = RestorationType.RestorationTypeCosts.SingleOrDefault(x => x.OperatingCenter == OperatingCenter);
                var restorationTypeFinalCost = typeCost?.FinalCost ?? 0;
                // Bug 3757 requested this be calculated as follows:
                if (PartialPavingSquareFootage <= 0)
                {
                    return RestorationSize * restorationTypeFinalCost;
                }
                else
                {
                    return PartialPavingSquareFootage * restorationTypeFinalCost;
                }
            }
        }

        public virtual RestorationMeasurementTypes MeasurementType => RestorationType.MeasurementType;

        public virtual IEnumerable<Restoration> RelatedRestorations
        {
            get
            {
                if (WorkOrder == null)
                {
                    return Enumerable.Empty<Restoration>();
                }

                return WorkOrder.Restorations.Where(x => x != this);
            }
        }

        // TODO: This will probably need to become a formula field.
        public virtual RestorationStatusTypes PartialRestorationStatus
        {
            get
            {
                if (!HasBeenAssignedToContractor)
                {
                    return RestorationStatusTypes.Unassigned;
                }

                if (!PartialRestorationDate.HasValue)
                {
                    return RestorationStatusTypes.Assigned;
                }

                if (!PartialRestorationApprovedAt.HasValue)
                {
                    return RestorationStatusTypes.CompletedPendingApproval;
                }

                return RestorationStatusTypes.Approved;
            }
        }

        public virtual bool HasBeenAssignedToContractor =>
            (AssignedContractor != null && AssignedContractorAt.HasValue);

        [View("Restoration Methods")]
        public virtual string PartialRestorationMethodDisplay => string.Join(", ", PartialRestorationMethods);

        #endregion

        #endregion

        #region Constructor

        public Restoration()
        {
            PartialRestorationMethods = new List<RestorationMethod>();
            FinalRestorationMethods = new List<RestorationMethod>();
        }

        #endregion

        #region Public Methods

        public virtual decimal CalculateAccruedCost()
        {
            var baseCost = EstimatedRestorationFootage.GetValueOrDefault(decimal.Zero);
            var multiplier = RestorationType.GetCostMultiplierForOperatingCenter(OperatingCenter);
            return baseCost * multiplier;
        }

        /// <summary>
        /// Assigns a contractor and sets the various due dates.
        /// </summary>
        public virtual void AssignContractor(Contractor contractor, DateTime dateAssigned)
        {
            // NOTE: A different contractor can be assigned after one is already set, the dates must be updated for this.

            // This method is not for unassigning a contractor.
            if (contractor == null)
            {
                throw new NotSupportedException("Can not assign a null contractor.");
            }

            if (RestorationType == null)
            {
                throw new InvalidOperationException("RestorationType must be set.");
            }

            AssignedContractor = contractor;
            AssignedContractorAt = dateAssigned;
            PartialRestorationDueDate = dateAssigned.AddDays(RestorationType.PartialRestorationDaysToComplete);
            FinalRestorationDueDate = dateAssigned.AddDays(RestorationType.FinalRestorationDaysToComplete);
        }

        #endregion
    }
}
