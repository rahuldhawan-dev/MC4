using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class ProductionWorkOrderProductionPrerequisiteMap : ClassMap<ProductionWorkOrderProductionPrerequisite>
    {
        public const string TABLE_NAME = "ProductionWorkOrdersProductionPrerequisites";

        public ProductionWorkOrderProductionPrerequisiteMap()
        {
            Table(TABLE_NAME);

            Id(x => x.Id);

            References(x => x.ProductionWorkOrder).Not.Nullable();
            References(x => x.ProductionPrerequisite).Not.Nullable();

            Map(x => x.SatisfiedOn).Nullable();
            Map(x => x.SkipRequirement).Not.Nullable();
            Map(x => x.SkipRequirementComments).Nullable();
        }
    }
}
