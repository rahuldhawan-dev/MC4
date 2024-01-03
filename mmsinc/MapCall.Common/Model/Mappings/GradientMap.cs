using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class GradientMap : EntityLookupMap<Gradient>
    {
        public GradientMap()
        {
            HasManyToMany(x => x.Towns)
               .Table("GradientTowns")
               .ParentKeyColumn("GradientId")
               .ChildKeyColumn("TownId");
        }
    }
}
