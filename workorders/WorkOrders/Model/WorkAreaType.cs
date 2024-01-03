using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using MMSINC.Exceptions;

namespace WorkOrders.Model
{
    [Table(Name = "dbo.WorkAreaTypes")]
    public class WorkAreaType : INotifyPropertyChanging, INotifyPropertyChanged
    {
        #region Constants

        private const short MAX_DESCRIPTION_LENGTH = 20;

        #endregion

        #region Private Static Members

        private static readonly PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        #endregion

        #region Private Members

        private string _description;

        private int _workAreaTypeID;

        private readonly EntitySet<DetectedLeak> _detectedLeaks;

        #endregion

        #region Properties

        [Column(Storage = "_description", DbType = "VarChar(20)")]
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

        [Column(Storage = "_workAreaTypeID", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int WorkAreaTypeID
        {
            get { return _workAreaTypeID; }
            set
            {
                if (_workAreaTypeID != value)
                {
                    SendPropertyChanging();
                    _workAreaTypeID = value;
                    SendPropertyChanged("WorkAreaTypeID");
                }
            }
        }

        [Association(Name = "WorkAreaType_DetectedLeak", Storage = "_detectedLeaks", OtherKey = "WorkAreaTypeID")]
        public EntitySet<DetectedLeak> DetectedLeaks
        {
            get { return _detectedLeaks; }
            set { _detectedLeaks.Assign(value); }
        }

        #endregion

        #region Constructors

        public WorkAreaType()
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
                        throw new DomainLogicException("Cannot save a WorkAreaType without a Description.");
                    break;
            }
        }
        // ReSharper restore UnusedPrivateMember

        private void attach_DetectedLeaks(DetectedLeak entity)
        {
            SendPropertyChanging();
            entity.WorkAreaType = this;
        }

        private void detach_DetectedLeaks(DetectedLeak entity)
        {
            SendPropertyChanging();
            entity.WorkAreaType = null;
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
