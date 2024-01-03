using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class JobCategoryMap : ClassMap<JobCategory>
    {
        #region Constants

        public const string TABLE_NAME = "JobCategories";

        #endregion

        #region Constructors

        public JobCategoryMap()
        {
            Table(TABLE_NAME);

            Id(x => x.Id);

            Map(x => x.Description).Not.Nullable();
        }

        #endregion
    }
}
