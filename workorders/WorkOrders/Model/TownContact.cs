using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Web.UI.WebControls;
using MapCall.Common.Model.Entities;

namespace WorkOrders.Model
{
    [Table(Name = "dbo.TownsContacts")]
    public class TownContact : INotifyPropertyChanging, INotifyPropertyChanged
    {
        #region Private Static Members

        private static readonly PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        #endregion

        private int _townId, _contactId, _contactTypeId, _townsContactsId;
        private EntityRef<Town> _town;
        private EntityRef<Contact> _contact;
        private EntityRef<ContactType> _contactType;

        [Column(Storage = "_townsContactsId", AutoSync = AutoSync.OnInsert, DbType = "int not null identity", IsPrimaryKey = true, IsDbGenerated = true)]
        public int TownsContactsID
        {
            get { return _townsContactsId; }
            set
            {
                if (_townsContactsId != value)
                {
                    SendPropertyChanging();
                    _townsContactsId = value;
                    SendPropertyChanged("TownsContactID");
                }
            }
        }

        [Column(Storage = "_townId", DbType = "int not null")]
        public int TownID
        {
            get { return _townId; }
            set
            {
                if (_townId != value)
                {
                    if (_town.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                    SendPropertyChanging();
                    _townId = value;
                    SendPropertyChanged("TownID");
                }
            }
        }

        [Column(Storage = "_contactId", DbType = "int not null")]
        public int ContactID
        {
            get { return _contactId; }
            set
            {
                if (_contactId != value)
                {
                    if (_contact.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                    SendPropertyChanging();
                    _contactId = value;
                    SendPropertyChanged("ContactID");
                }
            }
        }

        [Column(Storage = "_contactTypeId", DbType = "int not null")]
        public int ContactTypeID
        {
            get { return _contactTypeId; }
            set
            {
                if (_contactTypeId != value)
                {
                    if (_contactType.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                    SendPropertyChanging();
                    _contactTypeId = value;
                    SendPropertyChanged("ContactTypeID");
                }
            }
        }

        [Association(Name = "Town_TownsContacts", Storage = "_town",ThisKey = "TownID", IsForeignKey = true)]
        public Town Town
        {
            get { return _town.Entity; }
            set
            {
                var previousValue = _town.Entity;
                if ((previousValue != value)
                     || (_town.HasLoadedOrAssignedValue == false))
                {
                    SendPropertyChanging();
                    if ((previousValue != null))
                    {
                        _town.Entity = null;
                        previousValue.TownContacts.Remove(this);
                    }
                    _town.Entity = value;
                    if ((value != null))
                    {
                        value.TownContacts.Add(this);
                        _townId = value.TownID;
                    }
                    else
                    {
                        _townId = default(int);
                    }
                    SendPropertyChanged("Town");
                }
            }
        }

        [Association(Name = "Contact_TownsContacts", Storage = "_contact",ThisKey = "ContactID", IsForeignKey = true)]
        public Contact Contact
        {
            get { return _contact.Entity; }
            set
            {
                var previousValue = _contact.Entity;
                if (previousValue != value || _contact.HasLoadedOrAssignedValue == false)
                {
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _contact.Entity = null;
                        previousValue.TownContacts.Remove(this);
                    }
                    _contact.Entity = value;
                    if (value != null)
                    {
                        value.TownContacts.Add(this);
                        _contactId = value.ContactID;
                    }
                    else
                    {
                        _contactId = default(int);
                    }
                    SendPropertyChanged("Contact");
                }
            }
        }

        [Association(Name = "ContactType_TownsContacts", Storage = "_contactType", ThisKey = "ContactTypeID", IsForeignKey = true)]
        public ContactType ContactType
        {
            get { return _contactType.Entity; }
            set
            {
                var previousValue = _contactType.Entity;
                if (previousValue != value ||
                    _contactType.HasLoadedOrAssignedValue == false)
                {
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _contactType.Entity = null;
                        previousValue.TownContacts.Remove(this);
                    }
                    _contactType.Entity = value;
                    if (value != null)
                    {
                        value.TownContacts.Add(this);
                        _contactTypeId = value.ContactTypeID;
                    }
                    else
                    {
                        _contactTypeId = default(int);
                    }
                    SendPropertyChanged("ContactType");
                }
            }
        }

        public TownContact()
        {
            _town = default(EntityRef<Town>);
            _contact = default(EntityRef<Contact>);
            _contactType = default(EntityRef<ContactType>);
        }


        #region Private Methods

        protected virtual void SendPropertyChanging()
        {
            if (PropertyChanging != null)
                PropertyChanging(this, emptyChangingEventArgs);
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