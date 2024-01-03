using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace WorkOrders.Model
{
    [Table(Name = "dbo.ServiceSizes")]
    public class ServiceSize : INotifyPropertyChanging, INotifyPropertyChanged
    {
        #region Private Static Members

        private static readonly PropertyChangingEventArgs _emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        #endregion

        #region Private Members

        private int _recID;
        private bool _hyd;
        private bool _lat;
        private bool _main;
        private bool _meter;
        private int _record;
        private bool _serv;
        private string _sizeServ;
        private decimal? _valve;
        private readonly EntitySet<MainBreak> _mainBreaks;
    

        #endregion

        #region Properties

        [Column(Name = "Id", Storage = "_recID", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int ServiceSizeID
        {
            get { return _recID; }
            set
            {
                if (_recID != value)
                {
                    SendPropertyChanging();
                    _recID = value;
                    SendPropertyChanged("ServiceSizeID");
                }
            }
        }

        [Column(Storage = "_hyd", DbType = "bit NULL")]
        public bool Hydrant
        {
            get { return _hyd; }
            set
            {
                if (_hyd != value)
                {
                    SendPropertyChanging();
                    _hyd = value;
                    SendPropertyChanged("ServiceSizeID");
                }
            }
        }

        [Column(Storage = "_lat", DbType = "bit NULL")]
        public bool Lateral
        {
            get { return _lat; }
            set
            {
                if (_lat != value)
                {
                    SendPropertyChanging();
                    _lat = value;
                    SendPropertyChanged("Lat");
                }
            }
        }

        [Column(Storage = "_main", DbType = "bit NULL")]
        public bool Main
        {
            get { return _main; }
            set
            {
                if (_main != value)
                {
                    SendPropertyChanging();
                    _main = value;
                    SendPropertyChanged("Main");
                }
            }
        }

        [Column(Storage = "_meter", DbType = "bit NULL")]
        public bool Meter
        {
            get { return _meter; }
            set
            {
                if (_meter != value)
                {
                    SendPropertyChanging();
                    _meter = value;
                    SendPropertyChanged("Meter");
                }
            }
        }

        [Column(Storage = "_record", Name = "SortOrder", DbType = "Int NOT NULL")]
        public int SortOrder
        {
            get { return _record; }
            set
            {
                if (_record != value)
                {
                    SendPropertyChanging();
                    _record = value;
                    SendPropertyChanged("Record");
                }
            }
        }

        [Column(Storage = "_serv", DbType = "bit NULL")]
        public bool Service
        {
            get { return _serv; }
            set
            {
                if (_serv != value)
                {
                    SendPropertyChanging();
                    _serv = value;
                    SendPropertyChanged("Serv");
                }
            }
        }

        [Column(Storage = "_sizeServ", DbType = "varchar(10) NULL")]
        public string ServiceSizeDescription
        {
            get { return _sizeServ; }
            set
            {
                if (_sizeServ != value)
                {
                    SendPropertyChanging();
                    _sizeServ = value;
                    SendPropertyChanged("SizeServ");
                }
            }
        }

        [Column(Storage = "_valve", DbType = "decimal(18,2) NULL")]
        public decimal? Size
        {
            get { return _valve; }
            set
            {
                if (_valve != value)
                {
                    SendPropertyChanging();
                    _valve = value;
                    SendPropertyChanged("Size");
                }
            }
        }

        [Association(Name = "ServiceSize_MainBreak", Storage = "_mainBreaks", OtherKey = "ServiceSizeID")]
        public EntitySet<MainBreak> MainBreaks
        {
            get { return _mainBreaks; }
            set { _mainBreaks.Assign(value); }
        }

        #endregion

        #region Constructors

        public ServiceSize()
        {
            _mainBreaks = new EntitySet<MainBreak>(attach_MainBreaks, detach_MainBreaks);
        }

        #endregion

        #region Private Methods

        protected virtual void SendPropertyChanging()
        {
            if (PropertyChanging != null)
                PropertyChanging(this, _emptyChangingEventArgs);
        }

        protected virtual void SendPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private void attach_MainBreaks(MainBreak entity)
        {
            SendPropertyChanging();
            entity.ServiceSize = this;
        }

        private void detach_MainBreaks(MainBreak entity)
        {
            SendPropertyChanging();
            entity.ServiceSize = null;
        }

        #endregion

        #region Events

        public event PropertyChangingEventHandler PropertyChanging;

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Exposed Methods

        public override string ToString()
        {
            return ServiceSizeDescription;
        }

        #endregion
    }
}
