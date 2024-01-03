using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class MostRecentArcFlashStudyMap : ClassMap<MostRecentArcFlashStudy>
    {
        public MostRecentArcFlashStudyMap()
        {
            Table("MostRecentArcFlashStudies");
            ReadOnly();
            LazyLoad();
            Id(x => x.Id, "ArcFlashStudyId");
            References(x => x.Facility).Not.Nullable();
            Map(x => x.DateLabelsApplied);
            Map(x => x.ExpiringWithinAYear);

            // Need this so when SchemaExport doesn't create a table
            SchemaAction.None();
        }
    }
}
