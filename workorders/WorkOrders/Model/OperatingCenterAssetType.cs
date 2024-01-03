using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using MMSINC.Exceptions;

namespace WorkOrders.Model
{
    [Table(Name = "dbo.OperatingCenterAssetTypes")]
    public class OperatingCenterAssetType : INotifyPropertyChanging, INotifyPropertyChanged
    {
        #region Private Static Members

        private static readonly PropertyChangingEventArgs _emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        #endregion

        #region Private Members

        private int _operatingCenterAssetTypeID, _operatingCenterID, _assetTypeID;


        private EntityRef<AssetType> _assetType;

        private EntityRef<OperatingCenter> _operatingCenter;

        #endregion

        #region Properties

        [Column(Storage = "_operatingCenterAssetTypeID", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int OperatingCenterAssetTypeID
        {
            get { return _operatingCenterAssetTypeID; }
            set
            {
                if (_operatingCenterAssetTypeID != value)
                {
                    SendPropertyChanging();
                    _operatingCenterAssetTypeID = value;
                    SendPropertyChanged("OperatingCenterAssetTypeID");
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
                    if (_operatingCenter.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                    SendPropertyChanging();
                    _operatingCenterID = value;
                    SendPropertyChanged("OperatingCenterID");
                }
            }
        }

        [Column(Storage = "_assetTypeID", DbType = "Int NOT NULL")]
        public int AssetTypeID
        {
            get { return _assetTypeID; }
            set
            {
                if ((_assetTypeID != value))
                {
                    if (_assetType.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                    SendPropertyChanging();
                    _assetTypeID = value;
                    SendPropertyChanged("AssetTypeID");
                }
            }
        }

        [Association(Name = "OperatingCenter_OperatingCenterAssetType", Storage = "_operatingCenter", ThisKey = "OperatingCenterID", IsForeignKey = true)]
        public OperatingCenter OperatingCenter
        {
            get { return _operatingCenter.Entity; }
            set
            {
                var previousValue = _operatingCenter.Entity;
                if (((previousValue != value)
                     || (_operatingCenter.HasLoadedOrAssignedValue == false)))
                {
                    SendPropertyChanging();
                    if ((previousValue != null))
                    {
                        _operatingCenter.Entity = null;
                        previousValue.OperatingCenterAssetTypes.Remove(this);
                    }
                    _operatingCenter.Entity = value;
                    if ((value != null))
                    {
                        value.OperatingCenterAssetTypes.Add(this);
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

        [Association(Name = "AssetType_OperatingCenterAssetType", Storage = "_assetType", ThisKey = "AssetTypeID", IsForeignKey = true)]
        public AssetType AssetType
        {
            get { return _assetType.Entity; }
            set
            {
                var previousValue = _assetType.Entity;
                if (((previousValue != value)
                     || (_assetType.HasLoadedOrAssignedValue == false)))
                {
                    SendPropertyChanging();
                    if ((previousValue != null))
                    {
                        _assetType.Entity = null;
                        previousValue.OperatingCenterAssetTypes.Remove(this);
                    }
                    _assetType.Entity = value;
                    if ((value != null))
                    {
                        value.OperatingCenterAssetTypes.Add(this);
                        _assetTypeID = value.AssetTypeID;
                    }
                    else
                    {
                        _assetTypeID = default(int);
                    }
                    SendPropertyChanged("AssetType");
                }
            }
        }



        #endregion

        #region Constructors

        public OperatingCenterAssetType()
        {
            _operatingCenter = default(EntityRef<OperatingCenter>);
            _assetType = default(EntityRef<AssetType>);
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
                    ValidateCreationInfo();
                    break;
            }
        }

        private void ValidateCreationInfo()
        {
            if (AssetType == null)
                throw new DomainLogicException(
                    "Cannot save an OperatingCenterAssetType object without an AssetType.");
            if (OperatingCenter == null)
                throw new DomainLogicException(
                    "Cannot save an OperatingCenterAssetType object without an OperatingCenter.");
        }

        #endregion

        #region Events

        public event PropertyChangingEventHandler PropertyChanging;

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
