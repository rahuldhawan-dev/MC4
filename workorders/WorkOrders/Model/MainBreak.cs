using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using MMSINC.Exceptions;

namespace WorkOrders.Model
{
    [Table(Name = "dbo.MainBreaks")]
    public class MainBreak : INotifyPropertyChanging, INotifyPropertyChanged
    {
        #region Constants

        private const short MAX_MATERIAL_LENGTH = 20;
        private const short MAX_TYPEOFMAINBREAK_LENGTH = 30;
        //private const short MAX_MAPPAGE_LENGTH = 10;

        #endregion

        #region Private Static Members

        private static readonly PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        #endregion

        #region Private Members

        private int _mainBreakID,
            _workOrderID,
            _serviceSizeID,
            _mainConditionID,
            _customersAffected,
            _mainBreakSoilConditionID,
            _mainBreakDisinfectionMethodID,
            _mainBreakFlushMethodID;

        private int? _mainBreakMaterialID,
            _mainFailureTypeID,
            _footageReplaced,
            _replacedWithId;

        private decimal _depth, _shutdownTime;
        
        private decimal? _chlorineResidual;

        private bool _boilAlertIssued;

        private EntityRef<WorkOrder> _workOrder;

        private EntityRef<Coordinate> _coordinate;

        private EntityRef<MainCondition> _mainCondition;
        
        private EntityRef<MainBreakSoilCondition> _mainBreakSoilCondition;

        private EntityRef<MainFailureType> _mainFailureType;

        private EntityRef<ServiceSize> _serviceSize;

        private EntityRef<MainBreakMaterial> _mainBreakMaterial, _replacedWith;

        private EntityRef<MainBreakDisinfectionMethod> _mainBreakDisinfectionMethod;
        
        private EntityRef<MainBreakFlushMethod> _mainBreakFlushMethod;

        #endregion

        #region Properties

        [Column(Storage = "_mainBreakMaterialID", DbType = "int NOT NULL", CanBeNull = false)]
        public int? MainBreakMaterialID
        {
            get { return _mainBreakMaterialID; }
            set
            {
                if (_mainBreakMaterialID != value)
                {
                    SendPropertyChanging();
                    _mainBreakMaterialID = value;
                    SendPropertyChanged("MainBreakMaterialID");
                }
            }
        }

        [Association(Name = "WorkOrder_MainBreak", Storage = "_workOrder", ThisKey = "WorkOrderID", IsForeignKey = true)]
        public WorkOrder WorkOrder
        {
            get { return _workOrder.Entity; }
            set
            {
                var previousValue = _workOrder.Entity;
                if ((previousValue != value)
                    || (_workOrder.HasLoadedOrAssignedValue == false))
                {
                    if (previousValue != null && value != null)
                        throw new DomainLogicException("Cannot change the WorkOrder of a MainBreak record once it has been set.");
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _workOrder.Entity = null;
                        previousValue.MainBreaks.Remove(this);
                    }
                    _workOrder.Entity = value;
                    if (value != null)
                    {
                        value.MainBreaks.Add(this);
                        _workOrderID = value.WorkOrderID;
                    }
                    else
                    {
                        _workOrderID = default(int);
                    }
                    SendPropertyChanged("WorkOrder");
                }
            }
        }

