using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace WorkOrders.Model
{
    [Table(Name = "dbo.tblFacilities")]
    public class Facility : INotifyPropertyChanging, INotifyPropertyChanged
    {
        #region Private Members

        private int _recordID, _townID, _operatingCenterID, _departmentID;
        private int? _coordinateID;
        private string _facilityID;
        private EntitySet<Equipment> _equipments;
        private EntityRef<OperatingCenter> _operatingCenter;
        private EntityRef<Town> _town;
        private EntityRef<Coordinate> _coordinate;
        
        #endregion

        #region Private Static Members

        private static readonly PropertyChangingEventArgs _emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        #endregion
        
        #region Table Properties

        // THIS IS THE PRIMARY KEY, NOT FACILITY_ID 
        [Column(Name = "RecordID", Storage = "_recordID", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int RecordID
        {
            get { return _recordID; }
            set
            {
                if ((_recordID != value))
                {
                    SendPropertyChanging();
                    _recordID = value;
                    SendPropertyChanged("RecordID");
                }
            }
        }
        
        [Column(Storage="_facilityID", DbType = "VarChar(25)")]
        public string FacilityID
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

        [Column(Storage = "_operatingCenterID", DbType = "Int NOT NULL")]
        public int OperatingCenterID
        {
            get { return _operatingCenterID; }
            set
            {
                if ((_operatingCenterID != value))
                {
                    SendPropertyChanging();
                    _operatingCenterID = value;
                    SendPropertyChanged("OperatingCenterID");
                }
            }
        }

        [Column(Storage = "_townID", DbType = "Int NOT NULL")]
        public int TownID
        {
            get { return _townID; }
            set
            {
                if ((_townID != value))
                {
                    SendPropertyChanging();
                    _townID = value;
                    SendPropertyChanged("TownID");
                }
            }
        }

        [Column(Storage = "_departmentID", DbType = "Int NOT NULL")]
        public int DepartmentID
        {
            get { return _departmentID;  }
            set
            {
                if ((_departmentID != value))
                {
                    SendPropertyChanging();
                    _departmentID = value;
                    SendPropertyChanged("DepartmentID");
                }
            }
        }

        [Column(Storage = "_coordinateID", DbType = "Int NULL")]
        public int? CoordinateID
        {
            get
            {
                return _coordinateID;
            }
            set
            {
                if ((_coordinateID != value))
                {
                    if (_coordinate.HasLoadedOrAssignedValue)
                    {
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                    }
                    SendPropertyChanging();
                    _coordinateID = value;
                    SendPropertyChanged("CoordinateID");
                }
            }
        }

        #endregion

        #region Associations

        [Association(Name = "OperatingCenters_OperatingCenterID", Storage = "_operatingCenter", ThisKey = "OperatingCenterID", IsForeignKey = true)]
        public OperatingCenter OperatingCenter
        {
            get
            {
                return _operatingCenter.Entity;

            }
            set { _operatingCenter.Entity = value; }
        }
        //[Association(Name = "Coordinate_Equipment", Storage = "_coordinate", ThisKey = "CoordinateID", OtherKey = "CoordinateID", IsForeignKey = true)]
        //public Coordinate Coordinate
        //{
        //    get
        //    {
        //        return _coordinate.Entity;
        //    }
        //    set
        //    {
        //        Coordinate previousValue = _coordinate.Entity;
        //        if (((previousValue != value)
        //                    || (_coordinate.HasLoadedOrAssignedValue == false)))
        //        {
        //            SendPropertyChanging();
        //            if ((previousValue != null))
        //            {
        //                _coordinate.Entity = null;
        //                previousValue.Equipments.Remove(this);
        //            }
        //            _coordinate.Entity = value;
        //            if ((value != null))
        //            {
        //                value.Equipments.Add(this);
        //                _coordinateID = value.CoordinateID;
        //            }
        //            else
        //            {
        //                _coordinateID = default(int?);
        //            }
        //            SendPropertyChanged("Coordinate");
        //        }
        //    }
        //}

        //[Association(Name = "OperatingCenter_Equipment", Storage = "_operatingCenter", ThisKey = "OperatingCenterID", IsForeignKey = true)]
        //public OperatingCenter OperatingCenter
        //{
        //    get { return _operatingCenter.Entity; }
        //    set
        //    {
        //        OperatingCenter previousValue = _operatingCenter.Entity;
        //        if ((previousValue != value)
        //            || (_operatingCenter.HasLoadedOrAssignedValue == false))
        //        {
        //            SendPropertyChanging();
        //            if (previousValue != null)
        //            {
        //                _operatingCenter.Entity = null;
        //                previousValue.Equipments.Remove(this);
        //            }
        //            _operatingCenter.Entity = value;
        //            if (value != null)
        //            {
        //                value.Equipments.Add(this);
        //                _operatingCenterID = value.OperatingCenterID;
        //            }
        //            else
        //                _operatingCenterID = default(int);
        //            SendPropertyChanged("OperatingCenter");
        //        }
        //    }
        //}

        [Association(Name = "Town_Facility", Storage = "_town", ThisKey = "TownID", IsForeignKey = true)]
        public Town Town
        {
            get { return _town.Entity; }
            set
            {
                Town previousValue = _town.Entity;
                if ((previousValue != value)
                    || (_town.HasLoadedOrAssignedValue == false))
                {
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _town.Entity = null;
                        previousValue.Facilities.Remove(this);
                    }
                    _town.Entity = value;
                    if (value != null)
                    {
                        value.Facilities.Add(this);
                        _townID = value.TownID;
                    }
                    else
                        _townID = default(int);
                    SendPropertyChanged("Town");
                }
            }
        }

        [Association(Name = "Coordinate_Facility", Storage = "_coordinate", ThisKey = "CoordinateID", OtherKey = "CoordinateID", IsForeignKey = true)]
        public Coordinate Coordinate
        {
            get
            {
                return _coordinate.Entity;
            }
            set
            {
                Coordinate previousValue = _coordinate.Entity;
                if (((previousValue != value)
                            || (_coordinate.HasLoadedOrAssignedValue == false)))
                {
                    SendPropertyChanging();
                    if ((previousValue != null))
                    {
                        _coordinate.Entity = null;
                        previousValue.Facilities.Remove(this);
                    }
                    _coordinate.Entity = value;
                    if ((value != null))
                    {
                        value.Facilities.Add(this);
                        _coordinateID = value.CoordinateID;
                    }
                    else
                    {
                        _coordinateID = default(int);
                    }
                    SendPropertyChanged("Coordinate");
                }
            }
        }

        [Association(Name = "Facility_Equipments", Storage="_equipments", OtherKey="FacilityID")]
        public EntitySet<Equipment> Equipments
        {
            get { return _equipments; }
            set { _equipments.Assign(value); }
        }

        #endregion

        #region Constructors

        public Facility()
        {
            _equipments = new EntitySet<Equipment>(attach_Equipments, detach_Equipments);
            _town = default (EntityRef<Town>);
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

        private void attach_Equipments(Equipment entity)
        {
            SendPropertyChanging();
            entity.Facility = this;
        }

        private void detach_Equipments(Equipment entity)
        {
            SendPropertyChanging();
            entity.Facility = null;
        }

        #endregion  

        #region Events

        public event PropertyChangingEventHandler PropertyChanging;
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

    }
}
