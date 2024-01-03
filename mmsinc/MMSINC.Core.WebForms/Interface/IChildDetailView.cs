using System;

namespace MMSINC.Interface
{
    public interface IChildDetailView<TEntity> : IDetailView<TEntity>
        where TEntity : class
    {
        #region Events

        event EventHandler EventFiring;

        #endregion
    }
}