        [Column(Storage = "_mainBreakID", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int MainBreakID
        {
            get { return _mainBreakID; }
            set
            {
                if (_mainBreakID != value)
                {
                    SendPropertyChanging();
                    _mainBreakID = value;
                    SendPropertyChanged("MainBreakID");
                }
            }
        }

        [Column(Storage = "_workOrderID", DbType = "Int NOT NULL")]
        public int WorkOrderID
        {
            get { return _workOrderID; }
            set
            {
                if (_workOrderID != value)
                {
                    if (_workOrder.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                    SendPropertyChanging();
                    _workOrderID = value;
                    SendPropertyChanged("WorkOrderID");
                }
            }
        }

        [Column(Storage = "_mainFailureTypeID", DbType = "Int")]
        public int? MainFailureTypeID
        {
            get { return _mainFailureTypeID; }
            set
            {
                if (_mainFailureTypeID != value)
                {
                    if (_mainFailureType.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                    SendPropertyChanging();
                    _mainFailureTypeID = value;
                    SendPropertyChanged("MainFailureTypeID");
                }
            }
        }

        [Column(Storage = "_serviceSizeID", DbType = "Int NOT NULL")]
        public int ServiceSizeID
        {
            get { return _serviceSizeID; }
            set
            {
                if (_serviceSizeID != value)
                {
                    if (_serviceSize.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                    SendPropertyChanging();
                    _serviceSizeID = value;
                    SendPropertyChanged("ServiceSizeID");
                }
            }
        }

        [Column(Storage = "_depth", DbType = "Decimal(18,1) NOT NULL")]
        public decimal Depth
        {
            get { return _depth; }
            set
            {
                if (_depth != value)
                {
                    SendPropertyChanging();
                    _depth = value;
                    SendPropertyChanged("Depth");
                }
            }
        }

        [Column(Storage = "_chlorineResidual", DbType = "Decimal(3,1) NULL")]
        public decimal? ChlorineResidual
        {
            get { return _chlorineResidual; }
            set
            {
                if (_chlorineResidual != value)
                {
                    SendPropertyChanging();
                    _chlorineResidual = value;
                    SendPropertyChanged("ChlorineResidual");
                }
            }
        }

        [Column(Storage = "_mainConditionID", DbType = "Int NOT NULL")]
        public int MainConditionID
        {
            get { return _mainConditionID; }
            set
            {
                if (_mainConditionID != value)
                {
                    if (_mainCondition.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                    SendPropertyChanging();
                    _mainConditionID = value;
                    SendPropertyChanged("MainConditionID");
                }
            }
        }

        [Column(Storage = "_mainBreakSoilConditionID", DbType = "Int NOT NULL")]
        public int MainBreakSoilConditionID
        {
            get { return _mainBreakSoilConditionID; }
            set
            {
                if (_mainBreakSoilConditionID != value)
                {
                    if (_mainBreakSoilCondition.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                    SendPropertyChanging();
                    _mainBreakSoilConditionID = value;
                    SendPropertyChanged("MainBreakSoilConditionID");
                }
            }
        }

        [Column(Storage = "_mainBreakDisinfectionMethodID", DbType = "Int NOT NULL")]
        public int MainBreakDisinfectionMethodID
        {
            get { return _mainBreakDisinfectionMethodID; }
            set
            {
                if (_mainBreakDisinfectionMethodID != value)
                {
                    if (_mainBreakDisinfectionMethod.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                    SendPropertyChanging();
                    _mainBreakDisinfectionMethodID = value;
                    SendPropertyChanged("MainBreakDisinfectionMethodID");
                }
            }
        }

        [Column(Storage = "_mainBreakFlushMethodID", DbType = "Int NOT NULL")]
        public int MainBreakFlushMethodID
        {
            get { return _mainBreakFlushMethodID; }
            set
            {
                if (_mainBreakFlushMethodID != value)
                {
                    if (_mainBreakFlushMethod.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                    SendPropertyChanging();
                    _mainBreakFlushMethodID = value;
                    SendPropertyChanged("MainBreakFlushMethodID");
                }
            }
        }

        [Column(Storage = "_customersAffected", DbType = "Int NOT NULL")]
        public int CustomersAffected
        {
            get { return _customersAffected; }
            set
            {
                if (_customersAffected != value)
                {
                    SendPropertyChanging();
                    _customersAffected = value;
                    SendPropertyChanged("CustomersAffected");
                }
            }
        }

        [Column(Storage = "_footageReplaced", DbType = "Int NULL")]
        public int? FootageReplaced
        {
            get { return _footageReplaced; }
            set
            {
                if (_footageReplaced != value)
                {
                    SendPropertyChanging();
                    _footageReplaced = value;
                    SendPropertyChanged("FootageReplaced");
                }
            }
        }

        [Column(Storage = "_shutdownTime", DbType = "Decimal(5,1) NOT NULL")]
        public decimal ShutdownTime
        {
            get { return _shutdownTime; }
            set
            {
                if (_shutdownTime != value)
                {
                    SendPropertyChanging();
                    _shutdownTime = value;
                    SendPropertyChanged("ShutdownTime");
                }
            }
        }

        //[Column(Storage = "_coordinateID", DbType = "Int")]
        //public int? CoordinateID
        //{
        //    get { return _coordinateID; }
        //    set
        //    {
        //        if (_coordinateID != value)
        //        {
        //            if (_coordinate.HasLoadedOrAssignedValue)
        //                throw new ForeignKeyReferenceAlreadyHasValueException();
        //            SendPropertyChanging();
        //            _coordinateID = value;
        //            SendPropertyChanged("CoordinateID");
        //        }
        //    }
        //}

        [Column(Storage = "_boilAlertIssued", DbType = "TinyInt")]
        public bool BoilAlertIssued
        {
            get { return _boilAlertIssued; }
            set
            {
                if (_boilAlertIssued != value)
                {
                    SendPropertyChanging();
                    _boilAlertIssued = value;
                    SendPropertyChanged("BoilAlertIssued");
                }
            }
        }
        
        //[Association(Name = "MainBreak_MainBreakValveOperation", Storage = "_mainBreakValveOperations", OtherKey = "MainBreakID")]
        //public EntitySet<MainBreakValveOperation> MainBreakValveOperations
        //{
        //    get { return _mainBreakValveOperations; }
        //    set { _mainBreakValveOperations.Assign(value); }
        //}

        //[Association(Name = "Coordinate_MainBreak", Storage = "_coordinate", ThisKey = "CoordinateID", IsForeignKey = true)]
        //public Coordinate Coordinate
        //{
        //    get { return _coordinate.Entity; }
        //    set
        //    {
        //        var previousValue = _coordinate.Entity;
        //        if ((previousValue != value)
        //             || (_coordinate.HasLoadedOrAssignedValue == false))
        //        {
        //            SendPropertyChanging();
        //            if (previousValue != null)
        //            {
        //                _coordinate.Entity = null;
        //                previousValue.MainBreaks.Remove(this);
        //            }
        //            _coordinate.Entity = value;
        //            if (value != null)
        //            {
        //                value.MainBreaks.Add(this);
        //                _coordinateID = value.CoordinateID;
        //            }
        //            else
        //                _coordinateID = default(int?);
        //            SendPropertyChanged("Coordinate");
        //        }
        //    }
        //}

        [Association(Name = "MainCondition_MainBreak", Storage = "_mainCondition", ThisKey = "MainConditionID", IsForeignKey = true)]
        public MainCondition MainCondition
        {
            get { return _mainCondition.Entity; }
            set
            {
                var previousValue = _mainCondition.Entity;
                if (((previousValue != value)
                     || (_mainCondition.HasLoadedOrAssignedValue == false)))
                {
                    SendPropertyChanging();
                    if ((previousValue != null))
                    {
                        _mainCondition.Entity = null;
                        previousValue.MainBreaks.Remove(this);
                    }
                    _mainCondition.Entity = value;
                    if ((value != null))
                    {
                        value.MainBreaks.Add(this);
                        _mainConditionID = value.MainConditionID;
                    }
                    else
                    {
                        _mainConditionID = default(int);
                    }
                    SendPropertyChanged("MainCondition");
                }
            }
        }

        [Association(Name = "MainFailureType_MainBreak", Storage = "_mainFailureType", ThisKey = "MainFailureTypeID", IsForeignKey = true)]
        public MainFailureType MainFailureType
        {
            get { return _mainFailureType.Entity; }
            set
            {
                var previousValue = _mainFailureType.Entity;
                if ((previousValue != value)
                    || (_mainFailureType.HasLoadedOrAssignedValue == false))
                {
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _mainFailureType.Entity = null;
                        previousValue.MainBreaks.Remove(this);
                    }
                    _mainFailureType.Entity = value;
                    if (value != null)
                    {
                        value.MainBreaks.Add(this);
                        _mainFailureTypeID = value.MainFailureTypeID;
                    }
                    else
                    {
                        _mainFailureTypeID = default(int?);
                    }
                    SendPropertyChanged("MainFailureType");
                }
            }
        }

        [Association(Name = "ServiceSize_MainBreak", Storage = "_serviceSize", ThisKey = "ServiceSizeID", IsForeignKey = true)]
        public ServiceSize ServiceSize
        {
            get { return _serviceSize.Entity; }
            set
            {
                var previousValue = _serviceSize.Entity;
                if (((previousValue != value)
                     || (_serviceSize.HasLoadedOrAssignedValue == false)))
                {
                    SendPropertyChanging();
                    if ((previousValue != null))
                    {
                        _serviceSize.Entity = null;
                        previousValue.MainBreaks.Remove(this);
                    }
                    _serviceSize.Entity = value;
                    if ((value != null))
                    {
                        value.MainBreaks.Add(this);
                        _serviceSizeID = value.ServiceSizeID;
                    }
                    else
                    {
                        _serviceSizeID = default(int);
                    }
                    SendPropertyChanged("ServiceSize");
                }
            }
        }

        [Association(Name = "MainBreakMaterial_MainBreak", Storage = "_mainBreakMaterial", ThisKey = "MainBreakMaterialID", IsForeignKey = true)]
        public MainBreakMaterial MainBreakMaterial
        {
            get { return _mainBreakMaterial.Entity; }
            set
            {
                SendPropertyChanging();
                _mainBreakMaterial.Entity = value;
                SendPropertyChanged("MainBreakMaterial");
            }
        }

        [Association(Name = "MainBreakSoilCondition_MainBreak", Storage = "_mainBreakSoilCondition", ThisKey = "MainBreakSoilConditionID", IsForeignKey = true)]
        public MainBreakSoilCondition MainBreakSoilCondition
        {
            get { return _mainBreakSoilCondition.Entity; }
            set
            {
                SendPropertyChanging();
                _mainBreakSoilCondition.Entity = value;
                SendPropertyChanged("MainBreakSoilCondition");
            }
        }

        [Association(Name = "MainBreakDisinfectionMethod_MainBreak", Storage = "_mainBreakDisinfectionMethod", ThisKey = "MainBreakDisinfectionMethodID", IsForeignKey = true)]
        public MainBreakDisinfectionMethod MainBreakDisinfectionMethod
        {
            get { return _mainBreakDisinfectionMethod.Entity; }
            set
            {
                SendPropertyChanging();
                _mainBreakDisinfectionMethod.Entity = value;
                SendPropertyChanged("MainBreakDisinfectionMethod");
            }
        }

        [Association(Name = "MainBreakFlushMethod_MainBreak", Storage = "_mainBreakFlushMethod", ThisKey = "MainBreakFlushMethodID", IsForeignKey = true)]
        public MainBreakFlushMethod MainBreakFlushMethod
        {
            get { return _mainBreakFlushMethod.Entity; }
            set
            {
                SendPropertyChanging();
                _mainBreakFlushMethod.Entity = value;
                SendPropertyChanged("MainBreakFlushMethod");
            }
        }

        #region ReplacedWith

        [Column(Storage = "_replacedWithId", Name = "ReplacedWithId", DbType = "Int NULL", UpdateCheck = UpdateCheck.Never, CanBeNull = true)]
        public int? ReplacedWithId
        {
            get { return _replacedWithId; }
            set
            {
                if (_replacedWithId != value)
                {
                    if (_replacedWith.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                    SendPropertyChanging();
                    _replacedWithId = value;
                    SendPropertyChanged("ReplacedWithId");
                }
            }
        }

        [Association(Name = "ReplacedWith_MainBreak", Storage = "_replacedWith", ThisKey = "ReplacedWithId", IsForeignKey = true)]
        public MainBreakMaterial ReplacedWith
        {
            get { return _replacedWith.Entity; }
            set
            {
                SendPropertyChanging();
                _replacedWith.Entity = value;
                SendPropertyChanged("ReplacedWith");
            }
        }

        #endregion
        
        #endregion

        #region Constructors

        public MainBreak()
        {
            //_mainBreakValveOperations = new EntitySet<MainBreakValveOperation>(attach_MainBreakValveOperations, detach_MainBreakValveOperations);
            _coordinate = default(EntityRef<Coordinate>);
            _mainCondition = default(EntityRef<MainCondition>);
            _mainFailureType = default(EntityRef<MainFailureType>);
            _serviceSize = default(EntityRef<ServiceSize>);
            _workOrder = default(EntityRef<WorkOrder>);
            _serviceSize = default(EntityRef<ServiceSize>);
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
                    ValidateCreationInfo();
                    break;
            }
        }
        // ReSharper restore UnusedPrivateMember

        private void ValidateCreationInfo()
        {
            if (WorkOrder == null)
                throw new DomainLogicException(
                    "Cannot save a MainBreak without a WorkOrder."); 
            if (MainCondition == null)
                throw new DomainLogicException(
                    "Cannot save a MainBreak without a MainCondition.");
            if (MainBreakDisinfectionMethod == null)
                throw new DomainLogicException(
                    "Cannot save a MainBreak without a DisinfectionMethod.");
            if (MainBreakFlushMethod == null)
                throw new DomainLogicException(
                    "Cannot save a MainBreak without a FlushMethod.");
            if (MainBreakMaterial == null)
                throw new DomainLogicException(
                    "Cannot save a MainBreak without a Material.");
            if (MainFailureType == null)
                throw new DomainLogicException(
                    "Cannot save a MainBreak without a FailureType.");
            if (MainBreakSoilCondition == null)
                throw new DomainLogicException(
                    "Cannot save a MainBreak without a SoilCondition.");
            if (MainBreakFlushMethod == null)
                throw new DomainLogicException(
                    "Cannot save a MainBreak without a FlushMethod.");
            if (ServiceSize == null)
                throw new DomainLogicException(
                    "Cannot save a MainBreak without a MainSize.");
            
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

        //private void attach_MainBreakValveOperations(MainBreakValveOperation entity)
        //{
        //    SendPropertyChanging();
        //    entity.MainBreak = this;
        //}

        //private void detach_MainBreakValveOperations(MainBreakValveOperation entity)
        //{
        //    SendPropertyChanging();
        //    entity.MainBreak = null;
        //}

        #endregion

        #region Events

        public event PropertyChangingEventHandler PropertyChanging;

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
