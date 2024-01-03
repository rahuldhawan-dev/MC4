using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class WaterConstituentStateLimitMap : ClassMap<WaterConstituentStateLimit>
    {
        public WaterConstituentStateLimitMap()
        {
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity();

            Map(x => x.Description).Length(255);
            Map(x => x.Agency).Length(255);
            Map(x => x.Min).Precision(53);
            Map(x => x.Max).Precision(53);
            Map(x => x.Mcl).Column("MCL").Precision(53);
            Map(x => x.Mclg).Column("MCLG").Precision(53);
            Map(x => x.Smcl).Column("SMCL").Precision(53);
            Map(x => x.ActionLimit).Length(255);
            Map(x => x.Regulation).Length(255);
            Map(x => x.StateDEPAnalyteCode);

            References(x => x.UnitOfMeasure).Nullable();
            References(x => x.State).Nullable();
            References(x => x.WaterConstituent).Not.Nullable();
        }
    }
}
