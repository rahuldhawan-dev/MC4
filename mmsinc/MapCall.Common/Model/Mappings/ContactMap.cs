using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class ContactMap : ClassMap<Contact>
    {
        #region Constructors

        public ContactMap()
        {
            Id(x => x.Id, "ContactID");

            Map(x => x.BusinessPhoneNumber, "BusinessPhone")
               .Nullable()
               .Length(Contact.StringLengths.BUSINESS_PHONE);
            Map(x => x.CreatedBy)
               .Not.Nullable()
               .Length(Contact.StringLengths.CREATED_BY);
            Map(x => x.CreatedAt)
               .Not.Nullable();
            Map(x => x.Email)
               .Nullable()
               .Length(Contact.StringLengths.EMAIL);
            Map(x => x.FaxNumber, "Fax")
               .Nullable()
               .Length(Contact.StringLengths.FAX);
            Map(x => x.FirstName)
               .Nullable()
               .Length(Contact.StringLengths.FIRST_NAME);
            Map(x => x.HomePhoneNumber, "HomePhone")
               .Nullable()
               .Length(Contact.StringLengths.HOME_PHONE);
            Map(x => x.LastName)
               .Not.Nullable()
               .Length(Contact.StringLengths.LAST_NAME);
            Map(x => x.MiddleInitial)
               .Nullable()
               .Length(Contact.StringLengths.MIDDLE_INITIAL);
            Map(x => x.MobilePhoneNumber, "Mobile")
               .Nullable()
               .Length(Contact.StringLengths.MOBILE);

            References(x => x.Address)
               .Nullable()
               .Cascade.All();

            HasMany(x => x.TownContacts).KeyColumn("ContactId");
            HasMany(x => x.NotificationConfigurations).KeyColumn("ContactId");
        }

        #endregion
    }
}
