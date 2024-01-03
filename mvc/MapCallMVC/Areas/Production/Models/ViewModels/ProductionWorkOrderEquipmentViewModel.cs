using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Authentication;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;
using DataType = System.ComponentModel.DataAnnotations.DataType;

namespace MapCallMVC.Areas.Production.Models.ViewModels
{
    public class ProductionWorkOrderEquipmentViewModel : ViewModel<ProductionWorkOrderEquipment>
    {
        #region Constructor
        
        public ProductionWorkOrderEquipmentViewModel(IContainer container) : base(container) { }

        #endregion

        #region Properties

        [DoesNotAutoMap]
        public int ProductionWorkOrderId { get; set; }

        [EntityMap, EntityMustExist(typeof(Equipment))]
        public int Equipment { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(AsFoundCondition))]
        [Required]
        public int? AsFoundCondition { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(AssetConditionReason))]
        [RequiredWhen(nameof(AsFoundCondition), ComparisonType.EqualTo, MapCall.Common.Model.Entities.AsFoundCondition.Indices.UNABLE_TO_INSPECT, FieldOnlyVisibleWhenRequired = true)]
        public int? AsFoundConditionReason { get; set; }

        [StringLength(ProductionWorkOrderEquipment.StringLengths.COMMENT)]
        [RequiredWhen(nameof(AsFoundCondition), ComparisonType.EqualToAny, new[] { MapCall.Common.Model.Entities.AsFoundCondition.Indices.SERIOUS_DETERIORATION, MapCall.Common.Model.Entities.AsFoundCondition.Indices.SOME_DETERIORATION, MapCall.Common.Model.Entities.AsFoundCondition.Indices.QUESTIONABLE }, FieldOnlyVisibleWhenRequired = true)]
        [DataType(DataType.MultilineText)]
        public string AsFoundConditionComment { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(AsLeftCondition))]
        [Required]
        public int? AsLeftCondition { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(AssetConditionReason))]
        [RequiredWhen(nameof(AsLeftCondition), ComparisonType.EqualTo, MapCall.Common.Model.Entities.AsLeftCondition.Indices.UNABLE_TO_INSPECT, FieldOnlyVisibleWhenRequired = true)]
        public int? AsLeftConditionReason { get; set; }

        [StringLength(ProductionWorkOrderEquipment.StringLengths.COMMENT)]
        [RequiredWhen(nameof(AsLeftCondition), ComparisonType.EqualToAny, new[] { MapCall.Common.Model.Entities.AsLeftCondition.Indices.NEEDS_RE_INSPECTION, MapCall.Common.Model.Entities.AsLeftCondition.Indices.NEEDS_RE_INSPECTION_SOONER_THAN_NORMAL }, FieldOnlyVisibleWhenRequired = true)]
        [DataType(DataType.MultilineText)]
        public string AsLeftConditionComment { get; set; }

        [StringLength(ProductionWorkOrderEquipment.StringLengths.COMMENT)]
        [RequiredWhen(nameof(AsLeftCondition), ComparisonType.EqualToAny, new[] { MapCall.Common.Model.Entities.AsLeftCondition.Indices.NEEDS_REPAIR, MapCall.Common.Model.Entities.AsLeftCondition.Indices.NEEDS_EMERGENCY_REPAIR }, FieldOnlyVisibleWhenRequired = true)]
        [DataType(DataType.MultilineText)]
        public string RepairComment { get; set; }

        [EntityMap, EntityMustExist(typeof(ProductionWorkOrderPriority))]
        public int? Priority { get; set; }

        #endregion

        #region Exposed Methods

        public override ProductionWorkOrderEquipment MapToEntity(ProductionWorkOrderEquipment entity)
        {
            entity = base.MapToEntity(entity);

            if (entity.AsLeftCondition != null)
            {
                switch (entity.AsLeftCondition.Id)
                {
                    case MapCall.Common.Model.Entities.AsLeftCondition.Indices.NEEDS_REPAIR:
                        entity.Priority = new ProductionWorkOrderPriority { Id = (int)ProductionWorkOrderPriority.Indices.HIGH };
                        break;
                    case MapCall.Common.Model.Entities.AsLeftCondition.Indices.NEEDS_EMERGENCY_REPAIR:
                        entity.Priority = new ProductionWorkOrderPriority { Id = (int)ProductionWorkOrderPriority.Indices.EMERGENCY };
                        break;
                }

                if (entity.ProductionWorkOrder != null &&
                    MapCall.Common.Model.Entities.AsLeftCondition.AUTO_CREATE_PRODUCTION_WORK_ORDER_STATUSES.Contains(entity.AsLeftCondition.Id))
                {
                    var user = _container.GetInstance<IAuthenticationService<User>>().CurrentUser;

                    entity.ProductionWorkOrder.RequestedBy = user.Employee;
                    entity.ProductionWorkOrder.OrderNotes = RepairComment;
                }
            }

            return entity;
        }

        #endregion
    }
}