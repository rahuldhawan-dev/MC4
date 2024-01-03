using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using MMSINC.Exceptions;

namespace WorkOrders.Model
{
    [Table(Name = "dbo.tblPermissions")]
    public class Employee : INotifyPropertyChanging, INotifyPropertyChanged, IComparable<Employee>
    {
        #region Private Static Members

        private static readonly PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        #endregion

        #region Private Members

        private string _add1,
                       _cDCCode,
                       _cellNum,
                       _city,
                       _company,
                       _eMail,
                       _empNum,
                       _faxNum,
                       _fullName,
                       _location,
                       _inactive,
                       _phoneNum,
                       _region,
                       _st,
                       _userLevel,
                       _userName,
                       _zip,
                       _fBUserName,
                       _fBPassWord,
                       _xYMUserName,
                       _xYMPassword,
                       _uid, 
                       _workBasket;

        private char? _userInact;

        private int? _userID;

        private int _employeeID, _defaultOperatingCenterID;

        private readonly EntitySet<WorkOrder> _requestedWorkOrders,
                                              _createdWorkOrders,
                                              _approvedMaterialsWorkOrders,
                                              _approvedWorkOrders,
                                              _workOrdersCompleted,
                                              _workOrdersCancelled,
                                              _officeAssignedWorkOrders;

        private readonly EntitySet<EmployeeWorkOrder> _assignedWorkOrders;

        private readonly EntitySet<WorkOrderDescriptionChange> _workOrderDescriptionChanges;

        private readonly EntitySet<Restoration> _restorationsApproved,
                                                _restorationsRejected;

        private readonly EntitySet<Document> _documentsAdded, _documentsModified;

        private EntitySet<ReportViewing> _reportViewings;

        private EntitySet<OperatingCenterUser> _operatingCentersUsers;

        private EntityRef<OperatingCenter> _defaultOperatingCenter;

        private EntitySet<OperatingCenter> _operatingCenters;

        private EntitySet<Role> _roles;
        private EntitySet<AggregateRole> _aggregateRoles;

        #endregion

        #region Properties

        #region Table Column Properties

        [Column(Storage = "_add1", DbType = "VarChar(50)")]
        public string Add1
        {
            get { return _add1; }
            set
            {
                if ((_add1 != value))
                {
                    SendPropertyChanging();
                    _add1 = value;
                    SendPropertyChanged("Add1");
                }
            }
        }

        [Column(Storage = "_cDCCode", DbType = "VarChar(4)")]
        public string CDCCode
        {
            get { return _cDCCode; }
            set
            {
                if ((_cDCCode != value))
                {
                    SendPropertyChanging();
                    _cDCCode = value;
                    SendPropertyChanged("CDCCode");
                }
            }
        }

        [Column(Storage = "_cellNum", DbType = "VarChar(12)")]
        public string CellNum
        {
            get { return _cellNum; }
            set
            {
                if ((_cellNum != value))
                {
                    SendPropertyChanging();
                    _cellNum = value;
                    SendPropertyChanged("CellNum");
                }
            }
        }

        [Column(Storage = "_city", DbType = "VarChar(50)")]
        public string City
        {
            get { return _city; }
            set
            {
                if ((_city != value))
                {
                    SendPropertyChanging();
                    _city = value;
                    SendPropertyChanged("City");
                }
            }
        }

        [Column(Storage = "_company", DbType = "VarChar(10)")]
        public string Company
        {
            get { return _company; }
            set
            {
                if ((_company != value))
                {
                    SendPropertyChanging();
                    _company = value;
                    SendPropertyChanged("Company");
                }
            }
        }

        [Column(Storage = "_eMail", DbType = "VarChar(50)")]
        public string EMail
        {
            get { return _eMail; }
            set
            {
                if ((_eMail != value))
                {
                    SendPropertyChanging();
                    _eMail = value;
                    SendPropertyChanged("EMail");
                }
            }
        }

        [Column(Storage = "_empNum", DbType = "VarChar(15)")]
        public string EmpNum
        {
            get { return _empNum; }
            set
            {
                if ((_empNum != value))
                {
                    SendPropertyChanging();
                    _empNum = value;
                    SendPropertyChanged("EmpNum");
                }
            }
        }

