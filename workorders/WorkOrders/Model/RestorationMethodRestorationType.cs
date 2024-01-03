using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace WorkOrders.Model
{
    [Table(Name = "dbo.RestorationMethodsRestorationTypes")]
    public class RestorationMethodRestorationType : INotifyPropertyChanging, INotifyPropertyChanged
    {
        #region Private Static Members

        private static readonly PropertyChangingEventArgs _emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        #endregion

        #region Private Members

        private int _restorationMethodRestorationTypeID,
                    _restorationTypeID,
                    _restorationMethodID;

        private bool _initialMethod, _finalMethod;

        private EntityRef<RestorationType> _restorationType;
        private EntityRef<RestorationMethod> _restorationMethod;

        #endregion

        #region Properties

        [Column(Storage = "_restorationMethodRestorationTypeID", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int RestorationMethodRestorationTypeID
        {
            get { return _restorationMethodRestorationTypeID; }
            set
            {
                if (_restorationMethodRestorationTypeID != value)
                {
                    SendPropertyChanging();
                    _restorationMethodRestorationTypeID = value;
                    SendPropertyChanged("RestorationMethodRestorationTypeID");
                }
            }
        }

        [Column(Storage = "_restorationTypeID", DbType = "Int NOT NULL")]
        public int RestorationTypeID
        {
            get { return _restorationTypeID; }
            set
            {
                if (_restorationTypeID != value)
                {
                    if (_restorationType.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                }
                SendPropertyChanging();
                _restorationTypeID = value;
                SendPropertyChanged("RestorationTypeID");
            }
        }

        [Column(Storage = "_restorationMethodID", DbType = "Int NOT NULL")]
        public int RestorationMethodID
        {
            get { return _restorationMethodID; }
            set
            {
                if (_restorationMethodID != value)
                {
                    if (_restorationMethod.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                }
                SendPropertyChanging();
                _restorationMethodID = value;
                SendPropertyChanged("RestorationMethodID");
            }
        }

        [Column(Storage = "_initialMethod", DbType = "Bit NOT NULL")]
        public bool InitialMethod
        {
            get { return _initialMethod; }
            set
            {
                if (_initialMethod != value)
                {
                    SendPropertyChanging();
                    _initialMethod = value;
                    SendPropertyChanged("InitialMethod");
                }
            }
        }

        [Column(Storage = "_finalMethod", DbType = "Bit NOT NULL")]
        public bool FinalMethod
        {
            get { return _finalMethod; }
            set
            {
                if (_finalMethod != value)
                {
                    SendPropertyChanging();
                    _finalMethod = value;
                    SendPropertyChanged("FinalMethod");
                }
            }
        }

        [Association(Name = "RestorationType_RestorationMethodRestorationType", Storage = "_restorationType", ThisKey = "RestorationTypeID", IsForeignKey = true)]
        public RestorationType RestorationType
        {
            get { return _restorationType.Entity; }
            set
            {
                var previousValue = _restorationType.Entity;
                if ((previousValue != value)
                    || (_restorationType.HasLoadedOrAssignedValue == false))
                {
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _restorationType.Entity = null;
                        previousValue.RestorationMethodsRestorationTypes.Remove(this);
                    }
                    _restorationType.Entity = value;
                    if (value != null)
                    {
                        value.RestorationMethodsRestorationTypes.Add(this);
                        _restorationTypeID = value.RestorationTypeID;
                    }
                    else
                        _restorationTypeID = default(int);
                    SendPropertyChanged("RestorationType");
                }
            }
        }

        [Association(Name = "RestorationMethod_RestorationMethodRestorationType", Storage = "_restorationMethod", ThisKey = "RestorationMethodID", IsForeignKey = true)]
        public RestorationMethod RestorationMethod
        {
            get { return _restorationMethod.Entity; }
            set
            {
                var previousValue = _restorationMethod.Entity;
                if ((previousValue != value)
                    || (_restorationMethod.HasLoadedOrAssignedValue == false))
                {
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _restorationMethod.Entity = null;
                        previousValue.RestorationMethodsRestorationTypes.Remove(this);
                    }
                    _restorationMethod.Entity = value;
                    if (value != null)
                    {
                        value.RestorationMethodsRestorationTypes.Add(this);
                        _restorationMethodID = value.RestorationMethodID;
                    }
                    else
                        _restorationMethodID = default(int);
                    SendPropertyChanged("RestorationMethod");
                }
            }
        }

        #endregion

        #region Constructors

        public RestorationMethodRestorationType()
        {
        }

        #endregion

        #region Private Members

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

        #endregion

        #region Events

        public event PropertyChangingEventHandler PropertyChanging;

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
