using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class VoltageMap : EntityLookupMap<Voltage>
    {
        public VoltageMap()
        {
            HasManyToMany(x => x.UtilityTransformerKVARatings)
               .Table("UtilityTransformerKVARatingsVoltages")
               .ParentKeyColumn("VoltageId")
               .ChildKeyColumn("UtilityTransformerKVARatingId")
               .Cascade.All();
        }
    }
}
