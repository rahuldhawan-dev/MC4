using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using MMSINC.Exceptions;

namespace WorkOrders.Model
{
    [Table(Name = "dbo.RestorationTypeCosts")]
    public class RestorationTypeCost : INotifyPropertyChanging, INotifyPropertyChanged
    {
        #region Private Static Members

        private static readonly PropertyChangingEventArgs _emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        #endregion

        #region Private Members

        private int _restorationTypeCostID,
                    _operatingCenterID,
                    _restorationTypeID,
                    _finalCost;

        private double _cost;
        
        private EntityRef<OperatingCenter> _operatingCenter;

        private EntityRef<RestorationType> _restorationType;

        #endregion

        #region Properties

        [Column(Storage = "_restorationTypeCostID", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int RestorationTypeCostID
        {
            get { return _restorationTypeCostID; }
            set
            {
                if (_restorationTypeCostID != value)
                {
                    SendPropertyChanging();
                    _restorationTypeCostID = value;
                    SendPropertyChanged("RestorationTypeCostID");
                }
            }
        }

        [Column(Storage = "_operatingCenterID", DbType = "Int NOT NULL")]
        public int OperatingCenterID
        {
            get { return _operatingCenterID; }
            set
            {
                if (_operatingCenterID != value)
                {
                    if (_operatingCenter.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                }
                SendPropertyChanging();
                _operatingCenterID = value;
                SendPropertyChanged("OperatingCenterID");
            }
        }

        [Column(Storage = "_restorationTypeID", DbType = "Int NOT NULL")]
        public int RestorationTypeID
        {
            get { return _restorationTypeID; }
            set
            {
                if (_restorationTypeID != value)
                {
                    if (_restorationType.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                }
                SendPropertyChanging();
                _restorationTypeID = value;
                SendPropertyChanged("RestorationTypeID");
            }
        }

        [Column(Storage = "_cost", DbType = "Float NOT NULL")]
        public double Cost
        {
            get { return _cost; }
            set
            {
                if (_cost != value)
                {
                    SendPropertyChanging();
                    _cost = value;
                    SendPropertyChanged("Cost");
                }
            }
        }

        [Column(Storage = "_finalCost", DbType = "Int NOT NULL")]
        public int FinalCost
        {
            get { return _finalCost; }
            set
            {
                if (_finalCost != value)
                {
                    SendPropertyChanging();
                    _finalCost = value;
                    SendPropertyChanged("Final Cost");
                }
            }
        }

        [Association(Name = "OperatingCenter_RestorationTypeCost", Storage = "_operatingCenter", ThisKey = "OperatingCenterID", IsForeignKey = true)]
        public OperatingCenter OperatingCenter
        {
            get { return _operatingCenter.Entity; }
            set
            {
                var previousValue = _operatingCenter.Entity;
                if ((previousValue != value)
                    || (_operatingCenter.HasLoadedOrAssignedValue == false))
                {
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _operatingCenter.Entity = null;
                        previousValue.RestorationTypeCosts.Remove(this);
                    }
                    _operatingCenter.Entity = value;
                    if (value != null)
                    {
                        value.RestorationTypeCosts.Add(this);
                        _operatingCenterID = value.OperatingCenterID;
                    }
                    else
                        _operatingCenterID = default(int);
                    SendPropertyChanged("OperatingCenter");
                }
            }
        }

        [Association(Name = "RestorationType_RestorationTypeCost", Storage = "_restorationType", ThisKey = "RestorationTypeID", IsForeignKey = true)]
        public RestorationType RestorationType
        {
            get { return _restorationType.Entity; }
            set
            {
                RestorationType previousValue = _restorationType.Entity;
                if ((previousValue != value)
                    || (_restorationType.HasLoadedOrAssignedValue == false))
                {
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _restorationType.Entity = null;
                        previousValue.RestorationTypeCosts.Remove(this);
                    }
                    _restorationType.Entity = value;
                    if (value != null)
                    {
                        value.RestorationTypeCosts.Add(this);
                        _restorationTypeID = value.RestorationTypeID;
                    }
                    else
                        _restorationTypeID = default(int);
                    SendPropertyChanged("RestorationType");
                }
            }
        }

        #endregion

        #region Constructors

        public RestorationTypeCost()
        {
        }

        #endregion

        #region Private Members

        protected virtual void SendPropertyChanging()
        {
            if (PropertyChanging != null)
                PropertyChanging(this, _emptyChangingEventArgs);
        }

        protected virtual void SendPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void OnValidate(ChangeAction action)
        {
            switch (action)
            {
                case ChangeAction.Insert:
                case ChangeAction.Update:
                    if (OperatingCenter == null)
                        throw new DomainLogicException("Cannot save a RestorationTypeCost record without an Operating Center.");
                    if (RestorationType == null)
                        throw new DomainLogicException("Cannot save a RestorationTypeCost record without a Restoration Type.");
                    if (Cost == default(double))
                        throw new DomainLogicException("Cannot save a RestorationTypeCost record without a value for Cost.");
                    if (FinalCost == default(int))
                        throw new DomainLogicException("Cannot save a RestorationTypeCost record without a value for Final Cost");
                    break;
            }
        }

        #endregion

        #region Events

        public event PropertyChangingEventHandler PropertyChanging;

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
