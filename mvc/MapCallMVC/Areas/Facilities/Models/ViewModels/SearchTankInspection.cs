using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;
using IContainer = StructureMap.IContainer;

namespace MapCallMVC.Areas.Facilities.Models.ViewModels
{
    public class SearchTankInspection : SearchSet<TankInspection>
    {
        #region Properties

        [DropDown, EntityMap, EntityMustExist(typeof(TankInspectionType))]
        [View(TankInspection.Display.TANK_INSPECTION_TYPE)]
        public virtual int? TankInspectionType { get; set; }
        [DisplayName(TankInspection.Display.OBSERVATION_ID)]
        public virtual int? Id { get; set; }
        [DropDown, EntityMap, EntityMustExist(typeof(State))]
        [SearchAlias("OperatingCenter", "State", "State.Id")]
        public virtual int? State { get; set; }
        [DropDown("", "OperatingCenter", "ByStateId", DependsOn = "State"), EntityMap, EntityMustExist(typeof(OperatingCenter))]
        public virtual int? OperatingCenter { get; set; }
        [View(TankInspection.Display.OBSERVATION_DATE)]
        public DateRange ObservationDate { get; set; }
        [DropDown("", "Equipment", "ByOperatingCenterOnlyPotableWaterTanks", DependsOn = nameof(OperatingCenter))]
        [EntityMap, EntityMustExist(typeof(Equipment))]
        public int? Equipment { get; set; }
        [EntityMap, DropDown("", "Employee", "ActiveEmployeesByOperatingCenterId", DependsOn = nameof(OperatingCenter)), EntityMustExist(typeof(Employee))]
        public int? TankObservedBy { get; set; }

        #endregion
    }
}
