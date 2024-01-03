using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using MMSINC.Exceptions;

namespace WorkOrders.Model
{
    [Table(Name = "dbo.RestorationResponsePriorities")]
    public class RestorationResponsePriority : INotifyPropertyChanging, INotifyPropertyChanged
    {
        #region Constants

        private const short MAX_DESCRIPTION_LENGTH = 25;

        #endregion

        #region Private Static Members

        private static readonly PropertyChangingEventArgs _emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        #endregion

        #region Private Members

        private int _RestorationResponsePriorityID;

        private string _description;

        private readonly EntitySet<Restoration> _restorations;

        #endregion

        #region Properties

        #region Table Column Properties

        [Column(Storage = "_RestorationResponsePriorityID", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int RestorationResponsePriorityID
        {
            get { return _RestorationResponsePriorityID; }
            set
            {
                if (_RestorationResponsePriorityID != value)
                {
                    SendPropertyChanging();
                    _RestorationResponsePriorityID = value;
                    SendPropertyChanged("RestorationResponsePriorityID");
                }
            }
        }

        [Column(Storage = "_description", DbType = "VarChar(25)")]
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

        #endregion

        #region Association Properties

        [Association(Name = "ResponsePriority_Restoration", Storage = "_restorations", OtherKey = "ResponsePriorityID")]
        public EntitySet<Restoration> Restorations
        {
            get { return _restorations; }
            set { _restorations.Assign(value); }
        }

        #endregion

        #endregion

        #region Constructors

        public RestorationResponsePriority()
        {
            _restorations = new EntitySet<Restoration>(attach_Restorations,
                detach_Restorations);
        }

        #endregion

        #region Private Methods

        protected void OnValidate(ChangeAction action)
        {
            switch (action)
            {
                case ChangeAction.Insert:
                case ChangeAction.Update:
                    if (String.IsNullOrEmpty(Description))
                        throw new DomainLogicException("Description cannot be null");
                    break;
            }
        }

        private void attach_Restorations(Restoration entity)
        {
            SendPropertyChanging();
            entity.ResponsePriority = this;
        }
        private void detach_Restorations(Restoration entity)
        {
            SendPropertyChanging();
            entity.ResponsePriority = null;
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
