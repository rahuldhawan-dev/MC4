using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using MMSINC.Exceptions;

namespace WorkOrders.Model
{
    [Table(Name = "dbo.TownSections")]
    public class TownSection : INotifyPropertyChanging, INotifyPropertyChanged
    {
        #region Constants

        private const short MAX_NAME_LENGTH = 30;
        private const short MAX_TOWN_LENGTH = 30;

        #endregion

        #region Private Static Members

        private static readonly PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        #endregion

        #region Private Members

        private string _name;

        private int _townSectionID, _townID;

        private readonly EntitySet<WorkOrder> _workOrders;
        private readonly EntitySet<Valve> _valves;
        //private readonly EntitySet<Hydrant> _hydrants;

        #endregion

        #region Properties

        /// <summary>
        /// Primary key from the TownSections table.
        /// </summary>
        [Column(Name = "TownSectionID", Storage = "_townSectionID", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int TownSectionID
        {
            get { return _townSectionID; }
            set
            {
                if (_townSectionID != value)
                {
                    SendPropertyChanging();
                    _townSectionID = value;
                    SendPropertyChanged("TownSectionID");
                }
            }
        }

        /// <summary>
        /// Name of the Town Section.
        /// </summary>
        [Column(Name = "Name", Storage = "_name", DbType = "VarChar(30)")]
        public string Name
        {
            get { return _name; }
            set
            {
                if (value != null && value.Length > MAX_NAME_LENGTH)
                    throw new StringTooLongException("Name", MAX_NAME_LENGTH);
                if (_name != value)
                {
                    SendPropertyChanging();
                    _name = value;
                    SendPropertyChanged("Name");
                }
            }
        }

        /// <summary>
        /// Name of the Town to which the Town Section belongs.
        ///
        /// This is a string property, but it can be used to link to a Town
        /// object.  Going forward, this should be replaced with a foreign key
        /// (int).
        /// </summary>
        [Column(Storage = "_townID", DbType = "Int")]
        public int TownID
        {
            get { return _townID; }
            set
            {
                if (_townID != value)
                {
                    SendPropertyChanging();
                    _townID = value;
                    SendPropertyChanged("TownID");
                }
            }
        }

        /// <summary>
        /// Represents all of the WorkOrders which have been recorded for a
        /// given town section.
        /// </summary>
        [Association(Name = "TownSection_WorkOrder", Storage = "_workOrders", OtherKey = "TownSectionID")]
        public EntitySet<WorkOrder> WorkOrders
        {
            get { return _workOrders; }
            set { _workOrders.Assign(value); }
        }

        [Association(Name = "Valve_WorkOrder", Storage = "_valves",
            OtherKey = "TownSectionID")]
        public EntitySet<Valve> Valves
        {
            get { return _valves; }
            set { _valves.Assign(value); }
        } 

        #endregion

        #region Constructors

        public TownSection()
        {
            _workOrders = new EntitySet<WorkOrder>(attach_WorkOrders, detach_WorkOrders);
            _valves = new EntitySet<Valve>(attach_Valves, detach_Valves);

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

        private void attach_WorkOrders(WorkOrder entity)
        {
            SendPropertyChanging();
            entity.TownSection = this;
        }

        private void detach_WorkOrders(WorkOrder entity)
        {
            SendPropertyChanging();
            entity.TownSection = null;
        }

        private void attach_Valves(Valve entity)
        {
            SendPropertyChanging();
            entity.TownSection = this;
        }

        private void detach_Valves(Valve entity)
        {
            SendPropertyChanging();
            entity.TownSection = null;
        }

        //private void attach_Hydrants(Hydrant entity)
        //{
        //    SendPropertyChanging();
        //    entity.TownSection = this;
        //}

        //private void detach_Hydrants(Hydrant entity)
        //{
        //    SendPropertyChanging();
        //    entity.TownSection = null;
        //}

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
