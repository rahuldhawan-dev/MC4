using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class TaskGroupCategoryMap : ClassMap<TaskGroupCategory>
    {
        #region Constructors

        public TaskGroupCategoryMap()
        {
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity();

            Map(x => x.Description).Length(TaskGroupCategory.StringLengths.DESCRIPTION).Not.Nullable();
            Map(x => x.Type).Length(TaskGroupCategory.StringLengths.TYPE).Not.Nullable();
            Map(x => x.Abbreviation).Length(TaskGroupCategory.StringLengths.ABBREVIATION).Not.Nullable();
            Map(x => x.IsActive).Not.Nullable();
            HasMany(x => x.TaskGroups).KeyColumn("TaskGroupCategoryId").LazyLoad();
        }

        #endregion
    }
}