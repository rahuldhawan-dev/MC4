using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class StreetMap : ClassMap<Street>
    {
        #region Constructors

        public StreetMap()
        {
            Id(x => x.Id, "StreetID");

            // This is nullable in the database, but that's stupid.
            Map(x => x.FullStName)
               .Length(Street.StringLengths.FULL_ST_NAME)
               .Not.Nullable();
            // This is also nullable in the database, but that's stupid.
            Map(x => x.Name, "StreetName")
               .Length(Street.StringLengths.NAME)
               .Not.Nullable();
            Map(x => x.IsActive).Not.Nullable().Default("true");

            References(x => x.Prefix);
            References(x => x.Suffix);
            References(x => x.Town);
        }

        #endregion
    }
}
