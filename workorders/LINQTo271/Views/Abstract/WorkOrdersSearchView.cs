using MMSINC.View;
using WorkOrders;

namespace LINQTo271.Views.Abstract
{
    /// <summary>
    /// Generic abstract SearchView intended to be the base for all implemented
    /// search views in this project.
    /// </summary>
    /// <typeparam name="TEntity">Type of entity that a specific inheriting
    /// SearchView will deal with.</typeparam>
    public abstract class WorkOrdersSearchView<TEntity> : SearchView<TEntity>
        where TEntity : class
    {
        #region Properties

        #if DEBUG

        public override bool IsMvpPostBack
        {
            get
            {
                return (_isMvpPostBack == null) ?
                    IsPostBack : _isMvpPostBack.Value;
            }
        }

        #endif

        #endregion
    }
}