        [Column(Storage = "_faxNum", DbType = "VarChar(12)")]
        public string FaxNum
        {
            get { return _faxNum; }
            set
            {
                if ((_faxNum != value))
                {
                    SendPropertyChanging();
                    _faxNum = value;
                    SendPropertyChanged("FaxNum");
                }
            }
        }

        [Column(Storage = "_fullName", DbType = "VarChar(25)")]
        public string FullName
        {
            get { return _fullName; }
            set
            {
                if ((_fullName != value))
                {
                    SendPropertyChanging();
                    _fullName = value;
                    SendPropertyChanged("FullName");
                }
            }
        }

        [Column(Storage = "_location", DbType = "VarChar(3)")]
        public string Location
        {
            get { return _location; }
            set
            {
                if ((_location != value))
                {
                    SendPropertyChanging();
                    _location = value;
                    SendPropertyChanged("Location");
                }
            }
        }

        [Column(Storage = "_inactive", DbType = "VarChar(2)")]
        public string Inactive
        {
            get { return _inactive; }
            set
            {
                if ((_inactive != value))
                {
                    SendPropertyChanging();
                    _inactive = value;
                    SendPropertyChanged("Inactive");
                }
            }
        }

        [Column(Storage = "_phoneNum", DbType = "VarChar(12)")]
        public string PhoneNum
        {
            get { return _phoneNum; }
            set
            {
                if ((_phoneNum != value))
                {
                    SendPropertyChanging();
                    _phoneNum = value;
                    SendPropertyChanged("PhoneNum");
                }
            }
        }

        [Column(Storage = "_region", DbType = "VarChar(15)")]
        public string Region
        {
            get { return _region; }
            set
            {
                if ((_region != value))
                {
                    SendPropertyChanging();
                    _region = value;
                    SendPropertyChanged("Region");
                }
            }
        }

        [Column(Storage = "_st", DbType = "VarChar(50)")]
        public string St
        {
            get { return _st; }
            set
            {
                if ((_st != value))
                {
                    SendPropertyChanging();
                    _st = value;
                    SendPropertyChanged("St");
                }
            }
        }

        [Column(Storage = "_userInact", DbType = "Char(1)")]
        public char? UserInact
        {
            get { return _userInact; }
            set
            {
                if ((_userInact != value))
                {
                    SendPropertyChanging();
                    _userInact = value;
                    SendPropertyChanged("UserInact");
                }
            }
        }

        [Column(Storage = "_userLevel", DbType = "Char(4)")]
        public string UserLevel
        {
            get { return _userLevel; }
            set
            {
                if ((_userLevel != value))
                {
                    SendPropertyChanging();
                    _userLevel = value;
                    SendPropertyChanged("UserLevel");
                }
            }
        }

        [Column(Storage = "_userName", DbType = "VarChar(20) NOT NULL", CanBeNull = false)]
        public string UserName
        {
            get { return _userName; }
            set
            {
                if ((_userName != value))
                {
                    SendPropertyChanging();
                    _userName = value;
                    SendPropertyChanged("UserName");
                }
            }
        }

        [Column(Storage = "_zip", DbType = "VarChar(50)")]
        public string Zip
        {
            get { return _zip; }
            set
            {
                if ((_zip != value))
                {
                    SendPropertyChanging();
                    _zip = value;
                    SendPropertyChanged("Zip");
                }
            }
        }

        [Column(Storage = "_fBUserName", DbType = "VarChar(10)")]
        public string FBUserName
        {
            get { return _fBUserName; }
            set
            {
                if ((_fBUserName != value))
                {
                    SendPropertyChanging();
                    _fBUserName = value;
                    SendPropertyChanged("FBUserName");
                }
            }
        }

        [Column(Storage = "_fBPassWord", DbType = "VarChar(10)")]
        public string FBPassWord
        {
            get { return _fBPassWord; }
            set
            {
                if ((_fBPassWord != value))
                {
                    SendPropertyChanging();
                    _fBPassWord = value;
                    SendPropertyChanged("FBPassWord");
                }
            }
        }

