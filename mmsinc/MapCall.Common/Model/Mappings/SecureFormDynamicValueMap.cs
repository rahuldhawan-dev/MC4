using FluentNHibernate.Mapping;
using MMSINC.Metadata;
using MapCall.Common.Model.Migrations;

namespace MapCall.Common.Model.Mappings
{
    public class SecureFormDynamicValueMap : ClassMap<SecureFormDynamicValue>
    {
        public SecureFormDynamicValueMap()
        {
            Id(x => x.Id);

            Map(x => x.Key, "`Key`");
            Map(x => x.XmlValue).Length(int.MaxValue);
            Map(x => x.Type);

            References(x => x.SecureFormToken)
               .Column(CreateSecureFormTokenDynamicValuesTable.ColumnNames.SECURE_FORM_TOKEN_ID);
        }
    }
}
