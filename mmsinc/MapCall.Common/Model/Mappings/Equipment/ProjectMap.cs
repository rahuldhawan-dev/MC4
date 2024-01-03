using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class ProjectMap : ClassMap<Project>
    {
        public ProjectMap()
        {
            Id(x => x.Id, "ProjectId");

            Map(x => x.Name, "ProjName")
               .Length(Project.MAX_NAME_LENGTH)
               .Not.Nullable();

            HasMany(x => x.Sites);
        }
    }
}
