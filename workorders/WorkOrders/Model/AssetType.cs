using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using MMSINC.Exceptions;

namespace WorkOrders.Model
{
    [Table(Name = "dbo.AssetTypes")]
    public class AssetType : INotifyPropertyChanging, INotifyPropertyChanged
    {
        #region Constants

        private const short MAX_DESCRIPTION_LENGTH = 13, MAX_ONE_MAP_FEATURE_SOURCE = 255;

        #endregion

        #region Private Static Members

        private static readonly PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        #endregion

        #region Private Members

        private int _assetTypeID;

        private AssetTypeEnum? _typeEnum;

        private string _description, _oneMapFeatureSource;

        private readonly EntitySet<WorkOrder> _workOrders;

        private readonly EntitySet<WorkDescription> _workDescriptions;

        private readonly EntitySet<OperatingCenterAssetType> _operatingCenterAssetTypes;

        #endregion

        #region Properties

        public AssetTypeEnum TypeEnum
        {
            get
            {
                if (_typeEnum == null)
                    _typeEnum = AssetTypeRepository.GetEnumerationValue(this);
                return _typeEnum.Value;
            }
        }

        [Column(Storage = "_assetTypeID", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int AssetTypeID
        {
            get { return _assetTypeID; }
            set
            {
                if (_assetTypeID != value)
                {
                    SendPropertyChanging();
                    _assetTypeID = value;
                    SendPropertyChanged("AssetTypeID");
                }
            }
        }

        [Column(Storage = "_description", DbType = "VarChar(13) NOT NULL")]
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

        [Column(Storage = "_oneMapFeatureSource", DbType = "VarChar(255) NULL")]
        public string OneMapFeatureSource
        {
            get { return _oneMapFeatureSource; }
            set
            {
                if (value != null && value.Length > MAX_ONE_MAP_FEATURE_SOURCE)
                    throw new StringTooLongException("OneMapFeatureSource", MAX_ONE_MAP_FEATURE_SOURCE);
                if (_oneMapFeatureSource != value)
                {
                    SendPropertyChanging();
                    _description = value;
                    SendPropertyChanged("OneMapFeatureSource");
                }
            }
        }

        [Association(Name = "AssetType_WorkOrder", Storage = "_workOrders", OtherKey = "AssetTypeID")]
        public EntitySet<WorkOrder> WorkOrders
        {
            get { return _workOrders; }
            set { _workOrders.Assign(value); }
        }

        [Association(Name = "AssetType_WorkDescription", Storage = "_workDescriptions", OtherKey = "AssetTypeID")]
        public EntitySet<WorkDescription> WorkDescriptions
        {
            get { return _workDescriptions; }
            set { _workDescriptions.Assign(value); }
        }

        [Association(Name = "AssetType_OperatingCenterAssetType", Storage = "_operatingCenterAssetTypes", OtherKey = "AssetTypeID")]
        public EntitySet<OperatingCenterAssetType> OperatingCenterAssetTypes
        {
            get { return _operatingCenterAssetTypes; }
            set { _operatingCenterAssetTypes.Assign(value); }
        }


        #endregion

        #region Constructors

        public AssetType()
        {
            _workOrders = new EntitySet<WorkOrder>(attach_WorkOrders, detach_WorkOrders);
            _workDescriptions = new EntitySet<WorkDescription>(attach_WorkDescriptions, detach_WorkDescriptions);
            _operatingCenterAssetTypes = new EntitySet<OperatingCenterAssetType>(attach_OperatingCenterAssetTypes,detach_OperatingCenterAssetTypes);
        }

        #endregion

        #region Private Methods

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

        // ReSharper disable UnusedPrivateMember
        private void OnValidate(ChangeAction action)
        {
            switch (action)
            {
                case ChangeAction.Insert:
                case ChangeAction.Update:
                    if (String.IsNullOrEmpty(Description))
                        throw new DomainLogicException("Description cannot be null");
                    break;
            }
        }
        // ReSharper restore UnusedPrivateMember

        private void attach_WorkOrders(WorkOrder entity)
        {
            SendPropertyChanging();
            entity.AssetType = this;
        }

        private void detach_WorkOrders(WorkOrder entity)
        {
            SendPropertyChanging();
            entity.AssetType = null;
        }

        private void attach_WorkDescriptions(WorkDescription entity)
        {
            SendPropertyChanging();
            entity.AssetType = this;
        }

        private void detach_WorkDescriptions(WorkDescription entity)
        {
            SendPropertyChanging();
            entity.AssetType = null;
        }

        private void attach_OperatingCenterAssetTypes(OperatingCenterAssetType entity)
        {
            SendPropertyChanging();
            entity.AssetType = this;
        }

        private void detach_OperatingCenterAssetTypes(OperatingCenterAssetType entity)
        {
            SendPropertyChanging();
            entity.AssetType = null;
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

    public enum AssetTypeEnum
    {
        Valve, Hydrant, Main, Service, SewerOpening, SewerLateral, SewerMain, StormCatch, Equipment, MainCrossing
    }
}
