using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Utility.Permissions.Modules;
using MMSINC.Interface;
using StructureMap;
using WorkOrders.Model;

namespace WorkOrders.Library.Permissions
{
    /// <summary>
    /// Contains all the logic necessary for the security required in 271.
    /// </summary>
    public sealed class SecurityService : ISecurityService
    {
        #region Constants

        public const string HTTP_CONTEXT_KEY = "SecurityService_key";

        #endregion

        #region Private Members

        private Employee _employee;

        private IOperatingCenterRepository _operatingCenterRepository;
        private IEmployeeRepository _employeeRepository;
        
        private bool? _userHasAccess, _isAdmin;
        private OperatingCenter _defaultOperatingCenter;
        private OperatingCenter[] _adminOperatingCenters,
                                  _all271OperatingCenters,
                                  _userOperatingCenters;

        #endregion

        #if DEBUG && !LOCAL

        #region Private Static Members

        private static ISecurityService _instance; 

        #endregion

        #endif

        #region Properties

        /// <summary>
        /// Boolean indicating whether or not the current user is an Admin.
        /// </summary>
        public bool IsAdmin
        {
            get {
                if (_isAdmin == null)
                    _isAdmin = (AdminOperatingCenters.Count() > 0);
                return _isAdmin.Value;
            }
        }

        /// <summary>
        /// Boolean indicating whether or not the current user has any level
        /// (at least read) of access to 271.
        /// </summary>
        public bool UserHasAccess
        {
            get
            {
                if (_userHasAccess == null)
                    _userHasAccess = (UserOperatingCentersCount > 0);
                return _userHasAccess.Value;
            }
        }

        /// <summary>
        /// Employee object from tblPermissions related to the current user.
        /// </summary>
        public Employee Employee
        {
            get
            {
                return _employee ??
                       (_employee = EmployeeRepository.GetEmployeeByUserName(
                           CurrentUser.Name));
            }
        }

        /// <summary>
        /// Currently logged in user.
        /// </summary>
        public IUser CurrentUser { get; protected set; }

        /// <summary>
        /// Primary operating center of the currently logged in user.
        /// </summary>
        public OperatingCenter DefaultOperatingCenter
        {
            get
            {
                if(!IsAdmin && _defaultOperatingCenter == null)
                    _defaultOperatingCenter = Employee.DefaultOperatingCenter;
                return _defaultOperatingCenter;
            }
        }

        /// <summary>
        /// Set of operating centers for which the current user has at least
        /// 'read' access for 271.  If the current user is an admin for any
        /// operating center, this array is superceded by
        /// AdminOperatingCenters.
        /// </summary>
        public OperatingCenter[] UserOperatingCenters
        {
            get
            {
                return _userOperatingCenters ??
                       (_userOperatingCenters = (IsAdmin) ? AdminOperatingCenters
                                                    : GetUserOperatingCenters());
            }
        }

        /// <summary>
        /// Count of the operating centers for which the current user has at
        /// least 'read' access.
        /// </summary>
        public int UserOperatingCentersCount
        {
            get { return UserOperatingCenters.Count(); }
        }

        /// <summary>
        /// Set of operating centers for which the current user has
        /// administrative level access for 271.
        /// </summary>
        public OperatingCenter[] AdminOperatingCenters
        {
            get
            {
                return _adminOperatingCenters ??
                       (_adminOperatingCenters = GetAdminOperatingCenters());
            }
        }

        #region Protected

        protected IOperatingCenterRepository OperatingCenterRepository
        {
            get
            {
                if (_operatingCenterRepository == null)
                    _operatingCenterRepository =
                        DependencyResolver.Current.GetService<IOperatingCenterRepository>();
                return _operatingCenterRepository;
            }
        }

        protected IEmployeeRepository EmployeeRepository
        {
            get
            {
                if (_employeeRepository == null)
                    _employeeRepository =
                        DependencyResolver.Current.GetService<IEmployeeRepository>();
                return _employeeRepository;
            }
        }

        protected IEnumerable<OperatingCenter> All271OperatingCenters
        {
            get
            {
                if (_all271OperatingCenters == null)
                {
                    _all271OperatingCenters =
                        OperatingCenterRepository.GetAll271OperatingCenters();
                }
                return _all271OperatingCenters;
            }
        }

        #endregion

        #endregion

        #region Static Properties

        public static ISecurityService Instance
        {
            get
            {
                ISecurityService instance = null;

                #if DEBUG && !LOCAL
                instance = _instance;
                
                #elif !DEBUG || LOCAL
                instance =
                    (ISecurityService)System.Web.HttpContext.Current.Items[HTTP_CONTEXT_KEY];
                #endif

                if (instance == null)
                {
                    instance = new SecurityService();
                    Instance = instance;
                }
                return instance;
            }

            protected set
            {
                #if DEBUG && !LOCAL
                _instance = value;
                
                #elif !DEBUG || LOCAL
                if (!System.Web.HttpContext.Current.Items.Contains(HTTP_CONTEXT_KEY))
                    System.Web.HttpContext.Current.Items.Add(HTTP_CONTEXT_KEY, value);
                #endif
            }
        }

        #endregion

        #region Private Methods

        private OperatingCenter[] GetAdminOperatingCenters()
        {
            return All271OperatingCenters.Where(opCntr => CurrentUser.CanAdministrate(FieldServices.WorkManagement).In(opCntr.OpCntr)).OrderBy(oc => oc.State.Abbreviation).ThenBy(oc => oc.OpCntr).ToArray();
        }

        private OperatingCenter[] GetUserOperatingCenters()
        {
            return All271OperatingCenters.Where(opCntr => CurrentUser.CanRead(FieldServices.WorkManagement).In(opCntr.OpCntr)).OrderBy(oc => oc.State.Abbreviation).ThenBy(oc => oc.OpCntr).ToArray();
        }

        private void ResetPrivateMembers()
        {
            _employee = null;
            _userHasAccess = null;
            _isAdmin = null;
            _defaultOperatingCenter = null;
            _adminOperatingCenters = null;
            _userOperatingCenters = null;
        }

        #endregion

        #region Exposed Methods

        /// <summary>
        /// Initializes the service for the currently logged in user.
        /// </summary>
        /// <param name="user"></param>
        public void Init(IUser user)
        {
            CurrentUser = user;
            ResetPrivateMembers();
        }

        public int GetEmployeeID()
        {
            return Employee.EmployeeID;
        }

        #endregion

        #region Exposed Static Methods

        public static IEnumerable<OperatingCenter> SelectUserOperatingCenters()
        {
            return Instance.UserOperatingCenters;
        }
        
        #endregion
    }

    public interface ISecurityService : MMSINC.Utilities.Permissions.ISecurityService
    {
        #region Properties
        
        //THESE ARE DOMAIN SPECIFIC TO THIS PROJECT
        Employee Employee { get; }
        OperatingCenter[] AdminOperatingCenters { get; }
        OperatingCenter[] UserOperatingCenters { get; }
        OperatingCenter DefaultOperatingCenter { get; }
        int UserOperatingCentersCount { get; }

        #endregion

        #region Exposed Methods

        int GetEmployeeID();

        #endregion
    }
}
