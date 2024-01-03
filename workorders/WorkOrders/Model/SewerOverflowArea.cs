using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace WorkOrders.Model
{
    [Table(Name = "dbo.AreaCleanedUpTo")]
    public class SewerOverflowArea : INotifyPropertyChanging, INotifyPropertyChanged
    {
        #region Constants

        private static readonly PropertyChangingEventArgs emptyChangingEventArgs
            = new PropertyChangingEventArgs(String.Empty);

        #endregion

        #region Private Members

        private int _id;

        private string _description;

        private readonly EntitySet<SewerOverflow> _sewerOverflows;

        #endregion

        #region Properties

        [Column(Storage = "_id", AutoSync = AutoSync.OnInsert,
            DbType = "int NOT NULL IDENTITY", IsPrimaryKey = true,
            IsDbGenerated = true)]
        public int AreaCleanedUpToId
        {
            get => _id;
            set
            {
                if (_id != value)
                {
                    SendPropertyChanging();
                    _id = value;
                    SendPropertyChanged("AreaCleanedUpToId");
                }
            }
        }

        [Column(Storage = "_description", DbType = "varchar(255)", UpdateCheck = UpdateCheck.Never)]
        public string Description
        {
            get => _description;
            set
            {
                if (_description != value)
                {
                    SendPropertyChanging();
                    _description = value;
                    SendPropertyChanged("Description");
                }
            }
        }

        [Association(Name = "SewerOverflowArea_SewerOverflow", Storage = "_sewerOverflows", OtherKey = "AreaCleanedUpToId")]
        public EntitySet<SewerOverflow> SewerOverflows
        {
            get => _sewerOverflows;
            set => _sewerOverflows.Assign(value);
        }

        #endregion

        #region Constructors

        public SewerOverflowArea()
        {
            _sewerOverflows = new EntitySet<SewerOverflow>();
        }

        #endregion

        #region Private Methods

        private void SendPropertyChanging()
        {
            PropertyChanging?.Invoke(this, emptyChangingEventArgs);
        }

        private void SendPropertyChanged(String propertyName)
        {
            PropertyChanged?.Invoke(this,
                new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Events/Delegates

        public event PropertyChangedEventHandler PropertyChanged;

        public event PropertyChangingEventHandler PropertyChanging;

        #endregion

        #region Exposed Methods

        public override string ToString()
        {
            return Description;
        }

        #endregion
    }
}