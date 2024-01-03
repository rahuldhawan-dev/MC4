using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using MapCall.Common.Model.Entities;
using MMSINC.Exceptions;

namespace WorkOrders.Model
{
    [Table(Name = "dbo.ScheduleOfValueTypes")]
    public class ScheduleOfValueType
    {
        #region Private Members

        private int _id;
        private string _description;

        #endregion

        #region Properties

        [Column(Storage = "_id", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int Id
        {
            get { return _id; }
        }

        [Column(Storage = "_description", DbType = "Varchar(255)", UpdateCheck = UpdateCheck.Never)]
        public string Description
        {
            get { return _description; }
        }

        #endregion
        
        #region Exposed Methods

        public override string ToString()
        {
            return Description;
        }

        #endregion
    }


    [Table(Name = "ScheduleOfValueCategories")]
    public class ScheduleOfValueCategory
    {
        #region Private Static Members

        private static readonly PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        #endregion

        #region Private Members

        private int _id, _scheduleOfValueTypeID;
        private string _description;
        private readonly EntitySet<ScheduleOfValue> _scheduleOfValues;
        private EntityRef<ScheduleOfValueType> _scheduleOfValueType;

        #endregion

        #region Properties

        [Column(Storage = "_id", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int Id
        {
            get { return _id; }
        }

        [Column(Storage = "_description", DbType = "Varchar(255)", UpdateCheck = UpdateCheck.Never)]
        public string Description
        {
            get { return _description; }
        }

        [Association(Name= "ScheduleOfValueCategory_ScheduleOfValue", Storage = "_scheduleOfValues", OtherKey = "ScheduleOfValueCategoryID")]
        public EntitySet<ScheduleOfValue> ScheduleOfValues
        {
            get {  return _scheduleOfValues; }
            set { _scheduleOfValues.Assign(value); }
        }

        [Column(Storage = "_scheduleOfValueTypeID", DbType = "Int NOT NULL")]
        public int ScheduleOfValueTypeID
        {
            get { return _scheduleOfValueTypeID;}
            set { _scheduleOfValueTypeID = value; }
        }

        [Association(Name = "ScheduleOfValueType_ScheduleOfValueCategory",Storage = "_scheduleOfValueType", ThisKey = "ScheduleOfValueTypeID",IsForeignKey = true)]
        public ScheduleOfValueType ScheduleOfValueType
        {
            get { return _scheduleOfValueType.Entity; }
            set { _scheduleOfValueType.Entity = value; }
        }

        #endregion

        #region Exposed Methods
        public string Display { get { return Description + "[" + ScheduleOfValueType.Description + "]"; } }
        
        #endregion

        #region Events

        public event PropertyChangingEventHandler PropertyChanging;

        public event PropertyChangedEventHandler PropertyChanged;

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

        private void attach_ScheduleOfValues(ScheduleOfValue entity)
        {
            SendPropertyChanging();
            entity.ScheduleOfValueCategory = this;
        }

        private void detach_ScheduleOfValues(ScheduleOfValue entity)
        {
            SendPropertyChanging();
            entity.ScheduleOfValueCategory = null;
        }

        #endregion

        #region Constructors

        public ScheduleOfValueCategory()
        {
            _scheduleOfValues = new EntitySet<ScheduleOfValue>(attach_ScheduleOfValues, detach_ScheduleOfValues);
        }

        #endregion
    }
}