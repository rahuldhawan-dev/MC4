using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using MMSINC.Exceptions;

namespace WorkOrders.Model
{
    [Table(Name = "dbo.OperatingCenterStockedMaterials")]
    public class OperatingCenterStockedMaterial : INotifyPropertyChanging, INotifyPropertyChanged
    {
        #region Private Static Members

        private static readonly PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        #endregion

        #region Private Members

        private int _operatingCenterStockedMaterialID,
                    _operatingCenterID,
                    _materialID;

        private EntityRef<Material> _material;

        private EntityRef<OperatingCenter> _tblOpCntr;

        #endregion

        #region Properties

        [Column(Storage = "_operatingCenterStockedMaterialID", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int OperatingCenterStockedMaterialID
        {
            get { return _operatingCenterStockedMaterialID; }
            set
            {
                if ((_operatingCenterStockedMaterialID != value))
                {
                    SendPropertyChanging();
                    _operatingCenterStockedMaterialID = value;
                    SendPropertyChanged("OperatingCenterStockedMaterialID");
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
                    if (_tblOpCntr.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                    SendPropertyChanging();
                    _operatingCenterID = value;
                    SendPropertyChanged("OperatingCenterID");
                }
            }
        }

        [Column(Storage = "_materialID", DbType = "Int NOT NULL")]
        public int MaterialID
        {
            get { return _materialID; }
            set
            {
                if ((_materialID != value))
                {
                    if (_material.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                    SendPropertyChanging();
                    _materialID = value;
                    SendPropertyChanged("MaterialID");
                }
            }
        }

        [Association(Name = "Material_OperatingCenterStockedMaterial", Storage = "_material", ThisKey = "MaterialID", IsForeignKey = true)]
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
                        previousValue.OperatingCenterStockedMaterials.Remove(this);
                    }
                    _material.Entity = value;
                    if ((value != null))
                    {
                        value.OperatingCenterStockedMaterials.Add(this);
                        _materialID = value.MaterialID;
                    }
                    else
                    {
                        _materialID = default(int);
                    }
                    SendPropertyChanged("Material");
                }
            }
        }

        [Association(Name = "OperatingCenter_OperatingCenterStockedMaterial", Storage = "_tblOpCntr", ThisKey = "OperatingCenterID", IsForeignKey = true)]
        public OperatingCenter OperatingCenter
        {
            get { return _tblOpCntr.Entity; }
            set
            {
                var previousValue = _tblOpCntr.Entity;
                if ((previousValue != value)
                     || (_tblOpCntr.HasLoadedOrAssignedValue == false))
                {
                    SendPropertyChanging();
                    if ((previousValue != null))
                    {
                        _tblOpCntr.Entity = null;
                        previousValue.OperatingCenterStockedMaterials.Remove(this);
                    }
                    _tblOpCntr.Entity = value;
                    if ((value != null))
                    {
                        value.OperatingCenterStockedMaterials.Add(this);
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

        #endregion

        #region Constructors

        public OperatingCenterStockedMaterial()
        {
            _material = default(EntityRef<Material>);
            _tblOpCntr = default(EntityRef<OperatingCenter>);
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
            if (Material == null)
                throw new DomainLogicException(
                    "Cannot save an OperatingCenterStockedMaterial object without a Material.");
            if (OperatingCenter == null)
                throw new DomainLogicException(
                    "Cannot save an OperatingCenterStockedMaterial object without an OperatingCenter.");
        }

        #endregion

        #region Events

        public event PropertyChangingEventHandler PropertyChanging;

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
