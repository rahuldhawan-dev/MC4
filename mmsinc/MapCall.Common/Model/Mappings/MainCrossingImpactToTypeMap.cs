using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class MainCrossingImpactToTypeMap : EntityLookupMap<MainCrossingImpactToType>
    {
        public MainCrossingImpactToTypeMap()
        {
            HasManyToMany(x => x.MainCrossings)
               .Table("MainCrossingsImpactToTypes")
               .ParentKeyColumn("MainCrossingImpactToTypeId")
               .ChildKeyColumn("MainCrossingId")
               .Cascade.None();
        }
    }
}
