using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace WorkOrders.Model
{
    /// <summary>
    /// Very slimmed down, no saving, just for readonly stuff
    /// </summary>
    [Table(Name = "dbo.ContactTypes")]
    public class ContactType : INotifyPropertyChanging, INotifyPropertyChanged
    {
        private static readonly PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        private int _contactTypeID;
        private string _name;
        private readonly EntitySet<TownContact> _townContacts;

        #region Properties

        [Column(Storage="_contactTypeID", AutoSync = AutoSync.OnInsert, DbType="int not null identity", IsPrimaryKey = true, IsDbGenerated = true)]
        public int ContactTypeID { get { return _contactTypeID; } }

        [Column(Storage="_name", DbType="varchar(50) not null")]
        public string Name { get { return _name; } }

        [Association(Name = "Contact_TownsContacts", Storage = "_townContacts", OtherKey = "ContactTypeID")]
        public EntitySet<TownContact> TownContacts
        {
            get { return _townContacts; }
            set { _townContacts.Assign(value); }
        } 

        #endregion

        #region Constructors

        public ContactType()
        {
            _townContacts = new EntitySet<TownContact>(attach_TownContacts, detach_TownContacts);
        }

        #endregion

        #region Private Methods

        protected void SendPropertyChanging()
        {
            if (PropertyChanging != null)
                PropertyChanging(this, emptyChangingEventArgs);
        }

        protected void SendPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private void attach_TownContacts(TownContact entity)
        {
            SendPropertyChanging();
            entity.ContactType = this;
        }

        private void detach_TownContacts(TownContact entity)
        {
            SendPropertyChanging();
            entity.ContactType = null;
        }

        #endregion

        #region Events

        public event PropertyChangingEventHandler PropertyChanging;
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}