using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class PersonnelAreaMap : ClassMap<PersonnelArea>
    {
        public PersonnelAreaMap()
        {
            Id(x => x.Id)
               .Not.Nullable();

            Map(x => x.Description)
               .Length(PersonnelArea.MAX_DESCRIPTION_LENGTH)
               .Not.Nullable();

            Map(x => x.PersonnelAreaId)
               .Unique().Not.Nullable();

            References(x => x.OperatingCenter)
               .Nullable();
        }
    }
}
