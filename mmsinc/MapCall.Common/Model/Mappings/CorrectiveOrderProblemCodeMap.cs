using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class CorrectiveOrderProblemCodeMap : ClassMap<CorrectiveOrderProblemCode>
    {
        public CorrectiveOrderProblemCodeMap()
        {
            Id(x => x.Id);

            Map(x => x.Code).Not.Nullable();
            Map(x => x.Description).Not.Nullable();

            HasManyToMany(x => x.EquipmentTypes)
               .Table("CorrectiveOrderProblemCodesEquipmentTypes")
               .ParentKeyColumn("CorrectiveOrderProblemCodeId")
               .ChildKeyColumn("EquipmentTypeId");
        }
    }
}
