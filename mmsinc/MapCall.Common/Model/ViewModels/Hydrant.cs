using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Utilities;
using MMSINC.Utilities.Excel;

namespace MapCall.Common.Model.ViewModels
{
    #region Report Items

    public class AgedPendingAssetReportItem
    {
        public OperatingCenter OperatingCenter { get; set; }
        public string AssetType { get; set; }

        [DisplayName("0 - 90 Days")]
        public int ZeroToNinety { get; set; }

        [DisplayName("91 - 180 Days")]
        public int NinetyOneToOneEighty { get; set; }

        [DisplayName("181 - 360 Days")]
        public int OneEightyToThreeSixty { get; set; }

        [DisplayName("> 360 Days")]
        public int ThreeSixtyPlus { get; set; }

        public int Total { get; set; }

        [DoesNotExport]
        public int OperatingCenterId { get; set; }
    }

    public class ActiveHydrantDetailReportItem
    {
        #region Properties

        public string OperatingCenter { get; set; }
        public string Town { get; set; }
        public string HydrantBilling { get; set; }
        public string LateralSize { get; set; }
        public string HydrantSize { get; set; }
        public int Count { get; set; }

        #endregion
    }

    public class ActiveHydrantReportItem
    {
        #region Properties

        public string OperatingCenter { get; set; }
        public string HydrantBilling { get; set; }
        public int Count { get; set; }

        #endregion
    }

    public abstract class HydrantDueSomethingReportItem
    {
        #region Properties

        public string OperatingCenter { get; set; }

        [DoesNotExport]
        public int OperatingCenterId { get; set; }

        public string Town { get; set; }

        [DoesNotExport]
        public int TownId { get; set; }

        public int Count { get; set; }

        #endregion
    }

    public class HydrantDueInspectionReportItem : HydrantDueSomethingReportItem { }

    public class HydrantDuePaintingReportItem : HydrantDueSomethingReportItem { }

    public class HydrantRouteReportItem
    {
        public OperatingCenter OperatingCenter { get; set; }
        public Town Town { get; set; }
        public AssetStatus HydrantStatus { get; set; }
        public int Route { get; set; }
        public int Total { get; set; }
    }

    public class PublicHydrantCountReportItem
    {
        #region Properties

        public string OperatingCenter { get; set; }
        public string Town { get; set; }
        public string FireDistrict { get; set; }
        public string Status { get; set; }
        public string PremiseNumber { get; set; }
        public int Total { get; set; }

        #endregion
    }

    public class HydrantWorkOrdersByDescriptionReportItem
    {
        public int Id { get; set; }
        public OperatingCenter OperatingCenter { get; set; }
        public Town Town { get; set; }
        public string HydrantNumber { get; set; }
        public int HydrantSuffix { get; set; }
        public FireDistrict FireDistrict { get; set; }
        public string StreetNumber { get; set; }
        public Street Street { get; set; }
        public Street CrossStreet { get; set; }
        public HydrantManufacturer HydrantManufacturer { get; set; }
        public int? YearManufactured { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.DATE)]
        public DateTime? DateInstalled { get; set; }

        public AssetStatus HydrantStatus { get; set; }
        public HydrantBilling HydrantBilling { get; set; }
        public WorkDescription WorkDescription { get; set; }
    }

    #endregion

    /// <summary>
    /// View model for use with the hydrant notification template
    /// </summary>
    public class HydrantNotification
    {
        #region Properties

        public Hydrant Hydrant { get; set; }
        public int ActiveHydrantsOnPremise { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string RecordUrl { get; set; }

        #endregion
    }

    public interface
        ISearchHydrantWorkOrdersByDescriptionReportItem : ISearchSet<HydrantWorkOrdersByDescriptionReportItem>
    {
        int? OperatingCenter { get; set; }
        int? Town { get; set; }
        int? HydrantManufacturer { get; set; }
        IntRange YearManufactured { get; set; }
        bool? HasWorkOrder { get; set; }
        int[] HasWorkOrderWithWorkDescriptions { get; set; }
    }

    public interface ISearchHydrant : ISearchSet<Hydrant>
    {
        [Search(CanMap = false)] // Repository search method handles this.
        int? OpenWorkOrderWorkDescription { get; set; }
    }

    public interface ISearchHydrantForMap : ISearchSet<HydrantAssetCoordinate>
    {
        [Search(CanMap = false)] // Repository search method handles this.
        int? OpenWorkOrderWorkDescription { get; set; }
    }
}
