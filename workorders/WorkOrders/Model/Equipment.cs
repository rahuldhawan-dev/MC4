using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using MMSINC.Exceptions;

namespace WorkOrders.Model
{
    [Table(Name = "dbo.Equipment")]
    public class Equipment : INotifyPropertyChanging, INotifyPropertyChanged, IAsset
    {
        #region Constants

        public const int MAX_IDENTIFIER_LENGTH = 50;

        #endregion

        #region Private Static Members

        private static readonly PropertyChangingEventArgs _emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        #endregion

        #region Private Members

        private int _equipmentID,
            _equipmentAssetTypeID;

        private int? _facilityID, _typeID;

        private string _identifier, _criticalNotes;

        private int? _streetID,
                     _operatingCenterID,
                     _townID,
                     _assetStatusID,
                     _coordinateID;

        //private EntityRef<Coordinate> _coordinate;
        private EntityRef<AssetStatus> _assetStatus;
        //private EntityRef<OperatingCenter> _operatingCenter;
        private EntityRef<Town> _town;
        //private EntityRef<Street> _street;
        private EntityRef<Facility> _facility;
        private EntityRef<EquipmentPurpose> _equipmentPurpose;

        private readonly EntitySet<WorkOrder> _workOrders;

        private double? _latitude, _longitude;

        #endregion

        #region Properties
        
        #region Table Properties

        [Column(Name = "EquipmentID", Storage = "_equipmentID",
            AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY",
            IsPrimaryKey = true, IsDbGenerated = true)]
        public int EquipmentID
        {
            get { return _equipmentID; }
            set
            {
                if ((_equipmentID != value))
                {
                    SendPropertyChanging();
                    _equipmentID = value;
                    SendPropertyChanged("EquipmentID");
                }
            }
        }

        [Column(Name = "CriticalNotes", Storage = "_criticalNotes", DbType = "VarChar(150)")]
        public string CriticalNotes
        {
            get { return _criticalNotes; }
            set
            {
                if (value != null && value.Length > 150)
                    throw new StringTooLongException("CriticalNotes", 150);
                if (_criticalNotes != value)
                {
                    SendPropertyChanging();
                    _criticalNotes = value;
                    SendPropertyChanged("CriticalNotes");
                }
            }
        }

        [Column(Name = "FacilityID", Storage = "_facilityID", DbType = "Int NULL")]
        public int? FacilityID
        {
            get { return _facilityID; }
            set
            {
                if ((_facilityID != value))
                {
                    SendPropertyChanging();
                    _facilityID = value;
                    SendPropertyChanged("FacilityID");
                }
            }
        }

  //      [Column(Storage = "_identifier", DbType = "VarChar(20)")]
        public string Identifier
        {
            get
            {
                string facilityIdDisplay="";
                if (Facility != null && Facility.OperatingCenter !=null)
                {
                    facilityIdDisplay =  $"{ Facility?.OperatingCenter.OpCntr}-{ FacilityID}";
                }
                return  $"{facilityIdDisplay}-{EquipmentPurpose?.Abbreviation}-{EquipmentID}";
            }
            set
            {
                if (value != null && value.Length > MAX_IDENTIFIER_LENGTH)
                    throw new StringTooLongException("Identifier",
                        MAX_IDENTIFIER_LENGTH);
                if ((_identifier != value))
                {
                    SendPropertyChanging();
                    _identifier = value;
                    SendPropertyChanged("Identifier");
                }
            }
        }

        [Column(Name = "PurposeId", Storage = "_typeID", DbType = "Int")]
        public int? TypeID
        {
            get { return _typeID; }
            set
            {
                if ((_typeID != value))
                {
                    SendPropertyChanging();
                    _typeID = value;
                    SendPropertyChanged("TypeID");
                }
            }
        }

        [Association(Name = "EquipmentPurposes_TypeID", Storage = "_equipmentPurpose", ThisKey = "TypeID", IsForeignKey = true)]
        public EquipmentPurpose EquipmentPurpose
        {
            get
            {
                return _equipmentPurpose.Entity;
            }
            set
            {
                _equipmentPurpose.Entity = value;
            }
        }
        public int? OperatingCenterID
        {
            get { return 0; } // FacilityID.OperatingCenterID; }
        }

        public int TownID
        {
            get { return Facility.Town.TownID; }
        }
        
