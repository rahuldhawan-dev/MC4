using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using MMSINC.Exceptions;

namespace WorkOrders.Model
{
    [Table(Name = "dbo.DocumentData")]
    public class DocumentData : INotifyPropertyChanging, INotifyPropertyChanged
    {
        #region consts

        public const int HASH_LENGTH = 40;

        #endregion

        #region Private Static Members

        private static readonly PropertyChangingEventArgs _emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        #endregion

        #region Private Members

        private int _id, 
                    _fileSize;
        private string _hash;

        #endregion

        #region Properties

        #region Table Column Properties

        [Column(Storage = "_id", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int Id
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

        [Column(Storage = "_hash", DbType = "VarChar(40) NOT NULL", CanBeNull=false)]
        public string Hash
        {
            get { return _hash; }
            set
            {
                if (_hash != value)
                {
                    SendPropertyChanging();
                    _hash = value;
                    SendPropertyChanged("Hash");
                }
            }
        }

        [Column(Name = "FileSize", Storage = "_fileSize", DbType = "Int NOT NULL")]
        public int FileSize
        {
            get { return _fileSize; }
            set
            {
                if (_fileSize != value)
                {
                    SendPropertyChanging();
                    _fileSize = value;
                    SendPropertyChanged("FileSize");
                }
            }
        }

        #endregion

        #endregion

        #region Private Methods

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

        protected void OnValidate(ChangeAction action)
        {
            switch (action)
            {
                case ChangeAction.Insert:
                    if (string.IsNullOrWhiteSpace(Hash) || Hash.Length != HASH_LENGTH)
                        throw new DomainLogicException("Cannot save a DocumentData object without a valid hash.");
                    if (FileSize <= 0)
                        throw new DomainLogicException("Cannot save a DocumentData object with a FileSize less than or equal to 0.");
                    break;
                case ChangeAction.Update:
                    throw new DomainLogicException("Cannot update DocumentData objects.");
                case ChangeAction.Delete:
                    throw new DomainLogicException("Cannot delete DocumentData objects.");
            }
        }


        #endregion

        #region Events

        public event PropertyChangingEventHandler PropertyChanging;

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
