using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;

namespace MapCallMVC.Areas.Environmental.Models.ViewModels.PublicWaterSupplyFirmCapacities
{
    public class SearchPublicWaterSupplyFirmCapacityViewModel : SearchSet<PublicWaterSupplyFirmCapacity>
    {
        #region Properties

        [MultiSelect]
        [SearchAlias("pws.State", "s", "Id", Required = true)]
        public int[] State { get; set; }

        [MultiSelect("", nameof(PublicWaterSupply), "ByStateId", DependsOn = nameof(State))]
        [EntityMap]
        [EntityMustExist(typeof(PublicWaterSupply))]
        [SearchAlias("PublicWaterSupply", "pws", "Id", Required = true)]
        public int[] PublicWaterSupply { get; set; }

        public NumericRange TotalCapacityFacilitySumMGD { get; set; }

        public DateRange UpdatedAt { get; set; }

        public override string DefaultSortBy => "s.Abbreviation";

        public override bool DefaultSortAscending => true;

        #endregion
    }
}
