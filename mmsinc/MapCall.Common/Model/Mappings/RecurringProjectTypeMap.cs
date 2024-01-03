using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class RecurringProjectTypeMap : ClassMap<RecurringProjectType>
    {
        #region Constructors

        public RecurringProjectTypeMap()
        {
            Id(x => x.Id).GeneratedBy.Identity().Not.Nullable();

            Map(x => x.Description).Not.Nullable();
            Map(x => x.CreatedBy).Not.Nullable();
            Map(x => x.CreatedAt).Not.Nullable();

            HasMany(x => x.RecurringProjects).KeyColumn("ProjectTypeID");
        }

        #endregion
    }
}
