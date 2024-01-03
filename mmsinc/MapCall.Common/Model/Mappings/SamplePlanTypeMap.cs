using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Migrations._2016;

namespace MapCall.Common.Model.Mappings
{
    public class SamplePlanTypeMap : ClassMap<SamplePlanType>
    {
        #region Constructors

        public SamplePlanTypeMap()
        {
            Table(CreateWQSamplePlansTableForBug2918.TableNames.PLAN_TYPES);
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.Description).Not.Nullable().Length(50);
        }

        #endregion
    }
}
