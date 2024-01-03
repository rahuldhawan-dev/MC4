using System.Collections.Generic;
using System.Data.Common;

namespace Historian.Data.Client.Mappers
{
    public abstract class MapperBase<TEntity> : IMapper<TEntity>
    {
        #region Abstract Methods

        protected abstract TEntity MapItem(DbDataReader reader);

        #endregion

        #region Exposed Methods

        public IEnumerable<TEntity> Map(DbDataReader reader)
        {
            var ret = new List<TEntity>();

            while (reader.Read())
            {
                ret.Add(MapItem(reader));
            }

            return ret;
        }

        #endregion
    }

    public interface IMapper<TEntity>
    {
        #region Abstract Methods

        IEnumerable<TEntity> Map(DbDataReader reader);

        #endregion
    }
}
