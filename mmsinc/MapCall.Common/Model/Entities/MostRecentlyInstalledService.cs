using System;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.ViewModels;
using MMSINC.Data;
using MMSINC.Utilities.Excel;

namespace MapCall.Common.Model.Entities
{
    /// <summary>
    /// Represents the installation data of the most recently installed <see cref="Service"/> associated
    /// with a <see cref="Premise"/>.  The logic behind this is encapsulated in the sql view
    /// [MostRecentlyInstalledServicesView].
    /// </summary>
    [Serializable]
    public class MostRecentlyInstalledService : IThingWithShadow, IThingWithCoordinate
    {
        // this is actually `Premise.Id`
        public virtual int Id { get; set; }

        public virtual Premise Premise { get; set; }
        public virtual Service Service { get; set; }
        
        public virtual DateTime DateInstalled { get; set; }
        public virtual DateTime UpdatedAt { get; set; }
        public virtual User UpdatedBy
        {
            get => null;
            set { }
        }
        
        public virtual ServiceSize MeterSettingSize { get; set; }
        
        public virtual ServiceMaterial ServiceMaterial { get; set; }
        public virtual ServiceSize ServiceSize { get; set; }
        
        public virtual ServiceMaterial CustomerSideMaterial { get; set; }
        public virtual ServiceSize CustomerSideSize { get; set; }
        
        public virtual Coordinate Coordinate { get; set; }
        public virtual MapIcon Icon => Coordinate.Icon;
        
        public virtual User ServiceMaterialSetBy { get; set; }
        public virtual User CustomerMaterialSetBy { get; set; }

        public static object ToJSONObject(MostRecentlyInstalledService s)
        {
            return new {
                PremiseId = s.Premise.Id,
                ServiceId = s.Service.Id,
                ServiceMaterialId = s.ServiceMaterial?.Id,
                ServiceSizeId = s.ServiceSize?.Id,
                CustomerSideMaterialId = s.CustomerSideMaterial?.Id,
                CustomerSideSizeId = s.CustomerSideSize?.Id
            };
        }
    }

    public interface ISearchCurrentMaterial : ISearchSet<CurrentMaterialReportItem> { }
    
    public interface ISearchCurrentMaterialForMap : ISearchSet<CurrentMaterialCoordinate> { }

    public class CurrentMaterialReportItem
    {
        public virtual int ServiceId { get; set; }
        public virtual int PremiseId { get; set; }
        [DoesNotExport]
        public virtual string OperatingCenterCode { get; set; }
        [DoesNotExport]
        public virtual string OperatingCenterName { get; set; }
        public virtual string OperatingCenter =>
            $"{OperatingCenterCode} - {OperatingCenterName}";
        public virtual string Town { get; set; }
        public virtual string StreetNumber { get; set; }
        public virtual string Street { get; set; }
        public virtual string PremiseNumber { get; set; }
        public virtual string InstallationNumber { get; set; }
        public virtual string ServiceSize { get; set; }
        public virtual string ServiceMaterial { get; set; }
        public virtual string CustomerSideSize { get; set; }
        public virtual string CustomerSideMaterial { get; set; }
        public virtual DateTime InstallDate { get; set; }
        public virtual DateTime? UpdatedAt { get; set; }
        public virtual string ServiceType { get; set; }
        public virtual string ServiceCategory { get; set; }
        public virtual string PurposeOfInstallation { get; set; }
    }
}
