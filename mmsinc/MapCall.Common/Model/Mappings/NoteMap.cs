using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class NoteMap : ClassMap<Note>
    {
        public NoteMap()
        {
            Table("Note");

            Id(x => x.Id).GeneratedBy.Identity().Column("NoteID");

            // Map(x => x.Text).Column("Note").CustomType("StringClob").CustomSqlType("varchar(max)");
            Map(x => x.Text, "Note").Length(int.MaxValue);
            Map(x => x.CreatedAt).Not.Nullable();
            Map(x => x.LinkedId).Column("DataLinkID").Not.Nullable();
            Map(x => x.CreatedBy);

            References(x => x.DataType);
        }
    }
}
