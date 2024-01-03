using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using MMSINC.Exceptions;

namespace WorkOrders.Model
{
    [Table(Name="dbo.RequisitionTypes")]
    public class RequisitionType : INotifyPropertyChanging, INotifyPropertyChanged
    {
        #region Constants

        private const short MAX_DESCRIPTION_LENGTH = 50;

        #endregion

        #region Private Static Members

        private static readonly PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        #endregion

        #region Private Members

        private int _requisitionTypeID;
        private string _description;

        private readonly EntitySet<Requisition> _requisitions;

        #endregion

        #region Properties

        [Column(Storage = "_requisitionTypeID", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY",
            IsPrimaryKey = true, IsDbGenerated = true)]
        public int RequisitionTypeID
        {
            get { return _requisitionTypeID; }
            set
            {
                if (_requisitionTypeID != value)
                {
                    SendPropertyChanging();
                    _requisitionTypeID = value;
                    SendPropertyChanged("RequisitionTypeID");
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

        [Association(Name = "RequisitionType_Requisition", Storage = "_requisitions", OtherKey = "RequisitionTypeID")]
        public EntitySet<Requisition> Requisitions
        {
            get { return _requisitions; }
            set { _requisitions.Assign(value); }
        }

        #endregion

        #region Constructors

        public RequisitionType()
        {
            _requisitions = new EntitySet<Requisition>(attach_Requisitions, detach_Requisitions);
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

        private void detach_Requisitions(Requisition obj)
        {
            SendPropertyChanging();
            obj.RequisitionType = this;
        }

        private void attach_Requisitions(Requisition obj)
        {
            SendPropertyChanging();
            obj.RequisitionType = null;
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