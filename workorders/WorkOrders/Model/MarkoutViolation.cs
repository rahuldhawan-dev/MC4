using System;
using System.ComponentModel;
using System.Data.Linq.Mapping;

namespace WorkOrders.Model
{
    [Table(Name = "dbo.MarkoutViolations")]
    public class MarkoutViolation : INotifyPropertyChanging, INotifyPropertyChanged
    {
        #region Private Members

        private int _id;
        private int? _WorkOrderId;
        private DateTime _DateOfViolationNotice;
        private string _MarkoutViolationStatus, _MarkoutRequestNumber;
       

        #endregion

        #region Private Static Members

        private static readonly PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        #endregion

        #region Properties

        [Column(Storage = "_id", Name = "Id", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int Id
        {
            get { return _id; }
        }

        [Column(Storage = "_WorkOrderId", Name = "WorkOrderId", DbType = "Int NULL", UpdateCheck = UpdateCheck.Never, CanBeNull = true)]
        public int? WorkOrderId
        {
            get { return _WorkOrderId; }
        }

        //DateOfViolationNotice
        [Column(Storage = "_DateOfViolationNotice", Name = "DateOfViolationNotice", DbType = "datetime null", UpdateCheck = UpdateCheck.Never, CanBeNull = true)]
        public DateTime? DateOfViolationNotice
        {
            get { return _DateOfViolationNotice; }
        }
        //MarkoutRequestNumber
        [Column(Storage = "_MarkoutRequestNumber", Name = "MarkoutRequestNumber", DbType = "varchar(255) NULL", UpdateCheck = UpdateCheck.Never, CanBeNull = true)]
        public string MarkoutRequestNumber
        {
            get { return _MarkoutRequestNumber; }
        }
        //MarkoutViolationStatus
        [Column(Storage = "_MarkoutViolationStatus", Name = "MarkoutViolationStatus", DbType = "varchar(255) NULL", UpdateCheck = UpdateCheck.Never, CanBeNull = true)]
        public string MarkoutViolationStatus
        {
            get { return _MarkoutViolationStatus; }
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
}