using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using MapCall.Common.Model.Entities;
using MMSINC.Exceptions;

namespace WorkOrders.Model
{
    [Table(Name = "dbo.ScheduleOfValues")]
    public class ScheduleOfValue : INotifyPropertyChanging, INotifyPropertyChanged
    {
        #region Constants

        #endregion

        #region Private Static Members

        private static readonly PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        #endregion
        
        #region Private Members

        private int _id, _scheduleOfValueCategoryID;
        private int? _unitOfMeasureID;
        private string _description;
        private EntityRef<UnitOfMeasure> _unitOfMeasure;
        private EntityRef<ScheduleOfValueCategory> _scheduleOfValueCategory;
        private decimal? _laborUnitOvertimeCost, _laborUnitCost;

        #endregion

        #region Properties

        [Column(Storage = "_id", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int Id
        {
            get { return _id; }
            set
            {
                if (_id != value)
                {
                    SendPropertyChanging();
                    _id = value;
                    SendPropertyChanged("Id");
                }
            }
        }

        [Column(Storage = "_description", DbType = "varchar(255)", UpdateCheck = UpdateCheck.Never)]
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

        [Column(Storage = "_laborUnitOvertimeCost", DbType = "Money")]
        public decimal? LaborUnitOvertimeCost
        {
            get { return _laborUnitOvertimeCost; }
            set
            {
                if (_laborUnitOvertimeCost != value)
                {
                    SendPropertyChanging();
                    _laborUnitOvertimeCost = value;
                    SendPropertyChanged("LaborUnitOvertimeCost");
                }
            }
        }

        [Column(Storage = "_laborUnitCost", DbType = "Money")]
        public decimal? LaborUnitCost
        {
            get { return _laborUnitCost; }
            set
            {
                if (_laborUnitCost != value)
                {
                    SendPropertyChanging();
                    _laborUnitCost = value;
                    SendPropertyChanged("LaborUnitCost");
                }
            }
        }

        [Column(Storage = "_unitOfMeasureID", DbType = "Int NULL")]
        public int? UnitOfMeasureID { get; set; }

        [Association(Name = "UnitOfMeasure_ScheduleOfValue", Storage = "_unitOfMeasure", ThisKey = "UnitOfMeasureID", IsForeignKey = true)]
        public UnitOfMeasure UnitOfMeasure
        {
            get { return _unitOfMeasure.Entity; }
            set
            {
                var previousValue = _unitOfMeasure.Entity;
                if (((previousValue != value)
                     || (_unitOfMeasure.HasLoadedOrAssignedValue == false)))
                {
                    SendPropertyChanging();
                    if ((previousValue != null))
                    {
                        _unitOfMeasure.Entity = null;
                        //previousValue.MaterialsUseds.Remove(this);
                    }
                    _unitOfMeasure.Entity = value;
                    if ((value != null))
                    {
                        //value.MaterialsUseds.Add(this);
                        _unitOfMeasureID = value.UnitOfMeasureID;
                    }
                    else
                    {
                        _unitOfMeasureID = default(int);
                    }
                    SendPropertyChanged("UnitOfMeasure");
                }
            }
        }

        [Column(Storage = "_scheduleOfValueCategoryID", DbType = "Int NOT NULL", UpdateCheck = UpdateCheck.Never)]
        public int ScheduleOfValueCategoryID {
            get { return _scheduleOfValueCategoryID; }
            set
            {
                if (_scheduleOfValueCategoryID != value)
                {
                    if(_scheduleOfValueCategory.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                    SendPropertyChanging();
                    _scheduleOfValueCategoryID = value;
                    SendPropertyChanged("ScheduleOfValueCategoryID");
                }
            }
        }

        [Association(Name = "ScheduleOfValueCategory_ScheduleOfValue", 
            Storage = "_scheduleOfValueCategory", 
            ThisKey = "ScheduleOfValueCategoryID", 
            //OtherKey = "Id", 
            IsForeignKey = true)]
        public ScheduleOfValueCategory ScheduleOfValueCategory
        {
            get { return _scheduleOfValueCategory.Entity; }
            set
            {
                var previousValue = _scheduleOfValueCategory.Entity;
                if (((previousValue != value)
                     || (_scheduleOfValueCategory.HasLoadedOrAssignedValue == false)))
                {
                    SendPropertyChanging();
                    if ((previousValue != null))
                    {
                        _scheduleOfValueCategory.Entity = null;
                        //previousValue.MaterialsUseds.Remove(this);
                    }
                    _scheduleOfValueCategory.Entity = value;
                    if ((value != null))
                    {
                        //value.MaterialsUseds.Add(this);
                        _scheduleOfValueCategoryID = value.Id;
                    }
                    else
                    {
                        _scheduleOfValueCategoryID = default(int);
                    }
                    SendPropertyChanged("ScheduleOfValueCategory");
                }
            }
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

        #endregion
        
        #region Constructors

        public ScheduleOfValue()
        {
            _unitOfMeasure = default(EntityRef<UnitOfMeasure>);
            _scheduleOfValueCategory = default(EntityRef<ScheduleOfValueCategory>);
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