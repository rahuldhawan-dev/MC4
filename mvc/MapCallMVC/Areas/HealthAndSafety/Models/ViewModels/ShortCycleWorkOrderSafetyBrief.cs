using System;
using System.Collections.Generic;
using StructureMap;
using System.Linq;
using System.Web;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using System.ComponentModel.DataAnnotations;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;

namespace MapCallMVC.Areas.HealthAndSafety.Models.ViewModels
{
    public abstract class ShortCycleWorkOrderSafetyBriefViewModel : ViewModel<ShortCycleWorkOrderSafetyBrief>
    {
        #region Properties

        [Required, EntityMap, EntityMustExist(typeof(Employee))]
        public int? FSR { get; set; }
        [DoesNotAutoMap("Need for Employee Name Display")]
        public string FSRName { get; set; }
        [Required, DateOnly]
        public DateTime? DateCompleted { get; set; }
        [Required]
        public bool? IsPPEInGoodCondition { get; set; }
        [Required]
        public bool? HasCompletedDailyStretchingRoutine { get; set; }
        [Required]
        public bool? HasPerformedInspectionOnVehicle { get; set; }
        [Required, MultiSelect, EntityMap, EntityMustExist(typeof(ShortCycleWorkOrderSafetyBriefLocationType))]
        public int[] LocationTypes { get; set; }
        [Required, MultiSelect, EntityMap, EntityMustExist(typeof(ShortCycleWorkOrderSafetyBriefHazardType))]
        public int[] HazardTypes { get; set; }
        [Required, MultiSelect, EntityMap, EntityMustExist(typeof(ShortCycleWorkOrderSafetyBriefPPEType))]
        public int[] PPETypes { get; set; }
        [Required, MultiSelect, EntityMap, EntityMustExist(typeof(ShortCycleWorkOrderSafetyBriefToolType))]
        public int[] ToolTypes { get; set; }
        
        #endregion

        #region Constructor

        public ShortCycleWorkOrderSafetyBriefViewModel(IContainer container) : base(container) { }

        #endregion
    }

    public class CreateShortCycleWorkOrderSafetyBrief : ShortCycleWorkOrderSafetyBriefViewModel
    {
        #region Constructor

        public CreateShortCycleWorkOrderSafetyBrief(IContainer container) : base(container) { }

        #endregion
    }

    public class SearchShortCycleWorkOrderSafetyBrief : SearchSet<ShortCycleWorkOrderSafetyBrief>
    {
        [DropDown, EntityMustExist(typeof(State)), EntityMap]
        [SearchAlias("e.OperatingCenter", "o", "State.Id")]
        public int? State { get; set; }
        [EntityMap, DropDown("", "OperatingCenter", "ByStateId", DependsOn = nameof(State)), EntityMustExist(typeof(OperatingCenter))]
        [SearchAlias("FSR", "e", "OperatingCenter.Id", Required = true)]
        public int? OperatingCenter { get; set; }
        [EntityMap, DropDown("", "Employee", "ActiveEmployeesByOperatingCenterId", DependsOn = nameof(OperatingCenter)), EntityMustExist(typeof(Employee))]
        public int? FSR { get; set; }

        public DateRange DateCompleted { get; set; }
    }
}