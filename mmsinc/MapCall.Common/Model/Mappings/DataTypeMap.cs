using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class DataTypeMap : ClassMap<DataType>
    {
        public const string TABLE_NAME = "DataType";

        public DataTypeMap()
        {
            Table(TABLE_NAME);
            Id(x => x.Id, "DataTypeId");
            Map(x => x.Name, "Data_Type");
            Map(x => x.TableID, "Table_ID");
            Map(x => x.TableName, "Table_Name");

            HasMany(x => x.DocumentTypes)
               .KeyColumn("DataTypeId");
        }
    }
}