        ////TODO: also comes from facility
        //[Column(Storage = "_coordinateID", DbType = "Int NULL")]
        //public int? CoordinateID
        //{
        //    get
        //    {
        //        return _coordinateID;
        //    }
        //    set
        //    {
        //        if ((_coordinateID != value))
        //        {
        //            if (_coordinate.HasLoadedOrAssignedValue)
        //            {
        //                throw new ForeignKeyReferenceAlreadyHasValueException();
        //            }
        //            SendPropertyChanging();
        //            _coordinateID = value;
        //            SendPropertyChanged("CoordinateID");
        //        }
        //    }
        //}

        #endregion

        #region Associations

        public Coordinate Coordinate
        {
            get
            {
                return Facility != null ? Facility.Coordinate : null;
            }
        }

        public Town Town
        {
            get { return Facility.Town; }
        }
        
        [Association(Name = "Facility_Equipment", Storage = "_facility", ThisKey = "FacilityID", IsForeignKey = true)]
        public Facility Facility
        {
            get { return _facility.Entity; }
            set
            {
                Facility previousValue = _facility.Entity;
                if ((previousValue != value) ||
                    (_facility.HasLoadedOrAssignedValue == false))
                {
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _facility.Entity = null;
                        previousValue.Equipments.Remove(this);
                    }
                    _facility.Entity = value;
                    if (value != null)
                    {
                        value.Equipments.Add(this);
                        _facilityID = value.RecordID;
                    }
                    else
                        _facilityID = default(int);
                    SendPropertyChanged("Facility");
                }
            }
        }
        
        [Column(Storage = "_assetStatusID", DbType = "Int")]
        public int? StatusID
        {
            get
            {
                return _assetStatusID;
            }
            set
            {
                if ((_assetStatusID != value))
                {
                    if (_assetStatus.HasLoadedOrAssignedValue)
                    {
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                    }
                    SendPropertyChanging();
                    _assetStatusID = value;
                    SendPropertyChanged("StatusID");
                }
            }
        }

        [Association(Name = "AssetStatuses_Equipment", Storage = "_assetStatus", ThisKey = "StatusID", OtherKey = "AssetStatusID", IsForeignKey = true)]
        public AssetStatus AssetStatus
        {
            get
            {
                return _assetStatus.Entity;
            }
            set
            {
                var previousValue = _assetStatus.Entity;
                if (((previousValue != value)
                     || (_assetStatus.HasLoadedOrAssignedValue == false)))
                {
                    SendPropertyChanging();
                    if ((previousValue != null))
                    {
                        _assetStatus.Entity = null;
                        previousValue.Equipment.Remove(this);
                    }
                    _assetStatus.Entity = value;
                    if ((value != null))
                    {
                        value.Equipment.Add(this);
                        _assetStatusID = value.AssetStatusID;
                    }
                    else
                    {
                        _assetStatusID = default(int?);
                    }
                    SendPropertyChanged("AssetStatus");
                }
            }
        }
        #endregion

        #region Implementation of IAsset

        [Association(Name = "Equipment_WorkOrder", Storage = "_workOrders", OtherKey = "EquipmentID")]
        public EntitySet<WorkOrder> WorkOrders
        {
            get { return _workOrders; }
            set { _workOrders.Assign(value); }
        }

        public object AssetKey
        {
            get { return EquipmentID; }
        }

        public string AssetID
        {
            get { return Identifier; }
        }

        public double? Latitude
        {
            get
            {
                if (Coordinate != null) return Coordinate.Latitude;
                return null;
            }
            set
            {
                _latitude = value;
            }
        }

        public double? Longitude
        {
            get
            {
                if (Coordinate != null) return Coordinate.Longitude;
                return null;
            }
            set
            {
                _longitude = value;
            }
        }

        #endregion

        #endregion

        #region Constructors

        public Equipment()
        {
            _facility = default(EntityRef<Facility>);
            _assetStatus = default(EntityRef<AssetStatus>);
            _workOrders = new EntitySet<WorkOrder>(attach_WorkOrders, detach_WorkOrders);
        }

        #endregion

        #region Private Methods

        private void SendPropertyChanging()
        {
            if (PropertyChanging != null)
                PropertyChanging(this, _emptyChangingEventArgs);
        }

        private void SendPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private void attach_WorkOrders(WorkOrder entity)
        {
            SendPropertyChanging();
            entity.Equipment = this;
        }

        private void detach_WorkOrders(WorkOrder entity)
        {
            SendPropertyChanging();
            entity.Equipment = null;
        }

        #endregion

        #region Events

        public event PropertyChangingEventHandler PropertyChanging;
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Exposed Methods

        public override string ToString()
        {
            return Identifier;
        }

        #endregion
    }
}
