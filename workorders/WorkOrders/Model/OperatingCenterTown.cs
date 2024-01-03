using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using MMSINC.Exceptions;

namespace WorkOrders.Model
{
    [Table(Name = "dbo.OperatingCentersTowns")]
    public class OperatingCenterTown : INotifyPropertyChanging, INotifyPropertyChanged
    {
        #region Private Static Members

        private static readonly PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        #endregion

        #region Private Members

        private int _operatingCenterID,
                    _townID;
        private int? _mainSAPEquipmentID, _sewerMainSAPEquipmentID;

        private EntityRef<Town> _town;

        private EntityRef<OperatingCenter> _operatingCenter;

        #endregion

        #region Properties

        [Column(Storage = "_operatingCenterID", DbType = "Int NOT NULL", IsPrimaryKey = true)]
        public int OperatingCenterID
        {
            get { return _operatingCenterID; }
            set
            {
                if ((_operatingCenterID != value))
                {
                    if (_operatingCenter.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                    SendPropertyChanging();
                    _operatingCenterID = value;
                    SendPropertyChanged("OperatingCenterID");
                }
            }
        }

        [Column(Storage = "_townID", DbType = "Int NOT NULL", IsPrimaryKey = true)]
        public int TownID
        {
            get { return _townID; }
            set
            {
                if ((_townID != value))
                {
                    if (_town.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                    SendPropertyChanging();
                    _townID = value;
                    SendPropertyChanged("townID");
                }
            }
        }

        [Column(Storage = "_mainSAPEquipmentID", DbType = "Int")]
        public int? MainSAPEquipmentID
        {
            get { return _mainSAPEquipmentID; }
            set
            {
                if ((_mainSAPEquipmentID != value))
                {
                    SendPropertyChanging();
                    _mainSAPEquipmentID = value;
                    SendPropertyChanged("MainSAPEquipmentID");
                }
            }
        }

        [Column(Storage = "_sewerMainSAPEquipmentID", DbType = "Int")]
        public int? SewerMainSAPEquipmentID
        {
            get { return _sewerMainSAPEquipmentID; }
            set
            {
                if ((_sewerMainSAPEquipmentID != value))
                {
                    SendPropertyChanging();
                    _sewerMainSAPEquipmentID = value;
                    SendPropertyChanged("SewerMainSAPEquipmentID");
                }
            }
        }

        #region Associations

        [Association(Name = "OperatingCenter_OperatingCentersTown", Storage = "_operatingCenter", ThisKey = "OperatingCenterID", IsForeignKey = true)]
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
                    if ((previousValue != null))
                    {
                        _operatingCenter.Entity = null;
                        previousValue.OperatingCentersTowns.Remove(this);
                    }
                    _operatingCenter.Entity = value;
                    if ((value != null))
                    {
                        value.OperatingCentersTowns.Add(this);
                        _operatingCenterID = value.OperatingCenterID;
                    }
                    else
                    {
                        _operatingCenterID = default(int);
                    }
                    SendPropertyChanged("OperatingCenter");
                }
            }
        }

        [Association(Name = "OperatingCenter_OperatingCentersTown", Storage = "_town", ThisKey = "TownID", IsForeignKey = true)]
        public Town Town
        {
            get { return _town.Entity; }
            set
            {
                var previousValue = _town.Entity;
                if ((previousValue != value)
                     || (_town.HasLoadedOrAssignedValue == false))
                {
                    SendPropertyChanging();
                    if ((previousValue != null))
                    {
                        _town.Entity = null;
                        previousValue.OperatingCentersTowns.Remove(this);
                    }
                    _town.Entity = value;
                    if ((value != null))
                    {
                        value.OperatingCentersTowns.Add(this);
                        _townID = value.TownID;
                    }
                    else
                    {
                        _townID = default(int);
                    }
                    SendPropertyChanged("Town");
                }
            }
        }

    	#endregion

        #endregion

        #region Constructors

        public OperatingCenterTown()
        {
            _town = default(EntityRef<Town>);
            _operatingCenter = default(EntityRef<OperatingCenter>);
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

        private void ValidateCreationInfo()
        {
            if (Town == null)
                throw new DomainLogicException(
                    "Cannot save an OperatingCentersTowns object without a Town.");
            if (OperatingCenter == null)
                throw new DomainLogicException(
                    "Cannot save an OperatingCentersTown object without an OperatingCenter.");
        }

        #endregion

        #region Events

        public event PropertyChangingEventHandler PropertyChanging;

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
