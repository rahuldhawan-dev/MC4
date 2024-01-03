using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class DataTableLayoutMap : ClassMap<DataTableLayout>
    {
        #region Constructors

        public DataTableLayoutMap()
        {
            Id(x => x.Id);

            Map(x => x.Area)
               .Length(DataTableLayout.StringLengths.AREA)
               .Nullable();
            Map(x => x.Controller)
               .Length(DataTableLayout.StringLengths.CONTROLLER)
               .Not.Nullable();
            Map(x => x.LayoutName)
               .Length(DataTableLayout.StringLengths.LAYOUT_NAME)
               .Not.Nullable();

            HasMany(x => x.Properties).KeyColumn("DataTableLayoutId")
                                      .Inverse().Cascade.AllDeleteOrphan();
        }

        #endregion
    }
}
