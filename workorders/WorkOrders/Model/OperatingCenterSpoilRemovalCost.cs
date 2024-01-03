using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using MMSINC.Exceptions;

namespace WorkOrders.Model
{
    [Table(Name = "dbo.OperatingCenterSpoilRemovalCosts")]
    public class OperatingCenterSpoilRemovalCost : INotifyPropertyChanging, INotifyPropertyChanged
    {
        #region Private Static Members

        private static readonly PropertyChangingEventArgs _emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        #endregion

        #region Private Members

        private int _operatingCenterSpoilRemovalCostID,
                    _operatingCenterID;

        private short _cost;

        private EntityRef<OperatingCenter> _operatingCenter;

        #endregion

        #region Properties

        #region Table Column Properties

        [Column(Storage = "_operatingCenterSpoilRemovalCostID", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int OperatingCenterSpoilRemovalCostID
        {
            get { return _operatingCenterSpoilRemovalCostID; }
            set
            {
                if (_operatingCenterSpoilRemovalCostID != value)
                {
                    SendPropertyChanging();
                    _operatingCenterSpoilRemovalCostID = value;
                    SendPropertyChanged("OperatingCenterSpoilRemovalCostID");
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

        [Column(Storage = "_cost", DbType = "Smallint NOT NULL")]
        public short Cost
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

        #endregion

        #region Association Properties

        [Association(Name = "OperatingCenter_OperatingCenterSpoilRemovalCost", Storage = "_operatingCenter", ThisKey = "OperatingCenterID", IsForeignKey = true)]
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
                        previousValue.OperatingCenterSpoilRemovalCost = null;
                    }
                    _operatingCenter.Entity = value;
                    if (value != null)
                    {
                        value.OperatingCenterSpoilRemovalCost = this;
                        _operatingCenterID = value.OperatingCenterID;
                    }
                    else
                        _operatingCenterID = default(int);
                    SendPropertyChanged("OperatingCenter");
                }
            }
        }

        #endregion

        #endregion

        #region Constructors

        public OperatingCenterSpoilRemovalCost()
        {
        }

        #endregion

        #region Private Methods

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

        private void OnValidate(ChangeAction action)
        {
            switch (action)
            {
                case ChangeAction.Insert:
                case ChangeAction.Update:
                    if (OperatingCenter == null)
                        throw new DomainLogicException(
                            "Cannot save an OperatingCenterSpoilRemovalCost record without an OperatingCenter.");
                    if (Cost <= 0)
                        throw new DomainLogicException(
                            "Cannot save an OperatingCenterSpoilRemovalCost record with a cost of zero or less.");
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
