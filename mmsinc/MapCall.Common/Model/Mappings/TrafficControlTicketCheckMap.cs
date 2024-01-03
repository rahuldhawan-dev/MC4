using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class TrafficControlTicketCheckMap : ClassMap<TrafficControlTicketCheck>
    {
        public TrafficControlTicketCheckMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();

            References(x => x.TrafficControlTicket).Not.Nullable();

            Map(x => x.Amount).Not.Nullable();
            Map(x => x.CheckNumber).Not.Nullable();
            Map(x => x.Memo).Nullable();
            Map(x => x.Reconciled).Nullable();
            Map(x => x.Unique)
               .DbSpecificFormula(
                    "(CASE WHEN ((SELECT COUNT(1) FROM TrafficControlTicketChecks tctc1 where tctc1.CheckNumber = CheckNumber) = 1) THEN 1 ELSE 0 END)");
        }
    }
}
