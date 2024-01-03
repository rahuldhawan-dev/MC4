using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class ServiceCategoryMap : ClassMap<ServiceCategory>
    {
        #region Constants

        public const string TABLE_NAME = "ServiceCategories";
        public const string ID_COLUMN_NAME = nameof(ServiceCategory) + "Id";

        #endregion

        #region Constructors

        public ServiceCategoryMap()
        {
            Table(TABLE_NAME);
            Id(x => x.Id, ID_COLUMN_NAME);
            Map(x => x.Description);
            References(x => x.ServiceUtilityType).Nullable();
        }

        #endregion
    }
}
