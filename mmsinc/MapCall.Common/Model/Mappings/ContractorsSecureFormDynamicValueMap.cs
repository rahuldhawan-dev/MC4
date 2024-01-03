using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Migrations;

namespace MapCall.Common.Model.Mappings
{
    public class ContractorsSecureFormDynamicValueMap : ClassMap<ContractorsSecureFormDynamicValue>
    {
        public ContractorsSecureFormDynamicValueMap()
        {
            Id(x => x.Id);

            Map(x => x.Key, "`Key`");
            Map(x => x.XmlValue);
            Map(x => x.Type);

            References(x => x.SecureFormToken)
               .Column(CreateSecureFormTokenDynamicValuesTable.ColumnNames.SECURE_FORM_TOKEN_ID);
        }
    }
}
