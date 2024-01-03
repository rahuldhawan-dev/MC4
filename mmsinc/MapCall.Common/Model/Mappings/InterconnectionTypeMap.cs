using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class InterconnectionTypeMap : ClassMap<InterconnectionType>
    {
        #region Constructors

        public InterconnectionTypeMap()
        {
            Id(x => x.Id, "InterconnectionTypeId").GeneratedBy.Identity();

            Map(x => x.Description).Not.Nullable().Length(InterconnectionType.StringLengths.DESCRIPTION);

            HasMany(x => x.Interconnections).KeyColumn("TypeId");
        }

        #endregion
    }
}
