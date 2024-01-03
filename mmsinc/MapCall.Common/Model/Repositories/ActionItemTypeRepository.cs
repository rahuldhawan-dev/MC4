using System.Collections.Generic;
using System.Linq;
using MMSINC.Data.NHibernate;
using MapCall.Common.Model.Entities;
using NHibernate;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public class ActionItemTypeRepository : RepositoryBase<ActionItemType>, IActionItemTypeRepository
    {
        #region Constructors

        public ActionItemTypeRepository(ISession session, IContainer container) : base(session, container) { }

        #endregion

        #region Exposed Methods

        public IEnumerable<ActionItemType> GetByTableName(string tableName = null)
        {
            var results = (from dt in Linq where dt.DataType.TableName == tableName select dt);

            return !results.Any() ? GetDefaultList() : results;
        }
        
        public IEnumerable<ActionItemType> GetDefaultList()
        {
            return (from dt in Linq where dt.DataType == null select dt);
        }

        #endregion
    }

    public interface IActionItemTypeRepository : IRepository<ActionItemType>
    {
        #region Abstract Methods

        IEnumerable<ActionItemType> GetByTableName(string tableName = null);
        
        IEnumerable<ActionItemType> GetDefaultList();

        #endregion
    }
}
