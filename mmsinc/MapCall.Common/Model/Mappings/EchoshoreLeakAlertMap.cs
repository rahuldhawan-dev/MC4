using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class EchoshoreLeakAlertMap : ClassMap<EchoshoreLeakAlert>
    {
        public EchoshoreLeakAlertMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();

            References(x => x.PointOfInterestStatus).Not.Nullable();
            References(x => x.Coordinate).Not.Nullable();
            References(x => x.Hydrant1).Nullable();
            References(x => x.Hydrant2).Nullable();
            References(x => x.EchoshoreSite).Not.Nullable();

            Map(x => x.PersistedCorrelatedNoiseId).Not.Nullable();
            Map(x => x.DatePCNCreated).Not.Nullable();
            Map(x => x.FieldInvestigationRecommendedOn).Nullable();
            Map(x => x.Hydrant1Text).Not.Nullable().Length(EchoshoreLeakAlert.StringLengths.SOCKET_ID_TEXT);
            Map(x => x.Hydrant2Text).Not.Nullable().Length(EchoshoreLeakAlert.StringLengths.SOCKET_ID_TEXT);
            Map(x => x.DistanceFromHydrant1).Not.Nullable();
            Map(x => x.DistanceFromHydrant2).Not.Nullable();
            Map(x => x.Note).Length(int.MaxValue).CustomSqlType("text").Nullable();

            HasMany(x => x.WorkOrders).KeyColumn("EchoshoreLeakAlertId");
        }
    }
}
