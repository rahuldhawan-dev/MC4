using System;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;

namespace Contractors.Models.ViewModels
{
    public class SearchAsBuiltImage : SearchSet<AsBuiltImage>
    {
        #region Properties

        [View("Id")]
        public int? EntityId { get; set; }

        public virtual DateRange CoordinatesModifiedOn { get; set; }

        [View(AsBuiltImage.Display.TASK_NUMBER)]
        public string TaskNumber { get; set; }

        [DropDown]
        [SearchAlias("Town", "T", "State.Id")]
        public int? State { get; set; }

        [SearchAlias("Town", "T", "County.Id")]
        [DropDown("", "County", "ByStateId", DependsOn = "State", PromptText = "Select a state above")]
        public int? County { get; set; }

        [DropDown("", "Town", "ByCountyId", DependsOn = "County", PromptText = "Select a county above")]
        public int? Town { get; set; }

        [DropDown("", "TownSection", "ByTownId", DependsOn = "Town", PromptText = "Select a town above")]
        public int? TownSection { get; set; }
        // NOTE: Town does not filter on OperatingCenter because not all AsBuilts have their OperatingCenters set.
        [DropDown]
        public int? OperatingCenter { get; set; }

        public string ProjectName { get; set; }
        public string StreetPrefix { get; set; }
        public string Street { get; set; }
        public string StreetSuffix { get; set; }
        public string CrossStreet { get; set; }
        public string MapPage { get; set; }
        public DateRange DateInstalled { get; set; }
        public DateRange CreatedAt { get; set; }
        public DateRange PhysicalInService { get; set; }
        public bool? OfficeReviewRequired { get; set; }

        #endregion
    }
}