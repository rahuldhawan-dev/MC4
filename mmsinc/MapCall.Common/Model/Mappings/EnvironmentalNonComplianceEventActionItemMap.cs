using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class EnvironmentalNonComplianceEventActionItemMap : ClassMap<EnvironmentalNonComplianceEventActionItem>
    {
        private const string IN_30_DAY_INTERVAL_FROM_TARGET_SQL = 
                                 "(CASE WHEN (TargetedCompletionDate is not null AND (datediff(day, TargetedCompletionDate, GetDate()) % 30) = 0) THEN 1 ELSE 0 END)",
                             IN_30_DAY_INTERVAL_FROM_TARGET_SQLITE =
                                 "(CASE WHEN (TargetedCompletionDate is not null AND cast(JulianDay(date(TargetedCompletionDate)) - JulianDay(date('now')) as int) % 30 = 0) THEN 1 ELSE 0 END)";

        public EnvironmentalNonComplianceEventActionItemMap()
        {
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity();

            References(x => x.EnvironmentalNonComplianceEvent).Not.Nullable();
            References(x => x.Type).Column("ActionItemTypeId").Not.Nullable();

            // EditEnvironmentalNonComplianceEventActionItem has this as a required
            // field, but the db has null values.
            References(x => x.ResponsibleOwner).Nullable();

            Map(x => x.NotListedType).Length(EnvironmentalNonComplianceEventActionItem.StringLengths.NOT_LISTED_TYPE).Nullable();
            Map(x => x.ActionItem).Length(EnvironmentalNonComplianceEventActionItem.StringLengths.ACTION_ITEM).Not.Nullable();
            Map(x => x.TargetedCompletionDate).Not.Nullable();
            Map(x => x.DateCompleted).Nullable();
            Map(x => x.In30DayIntervalFromTargetDate)
               .DbSpecificFormula(IN_30_DAY_INTERVAL_FROM_TARGET_SQL, IN_30_DAY_INTERVAL_FROM_TARGET_SQLITE);
        }
    }
}
