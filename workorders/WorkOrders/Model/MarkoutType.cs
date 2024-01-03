using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using MMSINC.Exceptions;

namespace WorkOrders.Model
{
    [Table(Name = "dbo.MarkoutTypes")]
    public class MarkoutType : INotifyPropertyChanging, INotifyPropertyChanged
    {
        #region Constants

        private const short MAX_DESCRIPTION_LENGTH = 120;

        #endregion

        #region Private Static Members

        private static readonly PropertyChangingEventArgs _emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        #endregion

        #region Private Members

        private int _MarkoutTypeID, _order;
        private string _description;

        private readonly EntitySet<Markout> _markouts;
        private readonly EntitySet<WorkOrder> _workOrders;

        #endregion

        #region Properties

        [Column(Storage = "_MarkoutTypeID", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int MarkoutTypeID
        {
            get { return _MarkoutTypeID; }
            set
            {
                if (_MarkoutTypeID != value)
                {
                    SendPropertyChanging();
                    _MarkoutTypeID = value;
                    SendPropertyChanged("MarkoutTypeID");
                }
            }
        }

        [Column(Storage = "_description", DbType = "VarChar(20) NOT NULL", CanBeNull = false)]
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

        [Column(Storage = "_order", DbType = "Int NOT NULL")]
        public int Order
        {
            get { return _order; }
            set
            {
                if (_order != value)
                {
                    // OnOrderChanging(value);
                    SendPropertyChanging();
                    _order = value;
                    SendPropertyChanged("Order");
                    // OnOrderChanged();
                }
            }
        }

        [Association(Name = "MarkoutType_Markouts", Storage = "_markouts", OtherKey="MarkoutTypeID")]
        public EntitySet<Markout> Markouts
        {
            get { return _markouts; }
            set { _markouts.Assign(value); }
        }

        [Association(Name = "MarkoutTypeNeeded_WorkOrder", Storage = "_workOrders", OtherKey = "MarkoutTypeNeededID")]
        public EntitySet<WorkOrder> WorkOrders
        {
            get { return _workOrders; }
            set { _workOrders.Assign(value); }
        }

        #endregion

        #region Constructors

        public MarkoutType()
        {
            _markouts = new EntitySet<Markout>(attach_Markouts, detach_Markouts);
            _workOrders = new EntitySet<WorkOrder>(attach_WorkOrders, detach_WorkOrders);
        }

        #endregion

        #region Private Methods

        private void attach_Markouts(Markout entity)
        {
            SendPropertyChanging();
            entity.MarkoutType = this;
        }
        private void detach_Markouts(Markout entity)
        {
            SendPropertyChanging();
            entity.MarkoutType = null;
        }

        private void attach_WorkOrders(WorkOrder entity)
        {
            SendPropertyChanging();
            entity.MarkoutTypeNeeded = this;
        }
        private void detach_WorkOrders(WorkOrder entity)
        {
            SendPropertyChanging();
            entity.MarkoutTypeNeeded = null;
        }

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

        #region Exposed Methods

        public override string ToString()
        {
            return Description;
        }

        #endregion

        #region Events

        public event PropertyChangingEventHandler PropertyChanging;

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
