using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using MMSINC.Exceptions;

namespace WorkOrders.Model
{
    [Table(Name = "dbo.MainBreakValveOperations")]
    public class MainBreakValveOperation : INotifyPropertyChanging, INotifyPropertyChanged
    {
        #region Private Static Members

        private static readonly PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        #endregion

        #region Private Members

        private int _mainBreakValveOperationID,
                    _mainBreakID,
                    _valveID;

        private EntityRef<MainBreak> _mainBreak;

        private EntityRef<Valve> _valve;

        #endregion

        #region Properties

        [Association(Name = "MainBreak_MainBreakValveOperation", Storage = "_mainBreak", ThisKey = "MainBreakID", IsForeignKey = true)]
        public MainBreak MainBreak
        {
            get { return _mainBreak.Entity; }
            set
            {
                var previousValue = _mainBreak.Entity;
                if ((previousValue != value)
                    || (_mainBreak.HasLoadedOrAssignedValue == false))
                {
                    if (previousValue != null && value != null)
                        throw new DomainLogicException("Cannot change the MainBreak of a given MainBreakValveOperation after it has been set.");
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _mainBreak.Entity = null;
                        //previousValue.MainBreakValveOperations.Remove(this);
                    }
                    _mainBreak.Entity = value;
                    if (value != null)
                    {
                        //value.MainBreakValveOperations.Add(this);
                        _mainBreakID = value.MainBreakID;
                    }
                    else
                    {
                        _mainBreakID = default(int);
                    }
                    SendPropertyChanged("MainBreak");
                }
            }
        }

        [Column(Storage = "_mainBreakValveOperationID", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int MainBreakValveOperationID
        {
            get { return _mainBreakValveOperationID; }
            set
            {
                if ((_mainBreakValveOperationID != value))
                {
                    SendPropertyChanging();
                    _mainBreakValveOperationID = value;
                    SendPropertyChanged("MainBreakValveOperationID");
                }
            }
        }

        [Column(Storage = "_mainBreakID", DbType = "Int NOT NULL")]
        public int MainBreakID
        {
            get { return _mainBreakID; }
            set
            {
                if ((_mainBreakID != value))
                {
                    if (_mainBreak.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                    SendPropertyChanging();
                    _mainBreakID = value;
                    SendPropertyChanged("MainBreakID");
                }
            }
        }

        [Column(Storage = "_valveID", DbType = "Int NOT NULL")]
        public int ValveID
        {
            get { return _valveID; }
            set
            {
                if ((_valveID != value))
                {
                    if (_valve.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                    SendPropertyChanging();
                    _valveID = value;
                    SendPropertyChanged("ValveID");
                }
            }
        }

        [Association(Name = "Valve_MainBreakValveOperation", Storage = "_valve", ThisKey = "ValveID", IsForeignKey = true)]
        public Valve Valve
        {
            get { return _valve.Entity; }
            set
            {
                var previousValue = _valve.Entity;
                if (((previousValue != value)
                     || (_valve.HasLoadedOrAssignedValue == false)))
                {
                    SendPropertyChanging();
                    if ((previousValue != null))
                    {
                        _valve.Entity = null;
                        previousValue.MainBreakValveOperations.Remove(this);
                    }
                    _valve.Entity = value;
                    if ((value != null))
                    {
                        value.MainBreakValveOperations.Add(this);
                        _valveID = value.ValveID;
                    }
                    else
                    {
                        _valveID = default(int);
                    }
                    SendPropertyChanged("Valve");
                }
            }
        }

        #endregion

        #region Constructors

        public MainBreakValveOperation()
        {
            _mainBreak = default(EntityRef<MainBreak>);
            _valve = default(EntityRef<Valve>);
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

        private void OnValidate(ChangeAction action)
        {
            switch (action)
            {
                case ChangeAction.Insert:
                    ValidateCreationInfo();
                    break;
            }
        }

        private void ValidateCreationInfo()
        {
            if (MainBreak == null)
                throw new DomainLogicException(
                    "Cannot save a MainBreakValveOperation without a MainBreak.");
            if (Valve == null)
                throw new DomainLogicException(
                    "Cannot save a MainBreakValveOperation without a Valve.");
        }

        #endregion

        #region Events

        public event PropertyChangingEventHandler PropertyChanging;

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}