using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;

namespace MapCallMVC.Areas.Environmental.Models.ViewModels.EnvironmentalNonComplianceEvents
{
    public class SearchEnvironmentalNonComplianceEventActionItem : SearchSet<EnvironmentalNonComplianceEventActionItem>
    {
        #region Properties

        [DropDown, EntityMap, EntityMustExist(typeof(State))]
        [SearchAlias("EnvironmentalNonComplianceEvent", "State.Id")]
        public virtual int? State { get; set; }

        [DropDown("", "OperatingCenter", "ByStateId", DependsOn = "State"), EntityMap, EntityMustExist(typeof(OperatingCenter))]
        [SearchAlias("EnvironmentalNonComplianceEvent", "OperatingCenter.Id")]
        public virtual int? OperatingCenter { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(EnvironmentalNonComplianceEventActionItemType))]
        public virtual int? Type { get; set; }

        [Search(CanMap = false)]
        public virtual bool? Completed { get; set; }

        public DateRange DateCompleted { get; set; }

        #endregion

        #region Public Methods

        public override void ModifyValues(ISearchMapper mapper)
        {
            base.ModifyValues(mapper);

            if (Completed.HasValue)
            {
                if (Completed.Value)
                {
                    mapper.MappedProperties["DateCompleted"].Value = SearchMapperSpecialValues.IsNotNull;
                }
                else
                {
                    mapper.MappedProperties["DateCompleted"].Value = SearchMapperSpecialValues.IsNull;
                }
            }
        }

        #endregion
    }
}
