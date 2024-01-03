using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class MarkoutRequirementMap : ClassMap<MarkoutRequirement>
    {
        protected int DescriptionLength => MarkoutRequirement.DESCRIPTION_MAX_LENGTH;

        protected string IdName => "MarkoutRequirementID";

        public MarkoutRequirementMap()
        {
            Id(x => x.Id, IdName).GeneratedBy.Assigned();

            Map(x => x.Description)
               .Length(DescriptionLength)
                // ReSharper restore DoNotCallOverridableMethodsInConstructor
               .Not.Nullable()
               .Unique();
        }
    }
}
