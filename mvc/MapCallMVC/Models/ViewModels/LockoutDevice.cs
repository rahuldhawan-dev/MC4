using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories.Users;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Models.ViewModels
{
    public class LockoutDeviceViewModel : ViewModel<LockoutDevice>
    {
        #region Properties

        [Required, EntityMap, DropDown]
        [EntityMustExist(typeof(OperatingCenter))]
        public virtual int? OperatingCenter { get; set; }

        [EntityMap, DropDown]
        [EntityMustExist(typeof(LockoutDeviceColor))]
        public virtual int? LockoutDeviceColor { get; set; }

        [StringLength(LockoutDevice.StringLengths.DESCRIPTION)]
        public virtual string SerialNumber { get; set; }

        [StringLength(LockoutDevice.StringLengths.DESCRIPTION)]
        [Required]
        public virtual string Description { get; set; }
        
        #endregion
        
        #region Constructors

        public LockoutDeviceViewModel(IContainer container) : base(container) { }

        #endregion
    }

    public class CreateLockoutDevice : LockoutDeviceViewModel
    {
        #region Properties

        [Required, EntityMap(typeof(UserRepository))]
        [EntityMustExist(typeof(User))]
        [DropDown("", "User", "GetActiveUsersWithOpCenterIdAndRoleForLockoutDevice", DependsOn = "OperatingCenter")]
        public virtual int? Person { get; set; }

        [DropDown, EntityMap(MapDirections.None), EntityMustExist(typeof(State))]
        public int? State { get; set; }

        [DropDown("", "OperatingCenter", "ActiveByStateIdForHealthAndSafety", DependsOn = "State", PromptText = "Please select a state above")]
        public override int? OperatingCenter { get; set; }

        #endregion

        #region Constructors

        public CreateLockoutDevice(IContainer container) : base(container) {}

        #endregion
    }

    public class EditLockoutDevice : LockoutDeviceViewModel
    {
        #region Properties

        private User _person;

        [DoesNotAutoMap("Display only")]
        public User Person
        {
            get
            {
                if (_person == null)
                {
                    _person = _container.GetInstance<IRepository<LockoutDevice>>().Find(Id).Person;
                }
                return _person;
            }
        }

        [DropDown, EntityMap(MapDirections.None), EntityMustExist(typeof(State))]
        public int? State { get; set; }

        [DropDown("", "OperatingCenter", "ActiveByStateIdForHealthAndSafety", DependsOn = "State", PromptText = "Please select a state above")]
        public override int? OperatingCenter { get; set; }

        #endregion

        #region Constructors

        public EditLockoutDevice(IContainer container) : base(container) {}

	    #endregion
    }

    public class SearchLockoutDevice : SearchSet<LockoutDevice>
    {
        // We will need to search by Operating Center, Facility, SAP Equipment, SAP Work Order Number,
        // Currently Locked out, or Returned to Service, Employee (user) who created original out of service record, 
        // and Employee who Returned To Service.
        #region Properties

        [DropDown, EntityMap, EntityMustExist(typeof(OperatingCenter))]
        public int? OperatingCenter { get; set; }
        [DropDown("", "User", "GetActiveUsersWithOpCenterIdAndRoleAndIsAssignedToLockOutDevices", DependsOn = "OperatingCenter")]
        public virtual int? Person { get; set; }

        #endregion
    }
}