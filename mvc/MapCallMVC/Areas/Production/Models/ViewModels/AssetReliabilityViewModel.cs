using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MMSINC.Authentication;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.Production.Models.ViewModels
{
    public class AssetReliabilityViewModel : ViewModel<AssetReliability>
    {
        #region Private fields

        protected ProductionWorkOrder _productionWorkOrder;
        protected Equipment _equipment;

        #endregion
        public AssetReliabilityViewModel(IContainer container) : base(container) { }

        #region Properties

        [EntityMap, EntityMustExist(typeof(ProductionWorkOrder))]
        public int? ProductionWorkOrder { get; set; }
        [EntityMap, EntityMustExist(typeof(Equipment))]
        [View("Equipment ID")]
        public int? Equipment { get; set; }
        [EntityMap, EntityMustExist(typeof(Employee))]
        public int? Employee { get; set; }
        [Required, EntityMap, DropDown, EntityMustExist(typeof(AssetReliabilityTechnologyUsedType))]
        public int? AssetReliabilityTechnologyUsedType { get; set; }
        [RequiredWhen(nameof(AssetReliabilityTechnologyUsedType), ComparisonType.EqualTo, MapCall.Common.Model.Entities.AssetReliabilityTechnologyUsedType.Indices.OTHER, FieldOnlyVisibleWhenRequired = true)]
        [StringLength(AssetReliability.StringLengths.NOTE_LENGTH)]
        public string TechnologyUsedNote { get; set; }
        public DateTime? DateTimeEntered { get; set; }
        [Required]
        public int? RepairCostNotAllowedToFail { get; set; }
        [Required]
        public int? RepairCostAllowedToFail { get; set; }
        public int? CostAvoidance { get; set; }
        [StringLength(AssetReliability.StringLengths.NOTE_LENGTH)]
        [Required]
        public string CostAvoidanceNote { get; set; }

        #region Display Properties

        [DoesNotAutoMap("Display")]
        public string WorkOrder => _productionWorkOrder != null ? _productionWorkOrder.ToString() : string.Empty;
        [DoesNotAutoMap("Display")]
        public string WorkDescription => _productionWorkOrder != null ? _productionWorkOrder.ProductionWorkDescription.ToString() : string.Empty;
        [DoesNotAutoMap("Display")]
        public string WorkOrderNotes => _productionWorkOrder != null ? _productionWorkOrder.OrderNotes : string.Empty;
        [DoesNotAutoMap("Display")]
        public string EquipmentDescription => _equipment != null ? _equipment.Description : string.Empty;

        #endregion

        #endregion

        public override AssetReliability MapToEntity(AssetReliability entity)
        {
            base.MapToEntity(entity);
            entity.Employee = _container.GetInstance<IAuthenticationService<User>>().CurrentUser.Employee;
            entity.DateTimeEntered = _container.GetInstance<IDateTimeProvider>().GetCurrentDate();
            entity.CostAvoidance = RepairCostAllowedToFail.Value - RepairCostNotAllowedToFail.Value;
            return entity;
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            _productionWorkOrder = ProductionWorkOrder != null ? _container.GetInstance<IProductionWorkOrderRepository>().Find(ProductionWorkOrder.Value) : null;
            _equipment = Equipment != null ? _container.GetInstance<IEquipmentRepository>().Find(Equipment.Value) : null;
        }
    }

    public class CreateAssetReliability : AssetReliabilityViewModel
    {
        public CreateAssetReliability(IContainer container) : base(container) { }
    }

    public class EditAssetReliability : AssetReliabilityViewModel
    {
        public EditAssetReliability(IContainer container) : base(container) { }

        public override void Map(AssetReliability entity)
        {
            base.Map(entity);
            _productionWorkOrder = ProductionWorkOrder != null ? _container.GetInstance<IProductionWorkOrderRepository>().Find(ProductionWorkOrder.Value) : null;
            _equipment = Equipment != null ? _container.GetInstance<IEquipmentRepository>().Find(Equipment.Value) : null;
        }
    }

    public class SearchAssetReliability : SearchSet<AssetReliability>
    {
        [DropDown("", "OperatingCenter", "ByStateId", DependsOn = nameof(State)), EntityMustExist(typeof(OperatingCenter))]
        [SearchAlias("ProductionWorkOrder", "OperatingCenter.Id", Required = true)]
        public int? OperatingCenter { get; set; }
        
        [DropDown, EntityMustExist(typeof(State)), EntityMap]
        [SearchAlias("ProductionWorkOrder.OperatingCenter", "state", "State.Id")]
        public int? State { get; set;}
        
        [DropDown("", nameof(Facility), "GetActiveByOperatingCenterId", DependsOn = nameof(OperatingCenter)), EntityMustExist(typeof(Facility))]
        [SearchAlias("ProductionWorkOrder.Facility", "Id")]
        public int? Facility { get; set; }
        
        [DropDown, EntityMustExist(typeof(Equipment))]
        [SearchAlias("Equipment", "Id", Required = true)]
        public int? Equipment { get; set; }
        
        [DropDown, EntityMustExist(typeof(Employee))]
        public int? Employee { get; set;}
        
        [DropDown, EntityMustExist(typeof(EquipmentGroup)), EntityMap]
        [SearchAlias("Equipment.EquipmentType", "EquipmentGroup.Id")]
        public int? EquipmentGroup { get; set; }
    }
}