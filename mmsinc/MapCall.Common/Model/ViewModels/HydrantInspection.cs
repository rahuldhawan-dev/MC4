using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities;

namespace MapCall.Common.Model.ViewModels
{
    /// <summary>
    /// </summary>
    public class HydrantInspectionSearchResultViewModel
    {
        #region Properties

        public int Id { get; set; }
        public int HydrantId { get; set; }
        public string HydrantNumber { get; set; }

        public string OperatingCenter { get; set; }
        public string Town { get; set; }
        public string FunctionalLocation { get; set; }
        public int? SAPEquipmentId { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public DateTime DateInspected { get; set; }

        [View("Inspection Type")]
        public string HydrantInspectionType { get; set; }

        public bool? FullFlow { get; set; }
        public int? GallonsFlowed { get; set; }
        public decimal? GPM { get; set; }
        public decimal? MinutesFlowed { get; set; }
        public decimal? StaticPressure { get; set; }

        [View("Pre Residual/Free Chlorine")]
        public decimal? PreResidualChlorine { get; set; }

        [View("Post Residual/Free Chlorine")]
        public decimal? ResidualChlorine { get; set; }

        [View("Pre Total Chlorine")]
        public decimal? PreTotalChlorine { get; set; }

        [View("Post Total Chlorine")]
        public decimal? TotalChlorine { get; set; }

        public string WorkOrderRequestOne { get; set; }
        public string Remarks { get; set; }
        public string InspectedBy { get; set; }
        public string HydrantTagStatus { get; set; }
        public DateTime? DateAdded { get; set; }
        public string SAPErrorCode { get; set; }
        public string SAPNotificationNumber { get; set; }
        public string FreeNoReadReason { get; set; }
        public string TotalNoReadReason { get; set; }

        #endregion
    }

    public interface ISearchHydrantInspection : ISearchSet<HydrantInspectionSearchResultViewModel>
    {
        #region Abstract Properties

        [SearchAlias("hyd.OperatingCenter", "opc", "Id")]
        int[] OperatingCenter { get; set; }

        [SearchAlias("hyd.Town", "town", "Id")]
        int? Town { get; set; }

        [SearchAlias("hyd.FireDistrict", "fd", "Id")]
        int? FireDistrict { get; set; }

        [SearchAlias("Hydrant", "hyd", "HydrantSuffix")]
        int? HydrantSuffix { get; set; }

        [SearchAlias("HydrantInspectionType", "hit", "Id")]
        int? HydrantInspectionType { get; set; }

        [SearchAlias("Hydrant", "hyd", "Route")]
        int? Route { get; set; }

        [SearchAlias("InspectedBy", "inspectedBy", "Id")]
        int? InspectedBy { get; set; }

        [SearchAlias("HydrantTagStatus", "tagStat", "Id")]
        int? HydrantTagStatus { get; set; }

        // TODO: DateRange props are missing. 

        [Search(CanMap = false)]
        bool? WorkOrderRequired { get; set; }

        // Needed for ModifyValues and so that the search mapper
        // has a property for this. Not part of view.
        [SearchAlias("WorkOrderRequestOne", "wor1", "Id")]
        int? WorkOrderRequestOne { get; set; }

        [Search(CanMap = false)]
        bool? SAPEquipmentOnly { get; set; }

        // Needed for ModifyValues. Not part of view at the moment.
        [SearchAlias("Hydrant", "hyd", "SAPEquipmentId")]
        int? SAPEquipmentId { get; set; }

        [SearchAlias("FreeNoReadReason", "freeNoRead", "Id")]
        int? FreeNoReadReason { get; set; }

        [SearchAlias("TotalNoReadReason", "totalNoRead", "Id")]
        int? TotalNoReadReason { get; set; }

        #endregion
    }

    public class HydrantInspectionWorkOrderReportItem
    {
        #region Properties

        public int Id { get; set; }
        public string HydrantNumber { get; set; }
        public int HydrantId { get; set; }
        public string Manufacturer { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.DATE)]
        public DateTime? Date { get; set; }

        [DisplayName("Street #")]
        public string StreetNumber { get; set; }

        public string Street { get; set; }
        public string CrossStreet { get; set; }
        public string Town { get; set; }
        public string TownSection { get; set; }
        public string InspectedBy { get; set; }
        public string InspectionType { get; set; }
        public string WorkOrderRequestOne { get; set; }
        public string WorkOrderRequestTwo { get; set; }
        public string WorkOrderRequestThree { get; set; }
        public string WorkOrderRequestFour { get; set; }

        [Multiline]
        public string Remarks { get; set; }

        #endregion
    }

    public interface ISearchHydrantInspectionWorkOrder : ISearchSet<HydrantInspectionWorkOrderReportItem>
    {
        #region Abstract Properties

        [SearchAlias("hyd.OperatingCenter", "opc", "Id")]
        int? OperatingCenter { get; set; }

        [SearchAlias("hyd.Town", "town", "Id")]
        int? Town { get; set; }

        #endregion

        // TODO: Move Range/DateRange to MMSINC.Core so InspectionDate property can be added properly. 
    }

    public class HydrantFlushingReportItem
    {
        #region Properties

        public int Month { get; set; }
        public string BusinessUnit { get; set; }
        public decimal TotalGallons { get; set; }

        #endregion
    }

    public interface ISearchHydrantFlushingReport : ISearchSet<HydrantFlushingReportItem>
    {
        #region Abstract Properties

        [SearchAlias("hyd.OperatingCenter", "opc", "Id")]
        int? OperatingCenter { get; set; }

        [Search(CanMap = false)]
        int? Year { get; set; }

        #endregion
    }

    #region KPI Hydrants Inspected Report classes

    /// <summary>
    ///     This is the model for setting up the report with only month/year column
    /// </summary>
    public class KPIHydrantsInspectedReportItem
    {
        #region Properties

        public int MonthCompleted { get; set; }
        public int MonthTotal { get; set; }
        public int Year { get; set; }
        public string OperatingCenter { get; set; }
        public string HydrantInspectionType { get; set; }

        #endregion
    }

    /// <summary>
    ///     This is the report model with a column for each month
    /// </summary>
    public class KPIHydrantsInspectedReport : MonthlyReportViewModel
    {
        #region Properties

        public virtual string OperatingCenter { get; set; }
        public virtual string HydrantInspectionType { get; set; }

        public virtual int? TotalRequired { get; set; }

        //[DisplayName("Total Distinct Inspections")]
        //public virtual int? TotalDistinct { get; set; }
        [DisplayFormat(DataFormatString = CommonStringFormats.PERCENTAGE)]
        [DisplayName("% Completed")]
        public virtual decimal? Completed { get; set; }

        #endregion
    }

    public interface ISearchKPIHydrantInspectionReport : ISearchSet<KPIHydrantsInspectedReportItem>
    {
        #region Abstract Properties

        [SearchAlias("hyd.OperatingCenter", "opc", "Id")]
        int[] OperatingCenter { get; set; }

        [Search(CanMap = false)]
        int? Year { get; set; }

        #endregion
    }

    #endregion
}
