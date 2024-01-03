using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class ContactTypeMap : ClassMap<ContactType>
    {
        public ContactTypeMap()
        {
            Id(x => x.Id, "ContactTypeID");

            Map(x => x.Description, "Name")
               .Not.Nullable()
               .Unique()
                // ReSharper disable AccessToStaticMemberViaDerivedType
               .Length(ContactType.StringLengths.DESCRIPTION);
            // ReSharper restore AccessToStaticMemberViaDerivedType
        }
    }
}
