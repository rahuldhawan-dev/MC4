using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace WorkOrders.Model
{
    // NOTE: This is very very slimmed down compared to MapCall.Common's version because this is supposed to be 
    // entirely readonly on the 271 side. That means properties don't have setters and almost none of the actual
    // properties are referenced.
    [Table(Name = "dbo.TrafficControlTickets")]
    public class TrafficControlTicket : INotifyPropertyChanging, INotifyPropertyChanged
    {
        #region Private Members 

#pragma warning disable 649
        private int _id;
        private int? _WorkOrderId;
        private DateTime _WorkStartDate;
        private decimal _TotalHours;
        private int _NumberOfOfficers;
        private int? _billingPartyId;
        private EntityRef<BillingParty> _billingParty;
#pragma warning restore 649

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

        [Column(Storage = "_WorkStartDate", DbType = "datetime NOT NULL", UpdateCheck = UpdateCheck.Never)]
        public DateTime WorkStartDate
        {
            get { return _WorkStartDate; }
        }

        [Column(Storage = "_TotalHours", Name = "TotalHours", DbType = "decimal(5,2) NOT NULL", UpdateCheck = UpdateCheck.Never)]
        public decimal? TotalHours
        {
            get { return _TotalHours; }
        }

        [Column(Storage = "_NumberOfOfficers", Name = "NumberOfOfficers", DbType = "Int NOT NULL", UpdateCheck = UpdateCheck.Never)]
        public int? NumberOfOfficers
        {
            get { return _NumberOfOfficers; }
        }

        [Column(Storage = "_billingPartyId", Name = "BillingPartyId", DbType = "Int NULL", UpdateCheck = UpdateCheck.Never, CanBeNull = true)]
        public int? BillingPartyId
        {
            get { return _billingPartyId; }
        }

        [Association(Name = "BillingParty_TrafficControlTicket", Storage = "_billingParty", ThisKey = "BillingPartyId", IsForeignKey = true)]
        public BillingParty BillingParty
        {
            get { return _billingParty.Entity; }
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
