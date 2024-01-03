using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class EmployeeProductionSkillSetMap : ClassMap<EmployeeProductionSkillSet>
    {
        public const string TABLE_NAME = "EmployeeProductionSkillSet";

        public EmployeeProductionSkillSetMap()
        {
            Table(TABLE_NAME);
            Id(x => x.Id);
            References(x => x.Employee).Not.Nullable();
            References(x => x.ProductionSkillSet).Not.Nullable();
        }
    }
}
