using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class DocumentDataMap : ClassMap<DocumentData>
    {
        #region Constants

        public const string TABLE_NAME = "DocumentData";

        #endregion

        #region Constructors

        public DocumentDataMap()
        {
            Table(TABLE_NAME);
            Id(x => x.Id);
            Map(x => x.FileSize)
               .Not.Nullable()
               .Not.Update();
            Map(x => x.Hash)
               .Length(DocumentData.MAX_HASH_LENGTH)
               .Unique()
               .Not.Nullable()
               .Not.Update();
        }

        #endregion
    }
}
