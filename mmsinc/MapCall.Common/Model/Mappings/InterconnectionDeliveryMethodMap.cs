using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class InterconnectionDeliveryMethodMap : ClassMap<InterconnectionDeliveryMethod>
    {
        #region Constructors

        public InterconnectionDeliveryMethodMap()
        {
            Id(x => x.Id, "InterconnectionDeliveryMethodId").GeneratedBy.Identity();

            Map(x => x.Description).Not.Nullable().Length(InterconnectionDeliveryMethod.StringLengths.DESCRIPTION);

            HasMany(x => x.Interconnections).KeyColumn("DeliveryMethodId");
        }

        #endregion
    }
}
