using System.Data.Common;
using Historian.Data.Client.Entities;

namespace Historian.Data.Client.Mappers
{
    public class TagNameMapper : MapperBase<TagName>, ITagNameMapper
    {
        #region Private Methods

        protected override TagName MapItem(DbDataReader reader)
        {
            return new TagName {
                Name = reader["Tagname"].ToString(),
                Description = reader["Description"].ToString(),
                Units = reader["Engunits"].ToString()
            };
        }

        #endregion
    }

    public interface ITagNameMapper : IMapper<TagName> { }
}
