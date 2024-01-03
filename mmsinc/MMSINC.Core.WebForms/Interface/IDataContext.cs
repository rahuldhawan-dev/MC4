using System;
using System.Collections.Generic;
using System.Data.Linq;

namespace MMSINC.Interface
{
    /// <summary>
    /// Represents the contract that a System.Data.Linq.DataContext will
    /// fulfill, to cut down on the dependency to any specific type inherited
    /// from System.Data.Linq.DataContext.
    /// </summary>
    public interface IDataContext : IDisposable
    {
        #region Methods

        void SubmitChanges();

        int ExecuteCommand(string command, params object[] arguments);

        IEnumerable<TEntity> ExecuteQuery<TEntity>(string query, params object[] arguments);

        Table<TEntity> GetTable<TEntity>() where TEntity : class;

        #endregion
    }
}
