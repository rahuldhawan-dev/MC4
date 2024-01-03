using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using MMSINC.Exceptions;

namespace WorkOrders.Model
{
    [Table(Name = "dbo.DetectedLeaks")]
    public class DetectedLeak : INotifyPropertyChanging, INotifyPropertyChanged
    {
        #region Constants

        private const short MAX_MAPPAGE_LENGTH = 10,
                            MAX_EQUIPMENTUSED_LENGTH = 25;

        #endregion

        #region Private Static Members

        private static readonly PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        #endregion

        #region Private Members

        private int _workOrderID, _workAreaTypeID, _detectedLeakID;

        private int? _leakReportingSourceID,
                     _surveyStartingPointID,
                     _surveyEndingPointID;

        private short? _hydrantsSounded, _mainsSounded, _servicesSounded;

        private string _mapPage, _equipmentUsed, _accessPointsAndContacts;

        private decimal? _mileage;

        private bool? _soundRecorded;

        #pragma warning disable 649

        private EntityRef<WorkOrder> _workOrder;

        private EntityRef<WorkAreaType> _workAreaType;

        private EntityRef<LeakReportingSource> _leakReportingSource;

        private EntityRef<Valve> _surveyStartingPoint, _surveyEndingPoint;

        #pragma warning restore 649

        #endregion

