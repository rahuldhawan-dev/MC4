using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;

namespace MapCallMVC.Areas.HealthAndSafety.Models.ViewModels
{
    public class SearchGeneralLiabilityClaimActionItem : SearchSet<ActionItem<GeneralLiabilityClaim>>
    {
        #region Properties

        public int? Entity { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(State))]
        [SearchAlias("Entity.OperatingCenter", "State.Id", Required = true)]
        public virtual int? State { get; set; }

        [DropDown("", "OperatingCenter", "ByStateId", DependsOn = "State"), EntityMap, EntityMustExist(typeof(OperatingCenter))]
        [SearchAlias("Entity", "OperatingCenter.Id", Required = true)]
        public virtual int? OperatingCenter { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(ActionItemType))]
        [SearchAlias("ActionItem", "Type.Id")]
        public virtual int? Type { get; set; }

        [Search(CanMap = false)]
        public virtual bool? Completed { get; set; }

        [SearchAlias("ActionItem", "DateCompleted")]
        public DateRange DateCompleted { get; set; }

        #region Public Methods

        public override void ModifyValues(ISearchMapper mapper)
        {
            base.ModifyValues(mapper);

            if (Completed.HasValue)
            {
                mapper.MappedProperties["DateCompleted"].Value = Completed.Value
                    ? SearchMapperSpecialValues.IsNotNull
                    : SearchMapperSpecialValues.IsNull;
            }
        }

        #endregion

        #endregion
    }
}

