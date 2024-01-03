using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class InterconnectionFlowControlMethodMap : ClassMap<InterconnectionFlowControlMethod>
    {
        #region Constructors

        public InterconnectionFlowControlMethodMap()
        {
            Id(x => x.Id, "InterconnectionFlowControlMethodId").GeneratedBy.Identity();

            Map(x => x.Description).Not.Nullable().Length(InterconnectionFlowControlMethod.StringLengths.DESCRIPTION);

            HasMany(x => x.Interconnections).KeyColumn("FlowControlMethodId");
        }

        #endregion
    }
}
