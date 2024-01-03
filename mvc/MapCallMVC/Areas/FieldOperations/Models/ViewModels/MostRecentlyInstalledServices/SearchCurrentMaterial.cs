using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.ViewModels;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels.MostRecentlyInstalledServices
{
    public class SearchCurrentMaterial
        : SearchCurrentMaterialBase<CurrentMaterialReportItem>, ISearchCurrentMaterial
    {
        public static implicit operator SearchCurrentMaterialForMap(SearchCurrentMaterial search)
            => new SearchCurrentMaterialForMap {
                State = search.State,
                OperatingCenter = search.OperatingCenter,
                Town = search.Town,
                ServiceCategory = search.ServiceCategory,
                ServiceInstallationPurpose = search.ServiceInstallationPurpose,
                ServiceMaterial = search.ServiceMaterial,
                ServiceSize = search.ServiceSize,
                CustomerSideMaterial = search.CustomerSideMaterial,
                CustomerSideSize = search.CustomerSideSize,
                DateInstalled = search.DateInstalled,
                UpdatedAt = search.UpdatedAt
            };
    }

    public class SearchCurrentMaterialForMap
        : SearchCurrentMaterialBase<CurrentMaterialCoordinate>, ISearchCurrentMaterialForMap
    {
        public const int MAX_MAP_RESULT_COUNT = 10000;

        /// <remarks>
        /// Returns false and is not settable, because map coordinates shouldn't be paged.
        /// </remarks>
        public override bool EnablePaging
        {
            get => false;
            set { }
        }
    }

    public abstract class SearchCurrentMaterialBase<TSearchSet> : SearchSet<TSearchSet>
    {
        public const string MATERIAL_REQUIRED =
            "Either company side or customer side material must be selected.";

        // NOTE: MC-5788 required premises in this report to be limited to these statuses, as returning
        //       "Killed"/"Non Converted" records were showing deactivated lead service lines and causing
        //       pain/confusion/missed KPIs etc.
        [SearchAlias("premise." + nameof(Premise.StatusCode), nameof(PremiseStatusCode.Id))]
        public int[] PremiseStatus => new[] {
            PremiseStatusCode.Indices.ACTIVE,
            PremiseStatusCode.Indices.INACTIVE
        };
        
        [MultiSelect, Required]
        [SearchAlias("service.State", "Id")]
        public virtual int[] State { get; set; }
        
        [MultiSelect("", "OperatingCenter", "ByStateIds", DependsOn = nameof(State))]
        [SearchAlias("service.OperatingCenter", "operatingCenter", "Id")]
        public virtual int[] OperatingCenter { get; set; }
        
        [MultiSelect("", "Town", "ByOperatingCenterIds", DependsOn = nameof(OperatingCenter))]
        [SearchAlias("service.Town", "town", "Id")]
        public virtual int[] Town { get; set; }
        
        [View(Service.DisplayNames.SERVICE_CATEGORY)]
        [DropDown, EntityMap, EntityMustExist(typeof(ServiceCategory))]
        [SearchAlias("service.ServiceCategory", "category", "Id")]
        public virtual int? ServiceCategory { get; set; }
        
        [View(Service.DisplayNames.SERVICE_INSTALLATION_PURPOSE)]
        [DropDown, EntityMap, EntityMustExist(typeof(ServiceInstallationPurpose))]
        [SearchAlias("service.ServiceInstallationPurpose", "purpose", "Id")]
        public virtual int? ServiceInstallationPurpose { get; set; }
        
        [View(Service.DisplayNames.SERVICE_MATERIAL)]
        [DropDown, EntityMap, EntityMustExist(typeof(ServiceMaterial))]
        [RequiredWhen(nameof(CustomerSideMaterial), null, ErrorMessage = MATERIAL_REQUIRED)]
        [SearchAlias("ServiceMaterial", "material", "Id")]
        public virtual int? ServiceMaterial { get; set; }
        
        [View(Service.DisplayNames.SERVICE_SIZE)]
        [DropDown, EntityMap, EntityMustExist(typeof(ServiceSize))]
        [SearchAlias("ServiceSize", "size", "Id")]
        public virtual int? ServiceSize { get; set; }
        
        [View("Customer Side Material at Tie-In")]
        [DropDown, EntityMap, EntityMustExist(typeof(ServiceMaterial))]
        [RequiredWhen(nameof(ServiceMaterial), null, ErrorMessage = MATERIAL_REQUIRED)]
        [SearchAlias("CustomerSideMaterial", "custMaterial", "Id")]
        public virtual int? CustomerSideMaterial { get; set; }
        
        [View("Customer Side Size at Tie-In")]
        [DropDown, EntityMap, EntityMustExist(typeof(ServiceSize))]
        [SearchAlias("ServiceSize", "custSize", "Id")]
        public virtual int? CustomerSideSize { get; set; }
        
        public virtual DateRange DateInstalled { get; set; }
        
        public virtual DateRange UpdatedAt { get; set; }
    }
}
