using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Migrations;

namespace MapCall.Common.Model.Mappings
{
    public class ContractorsSecureFormTokenMap : ClassMap<ContractorsSecureFormToken>
    {
        public ContractorsSecureFormTokenMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();

            Map(x => x.UserId).Not.Nullable();
            Map(x => x.Token).Not.Update().Not.Nullable();
            Map(x => x.Area)
               .Length(MakeSecureFormTokenStringColumnsLonger.NEW_LENGTH)
               .Nullable();
            Map(x => x.Controller)
               .Length(MakeSecureFormTokenStringColumnsLonger.NEW_LENGTH)
               .Not.Nullable();
            Map(x => x.Action)
               .Length(AddSOPTablesForBug2662.TOKEN_ACTION_LENGTH)
               .Not.Nullable();
            Map(x => x.CreatedAt).Not.Update().Not.Nullable();

            HasMany(x => x.DynamicValues)
               .KeyColumn(CreateSecureFormTokenDynamicValuesTable.ColumnNames.SECURE_FORM_TOKEN_ID)
               .Cascade.AllDeleteOrphan()
               .Inverse()
               .Not.LazyLoad();
        }
    }
}
