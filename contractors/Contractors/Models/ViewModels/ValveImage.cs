using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;

namespace Contractors.Models.ViewModels
{
    public class SearchValveImage : SearchSet<ValveImage>
    {
        #region Properties

        [View("Id")]
        public int? EntityId { get; set; }

        public SearchString ValveNumber { get; set; }

        public string Location { get; set; }

        [DropDown]
        public int? OpenDirection { get; set; }

        public string NumberOfTurns { get; set; }

        [DropDown]
        public int? NormalPosition { get; set; }

        [DropDown]
        [SearchAlias("Town", "T", "State.Id")]
        public int? State { get; set; }

        [DropDown]
        public int? OperatingCenter { get; set; }

        [SearchAlias("Town", "T", "County.Id")]
        [DropDown("", "County", "ByStateId", DependsOn = "State", PromptText = "Select a state above")]
        public int? County { get; set; }

        [DropDown("", "Town", "ByCountyId", DependsOn = "County", PromptText = "Select a county above")]
        public int? Town { get; set; }
        public string TownSection { get; set; }

        public SearchString StreetNumber { get; set; }
        public string StreetPrefix { get; set; }
        public string Street { get; set; }
        public string StreetSuffix { get; set; }
        public string CrossStreet { get; set; }

        // Not a date field in the db
        public string DateCompleted { get; set; }
        public string ValveSize { get; set; }

        public bool? HasAsset { get; set; }
        public bool? HasValidValveNumber { get; set; }
        public bool? OfficeReviewRequired { get; set; }

        public DateRange CreatedAt { get; set; }
        #endregion

    }
}