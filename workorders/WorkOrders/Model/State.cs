using System;
using System.ComponentModel;
using System.Data.Linq.Mapping;
using System.Data.Linq;

namespace WorkOrders.Model
{
    [Table(Name = "dbo.States")]
    public class State : INotifyPropertyChanging, INotifyPropertyChanged
    {
        #region Private Static Methods

        private static readonly PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        #endregion

        #region Private Members

        private int _stateID;
        private string _name, _abbreviation;

        #endregion

        #region Properties

        [Column(Name = "StateID", Storage = "_stateID", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int StateID
        {
            get { return _stateID; }
            set
            {
                if (_stateID != value)
                {
                    SendPropertyChanging();
                    _stateID = value;
                    SendPropertyChanged("TownID");
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

        [Column(Name = "Abbreviation", Storage = "_abbreviation", DbType = "VarChar(50)")]
        public string Abbreviation
        {
            get { return _abbreviation; }
            set
            {
                if ((_abbreviation != value))
                {
                    SendPropertyChanging();
                    _abbreviation = value;
                    SendPropertyChanged("Abbreviation");
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

