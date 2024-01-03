using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    class AddressMap : ClassMap<Address>
    {
        #region Constructors

        public AddressMap()
        {
            Id(x => x.Id);

            Map(x => x.Address1)
               .Length(Address.StringLengths.ADDRESS_1)
               .Not.Nullable();
            Map(x => x.Address2)
               .Length(Address.StringLengths.ADDRESS_2)
               .Nullable();
            Map(x => x.ZipCode)
               .Length(Address.StringLengths.ZIP_CODE)
               .Not.Nullable();

            References(x => x.Town)
               .Not.Nullable();
        }

        #endregion
    }
}
