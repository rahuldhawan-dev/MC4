using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class FireDistrictMap : ClassMap<FireDistrict>
    {
        public FireDistrictMap()
        {
            Id(x => x.Id);

            Map(x => x.Address)
               .Length(FireDistrict.StringLengths.ADDRESS);
            Map(x => x.AddressCity)
               .Length(FireDistrict.StringLengths.ADDRESS_CITY);
            Map(x => x.AddressZip)
               .Length(FireDistrict.StringLengths.ADDRESS_ZIP);
            Map(x => x.Contact)
               .Length(FireDistrict.StringLengths.CONTACT);
            Map(x => x.DistrictName)
               .Length(FireDistrict.StringLengths.DISTRICT_NAME);
            Map(x => x.Fax)
               .Length(FireDistrict.StringLengths.FAX);
            Map(x => x.Phone)
               .Length(FireDistrict.StringLengths.PHONE);
            Map(x => x.Abbreviation)
               .Length(FireDistrict.StringLengths.ABBREVIATION);
            Map(x => x.PremiseNumber)
               .Length(FireDistrict.StringLengths.PREMISE_NUMBER);
            Map(x => x.UtilityName)
               .Length(FireDistrict.StringLengths.UTILITY_NAME);
            Map(x => x.UtilityDistrict);

            References(x => x.State);

            HasMany(x => x.TownFireDistricts)
               .KeyColumn("FireDistrictId");
            HasMany(x => x.Hydrants)
               .KeyColumn("FireDistrictId");
        }
    }
}
