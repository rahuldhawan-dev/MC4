using System;
using System.ComponentModel.DataAnnotations;
using MMSINC.Data;
using MMSINC.Utilities;

namespace MapCall.Common.Model.ViewModels
{
    public class ValveInspectionSearchResultViewModel
    {
        #region Properties

        public int Id { get; set; }
        public int ValveId { get; set; }
        public string ValveNumber { get; set; }
        public string OperatingCenter { get; set; }
        public string Town { get; set; }
        public string FunctionalLocation { get; set; }
        public int? SAPEquipmentId { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public DateTime DateInspected { get; set; }
        public bool Inspected { get; set; }
        public string PositionFound { get; set; }
        public string PositionLeft { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.DECIMAL_WITHOUT_TRAILING_ZEROS)]
        public decimal? Turns { get; set; }

        public string Remarks { get; set; }
        public string InspectedBy { get; set; }
        public string EmployeeId { get; set; }
        public DateTime DateAdded { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.DECIMAL_WITHOUT_TRAILING_ZEROS)]
        public decimal? ValveSize { get; set; }

        public string ValveZone { get; set; }
        public string SAPErrorCode { get; set; }
        public string SAPNotificationNumber { get; set; }

        #endregion
    }

    public interface ISearchValveInspection : ISearchSet<ValveInspectionSearchResultViewModel>
    {
        [SearchAlias("val.OperatingCenter", "opc", "Id")]
        int[] OperatingCenter { get; set; }

        [SearchAlias("val.Town", "town", "Id")]
        int? Town { get; set; }

        [SearchAlias("Valve", "val", "ValveSuffix")]
        int? ValveSuffix { get; set; }

        [SearchAlias("InspectedBy", "inspectedBy", "Id")]
        int? InspectedBy { get; set; }

        [SearchAlias("Valve", "val", "Route")]
        int? Route { get; set; }

        [Search(CanMap = false)]
        bool? WorkOrderRequired { get; set; }

        // Needed for ModifyValues and so that the search mapper
        // has a property for this. Not part of view.
        [SearchAlias("WorkOrderRequestOne", "wor1", "Id")]
        int? WorkOrderRequestOne { get; set; }

        [Search(CanMap = false)]
        bool? SAPEquipmentOnly { get; set; }

        // Needed for ModifyValues. Not part of view at the moment.
        [SearchAlias("Valve", "val", "SAPEquipmentId")]
        int? SAPEquipmentId { get; set; }

        [SearchAlias("val.ValveZone", "zone", "Id")]
        int? ValveZone { get; set; }
    }

    public interface IBaseSearchValveInspectionsByMonthReport
    {
        [SearchAlias("v.OperatingCenter", "oc", "Id")]
        int[] OperatingCenter { get; set; }

        [Search(CanMap = false)]
        int? Year { get; set; }
    }

    public interface ISearchValveInspectionsByMonthReport : IBaseSearchValveInspectionsByMonthReport,
        ISearchSet<ValveInspectionsByMonthReportItem> { }

    public interface ISearchRequiredValvesOperatedByMonthReport : IBaseSearchValveInspectionsByMonthReport,
        ISearchSet<RequiredValvesOperatedByMonthReportItem> { }

    public interface ISearchValvesOperatedByMonthReport : IBaseSearchValveInspectionsByMonthReport,
        ISearchSet<ValvesOperatedByMonthReportItem> { }

    #region Report Item Classes

    public class BaseValveInspectionsByMonthReportItem
    {
        public long TotalInspected { get; set; }
        public string SizeRange { get; set; }
        public string OperatingCenter { get; set; }
        public long Month { get; set; }
        public long Year { get; set; }
    }

    /// <summary>
    /// Start creating a complicated report here. 
    /// If the table needs to pivoted, you should still start here.
    /// Next is the Repository method to return these.
    /// After that the interface for the SearchSet
    /// </summary>
    public class ValveInspectionsByMonthReportItem : BaseValveInspectionsByMonthReportItem
    {
        public long TotalRequired { get; set; }
        public long TotalValves { get; set; }
        public long TotalDistinctValvesInspected { get; set; }
    }

    public class ValvesOperatedByMonthReportItem : BaseValveInspectionsByMonthReportItem
    {
        public virtual bool Operated { get; set; }
    }

    public class RequiredValvesOperatedByMonthReportItem : BaseValveInspectionsByMonthReportItem
    {
        public virtual bool Operated { get; set; }
    }

    #endregion

    #region Report Classes

    public class BaseValveInspectionsByMonthReport
    {
        public virtual string SizeRange { get; set; }
        public virtual string OperatingCenter { get; set; }

        public virtual long Year { get; set; }
        public virtual long Jan { get; set; }
        public virtual long Feb { get; set; }
        public virtual long Mar { get; set; }
        public virtual long Apr { get; set; }
        public virtual long May { get; set; }
        public virtual long Jun { get; set; }
        public virtual long Jul { get; set; }
        public virtual long Aug { get; set; }
        public virtual long Sep { get; set; }
        public virtual long Oct { get; set; }
        public virtual long Nov { get; set; }
        public virtual long Dec { get; set; }

        public virtual long Total
        {
            get { return Jan + Feb + Mar + Apr + May + Jun + Jul + Aug + Sep + Oct + Nov + Dec; }
        }
    }

    public class ValveInspectionsByMonthReport : BaseValveInspectionsByMonthReport
    {
        public virtual long TotalRequired { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.PERCENTAGE)]
        public virtual decimal? TotalPercentage => (decimal)Total / TotalRequired;
    }

    public class ValvesOperatedByMonthReport : BaseValveInspectionsByMonthReport
    {
        public virtual string Operated { get; set; }
    }

    public class RequiredValvesOperatedByMonthReport : BaseValveInspectionsByMonthReport
    {
        public virtual string Operated { get; set; }
    }

    #endregion
}
