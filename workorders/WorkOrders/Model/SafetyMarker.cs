using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using MMSINC.Exceptions;

namespace WorkOrders.Model
{
    [Table(Name = "dbo.SafetyMarkers")]
    public class SafetyMarker : INotifyPropertyChanging, INotifyPropertyChanged
    {
        #region Private Static Members

        private static readonly PropertyChangingEventArgs emptyChangingEventArgs =
            new PropertyChangingEventArgs(String.Empty);

        #endregion

        #region Private Members

        private DateTime? _markersRetrievedOn;

        private int _safetyMarkerID, _workOrderID;

        private short? _conesOnSite, _baracadesOnSite;

        private EntityRef<WorkOrder> _workOrder;

        #endregion

        #region Properties

        [Column(Storage = "_safetyMarkerID", AutoSync = AutoSync.OnInsert,
            DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true,
            IsDbGenerated = true)]
        public int SafetyMarkerID
        {
            get { return _safetyMarkerID; }
            set
            {
                if (_safetyMarkerID != value)
                {
                    SendPropertyChanging();
                    _safetyMarkerID = value;
                    SendPropertyChanged("SafetyMarkerID");
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

        [Column(Storage = "_markersRetrievedOn", DbType = "SmallDateTime")]
        public DateTime? MarkersRetrievedOn
        {
            get { return _markersRetrievedOn; }
            set
            {
                if (_markersRetrievedOn != value)
                {
                    SendPropertyChanging();
                    _markersRetrievedOn = value;
                    SendPropertyChanged("MarkersRetrievedOn");
                }
            }
        }

        [Column(Storage = "_conesOnSite", DbType = "SmallInt")]
        public short? ConesOnSite
        {
            get { return _conesOnSite; }
            set
            {
                if (_conesOnSite != value)
                {
                    if (AreRetrieved)
                        throw new DomainLogicException(
                            "Cannot change the number of Cones on site after the date of retrieval has been set.");
                    SendPropertyChanging();
                    _conesOnSite = value;
                    SendPropertyChanged("ConesOnSite");
                }
            }
        }

        [Column(Storage = "_baracadesOnSite", DbType = "SmallInt")]
        public short? BaracadesOnSite
        {
            get { return _baracadesOnSite; }
            set
            {
                if (_baracadesOnSite != value)
                {
                    if (AreRetrieved)
                        throw new DomainLogicException(
                            "Cannot change the number of Cones on site after the date of retrieval has been set.");
                    SendPropertyChanging();
                    _baracadesOnSite = value;
                    SendPropertyChanged("BaracadesOnSite");
                }
            }
        }

        [Association(Name = "WorkOrder_SafetyMarker", Storage = "_workOrder",
            ThisKey = "WorkOrderID", IsForeignKey = true)]
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
                        throw new DomainLogicException(
                            "Cannot change the WorkOrder of a SafetyMarker record after it has been set.");
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _workOrder.Entity = null;
                        previousValue.SafetyMarkers.Remove(this);
                    }
                    _workOrder.Entity = value;
                    if (value != null)
                    {
                        value.SafetyMarkers.Add(this);
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

        public bool AreRetrieved
        {
            get
            {
                return (MarkersRetrievedOn != null &&
                        MarkersRetrievedOn != DateTime.MinValue);
            }
        }

        #endregion

        #region Constructors

        public SafetyMarker()
        {
            _workOrder = default(EntityRef<WorkOrder>);
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
            if (ConesOnSite == 0 && BaracadesOnSite == 0)
                throw new DomainLogicException(
                    "Cannot save a SafetyMarker record with no cones or barricades.");
            if (WorkOrder == null)
                throw new DomainLogicException(
                    "Cannot save a SafetyMarker object without a WorkOrder.");
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
