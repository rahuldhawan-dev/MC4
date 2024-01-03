using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class TownContactMap : ClassMap<TownContact>
    {
        public const string TABLE_NAME = "TownsContacts";

        public TownContactMap()
        {
            Table(TABLE_NAME);
            Id(x => x.Id, "TownsContactsID");
            References(x => x.Contact)
               .Not.Nullable();
            References(x => x.Town)
               .Not.Nullable();
            References(x => x.ContactType)
               .Not.Nullable();
            //  HasOne(x => x.Contact);
            // HasOne(x => x.ContactType);
            //  HasOne(x => x.Town);
        }
    }
}
