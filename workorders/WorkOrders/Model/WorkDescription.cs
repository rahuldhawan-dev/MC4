using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using MMSINC.Exceptions;

namespace WorkOrders.Model
{
    [Table(Name = "dbo.WorkDescriptions")]
    public class WorkDescription : INotifyPropertyChanging, INotifyPropertyChanged
    {
        #region Constants

        private const short MAX_DESCRIPTION_LENGTH = 50;
        public const int WATER_MAIN_BREAK_REPAIR_ID = 74;
        public const int WATER_MAIN_BREAK_REPLACE_ID = 80;
        public const int SERVICE_LINE_RENEWAL_LEAD = 295;
        public static readonly int[] SERVICE_LINE_RENEWALS = { 59, 193, 295 };
        public static readonly int[] SERVICE_LINE_RETIRE = { 60, 298, 313, 314 };
        public static readonly int[] PITCHER_FILTER_REQUIREMENT = { 59, 295, 222, 307 };

        #endregion

        #region Private Static Members

        private static readonly PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        #endregion

        #region Private Members

        private int _workDescriptionID,
                    _assetTypeID,
                    _accountingTypeID,
                    _firstRestorationAccountingCodeID,
                    _firstRestorationProductCodeID;

        private int? _workCategoryID,
                     _secondRestorationAccountingCodeID,
                     _secondRestorationProductCodeID;

        private short _firstRestorationCostBreakdown;

        private bool _revisit, _digitalAsBuiltRequired;

        private short? _secondRestorationCostBreakdown;

        private string _description;

        private decimal _timeToComplete;

        private bool _showBusinessUnit, _showApprovalAccounting, _editOnly, _isActive;

        private EntityRef<AccountingType> _accountingType;

        private EntityRef<AssetType> _assetType;

        private EntityRef<WorkCategory> _workCategory;

        private EntityRef<RestorationAccountingCode>
            _firstRestorationAccountingCode, _secondRestorationAccountingCode;

        private EntityRef<RestorationProductCode> _firstRestorationProductCode,
                                                  _secondRestorationProductCode;

        private readonly EntitySet<WorkOrder> _workOrders;

        private readonly EntitySet<WorkOrderDescriptionChange>
            _toWorkOrderDescriptionChanges, _fromWorkOrderDescriptionChanges;

        #endregion

        #region Properties

        #region Table Column Properties

