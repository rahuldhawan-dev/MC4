using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace WorkOrders.Model
{
    [Table(Name = "dbo.SampleSites")]
    public class SampleSite : INotifyPropertyChanging, INotifyPropertyChanged
    {
        #region Private members

        private int _id;
        private int _premiseId;

        private EntityRef<Premise> _premise;

        #endregion

        #region Properties

        [Column(
            Name = "Id", 
            Storage = "_id",
            AutoSync = AutoSync.OnInsert, 
            DbType = "Int NOT NULL IDENTITY",
            IsPrimaryKey = true, 
            IsDbGenerated = true)]
        public int Id
        {
            get { return _id; }
        }

        [Column(
            Storage = "_premiseId", 
            DbType = "Int NOT NULL", 
            UpdateCheck = UpdateCheck.Never)]
        public int PremiseId
        {
            get => _premiseId;
            set
            {
                if (_premiseId != value)
                {
                    if (_premise.HasLoadedOrAssignedValue)
                    {
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                    }
                }

                SendPropertyChanging();
                _premiseId = value;
                SendPropertyChanged("PremiseId");
            }
        }

        [Association(
            Name = "SampleSite_Premise", 
            Storage = "_premise", 
            ThisKey = "PremiseId", 
            IsForeignKey = true)]
        public Premise Premise
        {
            get => _premise.Entity;
            set
            {
                var previousValue = _premise.Entity;
                if (previousValue != value || !_premise.HasLoadedOrAssignedValue)
                {
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _premise.Entity = null;
                    }
                    _premise.Entity = value;

                    _premiseId = value?.Id ?? default;
                        
                    SendPropertyChanged("Premise");
                }
            }
        }

        #endregion

        #region Private Methods

        private void SendPropertyChanging()
        {
            PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(string.Empty));
        }

        private void SendPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Events

        public event PropertyChangingEventHandler PropertyChanging;

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
