using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace WorkOrders.Model
{
    // NOTE: This is very very slimmed down compared to MapCall.Common's version because this is supposed to be 
    // entirely readonly on the 271 side. That means properties don't have setters and almost none of the actual
    // properties are referenced.
    [Table(Name = "dbo.tblJobObservations")]
    public class JobObservation : INotifyPropertyChanging, INotifyPropertyChanged
    {
        #region Private Members

        #pragma warning disable 649
        private int _JobObservationID;
        private int? _WorkOrderID, _overallQualityRatingID, _overallSafetyRatingID;
        private DateTime _ObservationDate;
        private EntityRef<OverallQualityRating> _overallQualityRating;
        private EntityRef<OverallSafetyRating> _overallSafetyRating;
        private EntityRef<WorkOrder> _workOrder;
#pragma warning restore 649

        #endregion

        #region Private Static Members

        private static readonly PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        #endregion

        #region Properties

        [Column(Storage = "_JobObservationID", Name = "JobObservationID", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int JobObservationID
        {
            get { return _JobObservationID; }
        }

        [Column(Name = "WorkOrderId", Storage = "_WorkOrderID", DbType = "Int NULL", UpdateCheck = UpdateCheck.Never, CanBeNull = true)]
        public int? WorkOrderID
        {
            get { return _WorkOrderID; }
        }

        [Association(Name = "WorkOrder_JobObservation", Storage = "_workOrder", ThisKey = "WorkOrderID", IsForeignKey = true)]
        public WorkOrder WorkOrder
        {
            get { return _workOrder.Entity; }
        }

        [Column(Storage = "_ObservationDate", DbType = "datetime NOT NULL", UpdateCheck = UpdateCheck.Never)]
        public DateTime ObservationDate
        {
            get { return _ObservationDate; }
        }

        [Column(Storage = "_overallQualityRatingID", Name = "OverallQualityRating", DbType = "Int NULL", UpdateCheck = UpdateCheck.Never, CanBeNull = true)]
        public int? OverallQualityRatingID
        {
            get { return _overallQualityRatingID; }
        }

        [Association(Name = "OverallQualityRating_JobObservation", Storage = "_overallQualityRating", ThisKey = "OverallQualityRatingID", IsForeignKey = true)]
        public OverallQualityRating OverallQualityRating
        {
            get { return _overallQualityRating.Entity; }
        }

        [Column(Storage = "_overallSafetyRatingID", Name = "OverallSafetyRating", DbType = "Int NULL", UpdateCheck = UpdateCheck.Never, CanBeNull = true)]
        public int? OverallSafetyRatingID
        {
            get { return _overallSafetyRatingID; }
        }

        [Association(Name = "OverallSafetyRating_JobObservation", Storage = "_overallSafetyRating", ThisKey = "OverallSafetyRatingID", IsForeignKey = true)]
        public OverallSafetyRating OverallSafetyRating
        {
            get { return _overallSafetyRating.Entity; }
        }

        #endregion

        #region Private Methods

        protected void SendPropertyChanging()
        {
            if (PropertyChanging != null)
                PropertyChanging(this, emptyChangingEventArgs);
        }

        protected void SendPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion


        #region Events

        public event PropertyChangingEventHandler PropertyChanging;
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }

    [Table(Name = "OverallQualityRatings")]
    public class OverallQualityRating
    {
        #region Private Members

#pragma warning disable 649
        private int _id;
        private string _description;
#pragma warning restore 649

        #endregion

        #region Private Static Members

        private static readonly PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        #endregion

        #region Properties

        [Column(Storage = "_id", Name = "Id", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int Id
        {
            get
            {
                return _id;
            }
        }

        [Column(Storage = "_description", DbType = "VarChar(50)")]
        public string Description
        {
            get { return _description; }
        }

        #endregion

        #region Private Methods

        protected void SendPropertyChanging()
        {
            if (PropertyChanging != null)
                PropertyChanging(this, emptyChangingEventArgs);
        }

        protected void SendPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
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
    
    [Table(Name = "OverallSafetyRatings")]
    public class OverallSafetyRating 
    {
        #region Private Members

#pragma warning disable 649
        private int _id;
        private string _description;
#pragma warning restore 649

        #endregion

        #region Private Static Members

        private static readonly PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        #endregion

        #region Properties

        [Column(Storage = "_id", Name = "Id", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int Id
        {
            get
            {
                return _id;
            }
        }

        [Column(Storage = "_description", DbType = "VarChar(50)")]
        public string Description
        {
            get { return _description; }
        }

        #endregion

        #region Private Methods

        protected void SendPropertyChanging()
        {
            if (PropertyChanging != null)
                PropertyChanging(this, emptyChangingEventArgs);
        }

        protected void SendPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
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