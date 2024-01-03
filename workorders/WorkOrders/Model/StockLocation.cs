using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using MMSINC.Exceptions;

namespace WorkOrders.Model
{
    [Table(Name = "dbo.StockLocations")]
    public class StockLocation : INotifyPropertyChanging, INotifyPropertyChanged
    {
        #region Constants

        private const short MAX_DESCRIPTION_LENGTH = 25, 
                            MAX_SAP_STOCK_LOCATION_LENGTH = 50;

        #endregion

        #region Private Static Members

        private static readonly PropertyChangingEventArgs _emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        #endregion

        #region Private Members

        private int _StockLocationID, _operatingCenterID;

        private bool _isActive;

        private string _description, _sapStockLocation;

        private EntityRef<OperatingCenter> _operatingcenter;

        private EntitySet<MaterialsUsed> _materialsuseds;

        #endregion

        #region Properties

        [Column(Storage = "_StockLocationID", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int StockLocationID
        {
            get { return _StockLocationID; }
            set
            {
                if (_StockLocationID != value)
                {
                    SendPropertyChanging();
                    _StockLocationID = value;
                    SendPropertyChanged("StockLocationID");
                }
            }
        }

        [Column(Storage = "_description", DbType = "VarChar(25) NOT NULL")]
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

        [Column(Storage = "_sapStockLocation", DbType = "VarChar(50) NULL")]
        public string SAPStockLocation
        {
            get { return _sapStockLocation; }
            set
            {
                if (value != null && value.Length > MAX_SAP_STOCK_LOCATION_LENGTH)
                    throw new StringTooLongException("SAPStockLocation", MAX_SAP_STOCK_LOCATION_LENGTH);
                if (_sapStockLocation != value)
                {
                    SendPropertyChanging();
                    _sapStockLocation = value;
                    SendPropertyChanged("SAPStockLocation");
                }
            }
        }

        [Column(Storage = "_isActive", DbType = "bit NOT NULL", CanBeNull = false)]
        public bool IsActive
        {
            get { return _isActive; }
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
        
        [Column(Storage = "_operatingCenterID", DbType = "Int NOT NULL")]
        public int OperatingCenterID
        {
            get { return _operatingCenterID; }
            set
            {
                if (_operatingCenterID != value)
                {
                    if (_operatingcenter.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                }
                SendPropertyChanging();
                _operatingCenterID = value;
                SendPropertyChanged("OperatingCenterID");
            }
        }

        [Association(Name = "OperatingCenter_StockLocation", Storage = "_operatingcenter", ThisKey = "OperatingCenterID", IsForeignKey = true)]
        public OperatingCenter OperatingCenter
        {
            get { return _operatingcenter.Entity; }
            set
            {
                var previousValue = _operatingcenter.Entity;
                if ((previousValue != value)
                    || (_operatingcenter.HasLoadedOrAssignedValue == false))
                {
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _operatingcenter.Entity = null;
                        previousValue.StockLocations.Remove(this);
                    }
                    _operatingcenter.Entity = value;
                    if (value != null)
                    {
                        value.StockLocations.Add(this);
                        _operatingCenterID = value.OperatingCenterID;
                    }
                    else
                        _operatingCenterID = default(int);
                    SendPropertyChanged("OperatingCenter");
                }
            }
        }

        [Association(Name = "StockLocation_MaterialsUsed", Storage = "_materialsuseds", OtherKey = "StockLocationID")]
        public EntitySet<MaterialsUsed> MaterialsUseds
        {
            get { return _materialsuseds; }
            set { _materialsuseds.Assign(value); }
        }

        #endregion

        #region Constructors

        public StockLocation()
        {
            _materialsuseds = new EntitySet<MaterialsUsed>(attach_MaterialsUseds, detach_MaterialsUseds);
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

        protected void OnValidate(ChangeAction action)
        {
            switch (action)
            {
                case ChangeAction.Insert:
                case ChangeAction.Update:
                    if (String.IsNullOrEmpty(Description))
                        throw new DomainLogicException("Description cannot be null");
                    if (OperatingCenter == null)
                        throw new DomainLogicException("OperatingCenter cannot be null");
                    break;
            }
        }

        private void attach_MaterialsUseds(MaterialsUsed entity)
        {
            SendPropertyChanging();
            entity.StockLocation = this;
        }

        private void detach_MaterialsUseds(MaterialsUsed entity)
        {
            SendPropertyChanging();
            entity.StockLocation = null;
        }

        #endregion

        #region Events

        public event PropertyChangingEventHandler PropertyChanging;

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Exposed Methods

        public override string ToString()
        {
            return Description;
        }

        #endregion
    }
}
