using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using MMSINC.Exceptions;

namespace WorkOrders.Model
{
    [Table(Name = "dbo.LeakReportingSources")]
    public class LeakReportingSource : INotifyPropertyChanging, INotifyPropertyChanged
    {
        #region Constants

        private const short MAX_DESCRIPTION_LENGTH = 25;

        #endregion

        #region Private Static Members

        private static readonly PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        #endregion

        #region Private Members

        private string _description;

        private int _leakReportingSourceID;

        private readonly EntitySet<DetectedLeak> _detectedLeaks;

        #endregion

        #region Properties

        [Column(Storage = "_description", DbType = "VarChar(25)")]
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

        [Column(Storage = "_leakReportingSourceID", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int LeakReportingSourceID
        {
            get { return _leakReportingSourceID; }
            set
            {
                if (_leakReportingSourceID != value)
                {
                    SendPropertyChanging();
                    _leakReportingSourceID = value;
                    SendPropertyChanged("LeakReportingSourceID");
                }
            }
        }

        [Association(Name = "LeakReportingSource_DetectedLeak", Storage = "_detectedLeaks", OtherKey = "LeakReportingSourceID")]
        public EntitySet<DetectedLeak> DetectedLeaks
        {
            get { return _detectedLeaks; }
            set { _detectedLeaks.Assign(value); }
        }

        #endregion

        #region Constructors

        public LeakReportingSource()
        {
            _detectedLeaks = new EntitySet<DetectedLeak>(attach_DetectedLeaks, detach_DetectedLeaks);
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
                    if (String.IsNullOrEmpty(Description))
                        throw new DomainLogicException("Cannot save a LeakReportingSource without a Description.");
                    break;
            }
        }
        // ReSharper restore UnusedPrivateMember

        private void attach_DetectedLeaks(DetectedLeak entity)
        {
            SendPropertyChanging();
            entity.LeakReportingSource = this;
        }

        private void detach_DetectedLeaks(DetectedLeak entity)
        {
            SendPropertyChanging();
            entity.LeakReportingSource = null;
        }

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

        #region Events

        public event PropertyChangingEventHandler PropertyChanging;

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
