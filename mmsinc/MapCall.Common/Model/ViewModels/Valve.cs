using MapCall.Common.Model.Entities;
using MMSINC.Data;

namespace MapCall.Common.Model.ViewModels
{
    public class ValveBPUReportItem
    {
        #region Properties

        public string OperatingCenter { get; set; }
        public string ValveBilling { get; set; }
        public string ValveStatus { get; set; }
        public string SizeRange { get; set; }
        public int Total { get; set; }

        #endregion
    }

    public class ValveRouteReportItem
    {
        public OperatingCenter OperatingCenter { get; set; }
        public Town Town { get; set; }
        public AssetStatus ValveStatus { get; set; }
        public int Route { get; set; }
        public int Total { get; set; }
    }

    public class ValveNotification
    {
        #region Properties

        public Valve Valve { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string RecordUrl { get; set; }

        #endregion
    }

    public class MunicipalValveZoneReportItem
    {
        #region Properties

        public string OperatingCenter { get; set; }
        public string Town { get; set; }
        public int SmallValves { get; set; }
        public int LargeValves { get; set; }
        public string SmallValveZone { get; set; }
        public string LargeValveZone { get; set; }

        #endregion
    }

    public class ValveDueInspectionReportItem
    {
        #region Properties

        public string OperatingCenter { get; set; }
        public int OperatingCenterId { get; set; }
        public string Town { get; set; }
        public int TownId { get; set; }
        public int Count { get; set; }

        #endregion
    }
    
    public interface ISearchValveForMap : ISearchSet<ValveAssetCoordinate> { }
    public interface ISearchBlowOffForMap : ISearchSet<BlowOffAssetCoordinate> { }
}
