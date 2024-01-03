using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Models.ViewModels
{
    public class BodyOfWaterViewModel : ViewModel<BodyOfWater>
    {
        [Required, DropDown(Area = "", Controller = "OperatingCenter", Action = "ByStateIds", DependsOn = nameof(State), PromptText = "Please select a state above.")]
        [EntityMap, EntityMustExist(typeof(OperatingCenter))]
        public int? OperatingCenter { get; set; }

        [Required, StringLength(BodyOfWater.StringLengths.DESCRIPTION)]
        public string Name { get; set; }

        [StringLength(BodyOfWater.StringLengths.CRITICAL_NOTES)]
        public string CriticalNotes { get; set; }

        public BodyOfWaterViewModel(IContainer container) : base(container) { }
    }
}