        [Column(Storage = "_workDescriptionID", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int WorkDescriptionID
        {
            get { return _workDescriptionID; }
            set
            {
                if (_workDescriptionID != value)
                {
                    SendPropertyChanging();
                    _workDescriptionID = value;
                    SendPropertyChanged("WorkDescriptionID");
                }
            }
        }

        [Column(Storage = "_description", DbType = "VarChar(50) NOT NULL")]
        public string Description
        {
            get { return _description; }
            set
            {
                if (value != null && value.Length > MAX_DESCRIPTION_LENGTH)
                    throw new StringTooLongException("Description", MAX_DESCRIPTION_LENGTH);
                if (_description != value)
                {
                    SendPropertyChanging();
                    _description = value;
                    SendPropertyChanged("Description");
                }
            }
        }

        [Column(Storage = "_assetTypeID", DbType = "Int NOT NULL")]
        public int AssetTypeID
        {
            get { return _assetTypeID; }
            set
            {
                if (_assetTypeID != value)
                {
                    if (_assetType.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                }
                SendPropertyChanging();
                _assetTypeID = value;
                SendPropertyChanged("AssetTypeID");
            }
        }

        [Column(Storage = "_accountingTypeID", DbType = "Int NOT NULL")]
        public int AccountingTypeID
        {
            get { return _accountingTypeID; }
            set
            {
                if (_accountingTypeID != value)
                {
                    if (_accountingType.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                }
                SendPropertyChanging();
                _accountingTypeID = value;
                SendPropertyChanged("AccountingTypeID");
            }
        }

        [Column(Storage = "_timeToComplete", DbType = "Decimal(18, 2) NOT NULL")]
        public decimal TimeToComplete
        {
            get { return _timeToComplete; }
            set
            {
                if (value == default(decimal))
                    throw new DomainLogicException("TimeToComplete must not be zero.");
                if (_timeToComplete != value)
                {
                    SendPropertyChanging();
                    _timeToComplete = value;
                    SendPropertyChanged("TimeToComplete");
                }
            }
        }

        [Column(Storage = "_workCategoryID", DbType = "Int")]
        public int? WorkCategoryID
        {
            get { return _workCategoryID; }
            set
            {
                if (_workCategoryID != value)
                {
                    if (_workCategory.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                }
                SendPropertyChanging();
                _workCategoryID = value;
                SendPropertyChanged("WorkCategoryID");
            }
        }

        [Column(Storage = "_firstRestorationAccountingCodeID", DbType = "Int NOT NULL")]
        public int FirstRestorationAccountingCodeID
        {
            get { return _firstRestorationAccountingCodeID; }
            set
            {
                if (_firstRestorationAccountingCodeID != value)
                {
                    if (_firstRestorationAccountingCode.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                }
                SendPropertyChanging();
                _firstRestorationAccountingCodeID = value;
                SendPropertyChanged("FirstRestorationAccountingCodeID");
            }
        }

        [Column(Storage = "_secondRestorationAccountingCodeID", DbType = "Int")]
        public int? SecondRestorationAccountingCodeID
        {
            get { return _secondRestorationAccountingCodeID; }
            set
            {
                if (_secondRestorationAccountingCodeID != value)
                {
                    if (_secondRestorationAccountingCode.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                }
                SendPropertyChanging();
                _secondRestorationAccountingCodeID = value;
                SendPropertyChanged("SecondRestorationAccountingCodeID");
            }
        }

        [Column(Storage = "_firstRestorationProductCodeID", DbType = "Int NOT NULL")]
        public int FirstRestorationProductCodeID
        {
            get { return _firstRestorationProductCodeID; }
            set
            {
                if (_firstRestorationProductCodeID != value)
                {
                    if (_firstRestorationProductCode.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                }
                SendPropertyChanging();
                _firstRestorationProductCodeID = value;
                SendPropertyChanged("FirstRestorationProductCodeID");
            }
        }

        [Column(Storage = "_secondRestorationProductCodeID", DbType = "Int")]
        public int? SecondRestorationProductCodeID
        {
            get { return _secondRestorationProductCodeID; }
            set
            {
                if (_secondRestorationProductCodeID != value)
                {
                    if (_secondRestorationProductCode.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                }
                SendPropertyChanging();
                _secondRestorationProductCodeID = value;
                SendPropertyChanged("SecondRestorationProductCodeID");
            }
        }

        [Column(Storage = "_firstRestorationCostBreakdown", DbType = "TinyInt NOT NULL")]
        public short FirstRestorationCostBreakdown
        {
            get { return _firstRestorationCostBreakdown; }
            set
            {
                if (_firstRestorationCostBreakdown != value)
                {
                    SendPropertyChanging();
                    _firstRestorationCostBreakdown = value;
                    SendPropertyChanged("FirstRestorationCostBreakdown");
                }
            }
        }

        [Column(Storage = "_secondRestorationCostBreakdown", DbType = "TinyInt")]
        public short? SecondRestorationCostBreakdown
        {
            get { return _secondRestorationCostBreakdown; }
            set
            {
                if (_secondRestorationCostBreakdown != value)
                {
                    SendPropertyChanging();
                    _secondRestorationCostBreakdown = value;
                    SendPropertyChanged("SecondRestorationCostBreakdown");
                }
            }
        }

        [Column(Storage = "_showBusinessUnit", DbType = "Bit NOT NULL")]
        public bool ShowBusinessUnit
        {
            get { return _showBusinessUnit; }
            set
            {
                if (_showBusinessUnit != value)
                {
                    SendPropertyChanging();
                    _showBusinessUnit = value;
                    SendPropertyChanged("ShowBusinessUnit");
                }
            }
        }

        [Column(Storage = "_showApprovalAccounting", DbType = "Bit NOT NULL")]
        public bool ShowApprovalAccounting
        {
            get { return _showApprovalAccounting; }
            set
            {
                if (_showApprovalAccounting != value)
                {
                    SendPropertyChanging();
                    _showApprovalAccounting = value;
                    SendPropertyChanged("ShowApprovalAccounting");
                }
            }
        }

        [Column(Storage = "_editOnly", DbType = "Bit NOT NULL")]
        public bool EditOnly
        {
            get { return _editOnly; }
            set
            {
                if (_editOnly != value)
                {
                    SendPropertyChanging();
                    _editOnly = value;
                    SendPropertyChanged("EditOnly");
                }
            }
        }

        [Column(Storage = "_isActive", DbType = "Bit NOT NULL")]
        public bool IsActive
        {
            get => _isActive;
            set
            {
                if (_isActive != value)
                {
                    SendPropertyChanging();
                    _isActive = value;
                    SendPropertyChanged("IsActive");
                }
            }
        }

        #endregion

        #region Association Properties

        [Column(Storage = "_revisit", DbType = "Bit NOT NULL")]
        public bool Revisit
        {
            get { return _revisit; }
            set
            {
                if (_revisit != value)
                {
                    SendPropertyChanging();
                    _revisit = value;
                    SendPropertyChanged("Revisit");
                }
            }
        }

        [Column(Storage = "_digitalAsBuiltRequired", DbType = "Bit NOT NULL", UpdateCheck = UpdateCheck.Never)]
        public bool DigitalAsBuiltRequired
        {
            get => _digitalAsBuiltRequired;
            set
            {
                if (_digitalAsBuiltRequired != value)
                {
                    SendPropertyChanging();
                    _digitalAsBuiltRequired = value;
                    SendPropertyChanged("DigitalAsBuiltRequired");
                }
            }
        }

        [Association(Name = "AssetType_WorkDescription", Storage = "_assetType", ThisKey = "AssetTypeID", IsForeignKey = true)]
        public AssetType AssetType
        {
            get { return _assetType.Entity; }
            set
            {
                var previousValue = _assetType.Entity;
                if ((previousValue != value)
                    || (_assetType.HasLoadedOrAssignedValue == false))
                {
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _assetType.Entity = null;
                        previousValue.WorkDescriptions.Remove(this);
                    }
                    _assetType.Entity = value;
                    if (value != null)
                    {
                        value.WorkDescriptions.Add(this);
                        _assetTypeID = value.AssetTypeID;
                    }
                    else
                        _assetTypeID = default(int);
                    SendPropertyChanged("AssetType");
                }
            }
        }

        [Association(Name = "AccountingType_WorkDescription", Storage = "_accountingType", 
            ThisKey = "AccountingTypeID", IsForeignKey = true)]
        public AccountingType AccountingType
        {
            get { return _accountingType.Entity; }
            set
            {
                var previousValue = _accountingType.Entity;
                if ((previousValue != value)
                    || (_accountingType.HasLoadedOrAssignedValue == false))
                {
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _accountingType.Entity = null;
                        previousValue.WorkDescriptions.Remove(this);
                    }
                    _accountingType.Entity = value;
                    if (value != null)
                    {
                        value.WorkDescriptions.Add(this);
                        _accountingTypeID = value.AccountingTypeID;
                    }
                    else
                        _accountingTypeID = default(int);
                    SendPropertyChanged("AccountingType");
                }
            }
        }

        [Association(Name = "WorkCategory_WorkDescription", Storage = "_workCategory", ThisKey = "WorkCategoryID", IsForeignKey = true)]
        public WorkCategory WorkCategory
        {
            get { return _workCategory.Entity; }
            set
            {
                var previousValue = _workCategory.Entity;
                if ((previousValue != value)
                    || (_workCategory.HasLoadedOrAssignedValue == false))
                {
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _workCategory.Entity = null;
                        previousValue.WorkDescriptions.Remove(this);
                    }
                    _workCategory.Entity = value;
                    if (value != null)
                    {
                        value.WorkDescriptions.Add(this);
                        _workCategoryID = value.WorkCategoryID;
                    }
                    else
                        _workCategoryID = default(int);
                    SendPropertyChanged("WorkCategory");
                }
            }
        }

        [Association(Name = "WorkDescription_WorkOrder", Storage = "_workOrders", OtherKey = "WorkDescriptionID")]
        public EntitySet<WorkOrder> WorkOrders
        {
            get { return _workOrders; }
            set { _workOrders.Assign(value); }
        }

        [Association(Name = "WorkDescription_ToWorkOrderDescriptionChange", Storage = "_toWorkOrderDescriptionChanges", OtherKey = "ToWorkDescriptionID")]
        public EntitySet<WorkOrderDescriptionChange> ToWorkOrderDescriptionChanges
        {
            get { return _toWorkOrderDescriptionChanges; }
            set { _toWorkOrderDescriptionChanges.Assign(value); }
        }

        [Association(Name = "WorkDescription_FromWorkOrderDescriptionChange", Storage=
            "_fromWorkOrderDescriptionChanges", OtherKey = "FromWorkDescriptionID")]
        public EntitySet<WorkOrderDescriptionChange> FromWorkOrderDescriptionChanges
        {
            get { return _fromWorkOrderDescriptionChanges; }
            set { _fromWorkOrderDescriptionChanges.Assign(value); }
        }

        [Association(Name = "FirstRestorationAccountingCode_WorkDescription", Storage = "_firstRestorationAccountingCode", ThisKey = "FirstRestorationAccountingCodeID", IsForeignKey = true)]
        public RestorationAccountingCode FirstRestorationAccountingCode
        {
            get { return _firstRestorationAccountingCode.Entity; }
            set
            {
                RestorationAccountingCode previousValue = _firstRestorationAccountingCode.Entity;
                if ((previousValue != value)
                    || (_firstRestorationAccountingCode.HasLoadedOrAssignedValue == false))
                {
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _firstRestorationAccountingCode.Entity = null;
                        previousValue.PrimaryWorkDescriptions.Remove(this);
                    }
                    _firstRestorationAccountingCode.Entity = value;
                    if (value != null)
                    {
                        value.PrimaryWorkDescriptions.Add(this);
                        _firstRestorationAccountingCodeID = value.RestorationAccountingCodeID;
                    }
                    else
                        _firstRestorationAccountingCodeID = default(int);
                    SendPropertyChanged("FirstRestorationAccountingCode");
                }
            }
        }

        [Association(Name = "SecondRestorationAccountingCode_WorkDescription", Storage = "_secondRestorationAccountingCode", ThisKey = "SecondRestorationAccountingCodeID", IsForeignKey = true)]
        public RestorationAccountingCode SecondRestorationAccountingCode
        {
            get { return _secondRestorationAccountingCode.Entity; }
            set
            {
                RestorationAccountingCode previousValue = _secondRestorationAccountingCode.Entity;
                if ((previousValue != value)
                    || (_secondRestorationAccountingCode.HasLoadedOrAssignedValue == false))
                {
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _secondRestorationAccountingCode.Entity = null;
                        previousValue.SecondaryWorkDescriptions.Remove(this);
                    }
                    _secondRestorationAccountingCode.Entity = value;
                    if (value != null)
                    {
                        value.SecondaryWorkDescriptions.Add(this);
                        _secondRestorationAccountingCodeID = value.RestorationAccountingCodeID;
                    }
                    else
                        _secondRestorationAccountingCodeID = default(int);
                    SendPropertyChanged("SecondRestorationAccountingCode");
                }
            }
        }

        [Association(Name = "FirstRestorationProductCode_WorkDescription", Storage = "_firstRestorationProductCode", ThisKey = "FirstRestorationProductCodeID", IsForeignKey = true)]
        public RestorationProductCode FirstRestorationProductCode
        {
            get { return _firstRestorationProductCode.Entity; }
            set
            {
                RestorationProductCode previousValue = _firstRestorationProductCode.Entity;
                if ((previousValue != value)
                    || (_firstRestorationProductCode.HasLoadedOrAssignedValue == false))
                {
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _firstRestorationProductCode.Entity = null;
                        previousValue.PrimaryWorkDescriptions.Remove(this);
                    }
                    _firstRestorationProductCode.Entity = value;
                    if (value != null)
                    {
                        value.PrimaryWorkDescriptions.Add(this);
                        _firstRestorationProductCodeID = value.RestorationProductCodeID;
                    }
                    else
                        _firstRestorationProductCodeID = default(int);
                    SendPropertyChanged("FirstRestorationProductCode");
                }
            }
        }

        [Association(Name = "SecondRestorationProductCode_WorkDescription", Storage = "_secondRestorationProductCode", ThisKey = "SecondRestorationProductCodeID", IsForeignKey = true)]
        public RestorationProductCode SecondRestorationProductCode
        {
            get { return _secondRestorationProductCode.Entity; }
            set
            {
                RestorationProductCode previousValue = _secondRestorationProductCode.Entity;
                if ((previousValue != value)
                    || (_secondRestorationProductCode.HasLoadedOrAssignedValue == false))
                {
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _secondRestorationProductCode.Entity = null;
                        previousValue.SecondaryWorkDescriptions.Remove(this);
                    }
                    _secondRestorationProductCode.Entity = value;
                    if (value != null)
                    {
                        value.SecondaryWorkDescriptions.Add(this);
                        _secondRestorationProductCodeID = value.RestorationProductCodeID;
                    }
                    else
                        _secondRestorationProductCodeID = default(int);
                    SendPropertyChanged("SecondRestorationProductCode");
                }
            }
        }

        #endregion

        #region Logical Properties

        /// <summary>
        /// Used to determine the showing of the Main Break tab 
        /// </summary>
        public bool IsMainReplaceOrRepair
        {
            get { return WorkDescriptionID == WATER_MAIN_BREAK_REPAIR_ID || WorkDescriptionID == WATER_MAIN_BREAK_REPLACE_ID; }
        }

        public bool IsServiceLineRenewal
        {
            get { return SERVICE_LINE_RENEWALS.Contains(WorkDescriptionID); }
        }

        public bool IsServiceLineRetire
        {
            get { return SERVICE_LINE_RETIRE.Contains(WorkDescriptionID); }
        }

        public bool RequiresPitcherFilter =>
            PITCHER_FILTER_REQUIREMENT.Contains(WorkDescriptionID);

        public string FirstRestorationAccountingString
        {
            get
            {
                return String.Format("{0} {1}",
                    FirstRestorationAccountingCode,
                    FirstRestorationProductCode);
            }
        }

        public virtual string SecondRestorationAccountingString
        {
            get
            {
                if (SecondRestorationAccountingCode != null)
                {
                    return String.Format("{0} {1}",
                        SecondRestorationAccountingCode,
                        SecondRestorationProductCode);
                }
                return null;
            }
        }

        public string FirstRestorationCostBreakdownString
        {
            get
            {
                return FirstRestorationCostBreakdown + "%";
            }
        }

        public string SecondRestorationCostBreakdownString
        {
            get
            {
                return (SecondRestorationCostBreakdown != null)
                           ? SecondRestorationCostBreakdown + "%"
                           : string.Empty;
            }
        }

        #endregion

        #endregion

        #region Constructors

        public WorkDescription()
        {
            _workOrders = new EntitySet<WorkOrder>(attach_WorkOrders,
                detach_WorkOrders);
            _toWorkOrderDescriptionChanges =
                new EntitySet<WorkOrderDescriptionChange>(
                    attach_ToWorkOrderDescriptionChanges,
                    detach_ToWorkOrderDescriptionChanges);
            _fromWorkOrderDescriptionChanges = 
                new EntitySet<WorkOrderDescriptionChange>(
                    attach_FromWorkOrderDescriptionChanges, 
                    detach_FromWorkOrderDescriptionChanges);
            _assetType = default(EntityRef<AssetType>);
            _workCategory = default(EntityRef<WorkCategory>);
            _accountingType = default(EntityRef<AccountingType>);
        }

        #endregion

        #region Private Methods

        // ReSharper disable UnusedPrivateMember
        private void OnValidate(ChangeAction action)
        {
            switch (action)
            {
                case ChangeAction.Insert:
                case ChangeAction.Update:
                    if (String.IsNullOrEmpty(Description))
                        throw new DomainLogicException("Description cannot be null.");
                    if (AssetType == null)
                        throw new DomainLogicException("AssetType cannot be null.");
                    if (TimeToComplete == default(decimal))
                        throw new DomainLogicException("TimeToComplete cannot be 0.");
                    break;
            }
        }
        // ReSharper restore UnusedPrivateMember

        private void attach_WorkOrders(WorkOrder entity)
        {
            SendPropertyChanging();
            entity.WorkDescription = this;
        }

        private void detach_WorkOrders(WorkOrder entity)
        {
            SendPropertyChanging();
            entity.WorkDescription = null;
        }

        private void attach_ToWorkOrderDescriptionChanges(WorkOrderDescriptionChange entity)
        {
            SendPropertyChanging();
            entity.ToWorkDescription = this;
        }

        private void detach_ToWorkOrderDescriptionChanges(WorkOrderDescriptionChange entity)
        {
            SendPropertyChanging();
            entity.ToWorkDescription = null;
        }

        private void attach_FromWorkOrderDescriptionChanges(WorkOrderDescriptionChange entity)
        {
            SendPropertyChanging();
            entity.FromWorkDescription = this;
        }

        private void detach_FromWorkOrderDescriptionChanges(WorkOrderDescriptionChange entity)
        {
            SendPropertyChanging();
            entity.FromWorkDescription = null;
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

        #region Exposed Methods

        public override string ToString()
        {
            return Description;
        }

        public virtual AccountingTypeEnum GetAccountingTypeEnum()
        {
            return AccountingType.TypeEnum;
        }

        public int CompareTo(object other)
        {
            var otherDescription = other as WorkDescription;
            return otherDescription == null ? -1 : CompareTo(otherDescription);
        }

        public int CompareTo(WorkDescription other)
        {
            return
                Description.CompareTo(other == null ? null : other.Description);
        }

        public static object WorkDescriptionToJson(WorkDescription wd)
        {
            return new {
                Id = wd.WorkDescriptionID,
                wd.Description,
                wd.DigitalAsBuiltRequired
            };
        }

        #endregion

        #region Events

        public event PropertyChangingEventHandler PropertyChanging;

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}

