using System.Collections.Generic;
using System.Linq;
using MMSINC.Data.NHibernate;
using MapCall.Common.Model.Entities;
using NHibernate;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public class DataTypeRepository : RepositoryBase<DataType>, IDataTypeRepository
    {
        public DataTypeRepository(ISession session, IContainer container) : base(session, container) { }

        public IEnumerable<DataType> GetByTableName(string tableName)
        {
            return (from dt in Linq where dt.TableName == tableName select dt);
        }

        public IEnumerable<DataType> GetByTableNameAndDataTypeName(string tableName, string dataTypeName)
        {
            return (from dt in Linq where dt.TableName == tableName && dt.Name == dataTypeName select dt);
        }
    }

    public interface IDataTypeRepository : IRepository<DataType>
    {
        IEnumerable<DataType> GetByTableName(string tableName);
        IEnumerable<DataType> GetByTableNameAndDataTypeName(string tableName, string dataTypeName);
    }
}
