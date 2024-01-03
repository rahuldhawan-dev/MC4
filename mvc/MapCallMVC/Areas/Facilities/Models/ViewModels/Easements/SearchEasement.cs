using MapCall.Common.Model.Entities;
using MMSINC.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MMSINC.Metadata;
using MMSINC.Validation;

namespace MapCallMVC.Areas.Facilities.Models.ViewModels.Easements
{
    public class SearchEasement : SearchSet<Easement>
    {
        [DropDown, EntityMap, EntityMustExist(typeof(State))]
        public int? State { get; set; }

        [EntityMap, EntityMustExist(typeof(OperatingCenter))]
        [DropDown("", "OperatingCenter", "ByStateId", DependsOn = nameof(State), PromptText = "Please select a State.")]
        public int? OperatingCenter { get; set; }

        [EntityMap, EntityMustExist(typeof(Town))]
        [DropDown("Town", "ByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Please select an operating center", Area = "")]
        public int? Town { get; set; }

        [EntityMap, EntityMustExist(typeof(TownSection))]
        [DropDown("TownSection", "ActiveByTownId", DependsOn = "Town", PromptText = "Please select an town", Area = "")]
        public virtual int? TownSection { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(EasementCategory))]
        public int? Category { get; set; }

        public SearchString BlockLot { get; set; }
        [View(Easement.Display.WBS)]
        public SearchString Wbs { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(GrantorType))]
        public int? GrantorType { get; set; }
    }
}