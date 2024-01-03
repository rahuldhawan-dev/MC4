using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class MeterMap : ClassMap<Meter>
    {
        #region Constructors

        public MeterMap()
        {
            Id(x => x.Id, "MeterId").GeneratedBy.Identity();

            References(x => x.Profile).Column("MeterProfileId");
            References(x => x.Status).Column("Status");

            Map(x => x.SerialNumber).Unique().Length(50);
            Map(x => x.OrcomEquipmentNumber).Length(50);
            Map(x => x.ERTNumber1LowOrOnly).Length(50);
            Map(x => x.ERTNumber2High).Length(50);
            Map(x => x.DatePurchased);
            Map(x => x.CreatedAt).Not.Nullable();
            Map(x => x.CreatedBy).Length(50);
            Map(x => x.PremiseID).Precision(10);
            Map(x => x.IsInterconnectMeter);

            HasManyToMany(x => x.Interconnections)
               .Table("InterconnectionsMeters")
               .ParentKeyColumn("MeterId")
               .ChildKeyColumn("InterconnectionId");
        }

        #endregion
    }
}