        [Column(Storage = "_xYMUserName", DbType = "VarChar(12)")]
        public string XYMUserName
        {
            get { return _xYMUserName; }
            set
            {
                if ((_xYMUserName != value))
                {
                    SendPropertyChanging();
                    _xYMUserName = value;
                    SendPropertyChanged("XYMUserName");
                }
            }
        }

        [Column(Storage = "_xYMPassword", DbType = "VarChar(12)")]
        public string XYMPassword
        {
            get { return _xYMPassword; }
            set
            {
                if ((_xYMPassword != value))
                {
                    SendPropertyChanging();
                    _xYMPassword = value;
                    SendPropertyChanged("XYMPassword");
                }
            }
        }

        [Column(Storage = "_userID", DbType = "Int")]
        public int? UserID
        {
            get { return _userID; }
            set
            {
                if ((_userID != value))
                {
                    SendPropertyChanging();
                    _userID = value;
                    SendPropertyChanged("userID");
                }
            }
        }

        [Column(Storage = "_uid", DbType = "VarChar(500)")]
        public string Uid
        {
            get { return _uid; }
            set
            {
                if ((_uid != value))
                {
                    SendPropertyChanging();
                    _uid = value;
                    SendPropertyChanged("uid");
                }
            }
        }

        [Column(Name = "RecID", Storage = "_employeeID", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int EmployeeID
        {
            get { return _employeeID; }
            set
            {
                if (_employeeID != value)
                {
                    SendPropertyChanging();
                    _employeeID = value;
                    SendPropertyChanged("EmployeeID");
                }
            }
        }

        [Column(Name="DefaultOperatingCenterID", Storage="_defaultOperatingCenterID", DbType="Int NOT NULL")]
        public int DefaultOperatingCenterID
        {
            get { return _defaultOperatingCenterID; }
            set
            {
                if (_defaultOperatingCenterID!=value)
                {
                    SendPropertyChanging();
                    _defaultOperatingCenterID = value;
                    SendPropertyChanged("DefaultOperatingCenterID");
                }
            }
        }

        [Column(Name="WorkBasket", Storage="_workBasket", DbType="VarChar(9)")]
        public string WorkBasket
        {
            get { return _workBasket; }
            set
            {
                if ((_workBasket != value))
                {
                    SendPropertyChanging();
                    _workBasket = value;
                    SendPropertyChanged("WorkBasket");
                }
            }
        }

        #endregion

        #region Association Properties

        [Association(Name = "Employee_WorkOrder", Storage = "_requestedWorkOrders", OtherKey = "RequestingEmployeeID")]
        public EntitySet<WorkOrder> RequestedWorkOrders
        {
            get { return _requestedWorkOrders; }
            set { _requestedWorkOrders.Assign(value); }
        }

        [Association(Name = "CreatedBy_WorkOrder", Storage = "_createdWorkOrders", OtherKey = "CreatorID")]
        public EntitySet<WorkOrder> CreatedWorkOrders
        {
            get { return _createdWorkOrders; }
            set { _createdWorkOrders.Assign(value); }
        }

        [Association(Name = "ResponsibleEmployee_WorkOrderDescriptionChange", Storage = "_workOrderDescriptionChanges", OtherKey = "ResponsibleEmployeeID")]
        public EntitySet<WorkOrderDescriptionChange> WorkOrderDescriptionChanges
        {
            get { return _workOrderDescriptionChanges; }
            set { _workOrderDescriptionChanges.Assign(value); }
        }

        [Association(Name = "MaterialsApprovedBy_WorkOrder", Storage = "_approvedMaterialsWorkOrders", OtherKey = "MaterialsApprovedByID")]
        public EntitySet<WorkOrder> ApprovedMaterialsWorkOrders
        {
            get { return _approvedMaterialsWorkOrders; }
            set { _approvedMaterialsWorkOrders.Assign(value); }
        }

        [Association(Name = "AssignedTo_WorkOrder", Storage = "_assignedWorkOrders", OtherKey = "AssignedToID")]
        public EntitySet<EmployeeWorkOrder> AssignedWorkOrders
        {
            get { return _assignedWorkOrders; }
            set { _assignedWorkOrders.Assign(value); }
        }

        [Association(Name = "ApprovedBy_WorkOrder", Storage = "_approvedWorkOrders", OtherKey = "ApprovedByID")]
        public EntitySet<WorkOrder> ApprovedWorkOrders
        {
            get { return _approvedWorkOrders; }
            set { _approvedWorkOrders.Assign(value); }
        }

        [Association(Name = "CompletedBy_WorkOrder", Storage = "_workOrdersCompleted", OtherKey = "CompletedByID")]
        public EntitySet<WorkOrder> WorkOrdersCompleted
        {
            get { return _workOrdersCompleted; }
            set { _workOrdersCompleted.Assign(value); }
        }

        [Association(Name = "CancelledBy_WorkOrder", Storage = "_workOrdersCancelled", OtherKey = "CancelledByID")]
        public EntitySet<WorkOrder> WorkOrdersCancelled
        {
            get { return _workOrdersCancelled; }
            set { _workOrdersCancelled.Assign(value); }
        }

        [Association(Name = "ApprovedBy_Restoration", Storage = "_restorationsApproved", OtherKey = "ApprovedByID")]
        public EntitySet<Restoration> RestorationsApproved
        {
            get { return _restorationsApproved; }
            set { _restorationsApproved.Assign(value); }
        }

        [Association(Name = "RejectedBy_Restoration", Storage = "_restorationsRejected", OtherKey = "RejectedByID")]
        public EntitySet<Restoration> RestorationsRejected
        {
            get { return _restorationsRejected; }
            set { _restorationsRejected.Assign(value); }
        }

        [Association(Name = "CreatedBy_Document", Storage = "_documentsAdded", OtherKey = "CreatedByID")]
        public EntitySet<Document> DocumentsAdded
        {
            get { return _documentsAdded; }
            set { _documentsAdded.Assign(value); }
        }
        
        [Association(Name = "ModifiedBy_Document", Storage = "_documentsModified", OtherKey = "ModifiedByID")]
        public EntitySet<Document> DocumentsModified
        {
            get { return _documentsModified; }
            set { _documentsModified.Assign(value); }
        }
        
        [Association(Name = "OperatingCenter_Employee", Storage = "_defaultOperatingCenter", ThisKey = "DefaultOperatingCenterID", IsForeignKey = true)]
        public OperatingCenter DefaultOperatingCenter
        {
            get { return _defaultOperatingCenter.Entity; }
            set
            {
                var previousValue = _defaultOperatingCenter.Entity;
                if ((previousValue != value)
                    || (_defaultOperatingCenter.HasLoadedOrAssignedValue == false))
                {
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _defaultOperatingCenter.Entity = null;
                        previousValue.Employees.Remove(this);
                    }
                    _defaultOperatingCenter.Entity = value;
                    if (value != null)
                    {
                        value.Employees.Add(this);
                        _defaultOperatingCenterID = value.OperatingCenterID;
                    }
                    else
                        _defaultOperatingCenterID = default(int);
                    SendPropertyChanged("OperatingCenter");
                }
            }
        }

        [Association(Name = "OfficeAssignment_WorkOrder", Storage = "_officeAssignedWorkOrders", OtherKey = "OfficeAssignmentID")]
        public EntitySet<WorkOrder> OfficeAssignedWorkOrders
        {
            get { return _officeAssignedWorkOrders; }
            set { _officeAssignedWorkOrders.Assign(value); }
        }

        [Association(Name = "Employee_ReportViewing", Storage = "_reportViewings", OtherKey = "EmployeeID")]
        public EntitySet<ReportViewing> ReportViewings
        {
            get { return _reportViewings; }
            set { _reportViewings.Assign(value); }
        }

        [Association(Name = "Employee_OperatingCentersUsers", Storage = "_operatingCentersUsers", OtherKey = "EmployeeID")]
        public EntitySet<OperatingCenterUser> OperatingCentersUsers
        {
            get { return _operatingCentersUsers; }
            set { _operatingCentersUsers.Assign(value); }
        }
        
        [Association(Name = "Employee_Roles", Storage = "_roles",
            OtherKey = "EmployeeId")]
        public EntitySet<Role> Roles
        {
            get => _roles;
            set => _roles.Assign(value);
        }
        
        [Association(Name = "Employee_AggregateRoles", Storage = "_aggregateRoles",
            OtherKey = "UserId")]
        public EntitySet<AggregateRole> AggregateRoles
        {
            get => _aggregateRoles;
            set => _aggregateRoles.Assign(value);
        }
        
        #endregion

        #endregion

        #region Constructors

        public Employee()
        {
            _requestedWorkOrders =
                new EntitySet<WorkOrder>(
                    attach_RequestedWorkOrders,
                    detach_RequestedWorkOrders);
            _createdWorkOrders =
                new EntitySet<WorkOrder>(
                    attach_CreatedWorkOrders,
                    detach_CreatedWorkOrders);
            _assignedWorkOrders =
                new EntitySet<EmployeeWorkOrder>(
                    attach_AssignedWorkOrders,
                    detach_AssignedWorkOrders);
            _workOrderDescriptionChanges =
                new EntitySet<WorkOrderDescriptionChange>(
                    attach_WorkOrderDescriptionChanges,
                    detach_WorkOrderDescriptionChanges);
            _approvedMaterialsWorkOrders =
                new EntitySet<WorkOrder>(
                    attach_ApprovedMaterialsWorkOrders,
                    detach_ApprovedMaterialsWorkOrders);
            _approvedWorkOrders =
                new EntitySet<WorkOrder>(
                    attach_ApprovedWorkOrders,
                    detach_ApprovedWorkOrders);
            _restorationsApproved =
                new EntitySet<Restoration>(
                    attach_RestorationsApproved,
                    detach_RestorationsApproved);
            _restorationsRejected =
                new EntitySet<Restoration>(
                    attach_RestorationsRejected,
                    detach_RestorationsRejected);
            _workOrdersCompleted = new EntitySet<WorkOrder>(
                attach_WorkOrdersCompleted,
                detach_WorkOrdersCompleted);
            _workOrdersCancelled = new EntitySet<WorkOrder>(
                attach_WorkOrdersCancelled,
                detach_WorkOrdersCancelled);
            _documentsAdded = new EntitySet<Document>(
                attach_DocumentsAdded,
                detach_DocumentsAdded);
            _documentsModified = new EntitySet<Document>(
                attach_DocumentsModified,
                detach_DocumentsModified);
            _officeAssignedWorkOrders = new EntitySet<WorkOrder>(
                    attach_OfficeAssignedWorkOrders,
                    detach_OfficeAssignedWorkOrders);
            _reportViewings = new EntitySet<ReportViewing>(
                attach_ReportViewings, detach_ReportViewings);
            _operatingCentersUsers = new EntitySet<OperatingCenterUser>(
                    attach_OperatingCentersUsers,
                    detach_OperatingCentersUsers
                );
            _roles = new EntitySet<Role>();
            _aggregateRoles = new EntitySet<AggregateRole>();
        }

        #endregion

        #region Override Methods

        public override string ToString()
        {
            return FullName;
        }

        public int CompareTo(Employee obj)
        {
            return FullName.CompareTo(obj == null ? null : obj.FullName);
        }

        #endregion

        #region Extension Methods

        // ReSharper disable UnusedPrivateMember
        private void OnValidate(ChangeAction action)
        {
            switch (action)
            {
                case ChangeAction.Insert:
                case ChangeAction.Update:
                    if (String.IsNullOrEmpty(UserName))
                        throw new DomainLogicException("Cannot save an Employee without a UserName.");
                    break;
            }
        }
        // ReSharper restore UnusedPrivateMember

        #endregion

        #region Private Methods

        private void attach_AssignedWorkOrders(EmployeeWorkOrder entity)
        {
            SendPropertyChanging();
            entity.AssignedTo = this;
        }
        private void detach_AssignedWorkOrders(EmployeeWorkOrder entity)
        {
            SendPropertyChanging();
            entity.AssignedTo = null;
        }

        private void attach_ApprovedWorkOrders(WorkOrder entity)
        {
            SendPropertyChanging();
            entity.ApprovedBy = this;
        }
        private void detach_ApprovedWorkOrders(WorkOrder entity)
        {
            SendPropertyChanging();
            entity.ApprovedBy = null;
        }

        private void attach_RequestedWorkOrders(WorkOrder entity)
        {
            SendPropertyChanging();
            entity.RequestingEmployee = this;
        }
        private void detach_RequestedWorkOrders(WorkOrder entity)
        {
            SendPropertyChanging();
            entity.RequestingEmployee = null;
        }

        private void attach_CreatedWorkOrders(WorkOrder entity)
        {
            SendPropertyChanging();
            entity.CreatedBy = this;
        }
        private void detach_CreatedWorkOrders(WorkOrder entity)
        {
            SendPropertyChanging();
            entity.CreatedBy = null;
        }

        private void attach_WorkOrdersCompleted(WorkOrder entity)
        {
            SendPropertyChanging();
            entity.CompletedBy = this;
        }
        private void detach_WorkOrdersCompleted(WorkOrder entity)
        {
            SendPropertyChanging();
            entity.CompletedBy = null;
        }

        private void attach_WorkOrdersCancelled(WorkOrder entity)
        {
            SendPropertyChanging();
            entity.CancelledBy = this;
        }
        private void detach_WorkOrdersCancelled(WorkOrder entity)
        {
            SendPropertyChanging();
            entity.CancelledBy = null;
        }

        private void attach_WorkOrderDescriptionChanges(WorkOrderDescriptionChange entity)
        {
            SendPropertyChanging();
            entity.ResponsibleEmployee = this;
        }
        private void detach_WorkOrderDescriptionChanges(WorkOrderDescriptionChange entity)
        {
            SendPropertyChanging();
            entity.ResponsibleEmployee = null;
        }

        private void attach_ApprovedMaterialsWorkOrders(WorkOrder entity)
        {
            SendPropertyChanging();
            entity.MaterialsApprovedBy = this;
        }
        private void detach_ApprovedMaterialsWorkOrders(WorkOrder entity)
        {
            SendPropertyChanging();
            entity.MaterialsApprovedBy = null;
        }

        private void attach_RestorationsApproved(Restoration entity)
        {
            SendPropertyChanging();
            entity.ApprovedBy = this;
        }
        private void detach_RestorationsApproved(Restoration entity)
        {
            SendPropertyChanging();
            entity.ApprovedBy = null;
        }

        private void attach_RestorationsRejected(Restoration entity)
        {
            SendPropertyChanging();
            entity.RejectedBy = this;
        }
        private void detach_RestorationsRejected(Restoration entity)
        {
            SendPropertyChanging();
            entity.RejectedBy = null;
        }

        private void attach_DocumentsAdded(Document entity)
        {
            SendPropertyChanging();
            entity.EmployeeCreatedBy = this;
        }
        private void detach_DocumentsAdded(Document entity)
        {
            SendPropertyChanging();
            entity.EmployeeCreatedBy = null;
        }

        private void attach_DocumentsModified(Document entity)
        {
            SendPropertyChanging();
            entity.EmployeeModifiedBy = this;
        }
        private void detach_DocumentsModified(Document entity)
        {
            SendPropertyChanging();
            entity.EmployeeModifiedBy = null;
        }

        private void attach_OfficeAssignedWorkOrders(WorkOrder entity)
        {
            SendPropertyChanging();
            entity.OfficeAssignment = this;
        }
        private void detach_OfficeAssignedWorkOrders(WorkOrder entity)
        {
            SendPropertyChanging();
            entity.OfficeAssignment = null;
        }

        private void attach_ReportViewings(ReportViewing entity)
        {
            SendPropertyChanging();
            entity.Employee = this;
        }
        private void detach_ReportViewings(ReportViewing entity)
        {
            SendPropertyChanging();
            entity.Employee = null;
        }

        private void attach_OperatingCentersUsers(OperatingCenterUser entity)
        {
            SendPropertyChanging();
            entity.Employee = this;
        }
        private void detach_OperatingCentersUsers(OperatingCenterUser entity)
        {
            SendPropertyChanging();
            entity.Employee = null;
        }
        
        private void SendPropertyChanging()
        {
            if (PropertyChanging != null)
                PropertyChanging(this, emptyChangingEventArgs);
        }

        private void SendPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Events

        public event PropertyChangingEventHandler PropertyChanging;

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