        #region Properties

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
                }
                SendPropertyChanging();
                _workOrderID = value;
                SendPropertyChanged("WorkOrderID");
            }
        }

        [Association(Name = "WorkOrder_DetectedLeak", Storage = "_workOrder", ThisKey = "WorkOrderID", IsForeignKey = true)]
        public WorkOrder WorkOrder
        {
            get { return _workOrder.Entity; }
            set
            {
                WorkOrder previousValue = _workOrder.Entity;
                if ((previousValue != value)
                    || (_workOrder.HasLoadedOrAssignedValue == false))
                {
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _workOrder.Entity = null;
                        previousValue.DetectedLeaks.Remove(this);
                    }
                    _workOrder.Entity = value;
                    if (value != null)
                    {
                        value.DetectedLeaks.Add(this);
                        _workOrderID = value.WorkOrderID;
                    }
                    else
                        _workOrderID = default(int);
                    SendPropertyChanged("WorkOrder");
                }
            }
        }

        [Column(Storage = "_workAreaTypeID", DbType = "Int NOT NULL")]
        public int WorkAreaTypeID
        {
            get { return _workAreaTypeID; }
            set
            {
                if (_workAreaTypeID != value)
                {
                    if (_workAreaType.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                }
                SendPropertyChanging();
                _workAreaTypeID = value;
                SendPropertyChanged("WorkAreaTypeID");
            }
        }

        [Association(Name = "WorkAreaType_DetectedLeak", Storage = "_workAreaType", ThisKey = "WorkAreaTypeID", IsForeignKey = true)]
        public WorkAreaType WorkAreaType
        {
            get { return _workAreaType.Entity; }
            set
            {
                WorkAreaType previousValue = _workAreaType.Entity;
                if ((previousValue != value)
                    || (_workAreaType.HasLoadedOrAssignedValue == false))
                {
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _workAreaType.Entity = null;
                        previousValue.DetectedLeaks.Remove(this);
                    }
                    _workAreaType.Entity = value;
                    if (value != null)
                    {
                        value.DetectedLeaks.Add(this);
                        _workAreaTypeID = value.WorkAreaTypeID;
                    }
                    else
                        _workAreaTypeID = default(int);
                    SendPropertyChanged("WorkAreaType");
                }
            }
        }

        [Column(Storage = "_leakReportingSourceID", DbType = "Int")]
        public int? LeakReportingSourceID
        {
            get { return _leakReportingSourceID; }
            set
            {
                if (_leakReportingSourceID != value)
                {
                    if (_leakReportingSource.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                }
                SendPropertyChanging();
                _leakReportingSourceID = value;
                SendPropertyChanged("LeakReportingSourceID");
            }
        }

        [Association(Name = "LeakReportingSource_DetectedLeak", Storage = "_leakReportingSource", ThisKey = "LeakReportingSourceID", IsForeignKey = true)]
        public LeakReportingSource LeakReportingSource
        {
            get { return _leakReportingSource.Entity; }
            set
            {
                LeakReportingSource previousValue = _leakReportingSource.Entity;
                if ((previousValue != value)
                    || (_leakReportingSource.HasLoadedOrAssignedValue == false))
                {
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _leakReportingSource.Entity = null;
                        previousValue.DetectedLeaks.Remove(this);
                    }
                    _leakReportingSource.Entity = value;
                    if (value != null)
                    {
                        value.DetectedLeaks.Add(this);
                        _leakReportingSourceID = value.LeakReportingSourceID;
                    }
                    else
                        _leakReportingSourceID = default(int);
                    SendPropertyChanged("LeakReportingSource");
                }
            }
        }

        [Column(Storage = "_surveyStartingPointID", DbType = "Int")]
        public int? SurveyStartingPointID
        {
            get { return _surveyStartingPointID; }
            set
            {
                if (_surveyStartingPointID != value)
                {
                    if (_surveyStartingPoint.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                }
                SendPropertyChanging();
                _surveyStartingPointID = value;
                SendPropertyChanged("SurveyStartingPointID");
            }
        }

        [Association(Name = "SurveyStartingPoint_DetectedLeak", Storage = "_surveyStartingPoint", ThisKey = "SurveyStartingPointID", IsForeignKey = true)]
        public Valve SurveyStartingPoint
        {
            get { return _surveyStartingPoint.Entity; }
            set
            {
                Valve previousValue = _surveyStartingPoint.Entity;
                if ((previousValue != value)
                    || (_surveyStartingPoint.HasLoadedOrAssignedValue == false))
                {
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _surveyStartingPoint.Entity = null;
                        previousValue.DetectedLeaks.Remove(this);
                    }
                    _surveyStartingPoint.Entity = value;
                    if (value != null)
                    {
                        value.DetectedLeaks.Add(this);
                        _surveyStartingPointID = value.ValveID;
                    }
                    else
                        _surveyStartingPointID = default(int);
                    SendPropertyChanged("SurveyStartingPoint");
                }
            }
        }

        [Column(Storage = "_surveyEndingPointID", DbType = "Int")]
        public int? SurveyEndingPointID
        {
            get { return _surveyEndingPointID; }
            set
            {
                if (_surveyEndingPointID != value)
                {
                    if (_surveyEndingPoint.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                }
                SendPropertyChanging();
                _surveyEndingPointID = value;
                SendPropertyChanged("SurveyEndingPointID");
            }
        }

        [Association(Name = "SurveyEndingPoint_DetectedLeak", Storage = "_surveyEndingPoint", ThisKey = "SurveyEndingPointID", IsForeignKey = true)]
        public Valve SurveyEndingPoint
        {
            get { return _surveyEndingPoint.Entity; }
            set
            {
                Valve previousValue = _surveyEndingPoint.Entity;
                if ((previousValue != value)
                    || (_surveyEndingPoint.HasLoadedOrAssignedValue == false))
                {
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _surveyEndingPoint.Entity = null;
                        previousValue.DetectedLeaks.Remove(this);
                    }
                    _surveyEndingPoint.Entity = value;
                    if (value != null)
                    {
                        value.DetectedLeaks.Add(this);
                        _surveyEndingPointID = value.ValveID;
                    }
                    else
                        _surveyEndingPointID = default(int);
                    SendPropertyChanged("SurveyEndingPoint");
                }
            }
        }

        [Column(Storage = "_mapPage", DbType = "VarChar(10)")]
        public string MapPage
        {
            get { return _mapPage; }
            set
            {
                if (value != null && value.Length > MAX_MAPPAGE_LENGTH)
                    throw new StringTooLongException("MapPage", MAX_MAPPAGE_LENGTH);
                if (_mapPage != value)
                {
                    SendPropertyChanging();
                    _mapPage = value;
                    SendPropertyChanged("MapPage");
                }
            }
        }

        [Column(Storage = "_equipmentUsed", DbType = "VarChar(25)")]
        public string EquipmentUsed
        {
            get { return _equipmentUsed; }
            set
            {
                if (value != null && value.Length > MAX_EQUIPMENTUSED_LENGTH)
                    throw new StringTooLongException("EquipmentUsed", MAX_EQUIPMENTUSED_LENGTH);
                if (_equipmentUsed != value)
                {
                    SendPropertyChanging();
                    _equipmentUsed = value;
                    SendPropertyChanged("EquipmentUsed");
                }
            }
        }

        [Column(Storage = "_detectedLeakID", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int DetectedLeakID
        {
            get { return _detectedLeakID; }
            set
            {
                if (_detectedLeakID != value)
                {
                    SendPropertyChanging();
                    _detectedLeakID = value;
                    SendPropertyChanged("DetectedLeakID");
                }
            }
        }

        [Column(Storage = "_accessPointsAndContacts", DbType = "Text", UpdateCheck = UpdateCheck.Never)]
        public string AccessPointsAndContacts
        {
            get { return _accessPointsAndContacts; }
            set
            {
                if (_accessPointsAndContacts != value)
                {
                    SendPropertyChanging();
                    _accessPointsAndContacts = value;
                    SendPropertyChanged("AccessPointsAndContacts");
                }
            }
        }

        [Column(Storage = "_soundRecorded", DbType = "Bit")]
        public bool? SoundRecorded
        {
            get { return _soundRecorded; }
            set
            {
                if (_soundRecorded != value)
                {
                    SendPropertyChanging();
                    _soundRecorded = value;
                    SendPropertyChanged("SoundRecorded");
                }
            }
        }

        [Column(Storage = "_mileage", DbType = "Decimal(18,2)")]
        public decimal? Mileage
        {
            get { return _mileage; }
            set
            {
                if (_mileage != value)
                {
                    SendPropertyChanging();
                    _mileage = value;
                    SendPropertyChanged("Mileage");
                }
            }
        }

        [Column(Storage = "_hydrantsSounded", DbType = "Smallint")]
        public short? HydrantsSounded
        {
            get { return _hydrantsSounded; }
            set
            {
                if (_hydrantsSounded != value)
                {
                    SendPropertyChanging();
                    _hydrantsSounded = value;
                    SendPropertyChanged("HydrantsSounded");
                }
            }
        }

        [Column(Storage = "_mainsSounded", DbType = "Smallint")]
        public short? MainsSounded
        {
            get { return _mainsSounded; }
            set
            {
                if (_mainsSounded != value)
                {
                    SendPropertyChanging();
                    _mainsSounded = value;
                    SendPropertyChanged("MainsSounded");
                }
            }
        }

        [Column(Storage = "_servicesSounded", DbType = "Smallint")]
        public short? ServicesSounded
        {
            get { return _servicesSounded; }
            set
            {
                if (_servicesSounded != value)
                {
                    SendPropertyChanging();
                    _servicesSounded = value;
                    SendPropertyChanged("ServicesSounded");
                }
            }
        }

        #endregion

        #region Private Methods

        protected virtual void SendPropertyChanging()
        {
            if (PropertyChanging != null)
                PropertyChanging(this, emptyChangingEventArgs);
        }

        protected virtual void SendPropertyChanged(String propertyName)
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
