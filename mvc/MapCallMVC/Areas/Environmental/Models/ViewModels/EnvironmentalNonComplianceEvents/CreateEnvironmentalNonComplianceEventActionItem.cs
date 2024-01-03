using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;
using System;
using System.ComponentModel.DataAnnotations;

namespace MapCallMVC.Areas.Environmental.Models.ViewModels.EnvironmentalNonComplianceEvents
{
    public class CreateEnvironmentalNonComplianceEventActionItem : ViewModel<EnvironmentalNonComplianceEvent>
    {
        #region Properties

        [DoesNotAutoMap]
        public int? OperatingCenter { get; set; }

        [DoesNotAutoMap]
        public int? OperatingCenterState { get; set; }

        [DropDown, Required, DoesNotAutoMap]
        public int? Type { get; set; }

        [DropDown("", "User", "GetActiveUsersByStateId", DependsOn = "OperatingCenterState"), DoesNotAutoMap, Required]
        public int? ResponsibleOwner { get; set; }

        [DoesNotAutoMap,
         StringLength(EnvironmentalNonComplianceEventActionItem.StringLengths.NOT_LISTED_TYPE),
         RequiredWhen("Type", ComparisonType.EqualTo, EnvironmentalNonComplianceEventActionItemType.Indices.NOT_LISTED)]
        public string NotListedType { get; set; }

        [Required, 
         DoesNotAutoMap, 
         StringLength(EnvironmentalNonComplianceEventActionItem.StringLengths.ACTION_ITEM)]
        public string ActionItem { get; set; }

        [Required, DoesNotAutoMap]

        public DateTime? TargetedCompletionDate { get; set; }

        [DoesNotAutoMap]
        public DateTime? DateCompleted { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(EnvironmentalNonComplianceEventCountsAgainstTarget))]
        public int? CountsAgainstTarget { get; set; }

        #endregion

        #region Constructors

        public CreateEnvironmentalNonComplianceEventActionItem(IContainer container) : base(container) { }

        #endregion

        #region Exposed Methods

        public override void Map(EnvironmentalNonComplianceEvent entity)
        {
            base.Map(entity);

            OperatingCenter = entity.OperatingCenter?.Id;
            OperatingCenterState = entity.OperatingCenter?.State?.Id;
        }

        public override EnvironmentalNonComplianceEvent MapToEntity(EnvironmentalNonComplianceEvent entity)
        {
            var type = _container.GetInstance<IRepository<EnvironmentalNonComplianceEventActionItemType>>()
                                 .Find(Type.Value);
            var responsibleOwner = ResponsibleOwner.HasValue
                ? _container.GetInstance<IRepository<User>>().Find(ResponsibleOwner.Value)
                : null;

            entity.ActionItems.Add(new EnvironmentalNonComplianceEventActionItem {
                EnvironmentalNonComplianceEvent = entity,
                Type = type,
                ResponsibleOwner = responsibleOwner,
                NotListedType = NotListedType,
                ActionItem = ActionItem,
                TargetedCompletionDate = TargetedCompletionDate.Value,
                DateCompleted = DateCompleted
            });

            return entity;
        }

        #endregion
    }
}
