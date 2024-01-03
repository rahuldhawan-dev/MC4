using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class ScadaTagNameMap : ClassMap<ScadaTagName>
    {
        #region Constructors

        public ScadaTagNameMap()
        {
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity();

            Map(x => x.TagName).Not.Nullable().Length(100);
            Map(x => x.Description).Nullable().Length(200);
            Map(x => x.Units).Nullable().Length(25);
            Map(x => x.Inactive).Not.Nullable().Default("false");
        }

        #endregion
    }
}
