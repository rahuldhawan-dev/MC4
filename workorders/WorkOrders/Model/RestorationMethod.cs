using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using MMSINC.Exceptions;

namespace WorkOrders.Model
{
    [Table(Name = "dbo.RestorationMethods")]
    public class RestorationMethod : INotifyPropertyChanging, INotifyPropertyChanged
    {
        #region Constants

        private const short MAX_DESCRIPTION_LENGTH = 35;

        #endregion

        #region Private Static Members

        private static readonly PropertyChangingEventArgs _emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        #endregion

        #region Private Members

        private int _restorationMethodID;
        private string _description;

        private readonly EntitySet<RestorationMethodRestorationType> _restorationMethodsRestorationTypes;

        private readonly EntitySet<Restoration> _partialRestorations,
                                                _finalRestorations;

        #endregion

        #region Properties

        [Column(Storage = "_restorationMethodID", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int RestorationMethodID
        {
            get { return _restorationMethodID; }
            set
            {
                if (_restorationMethodID != value)
                {
                    SendPropertyChanging();
                    _restorationMethodID = value;
                    SendPropertyChanged("RestorationMethodID");
                }
            }
        }

        [Column(Storage = "_description", DbType = "VarChar(35) NOT NULL")]
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

        [Association(Name = "RestorationMethod_RestorationMethodRestorationType", Storage = "_restorationMethodsRestorationTypes", OtherKey = "RestorationMethodID")]
        public EntitySet<RestorationMethodRestorationType> RestorationMethodsRestorationTypes
        {
            get { return _restorationMethodsRestorationTypes; }
            set { _restorationMethodsRestorationTypes.Assign(value); }
        }

        [Association(Name = "PartialRestorationMethod_Restoration", Storage = "_partialRestorations", OtherKey = "PartialRestorationMethodID")]
        public EntitySet<Restoration> PartialRestorations
        {
            get { return _partialRestorations; }
            set { _partialRestorations.Assign(value); }
        }

        [Association(Name = "FinalRestorationMethod_Restoration", Storage = "_finalRestorations", OtherKey = "FinalRestorationMethodID")]
        public EntitySet<Restoration> FinalRestorations
        {
            get { return _finalRestorations; }
            set { _finalRestorations.Assign(value); }
        }

        #endregion

        #region Constructors

        public RestorationMethod()
        {
            _restorationMethodsRestorationTypes =
                new EntitySet<RestorationMethodRestorationType>(
                    attach_RestorationMethodsRestorationTypes,
                    detach_RestorationMethodsRestorationTypes);
            _partialRestorations =
                new EntitySet<Restoration>(attach_PartialRestorations,
                    detach_PartialRestorations);
            _finalRestorations =
                new EntitySet<Restoration>(attach_FinalRestorations,
                    detach_FinalRestorations);
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

        private void attach_RestorationMethodsRestorationTypes(RestorationMethodRestorationType entity)
        {
            SendPropertyChanging();
            entity.RestorationMethod = this;
        }

        private void detach_RestorationMethodsRestorationTypes(RestorationMethodRestorationType entity)
        {
            SendPropertyChanging();
            entity.RestorationMethod = null;
        }

        private void attach_PartialRestorations(Restoration entity)
        {
            SendPropertyChanging();
            //entity.PartialRestorationMethod = this;
        }

        private void detach_PartialRestorations(Restoration entity)
        {
            SendPropertyChanging();
            //entity.PartialRestorationMethod = null;
        }

        private void attach_FinalRestorations(Restoration entity)
        {
            SendPropertyChanging();
            //entity.FinalRestorationMethod = this;
        }

        private void detach_FinalRestorations(Restoration entity)
        {
            SendPropertyChanging();
            //entity.FinalRestorationMethod = null;
        }

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
