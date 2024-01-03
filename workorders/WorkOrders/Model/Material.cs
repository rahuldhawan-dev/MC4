using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using MMSINC.Exceptions;

namespace WorkOrders.Model
{
    // NOTE: There's an OldPartNumbers columb in the database from before the
    //       SAP material part number change on 7/31/12

    [Table(Name = "dbo.Materials")]
    public class Material : INotifyPropertyChanging, INotifyPropertyChanged
    {
        #region Constants

        private const short MAX_PARTNUMBER_LENGTH = 15;

        #endregion

        #region Private Static Members

        private static readonly PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        #endregion

        #region Private Members

        private int _materialID;

        private string _partNumber, _description, _size;

        private readonly EntitySet<MaterialsUsed> _materialsUseds;

        private readonly EntitySet<OperatingCenterStockedMaterial> _operatingCenterStockedMaterials;

        private bool _isActive, _doNotOrder;

        #endregion

        #region Properties

        [Column(Storage = "_doNotOrder", DbType = "bit NOT NULL", CanBeNull = false)]
        public bool DoNotOrder
        {
            get { return _doNotOrder; }
            set
            {
                if (_doNotOrder != value)
                {
                    SendPropertyChanging();
                    _doNotOrder = value;
                    SendPropertyChanged("DoNotOrder");
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

        [Column(Storage = "_partNumber", DbType = "VarChar(15) NOT NULL", CanBeNull = false)]
        public string PartNumber
        {
            get { return _partNumber; }
            set
            {
                if (value != null && value.Length > MAX_PARTNUMBER_LENGTH)
                    throw new StringTooLongException("PartNumber", MAX_PARTNUMBER_LENGTH);
                if (_partNumber != value)
                {
                    SendPropertyChanging();
                    _partNumber = value;
                    SendPropertyChanged("PartNumber");
                }
            }
        }

        [Column(Storage = "_materialID", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int MaterialID
        {
            get { return _materialID; }
            set
            {
                if (_materialID != value)
                {
                    SendPropertyChanging();
                    _materialID = value;
                    SendPropertyChanged("MaterialID");
                }
            }
        }

        [Column(Storage = "_description", DbType = "Text", UpdateCheck = UpdateCheck.Never)]
        public string Description
        {
            get { return _description; }
            set
            {
                if (_description != value)
                {
                    SendPropertyChanging();
                    _description = value;
                    SendPropertyChanged("Description");
                }
            }
        }

        [Column(Storage = "_size", DbType = "Text", UpdateCheck = UpdateCheck.Never)]
        public string Size
        {
            get { return _size; }
            set
            {
                if (_size != value)
                {
                    SendPropertyChanging();
                    _size = value;
                    SendPropertyChanged("Size");
                }
            }
        }

        [Association(Name = "Material_MaterialsUsed", Storage = "_materialsUseds", OtherKey = "MaterialID")]
        public EntitySet<MaterialsUsed> MaterialsUseds
        {
            get { return _materialsUseds; }
            set { _materialsUseds.Assign(value); }
        }

        [Association(Name = "Material_OperatingCenterStockedMaterial", Storage = "_operatingCenterStockedMaterials", OtherKey = "MaterialID")]
        public EntitySet<OperatingCenterStockedMaterial> OperatingCenterStockedMaterials
        {
            get { return _operatingCenterStockedMaterials; }
            set { _operatingCenterStockedMaterials.Assign(value); }
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
                    if (String.IsNullOrEmpty(PartNumber))
                        throw new DomainLogicException("Cannot save a Material object without a PartNumber.");
                    break;
            }
        }
        // ReSharper restore UnusedPrivateMember

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

        private void attach_MaterialsUseds(MaterialsUsed entity)
        {
            SendPropertyChanging();
            entity.Material = this;
        }

        private void detach_MaterialsUseds(MaterialsUsed entity)
        {
            SendPropertyChanging();
            entity.Material = null;
        }

        private void attach_OperatingCenterStockedMaterials(OperatingCenterStockedMaterial entity)
        {
            SendPropertyChanging();
            entity.Material = this;
        }

        private void detach_OperatingCenterStockedMaterials(OperatingCenterStockedMaterial entity)
        {
            SendPropertyChanging();
            entity.Material = null;
        }

        #endregion

        #region Constructors

        public Material()
        {
            _materialsUseds = new EntitySet<MaterialsUsed>(attach_MaterialsUseds, detach_MaterialsUseds);
            _operatingCenterStockedMaterials = new EntitySet<OperatingCenterStockedMaterial>(attach_OperatingCenterStockedMaterials, detach_OperatingCenterStockedMaterials);
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