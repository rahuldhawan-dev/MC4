using System;
using System.ComponentModel;
using System.Data.Linq.Mapping;
using MMSINC.Exceptions;

namespace WorkOrders.Model
{
    [Table(Name = "dbo.EquipmentPurposes")]
    public class EquipmentPurpose : INotifyPropertyChanging, INotifyPropertyChanged
    {
        #region Constants

        public const int MAX_DESCRIPTION_LENGTH = 50;

        #region Private Static Members

        private static readonly PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        #endregion

        #endregion

        #region Private Members

        private int _id;

        private string _description;

        private string _abbreviation;

        #endregion

        #region Properties

        [Column(Storage = "_id", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int EquipmentPurposeId
        {
            get { return _id; }
            set
            {
                if (_id != value)
                {
                    SendPropertyChanging();
                    _id = value;
                    SendPropertyChanged("Id");
                }
            }
        }

        [Column(Storage = "_description", DbType = "VarChar(50)", UpdateCheck = UpdateCheck.Never)]
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

        [Column(Name = "Abbreviation", Storage = "_abbreviation", DbType = "VarChar(4)")]
        public string Abbreviation
        {
            get { return _abbreviation; }
            set
            {
                if (_abbreviation != value)
                {
                    SendPropertyChanging();
                    _abbreviation = value;
                    SendPropertyChanged("Abbreviation");
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

        private void SendPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
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