using System;
using System.Collections.Generic;
using System.ComponentModel;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels
{
    public class SearchMapImage : SearchSet<MapImage>
    {
        #region Properties

        [DisplayName("Id")]
        public int? EntityId { get; set; }

        [DropDown]
        [SearchAlias("Town", "T", "State.Id")]
        public int? State { get; set; }

        [SearchAlias("Town", "T", "County.Id")]
        [DropDown("", "County", "ByStateId", DependsOn = "State", PromptText = "Select a state above")]
        public int? County { get; set; }

        [DropDown("", "Town", "ByCountyId", DependsOn = "County", PromptText = "Select a county above")]
        public int? Town { get; set; }

        public string North { get; set; }
        public string South { get; set; }
        public string East { get; set; }
        public string West { get; set; }
        public string MapPage { get; set; }

        // This isn't a DateTime field.
        public string DateRevised { get; set; }

        #endregion
    }
}