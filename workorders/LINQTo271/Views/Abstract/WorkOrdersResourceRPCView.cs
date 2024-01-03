using System;
using System.Linq.Expressions;
using MMSINC.Common;
using MMSINC.Interface;
using MMSINC.View;
using WorkOrders;

namespace LINQTo271.Views.Abstract
{
    public abstract class WorkOrdersResourceRPCView<TEntity> : ResourceRPCView<TEntity>
        where TEntity : class
    {
        #region Properties

        #if DEBUG && !LOCAL
        // There wont be an actual user unless you're logged in. 
        // This is why we need to do this.
        public override IUser IUser
        {
            get
            {
                if (_iUser == null)
                    _iUser = base.IUser ?? new DebuggingUser();
                return _iUser;
            }
        }
        #endif

        #endregion

        #region Exposed Methods

        public override Expression<Func<TEntity, bool>> GenerateExpression()
        {
            return new ExpressionBuilder<TEntity>(d => true);
        }

        #endregion
    }
}
