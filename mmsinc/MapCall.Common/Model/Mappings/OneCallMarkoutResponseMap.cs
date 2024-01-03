using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class OneCallMarkoutResponseMap : ClassMap<OneCallMarkoutResponse>
    {
        public OneCallMarkoutResponseMap()
        {
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity();

            References(x => x.OneCallMarkoutTicket).Not.Nullable();
            References(x => x.CompletedBy).Not.Nullable();
            References(x => x.OneCallMarkoutResponseStatus).Nullable();
            References(x => x.OneCallMarkoutResponseTechnique).Nullable();

            Map(x => x.CompletedAt).Not.Nullable();
            Map(x => x.Comments).Nullable();
            Map(x => x.ReqNotified).Nullable();
            Map(x => x.Paint).Nullable();
            Map(x => x.Flag).Nullable();
            Map(x => x.Stake).Nullable();
            Map(x => x.Over500Feet).Nullable();
            Map(x => x.CrewMarkoutIsNeeded).Nullable();
            Map(x => x.NumberOfCsmo).Column("NumberOfCSMO").Nullable();
            Map(x => x.NumberOfCsmoUnableToLocate).Column("NumberOfCSMOUnableToLocate").Nullable();
            Map(x => x.TotalTimeSpentForCsmo).Column("TotalTimeSpentForCSMO").Nullable();
        }
    }
}
