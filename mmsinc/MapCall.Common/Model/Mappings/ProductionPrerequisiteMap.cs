using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class ProductionPrerequisiteMap : EntityLookupMap<ProductionPrerequisite>
    {
        public ProductionPrerequisiteMap()
        {
            Id(x => x.Id).GeneratedBy.Assigned();

            HasManyToMany(x => x.Equipment)
               .Table("EquipmentProductionPrerequisites")
               .ParentKeyColumn("ProductionPrerequisiteId")
               .ChildKeyColumn("EquipmentId")
               .Cascade.All();
        }
    }
}
