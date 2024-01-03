using System.Collections.Generic;
using System.Linq;
using Historian.Data.Client.Entities;
using Historian.Data.Client.Mappers;

namespace Historian.Data.Client.Repositories
{
    public class TagNameRepository : RepositoryBase<TagName, ITagNameMapper>, ITagNameRepository
    {
        #region Constants

        public struct Queries
        {
            #region Constants

            public const string GET_ALL =
                                    "SELECT * FROM OPENQUERY(FACILITY_CONNEX, 'SELECT Tagname, Description, Engunits FROM ihTags')",
                                FIND =
                                    "SELECT * FROM OPENQUERY(FACILITY_CONNEX, 'SELECT Tagname, Description, Engunits FROM ihTags' WHERE Tagname = ''{0}''');";

            #endregion
        }

        #endregion

        #region Constructors

        public TagNameRepository(ITagNameMapper mapper) : base(mapper) { }

        #endregion

        #region Exposed Methods

        public IEnumerable<TagName> GetAll()
        {
            return Find(Queries.GET_ALL);
        }

        public TagName Get(string tagName)
        {
            return Find(Queries.FIND, tagName).FirstOrDefault();
        }

        #endregion
    }

    public interface ITagNameRepository
    {
        #region Abstract Methods

        TagName Get(string tagName);
        IEnumerable<TagName> GetAll();

        #endregion
    }
}
