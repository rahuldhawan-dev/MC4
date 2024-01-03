using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace WorkOrders.Model
{
    /// <summary>
    /// Very slimmed down, no saving, just for readonly stuff
    /// </summary>
    [Table(Name = "dbo.Contacts")]
    public class Contact : INotifyPropertyChanging, INotifyPropertyChanged
    {
        private static readonly PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        private int _contactId;
        private string _email;
        private readonly EntitySet<TownContact> _townContacts;


        [Column(Storage = "_contactId", AutoSync = AutoSync.OnInsert, DbType = "int not null identity", IsPrimaryKey = true, IsDbGenerated = true)]
        public int ContactID { get { return _contactId; } }

        [Column(Storage = "_email", DbType = "varchar(255) not null")]
        public string Email { get { return _email; } }

        [Association(Name = "Contact_TownsContacts", Storage = "_townContacts", OtherKey = "ContactID")]
        public EntitySet<TownContact> TownContacts
        {
            get { return _townContacts; }
            set { _townContacts.Assign(value); }
        }

        #region Constructor

        public Contact()
        {
            _townContacts = new EntitySet<TownContact>();
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
            entity.Contact = this;
        }

        private void detach_TownContacts(TownContact entity)
        {
            SendPropertyChanging();
            entity.Contact = null;
        }

        #endregion


        #region Events

        public event PropertyChangingEventHandler PropertyChanging;
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}