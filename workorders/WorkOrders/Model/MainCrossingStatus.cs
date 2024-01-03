using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using MMSINC.Exceptions;

namespace WorkOrders.Model
{
    [Table(Name = "dbo.MainCrossingStatuses")]
    public class MainCrossingStatus : INotifyPropertyChanging, INotifyPropertyChanged
    {
        #region Constants

        private const short MAX_DESCRIPTION_LENGTH = 50;

        #endregion

        #region Private Static Members

        private static readonly PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        #endregion

        #region Private Members

        private int _MainCrossingStatusId;
        private string _description;
        private readonly EntitySet<MainCrossing> _mainCrossings;

        #endregion

        #region Properties

        [Column(Name = "Id", Storage = "_MainCrossingStatusId", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int MainCrossingStatusID
        {
            get { return _MainCrossingStatusId; }
            set
            {
                if (_MainCrossingStatusId != value)
                {
                    SendPropertyChanging();
                    _MainCrossingStatusId = value;
                    SendPropertyChanged("MainCrossingStatusID");
                }
            }
        }

        [Column(Storage = "_description", DbType = "VarChar(50) NOT NULL")]
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

        [Association(Name = "MainCrossingStatus_MainCrossing", Storage = "_mainCrossings", OtherKey = "MainCrossingStatusID")]
        public EntitySet<MainCrossing> MainCrossings
        {
            get { return _mainCrossings; }
            set { _mainCrossings.Assign(value); }
        }

        #endregion

        #region Constructors

        public MainCrossingStatus()
        {
            _mainCrossings = new EntitySet<MainCrossing>(
                attach_MainCrossings,
                detach_MainCrossings);
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

        private void attach_MainCrossings(MainCrossing entity)
        {
            SendPropertyChanging();
            entity.MainCrossingStatus = this;
        }

        private void detach_MainCrossings(MainCrossing entity)
        {
            SendPropertyChanging();
            entity.MainCrossingStatus = null;
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