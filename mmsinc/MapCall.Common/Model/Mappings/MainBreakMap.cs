using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class MainBreakMap : ClassMap<MainBreak>
    {
        public MainBreakMap()
        {
            Id(x => x.Id).GeneratedBy.Identity().Column("MainBreakID").Not.Nullable();

            References(x => x.WorkOrder).Not.Nullable();
            References(x => x.MainFailureType).Not.Nullable();
            References(x => x.MainBreakMaterial).Not.Nullable();
            References(x => x.MainCondition).Not.Nullable();
            References(x => x.MainBreakSoilCondition).Not.Nullable();
            References(x => x.MainBreakDisinfectionMethod).Not.Nullable();
            References(x => x.MainBreakFlushMethod).Not.Nullable();
            References(x => x.ServiceSize).Not.Nullable();
            References(x => x.ReplacedWith).Nullable();

            Map(x => x.Depth).Not.Nullable();
            Map(x => x.CustomersAffected).Not.Nullable();
            Map(x => x.ShutdownTime).Not.Nullable();

            // Required field, but db has tons of nulls.
            Map(x => x.ChlorineResidual).Nullable();
            Map(x => x.BoilAlertIssued).Not.Nullable();
            Map(x => x.FootageReplaced);
        }
    }
}
