using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class DocumentTypeMap : ClassMap<DocumentType>
    {
        public const string TABLE_NAME = "DocumentType";

        public DocumentTypeMap()
        {
            Table(TABLE_NAME);
            Id(x => x.Id, "DocumentTypeID");
            Map(x => x.Name, "Document_Type");
            References(x => x.DataType);

            HasManyToMany(x => x.Documents)
               .Table("DocumentLink")
               .ParentKeyColumn("DocumentTypeId")
               .ChildKeyColumn("DocumentId");
        }
    }
}
