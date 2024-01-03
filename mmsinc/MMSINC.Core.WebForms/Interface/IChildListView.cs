using System;

namespace MMSINC.Interface
{
    public interface IChildListView<TEntity> : IListView<TEntity>
        where TEntity : class
    {
        #region Events

        event EventHandler EventFiring;

        #endregion
    }
}
