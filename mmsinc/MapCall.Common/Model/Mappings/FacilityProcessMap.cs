using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class FacilityProcessMap : ClassMap<FacilityProcess>
    {
        #region Constants

        public const string TABLE_NAME = "FacilityProcesses";

        #endregion

        #region Constructors

        public FacilityProcessMap()
        {
            Table(TABLE_NAME);

            Id(x => x.Id);

            Map(x => x.FacilityProcessDescription).Nullable();

            References(x => x.Facility)
               .Not.Nullable();
            References(x => x.Process)
               .Not.Nullable();

            HasMany(x => x.FacilityProcessSteps)
               .KeyColumn("FacilityProcessId").Inverse().Cascade.AllDeleteOrphan();
            HasMany(x => x.FacilityProcessDocuments)
               .KeyColumn("LinkedId").Inverse().Cascade.None();
            HasMany(x => x.FacilityProcessNotes)
               .KeyColumn("LinkedId").LazyLoad().Inverse().Cascade.None();
            HasMany(x => x.Videos)
               .KeyColumn("LinkedId").LazyLoad().Inverse().Cascade.None();
        }

        #endregion
    }
}
