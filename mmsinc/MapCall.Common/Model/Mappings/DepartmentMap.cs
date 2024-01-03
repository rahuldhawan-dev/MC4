using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class DepartmentMap : ClassMap<Department>
    {
        #region Constructors

        public DepartmentMap()
        {
            Id(x => x.Id, "DepartmentID");

            Map(x => x.Description);
            Map(x => x.Code);

            HasMany(x => x.Facilities).KeyColumn("DepartmentID").LazyLoad();
            HasMany(x => x.BusinessUnits).KeyColumn("DepartmentID").LazyLoad();
        }

        #endregion
    }
}
