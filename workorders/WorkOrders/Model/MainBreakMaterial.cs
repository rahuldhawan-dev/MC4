using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using MMSINC.Exceptions;

namespace WorkOrders.Model
{
    [Table(Name = "dbo.MainBreakMaterials")]
    public class MainBreakMaterial : INotifyPropertyChanging, INotifyPropertyChanged
    {
        #region Constants

        private const int MAX_DESCRIPTION_LENGTH = 50;

        #endregion

        #region Private Members

        private string _description;
        private int _mainBreakMaterialID;

        #endregion

        #region Private Static Members

        private static readonly PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(string.Empty);
        
        #endregion

        #region Properties

        [Column(Storage = "_mainBreakMaterialID", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int MainBreakMaterialID
        {
            get { return _mainBreakMaterialID; }
            set
            {
                if (_mainBreakMaterialID != value)
                {
                    SendPropertyChanging();
                    _mainBreakMaterialID = value;
                    SendPropertyChanged("mainBreakMaterialID");
                }
            }
        }

        [Column(Storage = "_description", DbType = "VarChar(50) NULL", CanBeNull = true)]
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

        #region Private Methods

        private void SendPropertyChanging()
        {
            if (PropertyChanging != null)
                PropertyChanging(this, emptyChangingEventArgs);
        }

        private void SendPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Events

        #region Implementation of INotifyPropertyChanging

        public event PropertyChangingEventHandler PropertyChanging;

        #endregion

        #region Implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
        
        #endregion

        #region Private Methods

        // ReSharper disable UnusedPrivateMember
        private void OnValidate(ChangeAction action)
        {
            switch (action)
            {
                case ChangeAction.Insert:
                case ChangeAction.Update:
                    if (string.IsNullOrEmpty(Description))
                        throw new DomainLogicException("Cannot save a without a description.");
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
    }
}
