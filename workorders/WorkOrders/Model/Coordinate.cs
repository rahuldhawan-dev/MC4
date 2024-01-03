using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using MMSINC.Exceptions;

namespace WorkOrders.Model
{
    [Table(Name = "dbo.Coordinates")]
    public class Coordinate : INotifyPropertyChanging, INotifyPropertyChanged
    {
        #region Private Static Members

        private static readonly PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        #endregion

        #region Private Members

        private int _coordinateID;

        private double _latitude, _longitude;

        private EntitySet<SewerOpening> _sewerOpenings;
        private EntitySet<StormCatch> _stormCatches;
        private EntitySet<Facility> _facilities;
        private EntitySet<Hydrant> _hydrants;
        private EntitySet<Valve> _valves;

        #endregion

        #region Properties

        [Column(Storage = "_coordinateID", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int CoordinateID
        {
            get { return _coordinateID; }
            set
            {
                if (_coordinateID != value)
                {
                    SendPropertyChanging();
                    _coordinateID = value;
                    SendPropertyChanged("CoordinateID");
                }
            }
        }

        [Column(Storage = "_latitude", DbType = "Float NOT NULL")]
        public double Latitude
        {
            get { return _latitude; }
            set
            {
                if (_latitude != value)
                {
                    SendPropertyChanging();
                    _latitude = value;
                    SendPropertyChanged("Latitude");
                }
            }
        }

        [Column(Storage = "_longitude", DbType = "Float NOT NULL")]
        public double Longitude
        {
            get { return _longitude; }
            set
            {
                if (_longitude != value)
                {
                    SendPropertyChanging();
                    _longitude = value;
                    SendPropertyChanged("Longitude");
                }
            }
        }

        [Association(Name = "Coordinate_SewerOpening", Storage = "_sewerOpenings", ThisKey = "CoordinateID", OtherKey = "CoordinateID")]
        public EntitySet<SewerOpening> SewerOpenings
        {
            get
            {
                return _sewerOpenings;
            }
            set
            {
                _sewerOpenings.Assign(value);
            }
        }

        [Association(Name = "Coordinate_StormCatches", Storage = "_stormCatches", OtherKey = "CoordinateID")]
        public EntitySet<StormCatch> StormCatches
        {
            get
            {
                return _stormCatches;
            }
            set
            {
                _stormCatches.Assign(value);
            }
        }

        [Association(Name = "Coordinate_Facilities", Storage = "_facilities", OtherKey = "CoordinateID")]
        public EntitySet<Facility> Facilities
        {
            get
            {
                return _facilities;
            }
            set
            {
                _facilities.Assign(value);
            }
        }

        [Association(Name = "Coordinate_Hydrants", Storage = "_hydrants", OtherKey = "CoordinateID")]
        public EntitySet<Hydrant> Hydrants
        {
            get { return _hydrants; }
            set { _hydrants.Assign(value);}
        }

        [Association(Name = "Coordinate_Valves", Storage = "_valves", OtherKey = "CoordinateID")]
        public EntitySet<Valve> Valves
        {
            get { return _valves; }
            set { _valves.Assign(value); }
        }

        #endregion

        #region Constructors

        public Coordinate()
        {
            _sewerOpenings = new EntitySet<SewerOpening>(
                attach_SewerOpenings,
                detach_SewerOpenings);
            _stormCatches = new EntitySet<StormCatch>(
                attach_StormCatches,
                detach_StormCatches);
            _facilities = new EntitySet<Facility>(
                attach_Facilities,
                detach_Facilities);
            _valves = new EntitySet<Valve>(
                attach_Valves, 
                detach_Valves);
            _hydrants = new EntitySet<Hydrant>(
                attach_Hydrants, 
                detach_Hydrants);
        }

        #endregion

        #region Private Methods

        private void OnValidate(ChangeAction action)
        {
            switch (action)
            {
                case ChangeAction.Insert:
                case ChangeAction.Update:
                    if (Latitude == 0 || Longitude == 0)
                        throw new DomainLogicException("Cannot save Coordinate with Latitude or Longitude value of zero.");
                    break;
            }
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

        private void attach_SewerOpenings(SewerOpening entity)
        {
            SendPropertyChanging();
            entity.Coordinate = this;
        }

        private void detach_SewerOpenings(SewerOpening entity)
        {
            SendPropertyChanging();
            entity.Coordinate = null;
        }

        private void attach_StormCatches(StormCatch entity)
        {
            SendPropertyChanging();
            entity.Coordinate = this;
        }

        private void detach_StormCatches(StormCatch entity)
        {
            SendPropertyChanging();
            entity.Coordinate = null;
        }

        private void attach_Facilities(Facility entity)
        {
            SendPropertyChanging();
            entity.Coordinate = this;
        }

        private void detach_Facilities(Facility entity)
        {
            SendPropertyChanging();
            entity.Coordinate = null;
        }

        private void attach_Valves(Valve entity)
        {
            SendPropertyChanging();
            entity.Coordinate = this;
        }

        private void detach_Valves(Valve entity)
        {
            SendPropertyChanging();
            entity.Coordinate = null;
        }

        private void attach_Hydrants(Hydrant entity)
        {
            SendPropertyChanging();
            entity.Coordinate = this;
        }

        private void detach_Hydrants(Hydrant entity)
        {
            SendPropertyChanging();
            entity.Coordinate = null;
        }

        #endregion

        #region Events

        public event PropertyChangingEventHandler PropertyChanging;

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
