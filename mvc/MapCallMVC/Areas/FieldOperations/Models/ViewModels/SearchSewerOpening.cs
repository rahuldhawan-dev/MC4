using MapCall.Common.Model.Entities;
using MapCall.Common.Model.ViewModels;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels
{
    public class SearchSewerOpeningBase<TSearchSet> : SearchSet<TSearchSet>
    {
        #region Properties

        [DropDown]
        public int? OperatingCenter { get; set; }

        [DropDown("", "Town", "ByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Select an operating center above")]
        public int? Town { get; set; }

        [DropDown("Environmental", "WasteWaterSystem", "ByOperatingCenterAndTown", DependsOn = "OperatingCenter,Town", PromptText = "Select an operating center and town (optional)")]
        [View(MapCall.Common.Model.Entities.WasteWaterSystem.DisplayNames.WASTEWATER_SYSTEM)]
        public int? WasteWaterSystem { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(SewerOpeningType))]
        public int? SewerOpeningType { get; set; }

        [DropDown("", "TownSection", "ByTownId", DependsOn = "Town", PromptText = "Select a town above")]
        public int? TownSection { get; set; }

        public string StreetNumber { get; set; }

        [DropDown("", "Street", "ByTownId", DependsOn = "Town", PromptText = "Select a town above")]
        public int? Street { get; set; }

        [DropDown("", "Street", "ByTownId", DependsOn = "Town", PromptText = "Select a town above")]
        [View("Cross Street")]
        public int? IntersectingStreet { get; set; }

        [View("SAP Equipment")]
        public int? SAPEquipmentId { get; set; }

        [Search(CanMap = false)]
        public bool? HasSAPErrorCode { get; set; }

        [View(SewerOpening.Display.CRITICAL)]
        public bool? Critical { get; set; }
        public string SAPErrorCode { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(AssetStatus)), View("Opening Status")]
        public int? Status { get; set; }

        [View(SewerOpening.Display.TASK_NUMBER)]
        public string TaskNumber { get; set; }

        public SearchString OpeningNumber { get; set; }

        public int? OpeningSuffix { get; set; }

        [SearchAlias("FunctionalLocation", "fl", "Description")]
        public string FunctionalLocationDescription { get; set; }

        [DropDown("FieldOperations", "FunctionalLocation", "ByTownId", DependsOn = "Town", PromptText = "Select a town above")]
        public int? FunctionalLocation { get; set; }

        public bool? IsDoghouseOpening { get; set; }

        public DateRange DateInstalled { get; set; }

        [View("Date Added")]
        public DateRange CreatedAt { get; set; }

        [DropDown("FieldOperations", "SewerOpening", "RouteByTownId", DependsOn = "Town", PromptText = "Please select a town above")]
        public int? Route { get; set; }

        public SearchString GeoEFunctionalLocation { get; set; }

        public IntRange InspectionFrequency { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(RecurringFrequencyUnit))]
        public int? InspectionFrequencyUnit { get; set; }

        [View("Legacy ID")]
        public virtual string OldNumber { get; set; }
        public DateRange DateRetired { get; set; }

        [DropDown("", "BodyOfWater", "ByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Select an operating center above")]
        public int? BodyOfWater { get; set; }

        public SearchString OutfallNumber { get; set; }

        public SearchString LocationDescription { get; set; }

        #endregion

        #region Exposed Methods

        public override void ModifyValues(ISearchMapper mapper)
        {
            base.ModifyValues(mapper);

            if (!HasSAPErrorCode.HasValue)
            {
                return;
            }

            mapper.MappedProperties["SAPErrorCode"].Value = HasSAPErrorCode.Value ? SearchMapperSpecialValues.IsNotNull : SearchMapperSpecialValues.IsNull;
        }

        #endregion
    }

    public class SearchSewerOpening : SearchSewerOpeningBase<SewerOpening>
    {
        public static implicit operator SearchSewerOpeningForMap(SearchSewerOpening from)
        {
            return new SearchSewerOpeningForMap {
                BodyOfWater = from.BodyOfWater,
                CreatedAt = from.CreatedAt,
                Critical = from.Critical,
                DateInstalled = from.DateInstalled,
                DateRetired = from.DateRetired,
                FunctionalLocation = from.FunctionalLocation,
                FunctionalLocationDescription = from.FunctionalLocationDescription,
                GeoEFunctionalLocation = from.GeoEFunctionalLocation,
                HasSAPErrorCode = from.HasSAPErrorCode,
                InspectionFrequency = from.InspectionFrequency,
                IntersectingStreet = from.IntersectingStreet,
                IsDoghouseOpening = from.IsDoghouseOpening,
                LocationDescription = from.LocationDescription,
                OldNumber = from.OldNumber,
                OpeningNumber = from.OpeningNumber,
                OpeningSuffix = from.OpeningSuffix,
                OperatingCenter = from.OperatingCenter,
                OutfallNumber = from.OutfallNumber,
                Route = from.Route,
                SAPEquipmentId = from.SAPEquipmentId,
                SAPErrorCode = from.SAPErrorCode,
                SewerOpeningType = from.SewerOpeningType,
                Status = from.Status,
                Street = from.Street,
                StreetNumber = from.StreetNumber,
                TaskNumber = from.TaskNumber,
                Town = from.Town,
                TownSection = from.TownSection,
                WasteWaterSystem = from.WasteWaterSystem
            };
        }
    }

    public class SearchSewerOpeningForMap : SearchSewerOpeningBase<SewerOpeningAssetCoordinate>,
        ISearchSewerOpeningForMap
    {
        #region Consts

        public const int MAX_MAP_RESULT_COUNT = 10000;

        #endregion

        #region Properties

        /// <remarks>
        /// Returns false and is not settable, because map coordinates shouldn't be paged.
        /// </remarks>
        public override bool EnablePaging => false;

        #endregion
    }
}