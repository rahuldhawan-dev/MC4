using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class DataTableLayoutPropertyMap : ClassMap<DataTableLayoutProperty>
    {
        #region Consts

        public const string TABLE_NAME = "DataTableLayoutProperties";

        #endregion

        #region Constructors

        public DataTableLayoutPropertyMap()
        {
            Table(TABLE_NAME);
            Id(x => x.Id);
            Map(x => x.PropertyName)
               .Length(DataTableLayoutProperty.StringLengths.PROPERTY_NAME)
               .Not.Nullable();

            References(x => x.DataTableLayout)
               .Not.Nullable();
        }

        #endregion
    }
}
