using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class InterconnectionOperatingStatusMap : ClassMap<InterconnectionOperatingStatus>
    {
        #region Constants

        public const string TABLE_NAME = "InterconnectionOperatingStatuses";

        #endregion

        #region Constructors

        public InterconnectionOperatingStatusMap()
        {
            Table(TABLE_NAME);
            Id(x => x.Id, "InterconnectionOperatingStatusId").GeneratedBy.Identity();
            Map(x => x.Description).Not.Nullable().Length(InterconnectionOperatingStatus.StringLengths.DESCRIPTION);
            HasMany(x => x.Interconnections).KeyColumn("OperatingStatusId");
        }

        #endregion
    }
}
