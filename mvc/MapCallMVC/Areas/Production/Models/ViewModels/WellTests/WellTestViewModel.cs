using System;
using System.ComponentModel.DataAnnotations;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MMSINC.Utilities.ObjectMapping;
using StructureMap;
using System.Collections.Generic;
using System.Linq;

namespace MapCallMVC.Areas.Production.Models.ViewModels.WellTests
{
    public class WellTestViewModel : ViewModel<WellTest>
    {
        #region Fields

        private ProductionWorkOrder _productionWorkOrder;
        private Equipment _equipment;

        #endregion

        #region Properties

        [EntityMap,
         EntityMustExist(typeof(OperatingCenter))]
        public int? OperatingCenter { get; set; }

        [Secured,
         Required,
         EntityMap,
         EntityMustExist(typeof(Equipment))]
        public int? Equipment { get; set; }

        [Secured,
         Required,
         EntityMap, 
         EntityMustExist(typeof(ProductionWorkOrder))]
        public int? ProductionWorkOrder { get; set; }

        [DoesNotAutoMap("Display only")]
        public ProductionWorkOrder ProductionWorkOrderDisplay
        {
            get
            {
                if (_productionWorkOrder == null && ProductionWorkOrder.HasValue)
                {
                    _productionWorkOrder = _container.GetInstance<ProductionWorkOrderRepository>()
                                                     .Find(ProductionWorkOrder.GetValueOrDefault());
                }
                return _productionWorkOrder;
            }
            // This setter exists solely for a couple of unit tests because the unit tests
            internal set => _productionWorkOrder = value;
        }

        [DoesNotAutoMap("Display only")]
        public Equipment EquipmentDisplay
        {
            get
            {
                if (_equipment == null && Equipment.HasValue)
                {
                    _equipment = _container.GetInstance<EquipmentRepository>()
                                           .Find(Equipment.GetValueOrDefault());
                }
                return _equipment;
            }
            // This setter exists solely for a couple of unit tests because the unit tests
            internal set => _equipment = value;
        }

        public virtual string CompanySubsidiary => EquipmentDisplay?.Facility?.CompanySubsidiary?.Description;

        public virtual string WaterSystem => EquipmentDisplay?.Facility?.PublicWaterSupply?.Description;

        public virtual string WellName => EquipmentDisplay?.Description;

        [View(WellTest.DisplayNames.Equipment.WELL_DIAMETER_TOP)]
        public virtual string WellDiameterTop => EquipmentDisplay?.GetCharacteristicValue("DIAMETERTOP");

        [View(WellTest.DisplayNames.Equipment.WELL_DIAMETER_BOTTOM)]
        public virtual string WellDiameterBottom => EquipmentDisplay?.GetCharacteristicValue("DIAMETERBOTTOM");

        [View(WellTest.DisplayNames.Equipment.WELL_DEPTH)]
        public virtual string WellDepth => EquipmentDisplay?.GetCharacteristicValue("WELLDEPTH");

        [View(WellTest.DisplayNames.Equipment.PUMP_DEPTH)]
        public virtual string PumpDepth => EquipmentDisplay?.GetCharacteristicValue("PUMPDEPTH");

        public virtual string MethodOfMeasurement => EquipmentDisplay?.GetCharacteristicValue("METHOD_OF_MEASUREMENT");

        public virtual string IsWellVaulted => EquipmentDisplay?.GetCharacteristicValue("WELL_VAULTED");

        public virtual string WellType => EquipmentDisplay?.GetCharacteristicValue("WELL_TYPE");

        [View(WellTest.DisplayNames.Form.WELL_CAPACITY_RATING)]
        public virtual string WellCapacityRating => EquipmentDisplay?.GetCharacteristicValue("WELL_CAPACITY_RATING");

        [Required]
        public virtual DateTime? DateOfTest { get; set; }

        [Required,
         EntityMap,
         EntityMustExist(typeof(Employee)),
         DropDown("", "Employee", "GetByOperatingCenterId", DependsOn = nameof(OperatingCenter))]
        public int? Employee { get; set; }

        [Required,
         DropDown,
         EntityMap,
         EntityMustExist(typeof(WellTestGradeType))]
        public int? GradeType { get; set; }

        [Required,
         Range(WellTest.Ranges.PUMPING_RATE_MIN, WellTest.Ranges.PUMPING_RATE_MAX)]
        public virtual int? PumpingRate { get; set; }

        [Required,
         Range(WellTest.Ranges.MEASUREMENT_POINT_MIN, WellTest.Ranges.MEASUREMENT_POINT_MAX)]
        public virtual decimal? MeasurementPoint { get; set; }

        [Required, RegularExpression(WellTest.Validation.STATIC_WATER_LEVEL_REGEX,
             ErrorMessage = WellTest.Validation.STATIC_WATER_LEVEL_ERROR_MSG),
         CompareTo(nameof(PumpingWaterLevel), ComparisonType.LessThan, TypeCode.Decimal,
             ErrorMessage = WellTest.Validation.STATIC_WATER_LEVEL_GREATER_THAN_PUMPING_WATER_LEVEL_MSG)]
        public virtual decimal? StaticWaterLevel { get; set; }

        [Required,
         RegularExpression(WellTest.Validation.PUMPING_WATER_LEVEL_REGEX,
             ErrorMessage = WellTest.Validation.PUMPING_WATER_LEVEL_ERROR_MSG)]
        public virtual decimal? PumpingWaterLevel { get; set; }

        [AutoMap(MapDirections.ToViewModel)]
        public virtual decimal? DrawDown { get; set; }

        [AutoMap(MapDirections.ToViewModel)]
        public virtual decimal? SpecificCapacity { get; set; }

        #endregion

        #region Constructors

        public WellTestViewModel(IContainer container) : base(container) { }

        #endregion
    }
}