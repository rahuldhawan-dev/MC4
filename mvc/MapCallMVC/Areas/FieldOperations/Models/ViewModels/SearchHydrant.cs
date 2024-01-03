using System;
using System.ComponentModel;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.ViewModels;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels
{
    public class SearchHydrant : SearchHydrantBase<Hydrant>, ISearchHydrant
    {
        public static implicit operator SearchHydrantForMap(SearchHydrant search)
            => new SearchHydrantForMap {
                IsRelatedAssetSearch = search.IsRelatedAssetSearch,
                HasSAPErrorCode = search.HasSAPErrorCode,
                SAPErrorCode = search.SAPErrorCode,
                EntityId = search.EntityId,
                State = search.State,
                OperatingCenter = search.OperatingCenter,
                Town = search.Town,
                Gradient = search.Gradient,
                HydrantNumber = search.HydrantNumber,
                Status = search.Status,
                FireDistrict = search.FireDistrict,
                WorkOrderNumber = search.WorkOrderNumber,
                StreetNumber = search.StreetNumber,
                Street = search.Street,
                CrossStreet = search.CrossStreet,
                TownSection = search.TownSection,
                DateInstalled = search.DateInstalled,
                UpdatedAt = search.UpdatedAt,
                CreatedAt = search.CreatedAt,
                LastInspectionDate = search.LastInspectionDate,
                LastPaintedDate = search.LastPaintedDate,
                DateRetired = search.DateRetired,
                MapPage = search.MapPage,
                Zone = search.Zone,
                PaintingZone = search.PaintingZone,
                InspectionFrequency = search.InspectionFrequency,
                InspectionFrequencyUnit = search.InspectionFrequencyUnit,
                HydrantManufacturer = search.HydrantManufacturer,
                HydrantBilling = search.HydrantBilling,
                Route = search.Route,
                FunctionalLocation = search.FunctionalLocation,
                FunctionalLocationDescription = search.FunctionalLocationDescription,
                HydrantSuffix = search.HydrantSuffix,
                OutOfService = search.OutOfService,
                LegacyId = search.LegacyId,
                RequiresInspection = search.RequiresInspection,
                RequiresPainting = search.RequiresPainting,
                SAPEquipmentId = search.SAPEquipmentId,
                Critical = search.Critical,
                GISUID = search.GISUID,
                OpenWorkOrderWorkDescription = search.OpenWorkOrderWorkDescription,
                YearManufactured = search.YearManufactured,
                HasWorkOrder = search.HasWorkOrder,
                HasOpenWorkOrder = search.HasOpenWorkOrder,
                WaterSystem = search.WaterSystem,
                HydrantType = search.HydrantType,
                HydrantOutletConfiguration = search.HydrantOutletConfiguration
            };
    }

    public class SearchHydrantForMap : SearchHydrantBase<HydrantAssetCoordinate>, ISearchHydrantForMap
    {
        #region Consts

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

    public abstract class SearchHydrantBase<TSearchSet> : SearchSet<TSearchSet>
    {
        #region Private Fields

        private bool? _requiresInspection;
        private bool? _requiresPainting;

        #endregion

        #region Properties

        [Search(CanMap = false)]
        public bool IsRelatedAssetSearch { get; set; }

        [Search(CanMap = false)]
        public bool? HasSAPErrorCode { get; set; }

        public string SAPErrorCode { get; set; }

        // This is needed for AssetMap.
        [DisplayName("Id")]
        public int? EntityId { get; set; }

        [DropDown, SearchAlias("Town", "State.Id")]
        public int? State { get; set; }

        [DropDown("", "OperatingCenter", "ByStateIdForFieldServicesAssets", DependsOn = "State", DependentsRequired = DependentRequirement.None)]
        public int? OperatingCenter { get; set; }

        [DropDown("", "Town", "ByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Select an operating center above")]
        public int? Town { get; set; }
        [DropDown("", "Gradient", "ByTownId", DependsOn = "Town", PromptText = "Please select a town above.")]
        public int? Gradient { get; set; }
        public SearchString HydrantNumber { get; set; }

        [DropDown]
        public int? Status { get; set; }

        [DropDown("", "FireDistrict", "ByTownId", DependsOn = "Town", PromptText = "Select a town above")]
        public int? FireDistrict { get; set; }

        [View(Hydrant.Display.WORK_ORDER_NUMBER)]
        public string WorkOrderNumber { get; set; }

        public string StreetNumber { get; set; }

        [DropDown("", "Street", "ByTownId", DependsOn = "Town", PromptText = "Select a town above")]
        public int? Street { get; set; }

        [DropDown("", "Street", "ByTownId", DependsOn = "Town", PromptText = "Select a town above")]
        public int? CrossStreet { get; set; }

        [DropDown("", "TownSection", "ByTownId", DependsOn = "Town", PromptText = "Select a town above")]
        public int? TownSection { get; set; }

        public DateRange DateInstalled { get; set; }
        public DateRange UpdatedAt { get; set; }
        public DateRange CreatedAt { get; set; }
        [SearchAlias("HydrantDueInspection", "dueInspection", "LastInspectionDate")]
        public DateRange LastInspectionDate { get; set; }
        public DateRange DateRetired { get; set; }
        public string MapPage { get; set; }

        public int? Zone { get; set; }
        public int? PaintingZone { get; set; }

        public virtual int? InspectionFrequency { get; set; }
        [DropDown, EntityMustExist(typeof(RecurringFrequencyUnit))]
        public virtual int? InspectionFrequencyUnit { get; set; }

        [DropDown]
        public int? HydrantManufacturer { get; set; }

        [DropDown]
        public int? HydrantBilling { get; set; }

        [DropDown("FieldOperations", "Hydrant", "RouteByOperatingCenterIdAndOrTownId", DependsOn = "OperatingCenter,Town", DependentsRequired = DependentRequirement.One, PromptText = "Please select an Operating Center above")]
        public int? Route { get; set; }

        [DropDown("FieldOperations", "FunctionalLocation", "ByTownId", DependsOn = "Town", PromptText = "Select a town above")]
        public int? FunctionalLocation { get; set; }

        [SearchAlias("FunctionalLocation", "fl", "Description")]
        public string FunctionalLocationDescription { get; set; }

        public int? HydrantSuffix { get; set; }
        public bool? OutOfService { get; set; }

        [View("Legacy ID")]
        public string LegacyId { get; set; }

        // TODO: Why are RequiresInspection and RequiresPainting exclusive properties?
        // Jason mentions doing it in MC-1425's comments, but doesn't explain clearly
        // *why* it's done. Also, doing this in the setter is only working by pure luck.
        // If the model binder changes the order it sets the properties, then unexpected
        // results are going to occur. This should explicitly be done in the ModifyValues
        // override, or better, it should be enforced by validation. ie: Validate that only
        // one of these can have a value set.

        [SearchAlias("HydrantDueInspection", "dueInspection", "RequiresInspection")]
        public bool? RequiresInspection
        {
            get => _requiresInspection;
            set
            {
                if (value != null)
                {
                    RequiresPainting = null;
                }

                _requiresInspection = value;
            }
        }

        [SearchAlias("HydrantDuePainting", "duePainting", "RequiresPainting")]
        public bool? RequiresPainting
        {
            get => _requiresPainting;
            set
            {
                if (value != null)
                {
                    RequiresInspection = null;
                }

                _requiresPainting = value;
            }
        }
        
        [SearchAlias("HydrantDuePainting", "duePainting", "LastPaintedAt")]
        public DateRange LastPaintedDate { get; set; }

        public int? SAPEquipmentId { get; set; }

        [View(Hydrant.Display.CRITICAL)]
        public bool? Critical { get; set; }
        public SearchString GISUID { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(WorkDescription))]
        public int? OpenWorkOrderWorkDescription { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(HydrantType))]
        public int? HydrantType { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(HydrantOutletConfiguration))]
        public int? HydrantOutletConfiguration { get; set; }

        public override string DefaultSortBy =>
            RequiresInspection.GetValueOrDefault()
                ? "Route"
                // Bug 2625: Sort by suffix by default
                : "HydrantSuffix";

        public override bool DefaultSortAscending => true;

        public IntRange YearManufactured { get; set; }
        public bool? HasWorkOrder { get; set; }
        public bool? HasOpenWorkOrder { get; set; }

        [EntityMap, EntityMustExist(typeof(WaterSystem))]
        [MultiSelect("Admin", "WaterSystem", "ByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Select an operating center above")]
        public int[] WaterSystem { get; set; }

        #endregion

        #region Public Methods

        public override void ModifyValues(ISearchMapper mapper)
        {
            base.ModifyValues(mapper);
            
            if (RequiresInspection.GetValueOrDefault() && string.IsNullOrWhiteSpace(SortBy))
            {
                SortBy = "Route";
                SortAscending = true; // Needs to start from lowest number.
            }

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
