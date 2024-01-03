using MapCall.Common.Model.ViewModels;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;
using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels
{
    public class SearchNpdesRegulatorsDueInspectionBase<TSearchSet> : SearchSet<TSearchSet>
    {
        [Search(CanMap = false)]
        public int? SewerOpeningId { get; set; }

        [Required, DropDown, EntityMap, EntityMustExist(typeof(OperatingCenter))]
        [SearchAlias("OperatingCenter", "opc", "Id")]
        public int? OperatingCenter { get; set; }

        [EntityMap, EntityMustExist(typeof(Town))]
        [SearchAlias("Town", "town", "Id")]
        public int? Town { get; set; }

        [EntityMap, EntityMustExist(typeof(AssetStatus))]
        public int? Status { get; set; }

        [Search(CanMap = false)]
        public string NpdesPermitNumber { get; set; }

        [Search(CanMap = false)]
        public string SewerOpeningNumber { get; set; }

        [Search(CanMap = false)]
        public string BodyOfWater { get; set; }

        [Required]
        public RequiredDateRange DepartureDateTime { get; set; }
    }
    
    public class SearchNpdesRegulatorsDueInspection : SearchNpdesRegulatorsDueInspectionBase<SewerOpening>, ISearchNpdesRegulatorsDueInspectionItem
    {
        public static implicit operator SearchNpdesRegulatorsDueInspectionForMap(SearchNpdesRegulatorsDueInspection search)
        {
            return new SearchNpdesRegulatorsDueInspectionForMap {
                SewerOpeningId = search.SewerOpeningId,
                OperatingCenter = search.OperatingCenter,
                Town = search.Town,
                Status = search.Status,
                NpdesPermitNumber = search.NpdesPermitNumber,
                SewerOpeningNumber = search.SewerOpeningNumber,
                BodyOfWater = search.BodyOfWater,
                DepartureDateTime = search.DepartureDateTime
            };
        }
    }

    public class SearchNpdesRegulatorsDueInspectionForMap : SearchNpdesRegulatorsDueInspectionBase<SewerOpeningAssetCoordinate>, ISearchNpdesRegulatorsDueInspectionForMap
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
