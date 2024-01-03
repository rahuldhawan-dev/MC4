using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class DocumentMap : ClassMap<Document>
    {
        #region Constants

        public const string TABLE_NAME = "Document";

        #endregion

        #region Constructors

        public DocumentMap()
        {
            Table(TABLE_NAME);
            Id(x => x.Id, "DocumentID");
            Map(x => x.CreatedByStr, "CreatedBy")
               .Length(Document.StringLengths.CREATED_BY);
            Map(x => x.ModifiedByStr, "ModifiedBy")
               .Length(Document.StringLengths.MODIFIED_BY);
            Map(x => x.CreatedAt).Not.Nullable();
            Map(x => x.UpdatedAt).Not.Nullable();
            Map(x => x.FileName, "File_Name")
               .Length(Document.StringLengths.FILE_NAME);
            References(x => x.CreatedBy);
            References(x => x.UpdatedBy);
            References(x => x.DocumentType);
            References(x => x.DocumentData)
               .Nullable()
               .Not.Update();

            HasManyToMany(x => x.WorkOrders)
               .Cascade.SaveUpdate()
               .Table("DocumentsWorkOrders")
               .ParentKeyColumn("DocumentID")
               .ChildKeyColumn("WorkOrderID");
        }

        #endregion
    }
}
