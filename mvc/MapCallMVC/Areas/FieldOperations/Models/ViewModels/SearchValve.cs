using System.ComponentModel;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.ViewModels;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels
{
    public class SearchValve : SearchValveBase<Valve>
    {
        private static TSearchTo MapSearchModel<TSearchTo, TSearchSet>(SearchValve search)
            where TSearchTo : SearchValveBase<TSearchSet>, new()
        {
            return new TSearchTo {
                IsRelatedAssetSearch = search.IsRelatedAssetSearch,
                HasSAPErrorCode = search.HasSAPErrorCode,
                SAPErrorCode = search.SAPErrorCode,
                HasOpenWorkOrder = search.HasOpenWorkOrder,
                EntityId = search.EntityId,
                HasImages = search.HasImages,
                LegacyId = search.LegacyId,
                OperatingCenter = search.OperatingCenter,
                Town = search.Town,
                Gradient = search.Gradient,
                ValveNumber = search.ValveNumber,
                WorkOrderNumber = search.WorkOrderNumber,
                StreetNumber = search.StreetNumber,
                Street = search.Street,
                CrossStreet = search.CrossStreet,
                TownSection = search.TownSection,
                DateInstalled = search.DateInstalled,
                MapPage = search.MapPage,
                Status = search.Status,
                ValveSize = search.ValveSize,
                ValveType = search.ValveType,
                ValveControls = search.ValveControls,
                ValveBilling = search.ValveBilling,
                ValveZone = search.ValveZone,
                UpdatedAt = search.UpdatedAt,
                CreatedAt = search.CreatedAt,
                LastInspectionDate = search.LastInspectionDate,
                DateRetired = search.DateRetired,
                FunctionalLocation = search.FunctionalLocation,
                FunctionalLocationDescription = search.FunctionalLocationDescription,
                Route = search.Route,
                Traffic = search.Traffic,
                RequiresInspection = search.RequiresInspection,
                RequiresBlowOffInspection = search.RequiresBlowOffInspection,
                IsLargeValve = search.IsLargeValve,
                Critical = search.Critical,
                SAPEquipmentId = search.SAPEquipmentId,
                NormalPosition = search.NormalPosition,
                InNormalPosition = search.InNormalPosition,
                GISUID = search.GISUID,
                WaterSystem = search.WaterSystem,
                State = search.State
            };
        }

        public static implicit operator SearchValveForMap(SearchValve search)
            => MapSearchModel<SearchValveForMap, ValveAssetCoordinate>(search);

        public static implicit operator SearchBlowOffForMap(SearchValve search)
            => MapSearchModel<SearchBlowOffForMap, BlowOffAssetCoordinate>(search);
    }

    public class SearchValveForMap : SearchValveBase<ValveAssetCoordinate>, ISearchValveForMap
    {
        #region Constants

        public const int MAX_MAP_RESULT_COUNT = 10000;
        
        #endregion
        
        #region Properties
        
        /// <remarks>
        /// Returns false and is not settable, because map coordinates shouldn't be paged.
        /// </remarks>
        public override bool EnablePaging
        {
            get => false;
            set { }
        }
        
        #endregion
    }

    public class SearchBlowOffForMap : SearchValveBase<BlowOffAssetCoordinate>, ISearchBlowOffForMap
    {
        #region Constants

        public const int MAX_MAP_RESULT_COUNT = 10000;
        
        #endregion
        
        #region Properties
        
        /// <remarks>
        /// Returns false and is not settable, because map coordinates shouldn't be paged.
        /// </remarks>
        public override bool EnablePaging
        {
            get => false;
            set { }
        }
        
        #endregion
    }

    public abstract class SearchValveBase<TSearchSet> : SearchSet<TSearchSet>
    {
        #region Properties

        [Search(CanMap = false)]
        public bool IsRelatedAssetSearch { get; set; }

        [Search(CanMap = false)]
        public bool? HasSAPErrorCode { get; set; }

        public string SAPErrorCode { get; set; }

        public bool? HasOpenWorkOrder { get; set; }

        // This is needed for AssetMap.
        public int? EntityId { get; set; }

        public bool? HasImages { get; set; }

        [View("Legacy ID")]
        public string LegacyId { get; set; }

        [DropDown("", "OperatingCenter", "ByStateIdForFieldServicesAssets", DependsOn = "State", DependentsRequired = DependentRequirement.None)]
        public int? OperatingCenter { get; set; }

        [DropDown("", "Town", "ByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Please select an operating center above")]
        public int? Town { get; set; }
        [DropDown("", "Gradient", "ByTownId", DependsOn = "Town", PromptText = "Please select a town above.")]
        public int? Gradient { get; set; }
        public SearchString ValveNumber { get; set; }

        [View(Valve.Display.WORK_ORDER_NUMBER)]
        public string WorkOrderNumber { get; set; }

        public string StreetNumber { get; set; }

        [DropDown("", "Street", "ByTownId", DependsOn = "Town", PromptText = "Please select a town above")]
        public int? Street { get; set; }

        [DropDown("", "Street", "ByTownId", DependsOn = "Town", PromptText = "Please select a town above")]
        public int? CrossStreet { get; set; }

        [DropDown("", "TownSection", "ByTownId", DependsOn = "Town", PromptText = "Please select a town above")]
        public int? TownSection { get; set; }

        public DateRange DateInstalled { get; set; }
        public string MapPage { get; set; }

        [DropDown]
        public int? Status { get; set; }

        [DropDown]
        public int? ValveSize { get; set; }

        [DropDown]
        public int? ValveType { get; set; }

        [DropDown]
        public int? ValveControls { get; set; }

        [DropDown, DisplayName(Valve.Display.VALVE_BILLING)]
        public int? ValveBilling { get; set; }

        [DropDown]
        public int? ValveZone { get; set; }

        public DateRange UpdatedAt { get; set; }
        public DateRange CreatedAt { get; set; }
        public DateRange LastInspectionDate { get; set; }
        public DateRange DateRetired { get; set; }

        [DropDown("FieldOperations", "FunctionalLocation", "ActiveByTownIdForValveAssetType", DependsOn = "Town,ValveControls", PromptText = "Please select a town above", DependentsRequired = DependentRequirement.One)]
        public int? FunctionalLocation { get; set; }

        [SearchAlias("FunctionalLocation", "fl", "Description")]
        public string FunctionalLocationDescription { get; set; }
        
        [DropDown("FieldOperations", "Valve", "RouteByOperatingCenterIdAndOrTownId", DependsOn = "OperatingCenter,Town", DependentsRequired = DependentRequirement.One, PromptText = "Please select an Operating Center or Town above")]
        public int? Route { get; set; }

        public bool? Traffic { get; set; }
        public bool? RequiresInspection { get; set; }
        public bool? RequiresBlowOffInspection { get; set; }

        [BoolFormat(Valve.Display.SIZE_RANGE_LARGE_VALVE, Valve.Display.SIZE_RANGE_SMALL_VALVE), DisplayName(Valve.Display.IS_LARGE_VALVE)]
        public bool? IsLargeValve { get; set; }

        [View(Valve.Display.CRITICAL)]
        public bool? Critical { get; set; }
        public int? SAPEquipmentId { get; set; }

        [DropDown]
        public int? NormalPosition { get; set; }

        public bool? InNormalPosition { get; set; }
        public SearchString GISUID { get; set; }
        
        [EntityMap, EntityMustExist(typeof(WaterSystem))]
        [MultiSelect("Admin", "WaterSystem", "ByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Select an operating center above")]
        public int[] WaterSystem { get; set; }

        public override string DefaultSortBy => "ValveSuffix";

        public override bool DefaultSortAscending => true;

        [DropDown, SearchAlias("Town", "State.Id")]
        public int? State { get; set; }

        #endregion
        
        #region Exposed Methods

        public override void ModifyValues(ISearchMapper mapper)
        {
            base.ModifyValues(mapper);
            if (HasSAPErrorCode.HasValue)
            {
                if (HasSAPErrorCode.Value)
                {
                    mapper.MappedProperties["SAPErrorCode"].Value = SearchMapperSpecialValues.IsNotNull;
                }
                else
                {
                    mapper.MappedProperties["SAPErrorCode"].Value = SearchMapperSpecialValues.IsNull;
                }
            }
        }

        #endregion
    }
}