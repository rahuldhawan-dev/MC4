using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace WorkOrders.Model
{
    [Table(Name = "dbo.Counties")]
    public class County : INotifyPropertyChanging, INotifyPropertyChanged
    {
        #region Private Static Methods

        private static readonly PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        #endregion

        #region Private Members

        private int _countyID;
        private int _stateID;
        private string _name;

        private EntityRef<State> _state;

        #endregion

        #region Properties

        [Column(Name = "CountyID", Storage = "_countyID", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int CountyID
        {
            get { return _countyID; }
            set
            {
                if (_countyID != value)
                {
                    SendPropertyChanging();
                    _countyID = value;
                    SendPropertyChanged("CountyID");
                }
            }
        }

        [Column(Storage = "_stateID", DbType = "Int")]
        public int StateID
        {
            get { return _stateID; }
            set
            {
                if (_stateID != value)
                {
                    SendPropertyChanging();
                    _stateID = value;
                    SendPropertyChanged("StateID");
                }
            }
        }

        [Column(Name = "Name", Storage = "_name", DbType = "VarChar(50)")]
        public string Name
        {
            get { return _name; }
            set
            {
                if ((_name != value))
                {
                    SendPropertyChanging();
                    _name = value;
                    SendPropertyChanged("Name");
                }
            }
        }
        
        // STATE
        [Association(Name = "State_County", Storage = "_state", ThisKey = "StateID", OtherKey="StateID")]
        public State State
        {
            get { return _state.Entity; }
            set
            {
                var previousValue = _state.Entity;
                if ((previousValue != value || _state.HasLoadedOrAssignedValue == false))
                {
                    SendPropertyChanging();
                    if (previousValue != null)
                        _state.Entity = null;
                    _state.Entity = value;
                    if (value != null)
                        _stateID = value.StateID;
                    else
                        _stateID = default(int);
                    SendPropertyChanged("State");
                }
            }
        }

        #endregion

        #region Private Methods

        private void SendPropertyChanging()
        {
            if ((PropertyChanging != null))
                PropertyChanging(this, emptyChangingEventArgs);
        }

        private void SendPropertyChanged(String propertyName)
        {
            if ((PropertyChanged != null))
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Exposed Methods

        public override string ToString()
        {
            return Name;
        }

        #endregion

        #region Events

        public event PropertyChangingEventHandler PropertyChanging;

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
