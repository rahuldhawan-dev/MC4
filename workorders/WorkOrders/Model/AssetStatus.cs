using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using MMSINC.Exceptions;

namespace WorkOrders.Model
{
    [Table(Name = "dbo.AssetStatuses")]
    public class AssetStatus : INotifyPropertyChanging, INotifyPropertyChanged
    {
        #region Constants

        private const short MAX_DESCRIPTION_LENGTH = 50;

        #endregion

        #region Private Static Members

        private static readonly PropertyChangingEventArgs _emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        #endregion

        #region Private Members

        private int _AssetStatusID;
        private string _Description;

        private EntitySet<SewerOpening> _SewerOpenings;
        private EntitySet<StormCatch> _stormCatches;
        private EntitySet<Equipment> _equipment;

        #endregion

        #region Properties

        [Column(Storage = "_AssetStatusID", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int AssetStatusID
        {
            get
            {
                return _AssetStatusID;
            }
            set
            {
                if ((_AssetStatusID != value))
                {
                    SendPropertyChanging();
                    _AssetStatusID = value;
                    SendPropertyChanged("AssetStatusID");
                }
            }
        }

        [Column(Storage = "_Description", DbType = "VarChar(50)")]
        public string Description
        {
            get
            {
                return _Description;
            }
            set
            {
                if (value != null && value.Length > MAX_DESCRIPTION_LENGTH)
                    throw new StringTooLongException("Description", MAX_DESCRIPTION_LENGTH);
                if ((_Description != value))
                {
                    SendPropertyChanging();
                    _Description = value;
                    SendPropertyChanged("Description");
                }
            }
        }

        [Association(Name = "AssetStatuse_SewerOpening", Storage = "_SewerOpenings", ThisKey = "AssetStatusID", OtherKey = "AssetStatusID")]
        public EntitySet<SewerOpening> SewerOpenings
        {
            get
            {
                return _SewerOpenings;
            }
            set
            {
                _SewerOpenings.Assign(value);
            }
        }

        [Association(Name = "AssetStatuses_StormCatch", Storage = "_stormCatches", OtherKey = "AssetStatusID")]
        public EntitySet<StormCatch> StormCatches
        {
            get { return _stormCatches; }
            set { _stormCatches.Assign(value); }
        }
        
        [Association(Name = "AssetStatuses_Equipment", Storage = "_equipment", OtherKey = "StatusID")]
        public EntitySet<Equipment> Equipment
        {
            get { return _equipment; }
            set { _equipment.Assign(value); }
        }

        #endregion

        #region Constructors

        public AssetStatus()
		{
		    _SewerOpenings = new EntitySet<SewerOpening>(
                attach_SewerOpenings,
		        detach_SewerOpenings);
		    _stormCatches = new EntitySet<StormCatch>(
		        attach_StormCatches, 
                detach_StormCatches);
            _equipment = new EntitySet<Equipment>(
                attach_Equipment,detach_Equipment);
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

        private void attach_SewerOpenings(SewerOpening entity)
        {
            SendPropertyChanging();
            entity.AssetStatus = this;
        }

        private void detach_SewerOpenings(SewerOpening entity)
        {
            SendPropertyChanging();
            entity.AssetStatus = null;
        }

        private void attach_StormCatches(StormCatch entity)
        {
            SendPropertyChanging();
            entity.AssetStatus = this;
        }

        private void detach_StormCatches(StormCatch entity)
        {
            SendPropertyChanging();
            entity.AssetStatus = null;
        }

        private void attach_Equipment(Equipment entity)
        {
            SendPropertyChanging();
            entity.AssetStatus = this;
        }

        private void detach_Equipment(Equipment entity)
        {
            SendPropertyChanging();
            entity.AssetStatus = null;
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