using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class UtilityTransformerKVARatingMap : EntityLookupMap<UtilityTransformerKVARating>
    {
        public UtilityTransformerKVARatingMap()
        {
            HasManyToMany(x => x.Voltages)
               .Table("UtilityTransformerKVARatingsVoltages")
               .ParentKeyColumn("UtilityTransformerKVARatingId")
               .ChildKeyColumn("VoltageId")
               .Cascade.All();
        }
    }
}
