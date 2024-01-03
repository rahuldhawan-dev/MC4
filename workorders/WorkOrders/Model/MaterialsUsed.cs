using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using MMSINC.Exceptions;

namespace WorkOrders.Model
{
    [Table(Name = "dbo.MaterialsUsed")]
    public class MaterialsUsed : INotifyPropertyChanging, INotifyPropertyChanged
    {
        #region Constants

        private const string NOT_APPLICABLE = "n/a";

        #endregion

        #region Private Static Members

        private static readonly PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        #endregion

        #region Private Members

        private int _workOrderID, _materialsUsedID;

        private int? _materialID, _stocklocationID;

        // protected for the sake of testing
        protected short _quantity;

        private string _nonStockDescription;

        private EntityRef<Material> _material;

        private EntityRef<WorkOrder> _workOrder;

        private EntityRef<StockLocation> _stocklocation;

        #endregion

        #region Properties

        #region Logical Properties

        public string Description
        {
            get
            {
                return (Material == null)
                           ? NonStockDescription : Material.Description;
            }
            set { NonStockDescription = value; }
        }

        public string PartNumber
        {
            get
            {
                return (Material == null)
                           ? NOT_APPLICABLE : Material.PartNumber;
            }
        }

        #endregion

        #region Table Column Properties

        [Column(Storage = "_quantity", DbType = "SmallInt NOT NULL")]
        public short Quantity
        {
            get { return _quantity; }
            set
            {
                if (value < 1)
                    throw new DomainLogicException("Cannot set Quantity for a record of MaterialsUsed to a value less than one.");
                if (_quantity != value)
                {
                    SendPropertyChanging();
                    _quantity = value;
                    SendPropertyChanged("Quantity");
                }
            }
        }

        [Column(Storage = "_materialsUsedID", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int MaterialsUsedID
        {
            get { return _materialsUsedID; }
            set
            {
                if (_materialsUsedID != value)
                {
                    SendPropertyChanging();
                    _materialsUsedID = value;
                    SendPropertyChanged("MaterialsUsedID");
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

        [Column(Storage = "_materialID", DbType = "Int")]
        public int? MaterialID
        {
            get { return _materialID; }
            set
            {
                if (_materialID != value)
                {
                    if (_material.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                    SendPropertyChanging();
                    _materialID = value;
                    SendPropertyChanged("MaterialID");
                }
            }
        }

        [Column(Storage = "_nonStockDescription", DbType = "Text", UpdateCheck = UpdateCheck.Never)]
        public string NonStockDescription
        {
            get { return _nonStockDescription; }
            set
            {
                if (_nonStockDescription != value)
                {
                    SendPropertyChanging();
                    _nonStockDescription = value;
                    SendPropertyChanged("NonStockDescription");
                }
            }
        }

        [Column(Storage = "_stocklocationID", DbType = "Int")]
        public int? StockLocationID
        {
            get { return _stocklocationID; }
            set
            {
                if (_stocklocationID != value)
                {
                    if (_stocklocation.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                }
                SendPropertyChanging();
                _stocklocationID = value;
                SendPropertyChanged("StockLocationID");
            }
        }

        #endregion

        #region Association Properties

        [Association(Name = "WorkOrder_MaterialsUsed", Storage = "_workOrder", ThisKey = "WorkOrderID", IsForeignKey = true)]
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
                        throw new DomainLogicException("Cannot change the WorkOrder of a MaterialsUsed record once it has been set.");
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _workOrder.Entity = null;
                        previousValue.MaterialsUseds.Remove(this);
                    }
                    _workOrder.Entity = value;
                    if (value != null)
                    {
                        value.MaterialsUseds.Add(this);
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

        [Association(Name = "Material_MaterialsUsed", Storage = "_material", ThisKey = "MaterialID", IsForeignKey = true)]
        public Material Material
        {
            get { return _material.Entity; }
            set
            {
                var previousValue = _material.Entity;
                if (((previousValue != value)
                     || (_material.HasLoadedOrAssignedValue == false)))
                {
                    SendPropertyChanging();
                    if ((previousValue != null))
                    {
                        _material.Entity = null;
                        previousValue.MaterialsUseds.Remove(this);
                    }
                    _material.Entity = value;
                    if ((value != null))
                    {
                        value.MaterialsUseds.Add(this);
                        _materialID = value.MaterialID;
                    }
                    else
                    {
                        _materialID = default(int?);
                    }
                    SendPropertyChanged("Material");
                }
            }
        }

        [Association(Name = "StockLocation_MaterialsUsed", Storage = "_stocklocation", ThisKey = "StockLocationID", IsForeignKey = true)]
        public StockLocation StockLocation
        {
            get { return _stocklocation.Entity; }
            set
            {
                StockLocation previousValue = _stocklocation.Entity;
                if ((previousValue != value)
                    || (_stocklocation.HasLoadedOrAssignedValue == false))
                {
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _stocklocation.Entity = null;
                        previousValue.MaterialsUseds.Remove(this);
                    }
                    _stocklocation.Entity = value;
                    if (value != null)
                    {
                        value.MaterialsUseds.Add(this);
                        _stocklocationID = value.StockLocationID;
                    }
                    else
                        _stocklocationID = default(int);
                    SendPropertyChanged("StockLocation");
                }
            }
        }

        #endregion

        #endregion

        #region Private Methods

        // ReSharper disable UnusedPrivateMember
        // ReSharper disable UnusedMember.Local
        private void OnValidate(ChangeAction action)
        {
            if (Material != null)
                Description = null;
            switch (action)
            {
                case ChangeAction.Insert:
                case ChangeAction.Update:
                    ValidateCreationInfo();
                    break;
            }
        }
        // ReSharper restore UnusedMember.Local
        // ReSharper restore UnusedPrivateMember

        private void ValidateCreationInfo()
        {
            if (_quantity < 1)
                throw new DomainLogicException(
                    "Cannot save a record of MaterialsUsed with a Quantity less than one.");
            if (WorkOrder == null)
                throw new DomainLogicException(
                    "Cannot save a MaterialsUsed record without a WorkOrder.");
            if (Material != null && StockLocation == null)
                throw new DomainLogicException(
                    "Cannot save a MaterialsUsed with a stock Material without specifying Stock Location.");
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

        #region Constructors

        public MaterialsUsed()
        {
            _material = default(EntityRef<Material>);
            _workOrder = default(EntityRef<WorkOrder>);
        }

        #endregion

        #region Events

        public event PropertyChangingEventHandler PropertyChanging;

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
