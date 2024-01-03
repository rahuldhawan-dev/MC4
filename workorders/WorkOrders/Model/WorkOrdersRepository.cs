using MMSINC.Data.Linq;
using WorkOrders.Library.Permissions;
using SecurityServiceClass = WorkOrders.Library.Permissions.SecurityService;

namespace WorkOrders.Model
{
    /// <summary>
    /// Abstract Repository implementation, typed out to use the WorkOrdersDataContext, but to still require
    /// the specific entity type.
    /// </summary>
    /// <typeparam name="TUnitType">
    /// Specific entity type which an inherited Repository object/class should deal in.
    /// </typeparam>
    public abstract class WorkOrdersRepository<TUnitType> : Repository<TUnitType>
        where TUnitType : class, new()
    {
        #if DEBUG && !LOCAL

        #region Private Members

        protected static ISecurityService _securityService;

        #endregion
        
        #endif

        #region Properties
        
        protected static ISecurityService SecurityService
        {
            get
            {
                #if DEBUG && !LOCAL

                if (_securityService == null)
                    _securityService = SecurityServiceClass.Instance;
                return _securityService;

                #elif !DEBUG || LOCAL

                return SecurityServiceClass.Instance;

                #endif
            }
        }

        #endregion
    }
}