using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class StateWorkDescriptionMap : ClassMap<StateWorkDescription>
    {
        #region Constants

        public const string TABLE_NAME = "StatesWorkDescriptions";

        #endregion

        #region Constructors

        public StateWorkDescriptionMap()
        {
            Table(TABLE_NAME);
            Id(x => x.Id).GeneratedBy.Identity();
            References(x => x.State, "StateId").Not.Nullable();
            References(x => x.WorkDescription, "WorkDescriptionId").Not.Nullable();
            References(x => x.PlantMaintenanceActivityType, "PlantMaintenanceActivityTypeId").Not.Nullable();
        }

        #endregion
    }
}
