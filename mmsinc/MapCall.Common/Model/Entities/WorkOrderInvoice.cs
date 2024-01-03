using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Mappings;
using MMSINC.ClassExtensions.IListExtensions;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.Excel;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class WorkOrderInvoice : IEntity, IThingWithDocuments, IThingWithNotes
    {
        #region Constants

        public const decimal MATERIAL_MARKUP_RATE = 1.25m;

        #endregion

        #region Properties

        public virtual int Id { get; set; }
        public virtual WorkOrder WorkOrder { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.DATE)]
        public virtual DateTime? InvoiceDate { get; set; }

        public virtual bool IncludeMaterials { get; set; }

        [View(DisplayFormat = CommonStringFormats.DATE)]
        public virtual DateTime? SubmittedDate { get; set; }

        [View(DisplayFormat = CommonStringFormats.DATE)]
        public virtual DateTime? CanceledDate { get; set; }
        public virtual ScheduleOfValueType ScheduleOfValueType { get; set; }
        public virtual string InvoiceNotes { get; set; }

        #region References

        public virtual IList<WorkOrderInvoiceScheduleOfValue> WorkOrderInvoicesScheduleOfValues { get; set; }

        public virtual IList<ScheduleOfValue> ScheduleOfValues
        {
            get
            {
                return WorkOrderInvoicesScheduleOfValues
                      .Select(x => x.ScheduleOfValue).ToList();
            }
        }

        public virtual IList<WorkOrderInvoiceDocument> WorkOrderInvoiceDocuments { get; set; }
        public virtual IList<WorkOrderInvoiceNote> WorkOrderInvoiceNotes { get; set; }

        #endregion

        #region Logical Properties

        // Costs/Prices
        public virtual decimal TotalScheduleOfValuePrice =>
            (decimal)WorkOrderInvoicesScheduleOfValues.Sum(x => x.TotalPrice);

        public virtual decimal TotalMaterialPrice =>
            (IncludeMaterials && WorkOrder != null && WorkOrder.MaterialsUsed.Any())
                ? WorkOrder.TotalMaterialCost * MATERIAL_MARKUP_RATE
                : 0m;

        public virtual decimal TotalTrafficControlTicketPrice =>
            (WorkOrder != null && WorkOrder.TrafficControlTickets.Any())
                ? WorkOrder.TrafficControlTickets.Sum(x => x.TotalCharged).Value
                : 0m;

        public virtual decimal TotalInvoicePrice =>
            TotalScheduleOfValuePrice + TotalMaterialPrice + TotalTrafficControlTicketPrice;

        // Formula
        [View(DisplayName = "Status")]
        public virtual WorkOrderInvoiceStatus WorkOrderInvoiceStatus { get; set; }

        [DoesNotExport]
        public virtual string TableName => nameof(WorkOrderInvoice) + "s";

        public virtual IList<IDocumentLink> LinkedDocuments => WorkOrderInvoiceDocuments.Map(d => (IDocumentLink)d);
        public virtual IList<INoteLink> LinkedNotes => WorkOrderInvoiceNotes.Map(n => (INoteLink)n);

        public virtual bool ScheduleOfValuesMatchWorkOrderScheduleOfValues
        {
            get
            {
                var problems = false;
                // if we don't' have the same number of items with the same counts in each collection we have an issue
                // lets set this property to warn the user.
                if (WorkOrder == null)
                    return true;
                if (WorkOrder != null && WorkOrder.WorkOrdersScheduleOfValues.Count == 0)
                    return false;
                foreach (var wosov in WorkOrder.WorkOrdersScheduleOfValues)
                {
                    if (WorkOrderInvoicesScheduleOfValues.Count(
                            x => x.ScheduleOfValue == wosov.ScheduleOfValue && x.Total == wosov.Total)
                        !=
                        WorkOrder.WorkOrdersScheduleOfValues.Count(
                            x => x.ScheduleOfValue == wosov.ScheduleOfValue && x.Total == wosov.Total))
                        problems = true;
                }

                return !problems;
            }
        }

        #endregion

        #endregion

        #region Constructors

        public WorkOrderInvoice()
        {
            WorkOrderInvoicesScheduleOfValues = new List<WorkOrderInvoiceScheduleOfValue>();
            WorkOrderInvoiceDocuments = new List<WorkOrderInvoiceDocument>();
            WorkOrderInvoiceNotes = new List<WorkOrderInvoiceNote>();
        }

        #endregion
    }

    [Serializable]
    public class ScheduleOfValueType : EntityLookup { }

    [Serializable]
    public class ScheduleOfValueCategory : EntityLookup
    {
        public virtual ScheduleOfValueType ScheduleOfValueType { get; set; }

        public struct Indices
        {
            public const int LABOR = 21,
                             OTHER = 27,
                             ANOTHER_OTHER = 28,
                             EQUIPMENT = 22,
                             RESTORATION_MATERIALS = 23,
                             TEMP_ASPHALT = 24,
                             TEMP_SIDEWALK = 25,
                             PERMANENT_RESTORATION = 26; //27,//28
        }

        //This is in included in the _ActionBarHelp. IF you remove 
        //please update the action bar help to reflect the change.
        public static readonly int[] HAS_SUPERVISOR_MARKUP = {
            1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 27, 28
        };

        public static readonly int[] DOES_NOT_HAVE_SUPERVISOR_MARKUP = {
            Indices.LABOR,
            Indices.EQUIPMENT,
            Indices.RESTORATION_MATERIALS,
            Indices.TEMP_ASPHALT,
            Indices.TEMP_SIDEWALK,
            Indices.PERMANENT_RESTORATION
        };
    }

    [Serializable]
    public class ScheduleOfValue : EntityLookup
    {
        public virtual ScheduleOfValueCategory ScheduleOfValueCategory { get; set; }
        public virtual UnitOfMeasure UnitOfMeasure { get; set; }
        public virtual decimal? LaborUnitCost { get; set; }
        public virtual decimal? LaborUnitOvertimeCost { get; set; }
        public virtual decimal? MaterialCost { get; set; }
        public virtual decimal? MiscCost { get; set; }

        public override string ToString()
        {
            return Description;
        }
    }

    [Serializable]
    public class WorkOrderScheduleOfValue : IEntity
    {
        #region Constants

        public struct DisplayNames
        {
            public const string TOTAL = "Hours/Total";
        }

        #endregion

        public virtual int Id { get; set; }
        public virtual WorkOrder WorkOrder { get; set; }
        public virtual ScheduleOfValue ScheduleOfValue { get; set; }
        public virtual bool IsOvertime { get; set; }
        [View(DisplayNames.TOTAL)]
        public virtual decimal Total { get; set; }
        public virtual decimal LaborUnitCost { get; set; }
        public virtual string OtherDescription { get; set; }
    }

    [Serializable]
    public class WorkOrderInvoiceScheduleOfValue : IEntity
    {
        public const decimal MARKUP_COST = 0.15m, SUPERVISOR_COST = 0.10m;

        public virtual int Id { get; set; }
        public virtual WorkOrderInvoice WorkOrderInvoice { get; set; }
        public virtual ScheduleOfValue ScheduleOfValue { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.CURRENCY)]
        public virtual decimal? LaborUnitCost { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.CURRENCY)]
        public virtual decimal? MaterialCost { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.CURRENCY)]
        public virtual decimal? MiscCost { get; set; }

        public virtual decimal? Total { get; set; }
        public virtual bool? IsOvertime { get; set; }
        public virtual string OtherDescription { get; set; }
        public virtual bool IncludeWithInvoice { get; set; }
        public virtual bool IncludeMarkup { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.CURRENCY)]
        public virtual decimal? UnitPrice
        {
            get
            {
                var ret = (LaborUnitCost ?? 0) + (MaterialCost ?? 0);
                if (ScheduleOfValueCategory.HAS_SUPERVISOR_MARKUP.Contains(ScheduleOfValue.ScheduleOfValueCategory.Id))
                {
                    ret += ret * SUPERVISOR_COST;
                }

                if (IncludeMarkup)
                    ret += ret * MARKUP_COST;
                if (MiscCost.HasValue)
                    ret += MiscCost.Value;
                return Math.Round(ret, 2, MidpointRounding.AwayFromZero);
            }
        }

        [DisplayFormat(DataFormatString = CommonStringFormats.CURRENCY)]
        public virtual decimal? TotalPrice => (UnitPrice ?? 0) * (Total ?? 0);

        public virtual string Description =>
            (ScheduleOfValue.ScheduleOfValueCategory.Id == ScheduleOfValueCategory.Indices.OTHER ||
             ScheduleOfValue.ScheduleOfValueCategory.Id == ScheduleOfValueCategory.Indices.ANOTHER_OTHER)
                ? OtherDescription
                : ScheduleOfValue.Description;
    }

    [Serializable]
    public class WorkOrderInvoiceStatus : EntityLookup { }
}
