using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class InterconnectionPurchaseSellTransferMap : ClassMap<InterconnectionPurchaseSellTransfer>
    {
        #region Constants

        public const string TABLE_NAME = "InterconnectionPurchaseSellTransfer";

        #endregion

        #region Constructors

        public InterconnectionPurchaseSellTransferMap()
        {
            Table(TABLE_NAME);
            Id(x => x.Id, "InterconnectionPurchaseSellTransferId").GeneratedBy.Identity();
            Map(x => x.Description).Not.Nullable()
                                   .Length(InterconnectionPurchaseSellTransfer.StringLenghts.DESCRIPTION);
            HasMany(x => x.Interconnections).KeyColumn("PurchaseSellTransferId");
        }

        #endregion
    }
}
