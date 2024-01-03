using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class InterconnectionDirectionMap : ClassMap<InterconnectionDirection>
    {
        #region Constructors

        public InterconnectionDirectionMap()
        {
            Id(x => x.Id, "InterconnectionDirectionId").GeneratedBy.Identity();

            Map(x => x.Description).Not.Nullable().Length(InterconnectionDirection.StringLengths.DESCRIPTION);

            HasMany(x => x.Interconnections).KeyColumn("DirectionId");
        }

        #endregion
    }
}
