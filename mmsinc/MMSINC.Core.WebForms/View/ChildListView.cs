using System;
using MMSINC.Interface;

namespace MMSINC.View
{
    public abstract class ChildListView<TEntity> : ListView<TEntity>, IChildListView<TEntity>
        where TEntity : class
    {
        #region Events

        public event EventHandler EventFiring;

        #endregion

        #region Event Passthroughs

        protected virtual void OnChildEventFiring(EventArgs e)
        {
            if (EventFiring != null)
                EventFiring(this, e);
        }

        #endregion
    }
}
