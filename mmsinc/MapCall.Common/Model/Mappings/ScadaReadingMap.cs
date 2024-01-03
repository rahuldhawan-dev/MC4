using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class ScadaReadingMap : ClassMap<ScadaReading>
    {
        #region Constructors

        public ScadaReadingMap()
        {
            Table("ScadaReadings");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity();

            References(x => x.TagName).Not.Nullable();
            References(x => x.ScadaSignal)
               .Formula("(SELECT stn.ScadaSignalId FROM ScadaTagNames stn WHERE stn.Id = TagNameId)");

            Map(x => x.Timestamp).Not.Nullable();
            Map(x => x.Value).Not.Nullable().Precision(8).Scale(5);
        }

        #endregion
    }
}
