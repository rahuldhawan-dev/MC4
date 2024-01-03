using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;

namespace MapCallMVC.Areas.HealthAndSafety.Models.ViewModels
{
    public class SearchNearMissActionItem : SearchSet<ActionItem<NearMiss>>
    {
        #region Properties

        [SearchAlias("Entity", "ent", "Id", Required = true)]
        public virtual int? Entity { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(State))]
        [SearchAlias("oc.State", "st", "Id", Required = true)]
        public virtual int? State { get; set; }

        [DropDown("", "OperatingCenter", "ByStateId", DependsOn = "State"), EntityMap, EntityMustExist(typeof(OperatingCenter))]
        [SearchAlias("ent.OperatingCenter", "oc", "Id", Required = true)]
        public virtual int? OperatingCenter { get; set; }

        [SearchAlias("ActionItem", "ai", "Id", Required = true)]
        public virtual int? ActionItem { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(ActionItemType))]
        [SearchAlias("ai.Type", "type", "Id", Required = true)]
        public virtual int? Type { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(NearMissType))]
        [SearchAlias("ent.Type", "nearMissType", "Id")]
        public virtual int? NearMissType { get; set; }

        [Search(CanMap = false)]
        public virtual bool? Completed { get; set; }

        [SearchAlias("ActionItem", "ai", "DateCompleted")]
        public DateRange DateCompleted { get; set; }

        [DropDown("", "User", "GetActiveUsersByStateId", DependsOn = "State"), EntityMap, EntityMustExist(typeof(User))]
        [SearchAlias("ai.ResponsibleOwner", "ro", "Id", Required = true)]
        public virtual int? ResponsibleOwner { get; set; }

        [SearchAlias("ActionItem", "ai", "TargetedCompletionDate")]
        public DateRange TargetedCompletionDate { get; set; }

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
