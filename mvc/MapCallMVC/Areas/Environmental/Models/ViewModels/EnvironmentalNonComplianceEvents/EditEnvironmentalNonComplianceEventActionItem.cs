using System;
using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.Environmental.Models.ViewModels.EnvironmentalNonComplianceEvents
{
    public class EditEnvironmentalNonComplianceEventActionItem : ViewModel<EnvironmentalNonComplianceEventActionItem>
    {
        #region Properties

        [DoesNotAutoMap]
        public int? OperatingCenter { get; set; }

        [DoesNotAutoMap]
        public int? EnvironmentalNonComplianceEventId { get; set; }

        [DoesNotAutoMap]
        public int? OperatingCenterState { get; set; }

        [DropDown, Required, EntityMap, EntityMustExist(typeof(EnvironmentalNonComplianceEventActionItemType))]
        public int? Type { get; set; }

        // Greg - 1/13/2020 - We are intentionally pulling active users here so we can't set an inactive user as the responsible owner
        [DropDown("", "User", "GetActiveUsersByStateId", DependsOn = "OperatingCenterState"), EntityMap, EntityMustExist(typeof(User)), Required]
        public int? ResponsibleOwner { get; set; }

        [StringLength(EnvironmentalNonComplianceEventActionItem.StringLengths.NOT_LISTED_TYPE),
         RequiredWhen("Type", ComparisonType.EqualTo, EnvironmentalNonComplianceEventActionItemType.Indices.NOT_LISTED)]
        public string NotListedType { get; set; }

        [Required,
         StringLength(EnvironmentalNonComplianceEventActionItem.StringLengths.ACTION_ITEM)]
        public string ActionItem { get; set; }

        [Required]
        public DateTime? TargetedCompletionDate { get; set; }

        public DateTime? DateCompleted { get; set; }

        public bool? In30DayIntervalFromTargetDate { get; set; }

        #endregion

        #region Constructors

        public EditEnvironmentalNonComplianceEventActionItem(IContainer container) : base(container) { }

        #endregion

        #region Exposed Methods

        public override void Map(EnvironmentalNonComplianceEventActionItem entity)
        {
            base.Map(entity);

            EnvironmentalNonComplianceEventId = entity.EnvironmentalNonComplianceEvent?.Id;
            OperatingCenter = entity.EnvironmentalNonComplianceEvent?.OperatingCenter?.Id;
            OperatingCenterState = entity.EnvironmentalNonComplianceEvent?.OperatingCenter?.State?.Id;
        }
        
        #endregion
    }
}
